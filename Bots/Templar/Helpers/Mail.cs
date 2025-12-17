using System;
using System.Linq;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Frames;
using Styx.CommonBot.Profiles;
using Styx.Pathing;
using Styx.WoWInternals;
using Templar.GUI.Tabs;

namespace Templar.Helpers
{
    /// <summary>
    /// Handles mailbox-related operations, including scanning bags for items to mail and interacting with mailboxes.
    /// Supports mailing items based on quality, protection settings, and user preferences.
    /// </summary>
    public class Mail
    {
        // Constants for mail limits and delays (adjust as needed for MOP)
        private const int MaxAttachmentsPerMail = 12;
        private const int MailSendDelayMs = 2000; // Delay after sending to avoid UI spam

        /// <summary>
        /// Scans the player's bags and populates Variables.MailList with items eligible for mailing.
        /// Items are filtered by quality, soulbound status, protected items, and force-mail settings.
        /// </summary>
        public static void CheckBags()
        {
            Variables.MailList.Clear();

            foreach (var bagItem in StyxWoW.Me.BagItems.Where(bagItem =>
                !bagItem.IsSoulbound &&
                ProtectedItemSettings.Instance.ProtectedItems.All(pi => pi.Entry != bagItem.Entry) &&
                (!ProtectedItemsManager.GetAllItemIds().Contains(bagItem.Entry) || ForceMailManager.GetAllItemIds().Contains(bagItem.Entry))))
            {
                bool shouldMail = false;
                switch (bagItem.Quality)
                {
                    case WoWItemQuality.Common:
                        shouldMail = MailSettings.Instance.MailWhites;
                        break;
                    case WoWItemQuality.Uncommon:
                        shouldMail = MailSettings.Instance.MailGreens;
                        break;
                    case WoWItemQuality.Rare:
                        shouldMail = MailSettings.Instance.MailBlues;
                        break;
                    case WoWItemQuality.Epic:
                        shouldMail = MailSettings.Instance.MailPurples;
                        break;
                }

                if (shouldMail && !Variables.MailList.Contains(bagItem))
                {
                    Variables.MailList.Add(bagItem);
                }
            }

            CustomLog.Normal("Mail check complete. Items to mail: {0}", Variables.MailList.Count);
        }

        /// <summary>
        /// Main entry point for handling mailing logic. Checks for a recipient, locates mailboxes, and processes mailing.
        /// If no mailbox is found, disables mailing to prevent getting stuck.
        /// </summary>
        public static void HandleMailing()
        {
            if (string.IsNullOrEmpty(MailSettings.Instance.Recipient))
            {
                CustomLog.Normal("You need to add a recipient name in the GUI.");
                TreeRoot.Stop("Add a recipient");
                return;
            }

            if (Variables.MailList.Count == 0)
            {
                CustomLog.Normal("No items to mail. Skipping.");
                return;
            }

            if (Variables.CloseMailbox != null)
            {
                HandleCloseMailbox();
                return;
            }

            if (Variables.FarMailbox != null)
            {
                HandleFarMailbox();
                return;
            }

            CustomLog.Normal("Could not find any mailbox in the cache.");
            CustomLog.Normal("Disabling mail usage to not get stuck.");
            CustomLog.Normal("Manually find a mailbox on this continent and start the bot there with the mail setting active in the GUI.");
            CustomLog.Normal("Then you can go back to your original spot again.");
            MailSettings.Instance.Mail = false;
            MailSettings.Save();
        }

        /// <summary>
        /// Handles moving to and interacting with a far mailbox.
        /// </summary>
        private static void HandleFarMailbox()
        {
            if (Variables.FarMailbox == null)
            {
                CustomLog.Normal("Could not find far mailbox.");
                return;
            }

            double distance = StyxWoW.Me.Location.Distance(Variables.FarMailbox.Location);
            CustomLog.Diagnostic("We have a mailbox! Distance: {0}", distance);

            if (distance > 30)
            {
                Flightor.MoveTo(Variables.FarMailbox.Location, true);
            }
            else
            {
                HandleCloseMailbox();
            }
        }

        /// <summary>
        /// Handles interacting with a close mailbox and attaching/sending items.
        /// </summary>
        private static void HandleCloseMailbox()
        {
            if (Variables.CloseMailbox == null)
            {
                CustomLog.Normal("Could not find close mailbox.");
                return;
            }

            if (!Variables.CloseMailbox.WithinInteractRange)
            {
                Flightor.MoveTo(Variables.CloseMailbox.Location, true);
            }
            else
            {
                if (!MailFrame.Instance.IsVisible)
                {
                    Variables.CloseMailbox.Interact();
                    StyxWoW.Sleep(1000); // Wait for frame to open
                }
                else
                {
                    AttachAndSend();
                }
            }
        }

        /// <summary>
        /// Attaches items to the mail and sends it. Handles multiple mails if there are more than 12 items.
        /// </summary>
        private static void AttachAndSend()
        {
            if (!MailFrame.Instance.IsVisible)
            {
                return;
            }

            MailFrame.Instance.SwitchToSendMailTab();
            StyxWoW.Sleep(500); // Brief delay for tab switch

            int totalItems = Variables.MailList.Count;
            int mailsSent = 0;

            while (Variables.MailList.Count > 0 && mailsSent < 10) // Safety limit to avoid infinite loops
            {
                int attachments = 0;
                foreach (var bagItem in Variables.MailList.Take(MaxAttachmentsPerMail).ToList())
                {
                    try
                    {
                        MailFrame.Instance.AttachItem(bagItem);
                        Variables.MailList.Remove(bagItem);
                        attachments++;
                        CustomLog.Normal("Attached item {0}, attachments in this mail: {1}", bagItem.Name, attachments);
                        StyxWoW.Sleep(200); // Small delay between attachments
                    }
                    catch (Exception ex)
                    {
                        CustomLog.Normal("Failed to attach item {0}: {1}", bagItem.Name, ex.Message);
                    }
                }

                if (attachments > 0)
                {
                    // Set recipient and subject
                    Lua.DoString(string.Format("SendMailNameEditBox:SetText('{0}');", MailSettings.Instance.Recipient));
                    Lua.DoString(string.Format("SendMailSubjectEditBox:SetText('{0}');", "Goodies"));
                    StyxWoW.Sleep(500);

                    // Send the mail
                    Lua.DoString("SendMailMailButton:Click();");
                    StyxWoW.Sleep(MailSendDelayMs); // Wait for send to process
                    mailsSent++;
                    CustomLog.Normal("Sent mail {0} with {1} items.", mailsSent, attachments);
                }
                else
                {
                    break; // No more attachments possible
                }
            }

            if (Variables.MailList.Count > 0)
            {
                CustomLog.Normal("Remaining items to mail: {0}. Will handle in next run.", Variables.MailList.Count);
            }
            else
            {
                CustomLog.Normal("All items mailed successfully.");
            }
        }
    }
}
