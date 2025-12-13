#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 15:53:45 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 206 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/Shaman/ShamanCommon.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using CommonBehaviors.Actions;
using Oracle.Core.Managers;
using Oracle.Core.Spells;
using Oracle.Core.WoWObjects;
using Oracle.Healing;
using Oracle.Shared.Logging;
using Oracle.Shared.Utilities.Clusters.Data;
using Oracle.UI.Settings;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Linq;
using Action = Styx.TreeSharp.Action;

namespace Oracle.Classes.Shaman
{
    // Thanks to Singular devs for some of the methods in this class!

    public enum ShamanTalents
    {
        NaturesGuardian = 1,
        StoneBulwarkTotem,
        AstralShift,
        FrozenPower,
        EarthgrabTotem,
        WindwalkTotem,
        CallOfTheElements,
        TotemicPersistence,
        TotemicProjection,
        ElementalMastery,
        AncestralSwiftness,
        EchoOfTheElements,
        RushingStreams,
        AncestralGuidance,
        Conductivity,
        UnleashedFury,
        PrimalElementalist,
        ElementalBlast
    }

    public static class ShamanCommon
    {
        /// <summary>
        /// Temporary Enchant Id associated with Shaman Imbue
        /// Note: each enum value and Imbue.GetSpellName() must be maintained in a way to allow tranlating an enum into a corresponding spell name
        /// </summary>
        public enum Imbue
        {
            None = 0,

            Flametongue = 5,
            Windfury = 283,
            Earthliving = 3345,
            Frostbrand = 2,
            Rockbiter = 3021
        }

        #region Settings

        private static ShamanSettings Setting { get { return OracleSettings.Instance.Shaman; } }

        public static UnitBuffSelection UnitBuffSelection { get { return Setting.UnitBuffSelection; } }

        public static int ManaTideTotemPercent { get { return Setting.ManaTideTotemPercent; } }

        public static int LightningBoltPercent { get { return Setting.LightningBoltPercent; } }

        public static int HealingStreamTotemPercent { get { return Setting.HealingStreamTotemPercent; } }

        public static int AstralShiftPercent { get { return Setting.AstralShiftPercent; } }

        public static bool EnableWaterShield { get { return Setting.EnableWaterShield; } }

        public static bool HandleEarthShieldTarget { get { return Setting.HandleEarthShieldTarget; } }

        public static bool UseReinforcewithEarthElemental { get { return Setting.UseReinforcewithEarthElemental; } }

        public static int HealingSurgePercent { get { return Setting.HealingSurgePercent; } }

        public static int StoneBulwarkTotemPercent { get { return Setting.StoneBulwarkTotemPercent; } }

        public static HandleTankBuff HandleBuffonTank { get { return Setting.HandleBuffonTank; } }

        public static int HealingRainOveridePct { get { return Setting.HealingRainOveridePct; } }

        

        // AoE - Note: Limits are the amount of units (this will be evaluated as >=)
        public static int HealingRainPct { get { return (StyxWoW.Me.HasAura(RotationBase.Ascendance) ? Setting.HealingRainAscendancePct : Setting.HealingRainPct); } }

        public static int HealingRainLimit { get { return (StyxWoW.Me.HasAura(RotationBase.Ascendance) ? Setting.HealingRainAscendanceLimit : Setting.HealingRainLimit); } }

        public static int ChainHealPct { get { return Setting.ChainHealPct; } }

        public static int ChainHealLimit { get { return Setting.ChainHealLimit; } }

        // we ignore all settings and start healing like hell!!!
        public static int UrgentHealthPercentage { get { return Setting.UrgentHealthPercentage; } }

        // Oh shit moments!
        public static int HealingTideTotemLimit { get { return ((ShamanCommon.Exist(WoWTotem.ManaTide) || ShamanCommon.Exist(WoWTotem.HealingStream) ? 0 : Setting.HealingTideTotemLimit)); } } // never overite these totems!

        public static int HealingTideTotemPct { get { return ((ShamanCommon.Exist(WoWTotem.ManaTide) || ShamanCommon.Exist(WoWTotem.HealingStream) ? 0 : Setting.HealingTideTotemPct)); } } // never overite these totems!

        public static int SpiritLinkTotemLimit { get { return ((ShamanCommon.Exist(WoWTotem.Windwalk) ? 0 : Setting.SpiritLinkTotemLimit)); } } // never overite these totems!

