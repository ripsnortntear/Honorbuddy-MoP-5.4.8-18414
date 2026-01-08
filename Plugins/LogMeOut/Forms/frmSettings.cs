using System;
using System.Diagnostics;
using System.Windows.Forms;
using Styx;

namespace LogMeOut.Forms
{
    public partial class FrmSettings : Form
    {
        public FrmSettings()
        {
            InitializeComponent();
        }

        private void FrmSettingsLoad(object sender, EventArgs e)
        {
            //Rempli la liste déroulante des type de points
            foreach (var strNomPoint in Arrays.NamesPoints)
            {
                cmbPoints.Items.Add(strNomPoint);
            }
            //Importe les données de la fenêtre
            LoadSettings();
            //Vérifie si une nouvelle mise à jour est disponible
            if (Classes.Updater.IsAvailable)
            {
                labUpdateCurrentVersion.Text = Classes.Updater.Version.ToString();
                labUpdateNewVersion.Text = Classes.Updater.GetLatestVersion();
                tabControl.SelectedTab = tabUpdate;
            }
            else
            {
                //Supprime l'onglet Update
                tabControl.TabPages.Remove(tabUpdate);
            }
            //Applique le focus sur le bouton Cancel
            btnCancel.Focus();
        }

        /// <summary>
        /// Charge toutes les variables de la fenêtre depuis un fichier XML.
        /// </summary>
        private void LoadSettings()
        {
            //Crée un pointeur sur l'instance des données sauvegardées
            var settings = LogMeOutSettings.Instance;
            //Importe les données
            settings.Load();
            //Rempli les champs du tab Triggers
            chkBagsFull.Checked = settings.AlertOnBagsFull;
            chkTimeElapsed.Checked = settings.AlertOnTimeElapsed;
            numHoursElapsed.Value = settings.HoursElapsed;
            numMinutesElapsed.Value = settings.MinutesElapsed;
            chkDeaths.Checked = settings.AlertOnDeaths;
            numDeaths.Value = settings.NbDeaths;
            chkStucks.Checked = settings.AlertOnStucks;
            numStucks.Value = settings.NbStucks;
            chkTeleported.Checked = settings.AlertOnTeleportation;
            numTeleported.Value = settings.TeleportYards;
            chkAchievement.Checked = settings.AlertOnAchievement;
            numAchievement.Value = settings.AchievementId;
            chkMinutesInCombat.Checked = settings.AlertOnMinutesInCombat;
            numMinutesInCombat.Value = settings.NbMinutesInCombat;
            chkMobsKilled.Checked = settings.AlertOnMobsKilled;
            numMobsKilled.Value = settings.NbMobsKilled;
            chkLootedItem.Checked = settings.AlertOnLootedItem;
            numLootedItemTimes.Value = settings.LootedItemTimes;
            numLootedItemId.Value = settings.LootedItemId;
            chkCannotLoot.Checked = settings.AlertOnCannotLoot;
            chkDurabilityCheck.Checked = settings.AlertOnDurability;
            numDurabilityPercent.Value = settings.DurabilityPercent;
            chkWhispersReceived.Checked = settings.AlertOnWhispersReceived;
            numWhispesReceived.Value = settings.NbWhispersReceived;
            chkWhisperGm.Checked = settings.AlertOnWhisperGm;
            chkPoints.Checked = settings.AlertOnPoints;
            numPoints.Value = settings.NbPoints;
            cmbPoints.SelectedIndex = settings.TypePoints;
            chkLevelReached.Checked = settings.AlertOnLevelReached;
            numLevelReached.Value = settings.NbLevel;
            chkGotAura.Checked = settings.AlertOnGotAura;
            cmbTrainingSkillLine.DataSource = Classes.TrainingSkills.SkillsLearnt;
            chkTrainingSkill.Checked = settings.AlertOnTrainingSkill;
            numTrainingSkillLevel.Value = settings.TrainingSkillLevel;
            if (cmbTrainingSkillLine.Items.Count == 0)
            {
                chkTrainingSkill.Enabled = false;
                chkTrainingSkill.Checked = false;
            }
            cmbTrainingSkillLine.SelectedItem = settings.TrainingSkillLine;
            chkInactivity.Checked = settings.AlertOnInactivity;
            numInactivityTimeout.Value = settings.InactivityTimeout;
            txtGotAuraName.Text = settings.GotAuraName;
            chkPlayerFollows.Checked = settings.AlertOnPlayerFollows;
            numPlayerFollows.Value = settings.MinutesPlayerFollows;
            chkPlayerTargets.Checked = settings.AlertOnPlayerTargets;
            numPlayerTargets.Value = settings.MinutesPlayerTargets;
            chkBeepWhenFire.Checked = settings.BeepWhenFire;
            
            //Rempli les champs du tab Action Before
            switch (settings.ActionBefore)
            {
                case 0:
                    radBeforeNothing.Checked = true;
                    break;
                case 1:
                    radBeforeHearthstone.Checked = true;
                    break;
                case 2:
                    radBeforeSpell.Checked = true;
                    break;
                case 3:
                    radBeforeItem.Checked = true;
                    break;
                case 4:
                    radBeforeLua.Checked = true;
                    break;
            }
            txtSpellName.Text = settings.SpellName;
            numItemID.Value = settings.ItemId;
            txtLuaString.Text = settings.LuaString;
            numLuaWaitingTime.Value = settings.LuaWaitingTime;

            //Rempli les champs du tab Action After
            switch (settings.ActionAfter)
            {
                case 0 :
                    radAfterNothing.Checked = true;
                    break;
                case 1:
                    radAfterShutdown.Checked = true;
                    break;
                case 2:
                    radAfterBatchLigne.Checked = true;
                    break;
            }
            txtAfterBatchCommand.Text = settings.BatchCommand;
            txtAfterBatchArgument.Text = settings.BatchArgument;
            chkAfterKillReloggers.Checked = settings.KillReloggers;
            chkAfterClearAssembliesFolder.Checked = settings.ClearAssembliesFolder;
            chkAfterClearCacheFolder.Checked = settings.ClearCacheFolder;


            //Rempli les champs du tab Logging
            cmbColorLogs.SelectedItem = settings.ColorLogs;
            chkLoggingTime.Checked = settings.LoggingTime;
            numLoggingTime.Value = settings.LoggingTimeEvery;
            chkLoggingBeepWhenStuck.Checked = settings.LoggingBeepWhenStuck;

            //Rempli les champs du tab Exceptions
            chkExceptionBG.Checked = settings.ExceptionBg;
            chkExceptionInstance.Checked = settings.ExceptionInstance;
            chkExceptionMailbox.Checked = settings.ExceptionMailbox;
            chkExceptionCountDeathsBG.Checked = settings.ExceptionCountDeathsBg;
            chkExceptionsMembersPartyRaid.Checked = settings.ExceptionMembersPartyRaid;
            chkExceptionDontLogOut.Checked = settings.ExceptionDontLogOut;
            chkExceptionDontRunToCorpse.Checked = settings.ExceptionDontRunToCorpse;
            chkExceptionDeserterDebuff.Checked = settings.ExceptionDeserterDebuff;
        }

