#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-08-30 20:20:43 +0200 (Fr, 30 Aug 2013) $
 * $ID$
 * $Revision: 1705 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Paladin/Retribution.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

using System.Linq;
using System.Collections.Generic;
using CommonBehaviors.Actions;
using JetBrains.Annotations;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.TreeSharp;
using Lua = PureRotation.Helpers.Lua;

namespace PureRotation.Classes.Paladin
{
    [UsedImplicitly]
    public class Retribution : RotationBase
    {
        private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }
        private static IEnumerable<Styx.WoWInternals.WoWObjects.WoWUnit> attackers = null;
        private static PaladinSettings PaladinSettings { get { return PRSettings.Instance.Paladin; } }

        private Composite HandleCooldowns()
        {
            //SPELLID :: 84963 (Inquisition)
            return new PrioritySelector(
                new Decorator(ret=>Me.CurrentTarget!=null && Me.CurrentTarget.IsBoss && Me.CurrentTarget.IsWithinMeleeRange,
                    new PrioritySelector(Spell.Cast("Guardian of Ancient Kings", ret => Me.HasAura("Avenging Wrath")),
                Spell.Cast("Avenging Wrath", ret => Me.HasAura(84963)),
                Spell.Cast("Holy Avenger", ret => Me.HasAura("Avenging Wrath")))),
                Spell.Cast("Lifeblood", ret => !Me.HasAnyAura(Spell.HeroismBuff)));
        }

        private Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                Spell.PreventDoubleCast("Seal of Truth", ThrottleTime, ret => PaladinSettings.UseSealSwapping && !Me.HasAura("Seal of Truth") && attackers != null && attackers.Count() < PaladinSettings.SealofRighteousnessCount),
                Spell.Cast("Inquisition", ret => Me.CurrentHolyPower >= 3 && (!Me.HasAura(84963) || Spell.GetAuraTimeLeft("Inquisition") < 2)),
                Spell.Cast("Templar's Verdict", ret => (Me.CurrentHolyPower >= 4 || Me.HasAura(86172))),
                Spell.Cast("Hammer of Wrath", ret => Me.CurrentTarget.HealthPercent <= 20 || Me.HasAura("Avenging Wrath")),
                Spell.Cast("Exorcism", ret => Me.HasAura(59578) || !Spell.SpellOnCooldown("Exorcism")),
                Spell.Cast("Crusader Strike"),
                Spell.Cast("Judgment"),
                Spell.Cast("Templar's Verdict", ret => Me.CurrentHolyPower >= 3),
                Spell.Cast("Execution Sentence", ret => TalentManager.HasTalent(18) && Me.HasAura(84963)),
                Spell.CastOnGround("Light's Hammer", ret => Me.CurrentTarget.Location, ret => TalentManager.HasTalent(17) && attackers != null && attackers.Count() >= PaladinSettings.LightsHammerCount && Me.HasAura("Inquisition")),
                Spell.PreventDoubleCast("Seal of Truth", ThrottleTime, ret => PaladinSettings.UseSealSwapping && !Me.HasAura("Seal of Truth") && !Me.HasAura("Seal of Righteousness")));
        }

        private Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                Spell.PreventDoubleCast("Seal of Righteousness", ThrottleTime, ret => PaladinSettings.UseSealSwapping && !Me.HasAura("Seal of Righteousness") && attackers!=null && attackers.Count() >= PaladinSettings.SealofRighteousnessCount),
                Spell.PreventDoubleCast("Seal of Truth", ThrottleTime, ret => PaladinSettings.UseSealSwapping && !Me.HasAura("Seal of Truth") && attackers != null && attackers.Count() < PaladinSettings.SealofRighteousnessCount),
                Spell.Cast("Inquisition", ret => Me.CurrentHolyPower >= 3 && (!Me.HasAura(84963) || Spell.GetAuraTimeLeft("Inquisition") < 2)),
                Spell.Cast("Divine Storm", ret => (Me.CurrentHolyPower >= 3 || Me.HasAura(86172)) && attackers != null && attackers.Count() >= 2),
                Spell.Cast("Exorcism", ret => Me.HasAura(59578) || !Spell.SpellOnCooldown("Exorcism")),
                Spell.Cast("Hammer of the Righteous"),
                Spell.Cast("Judgment"),
                Spell.CastOnGround("Light's Hammer", ret => Me.CurrentTarget.Location, ret => TalentManager.HasTalent(17) && attackers != null && attackers.Count() >= PaladinSettings.LightsHammerCount && Me.HasAura("Inquisition")),
                Spell.Cast("Hammer of Wrath", ret => Me.CurrentTarget.HealthPercent <= 20 || Me.HasAura("Avenging Wrath")));
        }

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1705 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.PaladinRetribution; }
        }

        public override string Name
        {
            get { return "Retribution Paladin"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    Racials.UseRacials(),
                    new Decorator(ret => Me.HealthPercent < 100 && PRSettings.Instance.EnableSelfHealing, HandleDefensiveCooldowns()),
                    Item.HandleItems(), // Pop Trinkets, Drink potions...
                    new Action(ret=> { attackers=Unit.NearbyAttackableUnits(Me.Location,15f); return RunStatus.Failure;}),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns, HandleCooldowns()),
                                      Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                                      new Decorator(ret => PRSettings.Instance.UseAoEAbilities && attackers!=null && attackers.Count() >= PRSettings.Instance.Paladin.RetAoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleCooldowns()),
                                      Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                                      new Decorator(ret => PRSettings.Instance.UseAoEAbilities && attackers != null && attackers.Count() >= PRSettings.Instance.Paladin.RetAoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleCooldowns()),
                                      Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                      new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTarget()))));
            }
        }

        public override Composite PVPRotation
        {
            get { return PVERotation; }
        }

        public override Composite Medic
        {
            get
            {
                return new PrioritySelector(
                    Spell.PreventDoubleCast("Seal of Truth", ThrottleTime, ret => PaladinSettings.UseSealSwapping && !Me.HasAura("Seal of Truth") && !Me.HasAura("Seal of Righteousness"))
                    );
            }
        }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Lay on Hands", on => Me, ret=>Me.HealthPercent<=PaladinSettings.LayOnHandsPercent),
                Spell.Cast("Word of Glory", on => Me, ret => Me.HealthPercent <= PaladinSettings.WordGloryPercent),
                Spell.Cast("Flash of Light", on => Me, ret => Me.HealthPercent <= PaladinSettings.FlashLightPercent && Spell.GetAuraStackCount(114250) == 3),
                Spell.Cast("Sacred Shield", on => Me, ret => Me.HealthPercent <= PaladinSettings.SacredShieldPercent)
                );
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(ret => !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    new PrioritySelector(
                        Spell.PreventDoubleCast("Seal of Truth", ThrottleTime, ret => !Me.HasAura("Seal of Truth")),
                        Spell.Cast("Blessing of Kings", ret => PaladinSettings.BlessingSelection == PaladinBlessing.Kings && !StyxWoW.Me.HasAura("Blessing of Kings")), //todo: hasanyaura
                        Spell.Cast("Blessing of Might", ret => PaladinSettings.BlessingSelection == PaladinBlessing.Might && !StyxWoW.Me.HasAura("Blessing of Might")))); //todo: hasanyaura
            }
        }

        #endregion Overrides of RotationBase
    }
}