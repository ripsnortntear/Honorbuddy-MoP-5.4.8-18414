using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Styx.Common;

namespace RichieHolyPriestPvP
{
    public partial class UI : Form
    {
        public UI()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var setting = HolySettings.Instance;

            #region First Column

            #region Direct Heal

            UseGuardianSpiritBelowHealth.Value = setting.UseGuardianSpiritBelowHealth;
            PowerWordShieldHp.Value = setting.PowerWordShieldHp;
            RenewHp.Value = setting.RenewHp;
            PrayerOfMendingHp.Value = setting.PrayerOfMendingHp;
            InstantFlashHealBelowHP.Value = setting.InstantFlashHealBelowHP;
            FlashHealHp.Value = setting.FlashHealHp;
            CircleOfHealingBelowHealth.Value = setting.CircleOfHealingBelowHealth;
            DivineStarHP.Value = setting.DivineStarHP;
            MinInjuredCountToCastCascadeHalo.Value = setting.MinInjuredCountToCastCascadeHalo;
            MinInjuredCountToCastAOEHeal.Value = setting.MinInjuredCountToCastAOEHeal;
            UseBindingHealIfMyHP.Value = setting.UseBindingHealIfMyHP;

            #endregion

            UseLeapOfFaitHP.Value = setting.UseLeapOfFaitHP;
            HealthstonePercent.Value = setting.HealthstonePercent;
            VoidShiftBelowHp.Value = setting.VoidShiftBelowHp;
            DefCDsBelow.Value = setting.DefCDsBelow;
            UseDamageSpellsIfHealTargetHP.Value = setting.UseDamageSpellsIfHealTargetHP;
            UseShadowFiendIfMana.Value = setting.UseShadowFiendIfMana;
            PurifyWhenLowPrioDebuffCount.Value = setting.PurifyWhenLowPrioDebuffCount;
            PolyReactLatencyModifier.Value = setting.PolyReactLatencyModifier;
            CastLevitateAfterMs.Value = setting.CastLevitateAfterMs;
            SearchInterval.Value = setting.SearchInterval;
            DispelDelay.Value = setting.DispelDelay;
            AngelicFeatherDelay.Value = setting.AngelicFeatherDelay;
            InterruptSpellCastsAfter.Value = setting.InterruptSpellCastsAfter;
            UseDispelMagicHP.Value = setting.UseDispelMagicHP;
            BlanketAboveHP.Value = setting.BlanketAboveHP;
            PreferredBuff.SelectedIndex = setting.PreferredBuff;

            #endregion

            #region Secound Column

            UseTrinket.Checked = setting.UseTrinket;
            upperTrinketSlot.Checked = setting.TrinketSlotNumber == 13;
            lowerTrinketSlot.Checked = setting.TrinketSlotNumber != 13;
            UsePowerInfusion.Checked = setting.UsePowerInfusion;

            #region Defensive CDs

            DisableAllDefCDs.Checked = setting.DisableAllDefCDs;
            UseVoidTendrils.Checked = setting.UseVoidTendrils;
            UsePsychicScream.Checked = setting.UsePsychicScream;
            UsePsyfiend.Checked = setting.UsePsyfiend;
            UseDespareatePrayer.Checked = setting.UseDespareatePrayer;
            UseSpectralGuise.Checked = setting.UseSpectralGuise;
            UseHolyWordChackra.Checked = setting.UseHolyWordChackra;

            #endregion