        /// <summary>
        /// Sauvegarde toutes les variables de la fenêtre dans un fichier XML.
        /// </summary>
        private void SaveSettings()
        {
            //Crée un pointeur sur l'instance des données sauvegardées
            var settings = LogMeOutSettings.Instance;
            //Sauvegarde les champs du tab Triggers
            settings.AlertOnBagsFull = chkBagsFull.Checked;
            settings.AlertOnTimeElapsed = chkTimeElapsed.Checked;
            settings.HoursElapsed = (int)numHoursElapsed.Value;
            settings.MinutesElapsed = (int)numMinutesElapsed.Value;
            settings.AlertOnDeaths = chkDeaths.Checked;
            settings.NbDeaths = (int)numDeaths.Value;
            settings.AlertOnStucks = chkStucks.Checked;
            settings.NbStucks = (int)numStucks.Value;
            settings.AlertOnTeleportation = chkTeleported.Checked;
            settings.AlertOnAchievement = chkAchievement.Checked;
            settings.AchievementId = (int)numAchievement.Value;
            settings.TeleportYards = (int)numTeleported.Value;
            settings.AlertOnMinutesInCombat = chkMinutesInCombat.Checked;
            settings.NbMinutesInCombat = (int)numMinutesInCombat.Value;
            settings.AlertOnMobsKilled = chkMobsKilled.Checked;
            settings.NbMobsKilled = (int)numMobsKilled.Value;
            settings.AlertOnLootedItem = chkLootedItem.Checked;
            settings.LootedItemTimes = (int) numLootedItemTimes.Value;
            settings.LootedItemId = (int) numLootedItemId.Value;
            settings.AlertOnCannotLoot = chkCannotLoot.Checked;
            settings.AlertOnDurability = chkDurabilityCheck.Checked;
            settings.DurabilityPercent = (int)numDurabilityPercent.Value;
            settings.AlertOnWhispersReceived = chkWhispersReceived.Checked;
            settings.NbWhispersReceived = (int)numWhispesReceived.Value;
            settings.AlertOnWhisperGm = chkWhisperGm.Checked;
            settings.AlertOnPoints = chkPoints.Checked;
            settings.NbPoints = (int)numPoints.Value;
            settings.TypePoints = cmbPoints.SelectedIndex;
            settings.AlertOnLevelReached = chkLevelReached.Checked;
            settings.AlertOnGotAura = chkGotAura.Checked;
            settings.AlertOnTrainingSkill = chkTrainingSkill.Checked;
            settings.AlertOnInactivity = chkInactivity.Checked;
            settings.InactivityTimeout = (int) numInactivityTimeout.Value;
            settings.TrainingSkillLevel = (int)numTrainingSkillLevel.Value;
            if(cmbTrainingSkillLine.Items.Count != 0)
                settings.TrainingSkillLine = (SkillLine)cmbTrainingSkillLine.SelectedItem;
            settings.GotAuraName = txtGotAuraName.Text;
            settings.NbLevel = (int)numLevelReached.Value;
            settings.AlertOnPlayerFollows = chkPlayerFollows.Checked;
            settings.MinutesPlayerFollows = (int)numPlayerFollows.Value;
            settings.AlertOnPlayerTargets = chkPlayerTargets.Checked;
            settings.MinutesPlayerTargets = (int)numPlayerTargets.Value;
            settings.BeepWhenFire = chkBeepWhenFire.Checked;

            //Sauvegarde les champs du tab Action Before
            if (radBeforeNothing.Checked)
                settings.ActionBefore = 0;
            else if (radBeforeHearthstone.Checked)
                settings.ActionBefore = 1;
            else if (radBeforeSpell.Checked)
                settings.ActionBefore = 2;
            else if (radBeforeItem.Checked)
                settings.ActionBefore = 3;
            else if (radBeforeLua.Checked)
                settings.ActionBefore = 4;
            settings.SpellName = txtSpellName.Text;
            settings.ItemId = (int)numItemID.Value;
            settings.LuaString = txtLuaString.Text;
            settings.LuaWaitingTime = (int)numLuaWaitingTime.Value;

            //Sauvegarde les champs Action After
            if (radAfterNothing.Checked)
                settings.ActionAfter = 0;
            else if (radAfterShutdown.Checked)
                settings.ActionAfter = 1;
            else if (radAfterBatchLigne.Checked)
                settings.ActionAfter = 2;
            settings.BatchCommand = txtAfterBatchCommand.Text;
            settings.BatchArgument = txtAfterBatchArgument.Text;
            settings.KillReloggers = chkAfterKillReloggers.Checked;
            settings.ClearAssembliesFolder = chkAfterClearAssembliesFolder.Checked;
            settings.ClearCacheFolder = chkAfterClearCacheFolder.Checked;

            //Sauvegarde les champs du tab Logging
            settings.ColorLogs = cmbColorLogs.SelectedItem.ToString();
            settings.LoggingTime = chkLoggingTime.Checked;
            settings.LoggingTimeEvery = (int)numLoggingTime.Value;
            settings.LoggingBeepWhenStuck = chkLoggingBeepWhenStuck.Checked;

            //Sauvegarde les champs du tab Exceptions
            settings.ExceptionBg = chkExceptionBG.Checked;
            settings.ExceptionInstance = chkExceptionInstance.Checked;
            settings.ExceptionMailbox = chkExceptionMailbox.Checked;
            settings.ExceptionCountDeathsBg = chkExceptionCountDeathsBG.Checked;
            settings.ExceptionMembersPartyRaid = chkExceptionsMembersPartyRaid.Checked;
            settings.ExceptionDontLogOut = chkExceptionDontLogOut.Checked;
            settings.ExceptionDontRunToCorpse = chkExceptionDontRunToCorpse.Checked;
            settings.ExceptionDeserterDebuff = chkExceptionDeserterDebuff.Checked;

            //Sauvegarde les données
            settings.Save();
        }