        public static int SpiritLinkTotemPct { get { return ((ShamanCommon.Exist(WoWTotem.Windwalk) ? 0 : Setting.SpiritLinkTotemPct)); } } // never overite these totems!

        public static int EarthElementalTotemLimit { get { return ((ShamanCommon.Exist(WoWTotem.Tremor) ? 0 : Setting.EarthElementalTotemLimit)); } } // never overite these totems!

        public static int EarthElementalTotemPct { get { return ((ShamanCommon.Exist(WoWTotem.Tremor) ? 0 : Setting.EarthElementalTotemPct)); } } // never overite these totems!

        public static int AscendanceLimit { get { return Setting.AscendanceLimit; } }

        public static int AscendancePct { get { return Setting.AscendancePct; } }

        public static int SpiritwalkersGraceLimit { get { return Setting.SpiritwalkersGraceLimit; } }

        public static int SpiritwalkersGracePct { get { return Setting.SpiritwalkersGracePct; } }

        public static float ChainHealHopRange { get { return TalentManager.HasGlyph("Chaining") ? 25f : 13f; } }

        #endregion Settings

        #region booleans

        public static bool IsNotAboutToCastHealingRain { get { return !StyxWoW.Me.HasAura("Unleash Life") || CooldownTracker.SpellOnCooldown(RotationBase.HealingRain); } }

        #endregion booleans

        #region Shaman CreateClusteredHealBehavior

        public static void LoadClusterSpells()
        {
            OracleRoutine.Instance.ClusteredSpells.Clear();

            var key = 1;

            // Unleash Elements here.
            OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.HealingRain, SpellType.GroundEffect, HealingRainLimit, 0, HealingRainPct)); key++;

            OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.HealingTideTotem, SpellType.NearbyLowestHealth, HealingTideTotemLimit, 0, HealingTideTotemPct)); key++;

            OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.SpiritLinkTotem, SpellType.NearbyLowestHealth, SpiritLinkTotemLimit, 0, SpiritLinkTotemPct)); key++;

            OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.EarthElementalTotem, SpellType.NearbyLowestHealth, EarthElementalTotemLimit, 0, EarthElementalTotemPct)); key++;

            OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.ChainHeal, SpellType.Proximity, ChainHealLimit, 0, ChainHealPct));
        }

        #endregion Shaman CreateClusteredHealBehavior

        #region Shaman CreateCooldownBehavior

        public static void LoadCooldownSpells()
        {
            OracleRoutine.Instance.CooldownSpells.Clear();

            var key = 1;

            // we use NearbyLowestHealth so that we capture the units around us that are in trouble.
            // ClusterSpell accepts a delegate to pass into the composite during hook creation.

            OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.Ascendance, SpellType.NearbyLowestHealth, AscendanceLimit, 0, AscendancePct)); key++;

            OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.SpiritwalkersGrace, SpellType.NearbyLowestHealth, SpiritwalkersGraceLimit, 0, SpiritwalkersGracePct, ret => StyxWoW.Me.IsMoving));
        }

        #endregion Shaman CreateCooldownBehavior

        #region IMBUE SUPPORT

        private static DateTime nextImbueAllowed = DateTime.Now;

        // imbues are sometimes slow to appear on client... need to allow time
        // .. for buff to appear, otherwise will get in an imbue spam loop
        public static bool CanImbue(WoWItem item)
        {
            if (item != null && item.ItemInfo.IsWeapon)
            {
                // during combat, only mess with imbues if they are missing
                if (StyxWoW.Me.Combat && item.TemporaryEnchantment.Id != 0)
                    return false;

                // check if enough time has passed since last imbue
                // .. guards against detecting is missing immediately after a cast but before buff appears
                // .. (which results in imbue cast spam)
                if (nextImbueAllowed > DateTime.Now)
                    return false;

                switch (item.ItemInfo.WeaponClass)
                {
                    case WoWItemWeaponClass.Axe:
                        return true;
                    case WoWItemWeaponClass.AxeTwoHand:
                        return true;
                    case WoWItemWeaponClass.Dagger:
                        return true;
                    case WoWItemWeaponClass.Fist:
                        return true;
                    case WoWItemWeaponClass.Mace:
                        return true;
                    case WoWItemWeaponClass.MaceTwoHand:
                        return true;
                    case WoWItemWeaponClass.Polearm:
                        return true;
                    case WoWItemWeaponClass.Staff:
                        return true;
                    case WoWItemWeaponClass.Sword:
                        return true;
                    case WoWItemWeaponClass.SwordTwoHand:
                        return true;
                }
            }

            return false;
        }

        public static Decorator CreateShamanImbueMainHandBehavior(params Imbue[] imbueList)
        {
            return new Decorator(ret => CanImbue(StyxWoW.Me.Inventory.Equipped.MainHand),
                new PrioritySelector(
                    imb => imbueList.FirstOrDefault(i => SpellManager.HasSpell(i.ToSpellName())),

                    new Decorator(
                        ret => StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id != (int)ret
                            && SpellManager.HasSpell(((Imbue)ret).ToSpellName())
                            && SpellManager.CanCast(((Imbue)ret).ToSpellName(), null, false, false),
                        new Sequence(
                            new Action(ret => Lua.DoString("CancelItemTempEnchantment(1)")),
                            new WaitContinue(1,
                                ret => StyxWoW.Me.Inventory.Equipped.MainHand != null && (Imbue)StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == Imbue.None,
                                new ActionAlwaysSucceed()),
                            new DecoratorContinue(ret => ((Imbue)ret) != Imbue.None,
                                new Sequence(
                                    new Action(ret => Logger.Output("Imbuing main hand weapon with " + ((Imbue)ret).ToString())),
                                    new Action(ret => SpellManager.Cast(((Imbue)ret).ToSpellName(), null)),
                                    new Action(ret => SetNextAllowedImbueTime())
                                    )
                                )
                            )
                        )
                    )
                );
        }

        public static Composite CreateShamanImbueOffHandBehavior(params Imbue[] imbueList)
        {
            return new Decorator(ret => CanImbue(StyxWoW.Me.Inventory.Equipped.OffHand),
                new PrioritySelector(
                    imb => imbueList.FirstOrDefault(i => SpellManager.HasSpell(i.ToSpellName())),

                    new Decorator(
                        ret => StyxWoW.Me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id != (int)ret
                            && SpellManager.HasSpell(((Imbue)ret).ToSpellName())
                            && SpellManager.CanCast(((Imbue)ret).ToSpellName(), null, false, false),
                        new Sequence(
                            new Action(ret => Lua.DoString("CancelItemTempEnchantment(2)")),
                            new WaitContinue(1,
                                ret => StyxWoW.Me.Inventory.Equipped.OffHand != null && (Imbue)StyxWoW.Me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id == Imbue.None,
                                new ActionAlwaysSucceed()),
                            new DecoratorContinue(ret => ((Imbue)ret) != Imbue.None,
                                new Sequence(
                                    new Action(ret => Logger.Output("Imbuing Off hand weapon with " + ((Imbue)ret).ToString())),
                                    new Action(ret => SpellManager.Cast(((Imbue)ret).ToSpellName(), null)),
                                    new Action(ret => SetNextAllowedImbueTime())
                                    )
                                )
                            )
                        )
                    )
                );
        }

        public static bool HasTalent(ShamanTalents tal)
        {
            return TalentManager.IsSelected((int)tal);
        }

        public static Imbue GetImbue(WoWItem item)
        {
            if (item != null)
                return (Imbue)item.TemporaryEnchantment.Id;

            return Imbue.None;
        }

        public static bool IsImbuedForDPS(WoWItem item)
        {
            Imbue imb = GetImbue(item);
            return imb == Imbue.Flametongue || imb == Imbue.Windfury;
        }

        public static bool IsImbuedForHealing(WoWItem item)
        {
            return GetImbue(item) == Imbue.Earthliving;
        }

        public static void SetNextAllowedImbueTime()
        {
            // 2 seconds to allow for 0.5 seconds plus latency for buff to appear
            nextImbueAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 500); // 1500 + (int) StyxWoW.WoWClient.Latency << 1);
        }

        public static string ToSpellName(this Imbue i)
        {
            return i.ToString() + " Weapon";
        }

        #endregion IMBUE SUPPORT

        #region Totems

        public static bool IsRealTotem(WoWTotem ti)
        {
            return ti != WoWTotem.None
                && ti != WoWTotem.DummyAir
                && ti != WoWTotem.DummyEarth
                && ti != WoWTotem.DummyFire
                && ti != WoWTotem.DummyWater;
        }

        /// <summary>
        /// gets reference to array element in Me.Totems[] corresponding to WoWTotemType of wt.  Return is always non-null and does not indicate totem existance
        /// </summary>
        /// <param name="wt">WoWTotem of slot to reference</param>
        /// <returns>WoWTotemInfo reference</returns>
        public static WoWTotemInfo GetTotem(WoWTotem wt)
        {
            return GetTotem(wt.ToType());
        }

        /// <summary>
        /// check if a type of totem (ie Air Totem) exists
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Exist(WoWTotemType type)
        {
            WoWTotem wt = GetTotem(type).WoWTotem;
            return IsRealTotem(wt);
        }

        /// <summary>
        /// check if a specific totem (ie Mana Tide Totem) exists
        /// </summary>
        /// <param name="wtcheck"></param>
        /// <returns></returns>
        public static bool Exist(WoWTotem wtcheck)
        {
            WoWTotemInfo tiexist = GetTotem(wtcheck);
            WoWTotem wtexist = tiexist.WoWTotem;
            return wtcheck == wtexist && IsRealTotem(wtcheck);
        }

        /// <summary>
        /// gets reference to array element in Me.Totems[] corresponding to type.  Return is always non-null and does not indicate totem existance
        /// </summary>
        /// <param name="type">WoWTotemType of slot to reference</param>
        /// <returns>WoWTotemInfo reference</returns>
        public static WoWTotemInfo GetTotem(WoWTotemType type)
        {
            return StyxWoW.Me.Totems[(int)type - 1];
        }

        public static WoWTotemType ToType(this WoWTotem totem)
        {
            return (WoWTotemType)((long)totem >> 32);
        }

        #endregion Totems

        #region Unit Targets

        private static WoWUnit UnitBuffSelectionTarget
        {
            get
            {
                switch (ShamanCommon.UnitBuffSelection)
                {
                    case UnitBuffSelection.None:
                        return null;
                    case UnitBuffSelection.Tank:
                        return (OracleRoutine.IsViable(Tank) && !Tank.IsMe ? Tank : null);
                    case UnitBuffSelection.SecondTank:
                        return (OracleRoutine.IsViable(SecondTank) && !SecondTank.IsMe ? SecondTank : null);
                    case UnitBuffSelection.You:
                        return StyxWoW.Me;
                }
                return null;
            }
        }

        #endregion Unit Targets


        private static WoWUnit Tank { get { return OracleTanks.MainTank; } }

        private static WoWUnit SecondTank { get { return OracleTanks.AssistTank; } }

        public static Composite CreateHealingRainBehaviour()
        {
            const int UnleashElements = RotationBase.UnleashElements;
            const int AncestralSwiftness = RotationBase.AncestralSwiftness;
            const int HealingRain = RotationBase.HealingRain;

            return new Decorator(ret =>
            {
                if (UnitBuffSelection == UnitBuffSelection.None)
                    return false;

                if (!OracleRoutine.IsViable(UnitBuffSelectionTarget))
                    return false;

                if (UnitBuffSelection != UnitBuffSelection.You && OracleRoutine.IsViable(UnitBuffSelectionTarget) && UnitBuffSelectionTarget.IsMe)
                    return false;

                if (StyxWoW.Me.ManaPercent < HealingRainOveridePct) // needs a settings.
                    return false;

                if (StyxWoW.Me.HasAura(HealingRain))
                    return false;

                return true;
            },
                new Sequence(
                    new PrioritySelector(new Decorator(ret => StyxWoW.Me.HasAura("Unleash Life") || CooldownTracker.SpellOnCooldown(UnleashElements), new ActionAlwaysSucceed()), CooldownTracker.Cast(UnleashElements, on => null)),
                    new PrioritySelector(new Decorator(ret => !ShamanCommon.HasTalent(ShamanTalents.AncestralSwiftness) || (ShamanCommon.HasTalent(ShamanTalents.AncestralSwiftness) && CooldownTracker.SpellOnCooldown(AncestralSwiftness)), new ActionAlwaysSucceed()), CooldownTracker.Cast(AncestralSwiftness, on => null)),
                    Spell.CreateWaitForLagDuration(),
                    CooldownTracker.CastOnGround(HealingRain, on => UnitBuffSelectionTarget.Location)));
        }
    }
}