            UseVoidShiftToHeal.Checked = setting.UseVoidShiftToHeal;
            UseVoidShiftToHealMe.Checked = setting.UseVoidShiftToHealMe;
            LeapOfFaithOnScatteredTeammate.Checked = setting.LeapOfFaithOnScatteredTeammate;
            CastLevitate.Checked = setting.CastLevitate;
            InterruptFocus.Checked = setting.InterruptFocus;
            AutoFace.Checked = setting.AutoFace;
            FadeAuto.Checked = setting.FadeAuto;
            AutoMassDispel.Checked = setting.AutoMassDispel;
            UseRacial.Checked = setting.UseRacial;
            RightClickMovementOff.Checked = setting.RightClickMovementOff;
            DontCancelDominateMind.Checked = setting.DontCancelDominateMind;
            DispelLowPrio.Checked = setting.DispelLowPrio;
            HealPets.Checked = setting.HealPets;
            PowerWordFortitude.Checked = setting.PowerWordFortitude;
            FearWardSelf.Checked = setting.FearWardSelf;
            StopCastOnInterrupt.Checked = setting.StopCastOnInterrupt;
            CounteractPoly.Checked = setting.CounteractPoly;
            UseShadowFiend.Checked = setting.UseShadowFiend;
            UseBlanketOnlyInArena.Checked = setting.UseBlanketOnlyInArena;
            UseAngelicFeather.Checked = setting.UseAngelicFeather;
            CastFailedUserInitiatedSpell.Checked = setting.CastFailedUserInitiatedSpell;
            CollectStatistics.Checked = setting.CollectStatistics;

            #endregion

            OverrideControls();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void defaults_Click(object sender, EventArgs e)
        {
            HolySettings.Instance.InitializeDefaultValues();
            Form1_Load(sender, e);
        }