        private void ChkHoursElapsedCheckedChanged(object sender, EventArgs e)
        {
            numHoursElapsed.Enabled = chkTimeElapsed.Checked;
            numMinutesElapsed.Enabled = chkTimeElapsed.Checked;
        }

        private void ChkDeathsCheckedChanged(object sender, EventArgs e)
        {
            numDeaths.Enabled = chkDeaths.Checked;
        }

        private void ChkStucksCheckedChanged(object sender, EventArgs e)
        {
            numStucks.Enabled = chkStucks.Checked;
        }

        private void ChkMinutesInCombatCheckedChanged(object sender, EventArgs e)
        {
            numMinutesInCombat.Enabled = chkMinutesInCombat.Checked;
        }

        private void ChkMobsKilledCheckedChanged(object sender, EventArgs e)
        {
            numMobsKilled.Enabled = chkMobsKilled.Checked;
        }

        private void ChkPointsCheckedChanged(object sender, EventArgs e)
        {
            numPoints.Enabled = chkPoints.Checked;
            cmbPoints.Enabled = chkPoints.Checked;
        }

        private void ChkLevelReachedCheckedChanged(object sender, EventArgs e)
        {
            numLevelReached.Enabled = chkLevelReached.Checked;
        }

        private void ChkGotAuraCheckedChanged(object sender, EventArgs e)
        {
            txtGotAuraName.Enabled = chkGotAura.Checked;
        }

