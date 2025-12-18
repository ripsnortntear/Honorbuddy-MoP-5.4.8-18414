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
        public class Mail
        {
            private
            const int MaxAttachmentsPerMail = 12;
            private
            const int MailSendDelayMs = 2000;
            public static void CheckBags()
            {
                Variables.MailList.Clear();
                foreach(var bagItem in StyxWoW.Me.BagItems.Where(bagItem => !bagItem.IsSoulbound && ProtectedItemSettings.Instance.ProtectedItems.All(pi => pi.Entry != bagItem.Entry) && (!ProtectedItemsManager.GetAllItemIds().Contains(bagItem.Entry) || ForceMailManager.GetAllItemIds().Contains(bagItem.Entry))))
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
                MailSettings.Instance.Mail = false;
                MailSettings.Save();
            }
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
                        StyxWoW.Sleep(1000);
                    }
                    else
                    {
                        AttachAndSend();
                    }
                }
            }
            private static void AttachAndSend()
            {
                if (!MailFrame.Instance.IsVisible) return;
                MailFrame.Instance.SwitchToSendMailTab();
                StyxWoW.Sleep(500);
                int mailsSent = 0;
                while (Variables.MailList.Count > 0 && mailsSent < 10)
                {
                    int attachments = 0;
                    foreach(var bagItem in Variables.MailList.Take(MaxAttachmentsPerMail).ToList())
                    {
                        try
                        {
                            Lua.DoString(string.Format("ClickSendMailItemButton({0}, {1});", bagItem.BagIndex + 1, bagItem.BagSlot + 1));
                            Variables.MailList.Remove(bagItem);
                            attachments++;
                            CustomLog.Normal("Attached item {0}, attachments in this mail: {1}", bagItem.Name, attachments);
                            StyxWoW.Sleep(200);
                        }
                        catch (Exception ex)
                        {
                            CustomLog.Normal("Failed to attach item {0}: {1}", bagItem.Name, ex.Message);
                        }
                    }
                    if (attachments > 0)
                    {
                        Lua.DoString(string.Format("SendMailNameEditBox:SetText('{0}');", MailSettings.Instance.Recipient));
                        Lua.DoString("SendMailSubjectEditBox:SetText('Goodies');");
                        StyxWoW.Sleep(500);
                        Lua.DoString("SendMailMailButton:Click();");
                        StyxWoW.Sleep(MailSendDelayMs);
                        mailsSent++;
                        CustomLog.Normal("Sent mail {0} with {1} items.", mailsSent, attachments);
                    }
                    else
                    {
                        break;
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