        private void save_Click(object sender, EventArgs e)
        {
            OverrideControls();

            var setting = HolySettings.Instance;

            #region First Column

            #region Direct Heal

            setting.UseGuardianSpiritBelowHealth = (int)UseGuardianSpiritBelowHealth.Value;
            setting.PowerWordShieldHp = (int)PowerWordShieldHp.Value;
            setting.RenewHp = (int)RenewHp.Value;
            setting.PrayerOfMendingHp = (int)PrayerOfMendingHp.Value;
            setting.InstantFlashHealBelowHP = (int)InstantFlashHealBelowHP.Value;
            setting.FlashHealHp = (int)FlashHealHp.Value;
            setting.CircleOfHealingBelowHealth = (int)CircleOfHealingBelowHealth.Value;
            setting.DivineStarHP = (int)DivineStarHP.Value;
            setting.MinInjuredCountToCastCascadeHalo = (int)MinInjuredCountToCastCascadeHalo.Value;
            setting.MinInjuredCountToCastAOEHeal = (int)MinInjuredCountToCastAOEHeal.Value;
            setting.UseBindingHealIfMyHP = (int)UseBindingHealIfMyHP.Value;

            #endregion

            setting.UseLeapOfFaitHP = (int)UseLeapOfFaitHP.Value;
            setting.HealthstonePercent = (int)HealthstonePercent.Value;
            setting.VoidShiftBelowHp = (int)VoidShiftBelowHp.Value;
            setting.DefCDsBelow = (int)DefCDsBelow.Value;
            setting.UseDamageSpellsIfHealTargetHP = (int)UseDamageSpellsIfHealTargetHP.Value;
            setting.UseShadowFiendIfMana = (int)UseShadowFiendIfMana.Value;
            setting.PurifyWhenLowPrioDebuffCount = (int)PurifyWhenLowPrioDebuffCount.Value;
            setting.PolyReactLatencyModifier = (int)PolyReactLatencyModifier.Value;
            setting.CastLevitateAfterMs = (int)CastLevitateAfterMs.Value;
            setting.SearchInterval = (int)SearchInterval.Value;
            setting.DispelDelay = (int)DispelDelay.Value;
            setting.AngelicFeatherDelay = (int)AngelicFeatherDelay.Value;
            setting.InterruptSpellCastsAfter = (int)InterruptSpellCastsAfter.Value;
            setting.UseDispelMagicHP = (int)UseDispelMagicHP.Value;
            setting.BlanketAboveHP = (int)BlanketAboveHP.Value;
            setting.PreferredBuff = PreferredBuff.SelectedIndex;

            #endregion

            #region Secound Column

            setting.UseTrinket = UseTrinket.Checked;
            setting.TrinketSlotNumber = upperTrinketSlot.Checked ? 13 : 14;
            setting.UsePowerInfusion = UsePowerInfusion.Checked;

            #region Defensive CDs

            setting.DisableAllDefCDs = DisableAllDefCDs.Checked;
            setting.UseVoidTendrils = UseVoidTendrils.Checked;
            setting.UsePsychicScream = UsePsychicScream.Checked;
            setting.UsePsyfiend = UsePsyfiend.Checked;
            setting.UseDespareatePrayer = UseDespareatePrayer.Checked;
            setting.UseSpectralGuise = UseSpectralGuise.Checked;
            setting.UseHolyWordChackra = UseHolyWordChackra.Checked;

            #endregion

            setting.UseVoidShiftToHeal = UseVoidShiftToHeal.Checked;
            setting.UseVoidShiftToHealMe = UseVoidShiftToHealMe.Checked;
            setting.LeapOfFaithOnScatteredTeammate = LeapOfFaithOnScatteredTeammate.Checked;
            setting.CastLevitate = CastLevitate.Checked;
            setting.InterruptFocus = InterruptFocus.Checked;
            setting.AutoFace = AutoFace.Checked;
            setting.FadeAuto = FadeAuto.Checked;
            setting.AutoMassDispel = AutoMassDispel.Checked;
            setting.UseRacial = UseRacial.Checked;
            setting.RightClickMovementOff = RightClickMovementOff.Checked;
            setting.DontCancelDominateMind = DontCancelDominateMind.Checked;
            setting.DispelLowPrio = DispelLowPrio.Checked;
            setting.HealPets = HealPets.Checked;
            setting.PowerWordFortitude = PowerWordFortitude.Checked;
            setting.FearWardSelf = FearWardSelf.Checked;
            setting.StopCastOnInterrupt = StopCastOnInterrupt.Checked;
            setting.CounteractPoly = CounteractPoly.Checked;
            setting.UseShadowFiend = UseShadowFiend.Checked;
            setting.UseBlanketOnlyInArena = UseBlanketOnlyInArena.Checked;
            setting.UseAngelicFeather = UseAngelicFeather.Checked;
            setting.CastFailedUserInitiatedSpell = CastFailedUserInitiatedSpell.Checked;
            setting.CollectStatistics = CollectStatistics.Checked;

            #endregion

            if (HolySettings.Instance.RightClickMovementOff && Styx.CommonBot.TreeRoot.Current.Name != "BGBuddy")
                Styx.WoWInternals.Lua.DoString("RunMacroText('/run ConsoleExec(\"Autointeract 0\")');");
            else
                Styx.WoWInternals.Lua.DoString("RunMacroText('/run ConsoleExec(\"Autointeract 1\")');");

            HolySettings.Instance.Save();
            Logging.Write("----------------------------------");
            Logging.Write("Your settings have been saved");
            HolySettings.Print();

            this.Close();
        }

        private void OverrideControls()
        {
            if (Main.IsProVersion)
            {
                DisabledFeatures.Visible = false;
            }
            else
            {
                HealthstonePercent.Enabled = false;
                DefensiveCDs.Enabled = false;
                UseDamageSpellsIfHealTargetHP.Enabled = false;
                PolyReactLatencyModifier.Enabled = false;
                DispelDelay.Enabled = false;
                InterruptSpellCastsAfter.Enabled = false;
                UseDispelMagicHP.Enabled = false;
                BlanketAboveHP.Enabled = false;
                UseVoidShiftToHealMe.Enabled = false;
                LeapOfFaithOnScatteredTeammate.Enabled = false;
                InterruptFocus.Enabled = false;
                AutoMassDispel.Enabled = false;
                RightClickMovementOff.Enabled = false;
                CounteractPoly.Enabled = false;
                UseBlanketOnlyInArena.Enabled = false;
                CastFailedUserInitiatedSpell.Enabled = false;

                DisabledFeatures.Visible = true;
            }
        }
    }
}