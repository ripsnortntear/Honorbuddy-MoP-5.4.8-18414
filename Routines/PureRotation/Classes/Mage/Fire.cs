#region Revision info
/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Mage/Fire.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade: $
 */
#endregion

using System.Linq;
using CommonBehaviors.Actions;
using JetBrains.Annotations;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;

namespace PureRotation.Classes.Mage
{
    [UsedImplicitly]
    class Fire : RotationBase
    {
        //private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }
        internal static int IgnitePower;
        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Common.HandleCommonDefensiveCooldowns());
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                CooldownTracker.Cast("Combustion", ret => MageSettings.CombustionType == SpellUsage.OnCooldown && Me.CurrentTarget != null && Spell.GetMyAuraTimeLeft("Ignite", Me.CurrentTarget) > 0 && Spell.GetMyAuraTimeLeft("Pyroblast", Me.CurrentTarget) > 0),
                CooldownTracker.Cast("Combustion", ret => MageSettings.CombustionType == SpellUsage.OnCondition && Spell.GetMyAuraTimeLeft("Ignite", Me.CurrentTarget) > 0 && IgnitePower > MageSettings.IgniteCombustion),
                CooldownTracker.Cast("Combustion", ret => MageSettings.CombustionType == SpellUsage.PoolWithCooldowns && NeedCombustion),
                CooldownTracker.Cast("Presence of Mind", ret => NeedPresenceofMind),
                CooldownTracker.PreventDoubleCast("Alter Time", 2, ret => NeedAlterTimeNoPoM || NeedAlterTimePoM),
                CooldownTracker.Cast("Alter Time", ret => NeedForceExpireAlterTimeNoPoM || NeedForceExpireAlterTimePoM),
                Common.HandleCommonOffensiveCooldowns()
                );
        }

        private static Composite HandleSingleTarget()
        {

            return new PrioritySelector(
                Common.HandleSingleTargetMageBombs(),
                Spell.Cast("Pyroblast", ret => NeedPyroBlast),
                Spell.Cast("Inferno Blast", ret => NeedInfernoBlast),
                new Decorator(ret => MageSettings.MultiDoTSingleTarget, Common.HandleBombMultiDoT()),
                Spell.Cast("Fireball", ret => !Me.IsMoving()),
                Spell.PreventDoubleCast("Scorch", 0.1, on => Me.CurrentTarget, ret => Me.IsMoving(), true));
        }

        private static Composite HandleSingleTargetAoE()
        {

            return new PrioritySelector(
                Spell.Cast("Pyroblast", ret => NeedPyroBlast),
                Spell.Cast("Inferno Blast", ret => NeedInfernoBlast),
                //Spell.Cast("Fireball", ret => !Me.IsMoving()), // too slow for multidotting just use scorch to build Heating Up.
                Spell.PreventDoubleCast("Scorch", 0.1, on => Me.CurrentTarget, ret => true, true));
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                Common.HandleBombMultiDoT(),
                Spell.Cast("Inferno Blast", ret => TheMoonHasAligned),
                Spell.CastOnGround("Flamestrike", loc => Me.CurrentTarget.Location, ret => Me.ManaPercent > 30),
                Spell.Cast("Dragon's Breath", ret => NeedDragonsBreath),
                HandleSingleTargetAoE()
                );
        }
		
		//Thanks to KingWoW CC - but we need it supporting Multi-Language
        internal static int IgniteValue()
        {

            string _s = "local n=GetSpellInfo(12654);return select(15,UnitDebuff(\"target\", n, nil, \"PLAYER\"))";
            int IgnitePower = 0;
            try
            {
                using (StyxWoW.Memory.AcquireFrame())
                {
                    IgnitePower = Styx.WoWInternals.Lua.GetReturnVal<int>(_s, 0);
                }
                return IgnitePower;
            }
            catch
            {
                //will return 0 on error, should be fine so far, except that we will never be able to cast Combustion
                return IgnitePower;
            }
        }
        private static Composite SetCachedVars
        {
            get
            {
                return new Action(delegate
                {
                    IgnitePower = IgniteValue();
                    return RunStatus.Failure;
                });

            }
        }
        #region booleans
        private static bool TheMoonHasAligned { get { return Me.CurrentTarget != null && Me.CurrentTarget.HasAllMyAuras("Ignite", "Pyroblast") && !Me.ActiveAuras.ContainsKey("Pyroblast!"); } }
        private static bool NeedInfernoBlast { get { return Me.HasAura("Heating Up") && !Me.HasAura("Pyroblast!"); } }

        private static bool Pyroblast { get { return Me.ActiveAuras.ContainsKey("Pyroblast!"); } }
        private static bool AlterTime { get { return UseAlterTime && Me.ActiveAuras.ContainsKey("Alter Time"); } }
        private static bool PoM { get { return Me.HasAura("Presence of Mind"); } }
        
        //private static bool NeedCombustionNoPoM { get { return UseCombustion && !TalentManager.HasTalent(1) && Me.CurrentTarget.HasAura("Ignite") && CooldownTracker.SpellOnCooldown("Alter Time") && !Me.ActiveAuras.ContainsKey("Pyroblast!") && Spell.Lastspellcast == "Pyroblast"; } }
        //private static bool NeedCombustionPoM { get { return UseCombustion && TalentManager.HasTalent(1) && Me.CurrentTarget.HasAura("Ignite") && CooldownTracker.SpellOnCooldown("Presence of Mind") && CooldownTracker.SpellOnCooldown("Alter Time") && !Me.ActiveAuras.ContainsKey("Pyroblast!") && !Me.HasAura("Presence of Mind") && Spell.Lastspellcast == "Pyroblast"; } }
        private static bool NeedCombustion { get { return Me.CurrentTarget.HasAura("Ignite") && !Pyroblast && Spell.Lastspellcast == "Pyroblast" && !Me.HasAnyAura("Alter Time", "Presence of Mind"); } }
        
        private static bool NeedDragonsBreath { get { return (!Me.ActiveAuras.ContainsKey("Heating Up") && !Pyroblast && Me.IsSafelyFacing(Me.CurrentTarget) && Me.CurrentTarget.Distance <= 8); } }
        private static bool NeedAlterTimeNoPoM { get { return !TalentManager.HasTalent(1) && Me.HasAnyAura(116257, 116014) && Pyroblast && !AlterTime; } }
        private static bool NeedAlterTimePoM { get { return TalentManager.HasTalent(1) && Me.HasAnyAura(116257, 116014) && Pyroblast && PoM && !AlterTime; } }
        private static bool NeedForceExpireAlterTimePoM { get { return TalentManager.HasTalent(1) /*&& Me.HasAnyAura("Invoker's Energy", "Rune of Power")*/ && !Pyroblast && !PoM && AlterTime; } }
        private static bool NeedForceExpireAlterTimeNoPoM { get { return !TalentManager.HasTalent(1) /*&& Me.HasAnyAura("Invoker's Energy", "Rune of Power")*/ && !Pyroblast && AlterTime; } }
        private static bool NeedPresenceofMind { get { return UsePresenceofMind && Pyroblast && Me.HasAnyAura(116257, 116014) && !CooldownTracker.SpellOnCooldown("Alter Time"); } }
        private static bool NeedPyroBlast { get { return PoM || Pyroblast; } }
        #endregion

        #region Settings

        private static MageSettings MageSettings { get { return PRSettings.Instance.Mage; } }
        private static bool UseAlterTime { get { return MageSettings.EnableAlterTime; } }
        private static bool UsePresenceofMind { get { return MageSettings.EnablePresenceofMind; } }

        #endregion

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.MageFire; }
        }

        public override string Name
        {
            get { return "Fire Mage"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => (MageSettings.EnableLagFix && SpellManager.GlobalCooldown) || HotKeyManager.IsPaused || Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    HandleDefensiveCooldowns(), // Stay Alive! 
                    Racials.UseRacials(),
                    Item.HandleItems(),
                    Common.HandleCommon(),
                    CachedUnits.Pulse,
                    SetCachedVars,
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget != null && (Me.CurrentTarget.IsBoss() && !EncounterSpecific.HasBadAura), HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && (PRSettings.Instance.UseAoEAbilities && Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() >= PRSettings.Instance.Mage.AoECount), HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsSpecialKey, Spell.Cast("Time Warp")),
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && (PRSettings.Instance.UseAoEAbilities && Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() >= PRSettings.Instance.Mage.AoECount), HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsSpecialKey, Spell.Cast("Time Warp")),
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
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
                return new PrioritySelector();
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(ret => !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    new PrioritySelector(
                        Common.HandleCommonPreCombat()
                        ));
            }
        }

        internal override string Help
        {
            get
            {
                return @"
---------------------------------------------------------
Special Key: Will activate Time Warp

Recommend using Glyph of Combustion with this routine.

Latest Updates [26-Jun-2013]
- Added experimental setting to reduce in game lag.

Enjoy! --Millz
---------------------------------------------------------
";
            }
        }

        #endregion
    }
}