        private void RadSpellCheckedChanged(object sender, EventArgs e)
        {
            txtSpellName.Enabled = radBeforeSpell.Checked;
        }

        private void RadItemCheckedChanged(object sender, EventArgs e)
        {
            numItemID.Enabled = radBeforeItem.Checked;
        }

        private void ChkLoggingHoursCheckedChanged(object sender, EventArgs e)
        {
            numLoggingTime.Enabled = chkLoggingTime.Checked;
        }

        private void RadAfterBatchLigneCheckedChanged(object sender, EventArgs e)
        {
            txtAfterBatchCommand.Enabled = radAfterBatchLigne.Checked;
            txtAfterBatchArgument.Enabled = radAfterBatchLigne.Checked;
        }

        private void ChkWhispesReceivedCheckedChanged(object sender, EventArgs e)
        {
            numWhispesReceived.Enabled = chkWhispersReceived.Checked;
        }

        private void ChkPlayerFollowsCheckedChanged(object sender, EventArgs e)
        {
            numPlayerFollows.Enabled = chkPlayerFollows.Checked;
        }

        private void ChkLootedItemCheckedChanged(object sender, EventArgs e)
        {
            numLootedItemTimes.Enabled = chkLootedItem.Checked;
            numLootedItemId.Enabled = chkLootedItem.Checked;
        }

