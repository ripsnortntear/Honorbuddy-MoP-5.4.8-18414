using System;
using System.Collections.Generic;
using System.Linq;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Classes.Mage
{
    static class Common
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }
        private static MageSettings MageSettings { get { return PRSettings.Instance.Mage; } }

        #region Composites

        internal static Composite HandleCommon()
        {
            return new PrioritySelector(
                HandleManaGem(),
                Spell.InterruptSpellCasts(ret => Me.FocusedUnit ?? Me.CurrentTarget),
                Spell.Cast("Spellsteal", ret => MageSettings.UseSpellsteal && TargetHasStealableBuff()),

                // Tier 6 Talent Abilities
                Spell.PreventDoubleCastOnGround("Evocation", 2, loc => Me.Location, ret => !Me.IsMoving && !Me.HasAura("Alter Time") && NeedRuneofPower),
                Spell.PreventDoubleCast("Evocation", 4, ret => !Me.IsMoving && (NeedEvocation || NeedInvocation)),
                Spell.PreventDoubleCast("Incanter's Ward", ThrottleTime, on => Me, ret => NeedIncantersWard, true)
                );
        }

        internal static Composite HandleManaGem()
        {
            return new PrioritySelector(
                Item.UseBagItem(36799/*Mana Gem*/, ret => MageSettings.EnableManaGem && (Me.ManaPercent < MageSettings.ManaGemPercent) && !Me.HasAura("Alter Time"), "Mana Gem"),
                Item.UseBagItem(81901/*Brilliant Mana Gem*/, ret => MageSettings.EnableManaGem && (Me.ManaPercent < MageSettings.ManaGemPercent) && !Me.HasAura("Alter Time"), "Brilliant Mana Gem")
                );
        }

        internal static Composite HandleCommonDefensiveCooldowns()
        {
            return new PrioritySelector(
                Lua.CancelMyAura("Ice Block", ret => PRSettings.Instance.Mage.useIceBlock && Me.HasAura("Ice Block") && Me.HealthPercent > 80),
                Spell.Cast("Ice Barrier", ret => !Me.IsCrowdControlled() && NeedIceBarrier),
                Spell.Cast("Mage Ward", ret => NeedMageWard),
                Spell.Cast("Ice Block", on => Me, ret => !Me.IsCrowdControlled() && NeedIceBlock),
                Spell.Cast("Temporal Shield", on => Me, ret => NeedTemporalShield),
                Item.UseBagItem(5512, ret => StyxWoW.Me.HealthPercent <= 50, "Healthstone [" + Me.HealthPercent + "% HP]")
                );
        }

        internal static Composite HandleCommonOffensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Mirror Image", ret => NeedMirrorImage),
                Spell.Cast("Lifeblood", ret => !Me.HasAnyAura(Spell.HeroismBuff))
                );
        }

        internal static Composite HandleCommonPreCombat()
        {
            return new PrioritySelector(
                // Frost
                Spell.PreventDoubleCast("Frost Armor", 4, on => Me, ret => !Me.HasAura("Frost Armor") && Me.Specialization == WoWSpec.MageFrost),

                // Fire
                Spell.PreventDoubleCast("Molten Armor", 4, on => Me, ret => !Me.HasAura("Molten Armor") && Me.Specialization == WoWSpec.MageFire),

                // Arcane
                Spell.PreventDoubleCast("Mage Armor", 4, on => Me, ret => !Me.HasAura("Mage Armor") && Me.Specialization == WoWSpec.MageArcane),

                // General
                Spell.CastRaidBuff("Arcane Brilliance", ret => true),
                Spell.PreventDoubleCast("Conjure Mana Gem", 1, on => Me, ret => Me.BagItems.FirstOrDefault(x => x.Entry == 81901 || x.Entry == 36799) == null),
                Spell.PreventDoubleCastOnGround("Evocation", 2, loc => Me.Location, ret => !Me.IsMoving() && Me.CurrentTarget != null && Me.CurrentTarget.IsBoss && NeedRuneofPower),
                Spell.PreventDoubleCast("Evocation", 2, on => Me, ret => !Me.IsMoving() && NeedInvocation && Me.CurrentTarget != null),
                Spell.PreventDoubleCast("Ice Barrier", 2, on => Me, ret => Me.GotTarget && !Me.HasAura("Ice Barrier") && TalentManager.HasTalent(6))
                );
        }

        public static bool TargetHasStealableBuff()
        {
            using (StyxWoW.Memory.AcquireFrame())
            {
                for (int i = 1; i <= Me.CurrentTarget.GetAllAuras().Count; i++)
                {
                    try
                    {
                        List<string> luaRet =
                            Styx.WoWInternals.Lua.GetReturnValues(
                                String.Format(
                                    "local buffName, _, _, _, _, _, _, _, isStealable = UnitAura(\"target\", {0}, \"HELPFUL\") return isStealable,buffName",
                                    i));
                        if (luaRet != null && luaRet[0] == "1")
                        {
                            var stealableSpell = !Me.HasAura(luaRet[1]) && (luaRet[1] != "Arcane Brilliance" && luaRet[1] != "Dalaran Brilliance");
                            return stealableSpell;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
                return false;
            }
        }

        #region Tier 5 Talent -> Mage Bombs

        public static Composite HandleBombMultiDoT()
        {

            return new PrioritySelector(

                new Decorator(ret => MageSettings.MultiDoTStyle == MultiDoTMethod.AroundTarget,
                    new PrioritySelector(
                        Spell.PreventDoubleMultiDoT("Nether Tempest", 0.5, Me.CurrentTarget, 20, 1, ret => TalentManager.HasTalent(13)),
                        Spell.PreventDoubleMultiDoT("Living Bomb", 1.5, Me.CurrentTarget, 20, 1, ret => TalentManager.HasTalent(14) && CanCastLivingBomb),
                        Spell.PreventDoubleCast("Frost Bomb", ThrottleTime, ret => NeedFrostBomb)
                        )
                    ),

                new Decorator(ret => MageSettings.MultiDoTStyle == MultiDoTMethod.EverythingAroundMe,
                    new PrioritySelector(
                        Spell.PreventDoubleMultiDoT("Nether Tempest", 0.5, Me, 40, 1, ret => TalentManager.HasTalent(13)),
                        Spell.PreventDoubleMultiDoT("Living Bomb", 1.5, Me, 40, 1, ret => TalentManager.HasTalent(14) && CanCastLivingBomb),
                        Spell.PreventDoubleCast("Frost Bomb", ThrottleTime, ret => NeedFrostBomb)
                        )
                    )
                );
        }


        public static Composite HandleSingleTargetMageBombs()
        {
            return new PrioritySelector(
                Spell.PreventDoubleCast("Nether Tempest", ThrottleTime, ret => NeedNetherTempest),
                Spell.PreventDoubleCast("Living Bomb", ThrottleTime, ret => NeedLivingBomb && CanCastLivingBomb),
                Spell.PreventDoubleCast("Frost Bomb", ThrottleTime, ret => NeedFrostBomb)
                );
        }
        #endregion

        #endregion

        #region Booleans

        public static bool CanCastLivingBomb { get { return Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 60).Count(x => x.HasMyAura("Living Bomb")) < 3; } }
        private static bool NeedIceBarrier { get { return StyxWoW.Me.HealthPercent < 80 && TalentManager.HasTalent(6); } }
        private static bool NeedMageWard { get { return StyxWoW.Me.HealthPercent < 50; } }
        private static bool NeedIceBlock { get { return PRSettings.Instance.Mage.useIceBlock && StyxWoW.Me.HealthPercent < 20 && !StyxWoW.Me.ActiveAuras.ContainsKey("Hypothermia"); } }
        private static bool NeedTemporalShield { get { return StyxWoW.Me.HealthPercent < 35 && TalentManager.HasTalent(4); } }
        private static bool NeedEvocation { get { return Me.ManaPercent < MageSettings.EvocationPercent; } }
        private static bool NeedMirrorImage { get { return MageSettings.EnableMirrorImage && Me.CurrentTarget != null && Me.CurrentTarget.IsBoss(); } }

        #region Tier 5 Talents
        private static bool NeedNetherTempest { get { return TalentManager.HasTalent(13) && (!Me.CurrentTarget.HasMyAura("Nether Tempest") || Spell.GetMyAuraTimeLeft(114923, Me.CurrentTarget) <= 1); } }
        private static bool NeedLivingBomb { get { return TalentManager.HasTalent(14) && !Me.CurrentTarget.HasMyAura(44457); } }
        private static bool NeedFrostBomb { get { return TalentManager.HasTalent(15); } }
        #endregion

        #region Tier 6 Talents
        private static bool NeedInvocation { get { return TalentManager.HasTalent(16) && !Me.HasAura(116257); } }
        private static bool NeedRuneofPower { get { return TalentManager.HasTalent(17) && !Me.HasAura(116014) && !Me.IsMoving && !Me.IsOnTransport; } }
        private static bool NeedIncantersWard { get { return Me.HealthPercent < 95 && MageSettings.UseIncantersWard && TalentManager.HasTalent(18) && Me.GetAuraFromName("Incanter's Ward") == null; } }
        #endregion

        #endregion

        #region Debuff Refresh Calculation
        private static readonly double Clientlag = StyxWoW.WoWClient.Latency * 2 / 1000.0;

        private static double DebuffRefreshTotalSeconds(double buffer, double traveltime, double casttime)
        {
            {
                return buffer + traveltime + casttime + Clientlag;
            }
        }

        private static double Traveltime(double travelSpeed)
        {
            {
                if (travelSpeed < 1) return 0;

                if (Me.CurrentTarget != null && Me.CurrentTarget.Distance < 1) return 0;

                if (Me.CurrentTarget != null)
                {
                    double t = Me.CurrentTarget.Distance / travelSpeed;
                    //Logger.DebugLog("Travel time: {0}", t);
                    return t;
                }

                return 0;
            }
        }
        #endregion

    }
}