        private void ChkTrainingSkillCheckedChanged(object sender, EventArgs e)
        {
            numTrainingSkillLevel.Enabled = chkTrainingSkill.Checked;
            cmbTrainingSkillLine.Enabled = chkTrainingSkill.Checked;
        }

        private void ChkDurabilityCheckCheckedChanged(object sender, EventArgs e)
        {
            numDurabilityPercent.Enabled = chkDurabilityCheck.Checked;
        }

        private void ChkInactivityCheckedChanged(object sender, EventArgs e)
        {
            numInactivityTimeout.Enabled = chkInactivity.Checked;
        }

        private void CheckBox1CheckedChanged(object sender, EventArgs e)
        {
            numPlayerTargets.Enabled = chkPlayerTargets.Checked;
        }

        private void ChkTeleportedCheckedChanged(object sender, EventArgs e)
        {
            numTeleported.Enabled = chkTeleported.Checked;
        }

        private void ChkAchievementCheckedChanged(object sender, EventArgs e)
        {
            numAchievement.Enabled = chkAchievement.Checked;
        }

        private void TxtSpellNameEnter(object sender, EventArgs e)
        {
            //Si le contrôle contient le texte d'informations, on le vide
            if (txtSpellName.ForeColor != System.Drawing.Color.Black)
            {
                txtSpellName.Text = "";
                txtSpellName.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void TxtSpellNameTextChanged(object sender, EventArgs e)
        {
            //Si le champ est vide, on remet le texte d'informations
            if (txtSpellName.Text == "Enter the spell name (in english) here")
                txtSpellName.ForeColor = System.Drawing.Color.Gray;
        }

        private void TxtSpellNameLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSpellName.Text))
                txtSpellName.Text = "Enter the spell name (in english) here";
        }


        private void TxtLuaStringEnter(object sender, EventArgs e)
        {
            //Si le contrôle contient le texte d'informations, on le vide
            if (txtLuaString.ForeColor != System.Drawing.Color.Black)
            {
                txtLuaString.Text = "";
                txtLuaString.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void TxtLuaStringTextChanged(object sender, EventArgs e)
        {
            //Si le champ est vide, on remet le texte d'informations
            if (txtLuaString.Text == "Enter the lua string to run here")
                txtLuaString.ForeColor = System.Drawing.Color.Gray;
        }

        private void TxtLuaStringLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLuaString.Text))
                txtLuaString.Text = "Enter the lua string to run here";
        }


        private void RadBeforeLuaCheckedChanged(object sender, EventArgs e)
        {
            txtLuaString.Enabled = radBeforeLua.Checked;
            numLuaWaitingTime.Enabled = radBeforeLua.Checked;
        }

        private void BtnShowTimerClick(object sender, EventArgs e)
        {
            //Création de la fenêtre timer
            var frmTimerWindow = new FrmTimer();
            frmTimerWindow.Show();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            //Ferme la fenêtre
            Close();
        }

        private void BtnSaveAndCloseClick(object sender, EventArgs e)
        {
            //Sauvegarde les données de la fenêtre
            SaveSettings();
            //Ferme la fenêtre
            Close();
        }

        private void BtnUpdateWebsiteClick(object sender, EventArgs e)
        {
            Process.Start(Classes.Updater.StrWebSite);
        }

        private void BtnUpdateUpdateClick(object sender, EventArgs e)
        {
            //Désactive le bouton durant la mise à jour
            btnUpdateUpdate.Enabled = false;
            btnUpdateUpdate.Text = "Updating...";
            //Rafraichit la fenêtre
            Application.DoEvents();
            //Effectue la mise à jour
            if (Classes.Updater.InstallLatestVersion())
                btnUpdateUpdate.Text = "Updated !";
            else
            {
                btnUpdateUpdate.Enabled = true;
                btnUpdateUpdate.Text = "Update !";
            }
        }
    }
}
