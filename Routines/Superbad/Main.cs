#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Inventory;
using Styx.CommonBot.POI;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

#endregion

/*
 TODO:
 * 
1. When "Open with Ravage" is enabled the bot does not actually go behind the target, so if the target is already facing you, the bot just idles there in front of the mob in stealth
3. Enable some sort of move-behind-target (useful with Dungeonbuddy)
6. Enable more evened-out use of Force of Nature during low level questing to achieve better overall efficiency
7. Enable opening with Faerie Fire for flying mobs (for questing)
10. Berserk settings
11. Choose between mitigation and life style @ guardian#
 * gib option for bear hug (on cd?)
 * explanation of taunt!
 * symbiosis spells not working properly (i guess :D)
 */

/*
 -Fixed: Nullpointer exception
 -Fixed: Racial settings works for Darkflight now
 -Added: Auto taunt for all Soo bosses! :-)
 * Fixed another bug @ target_time_to_die that caused lag spikes. //
    Fixed a bug @ Maul that caused lag spikes. //
    We will never ever leave form to cast healing_touch anymore. (If we want to consume PS proc). 
 * Fixed a bug @ clash
 * will no longer count crawler mines
 * fixed pvp ravage proc
 * FB finisher
 * fff a bit less agressive
 */


namespace Superbad
{
    public partial class Superbad : CombatRoutine
    {
        private static readonly HashSet<int> Bloodlust = new HashSet<int> { 2825, 32182, 80353, 90355 };

        private static readonly String[] Cc =
        {
            "Frozen", "Hammer of Justice", "Bear Hug", "Mighty Bash", "Hungering Cold", "Shockwave",
            "Howl of Terror", "Fear", "Psychic Scream", "Blood Fear", "Deep Freeze", "Asphyxiate", "Frostjaw",
            "Intimidating Shout", "Fist of Justice"
        };

        private static readonly String[] Mdw =
        {
            "Mark of the Wild", "Legacy of the Emperor", "Blessing of Kings", "Embrace of the Shale Spider"
        };

        internal static HashSet<int> DebuffRootHs = new HashSet<int>
        {
            96294, 116706, 64695, 339, 113770, 19975, 113275, 113275, 19185, 33395, 63685, 39965, 122, 110693, 55536,
            87194, 111340, 45334, 90327, 102359, 128405, 13099, 115197, 50245, 91807, 123407, 107566, 54706, 114404,
            4167
        };

        public static DateTime HtCast;

        public static bool BurstMode = false;
        public static bool Paused = false;
        public static bool EnableMovement = false;


        public static bool English;
        public static bool Russian;

        private static int _unitCount;
        public static bool Facing;
        private static bool _validtargetinrange;
        private static WoWSpec _currentSpec;
        private static double _calcfrenziedheal;
        private static double _healthmissing;
        private static double _rage;
        private static double _energy = 100;
        public static double dps;
        private static double combo_points;
        private static double time_to_max;
        private static double _attackpower;
        private static double _agi;
        private static double _stamina;
        private static double charges;
        private static double EnergyRegen;
        private static double _healthPercent;
        private static bool _targetNotBoss;
        public static ShapeshiftForm CurrentShape;
        private static double _distance;
        private static bool _targetAboveGround;
        public static bool HasSpellDash;
        public static bool HasSpellHealingTouch;
        public static bool HasSpellProwl;
        public static bool HasSpellMarkoftheWild;
        public static bool HasSpellAquaticForm;
        public static bool HasSpellSavageRoar;
        public static bool HasSpellSoulSwap;
        public static bool HasSpellRedirect;
        public static bool HasSpellShattering;
        public static bool HasSpellConsecration;
        public static IEnumerable<WoWUnit> UnitList;
        public static double _gcdTimeLeftTotalSeconds;
        public static bool AoeMode;
        public static bool Synapse;
        public static WoWItem Gloves;
        private readonly Stopwatch _watch = new Stopwatch();
        private int _delay;
        private bool releaseOnce;


        public override string Name
        {
            get { return "Superbad v 4.0"; }
        }

        public override WoWClass Class
        {
            get { return WoWClass.Druid; }
        }

        public override bool WantButton
        {
            get { return true; }
        }

        public override bool NeedPreCombatBuffs
        {
            get
            {
                if (!StyxWoW.IsInGame || !StyxWoW.IsInWorld)
                    return false;
                if (StyxWoW.Me.Combat || StyxWoW.Me.IsDead || StyxWoW.Me.IsGhost || StyxWoW.Me.IsOnTransport ||
                    StyxWoW.Me.Mounted || StyxWoW.Me.HasBuff("Drink") || StyxWoW.Me.HasBuff("Food") ||
                    StyxWoW.Me.IsCasting || StyxWoW.Me.IsChanneling || StyxWoW.Me.Pacified || StyxWoW.Me.IsFlying ||
                    StyxWoW.Me.Mounted || CurrentShape == ShapeshiftForm.EpicFlightForm ||
                    CurrentShape == ShapeshiftForm.FlightForm) return false;
                if (SuperbadSettings.Instance.Mdw)
                    if (buff.mark_of_the_wild.down)
                        if (mark_of_the_wild())
                            return false;
                if (SuperbadSettings.Instance.Mdwgroup)
                    if (PartyBuff.NeedGrpBuff())
                        if (mark_of_the_wild())
                            return false;
                if (NeedCat() && SuperbadSettings.Instance.SavageFarm)
                    if (CurrentShape == ShapeshiftForm.Cat)
                        if (buff.savage_roar.down)
                        {
                            if (savage_roar())
                                return false;
                        }
                if (buff.predatory_swiftness.up && HasSpellHealingTouch && !buff.prowl.up)
                {
                    if (healing_touch())
                        return false;
                }
                if (StyxWoW.Me.IsSwimming
                    && CurrentShape != ShapeshiftForm.Aqua)
                    if (aquatic_form())
                        return false;
                if (!StyxWoW.Me.HasAura("Symbiosis") && NeedCat() &&
                    _currentSpec == WoWSpec.DruidFeral)
                {
                    WoWPlayer symbtarget = SymbiosisTargetFeral();
                    if (symbtarget != null && symbtarget != StyxWoW.Me)
                    {
                        Symbiosis(symbtarget);
                        return false;
                    }
                }
                if (!StyxWoW.Me.HasAura("Symbiosis") && NeedBear() &&
                    _currentSpec == WoWSpec.DruidGuardian)
                {
                    WoWPlayer symbtarget = SymbiosisTargetGuardian();
                    if (symbtarget != null && symbtarget != StyxWoW.Me)
                    {
                        Symbiosis(symbtarget);
                        return false;
                    }
                }

                if (NeedCat() && SuperbadSettings.Instance.StayInStealth && !buff.prowl.up && !StyxWoW.Me.Mounted &&
                    CurrentShape != ShapeshiftForm.Aqua && CurrentShape != ShapeshiftForm.Travel)
                {
                    if (prowl())
                        return false;
                }
                if (!StyxWoW.Me.HasAura("Lightning Shield"))
                    if (LightningShield())
                        return false;
                if (!StyxWoW.Me.IsSwimming)
                    Travelform();
                return false;
            }
        }

        public override bool NeedRest
        {
            get
            {
                if (!StyxWoW.IsInGame || !StyxWoW.IsInWorld)
                    return false;
                if (StyxWoW.Me.Pacified)
                    return false;
                if (StyxWoW.Me.Combat)
                    return false;
                if (StyxWoW.Me.IsFlying && StyxWoW.Me.IsOnTransport)
                    return false;
                if (StyxWoW.Me.IsDead || StyxWoW.Me.IsGhost || StyxWoW.Me.IsCasting || StyxWoW.Me.Mounted ||
                    CurrentShape == ShapeshiftForm.EpicFlightForm ||
                    CurrentShape == ShapeshiftForm.FlightForm) return false;
                if (!StyxWoW.Me.HasBuff("Drink") && !StyxWoW.Me.HasBuff("Food"))
                {
                    if (SuperbadSettings.Instance.WaitSickness && buff.resurrection_sickness.up)
                        return true;
                    if (_healthPercent <= SuperbadSettings.Instance.OoCReju && !buff.rejuvenation.up)
                        return true;
                    if (_healthPercent <= SuperbadSettings.Instance.OoCHealingTouch)
                        return true;
                }
                if (SuperbadSettings.Instance.UseRest && !StyxWoW.Me.IsSwimming &&
                    _healthPercent <= SuperbadSettings.Instance.RestHealth &&
                    !StyxWoW.Me.HasBuff("Food") && Consumable.GetBestFood(false) != null)
                    return true;
                if (SuperbadSettings.Instance.UseRest && !StyxWoW.Me.IsSwimming &&
                    StyxWoW.Me.ManaPercent <= SuperbadSettings.Instance.RestMana &&
                    !StyxWoW.Me.HasBuff("Drink") && Consumable.GetBestDrink(false) != null)
                    return true;
                if ((StyxWoW.Me.HasBuff("Food") && _healthPercent < 95) ||
                    (StyxWoW.Me.HasBuff("Drink") && StyxWoW.Me.ManaPercent < 95))
                    return true;
                return (SuperbadSettings.Instance.UseRest &&
                        (StyxWoW.Me.ManaPercent <= SuperbadSettings.Instance.RestMana) ||
                        _healthPercent <= SuperbadSettings.Instance.RestHealth) &&
                       !StyxWoW.Me.CurrentMap.IsBattleground;
            }
        }

        public static Dictionary<InventorySlot, WoWItem> EquippedItems
        {
            get
            {
                var equipped = new Dictionary<InventorySlot, WoWItem>();
                WoWItem[] items = StyxWoW.Me.Inventory.Equipped.Items;

                equipped.Clear();
                for (int i = 0; i < 23; i++)
                    equipped.Add((InventorySlot) (i + 1), items[i]);

                return equipped;
            }
        }

        public override bool NeedDeath
        {
            get
            {
                if (!StyxWoW.IsInGame || !StyxWoW.IsInWorld)
                    return false;
                return StyxWoW.Me.IsDead;
            }
        }

        public static double AP { get; set; }
        public static double Mastery { get; set; }

        public static double Crit { get; set; }
        public static double Multiplier { get; set; }

        public override void OnButtonPress()
        {
            Logging.Write("Config opened!");
            //new Form3().ShowDialog();
            //new SuperbadConfig().ShowDialog();
            new GUI().ShowDialog();
        }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        public override void Initialize()
        {
            Logging.Write("");
            Logging.Write("Hello " + StyxWoW.Me.Race + " " + StyxWoW.Me.Class);
            Logging.Write("Thank you for using Superbad");
            Logging.Write("");
            EventHandlers.Init();
            if (SuperbadSettings.Instance.Update)
                Updater.CheckForUpdate();
            TalentManager.Update();
            Lua.Events.AttachEvent("MODIFIER_STATE_CHANGED", HandleModifierStateChanged);
            //  Lua.Events.AttachEvent("UNIT_POWER", grabUnitPower);

            grabMainHandDPS();

            var localeLanguage = Lua.GetReturnVal<string>("return GetLocale();", 0);

            switch (localeLanguage)
            {
                case "enUS":
                    English = true;
                    Logging.Write("Detected client with english language.");
                    break;
                case "enGB":
                    English = true;
                    Logging.Write("Detected client with english language.");
                    break;
                case "ruRU":
                    Russian = true;
                    Logging.Write("Detected client with russian language.");
                    break;
            }
            SetLearnedSpells();
            SuperbadSettings.printSettings();
            GCD.GcdSpell = GCD.GetGlobalCooldownSpell;
            SynapseSprings();
        }

        private void SynapseSprings()
        {
            WoWItem item = StyxWoW.Me.Inventory.GetItemBySlot((uint) WoWInventorySlot.Hands);
            if (item == null) return;
            var itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return;
            Synapse = true;
            Gloves = item;
        }

        private void grabMainHandDPS()
        {
            var swingMin = Lua.GetReturnVal<float>("return UnitDamage(\"player\");", 0);
            var swingMax = Lua.GetReturnVal<float>("return UnitDamage(\"player\");", 1);
            float swingAvg = (swingMin + swingMax)/2;
            dps = swingAvg/2;
        }

        public static void SetLearnedSpells()
        {
            HasSpellDash = SpellManager.HasSpell("Dash");
            HasSpellHealingTouch = SpellManager.HasSpell("Healing Touch");
            HasSpellProwl = SpellManager.HasSpell("Prowl");
            HasSpellMarkoftheWild = SpellManager.HasSpell("Mark of the Wild");
            HasSpellAquaticForm = SpellManager.HasSpell("Aquatic Form");
            HasSpellSavageRoar = SpellManager.HasSpell("Savage Roar");
            HasSpellSoulSwap = SpellManager.HasSpell(110810);
            HasSpellRedirect = SpellManager.HasSpell(110730);
            HasSpellShattering = SpellManager.HasSpell(112997);
            HasSpellConsecration = SpellManager.HasSpell(110701);
        }

        public override void Pull()
        {
            if (SuperbadSettings.Instance.UseTargeting)
                EnsureTarget();
            //MoveToLos
            if (SuperbadSettings.Instance.UseMovement)
            {
                if (StyxWoW.Me.CurrentTarget != null && !StyxWoW.Me.CurrentTarget.InLineOfSpellSight &&
                    StyxWoW.Me.CurrentTarget != StyxWoW.Me)
                    Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
            }

            //FaceTarget
            if (SuperbadSettings.Instance.UseFacing)
                if (StyxWoW.Me.CurrentTarget != null && !StyxWoW.Me.IsMoving && !StyxWoW.Me.CurrentTarget.IsMe &&
                    !StyxWoW.Me.IsSafelyFacing(StyxWoW.Me.CurrentTarget, 70f))
                    StyxWoW.Me.CurrentTarget.Face();
            //Movement
            if (SuperbadSettings.Instance.UseMovement)
            {
                if (!StyxWoW.Me.IsCasting && !StyxWoW.Me.IsChanneling)
                {
                    WoWUnit currentTarget = StyxWoW.Me.CurrentTarget;
                    if (currentTarget == null)
                        return;
                    if (currentTarget == StyxWoW.Me)
                        return;

                    bool movebehind = false;
                    WoWPoint behindPoint = StyxWoW.Me.GetPosition();

                    if (SuperbadSettings.Instance.PullStealth && SuperbadSettings.Instance.StealthOpener == 1
                        && NeedCat() && !Group.MeIsTank && currentTarget.CurrentTarget != StyxWoW.Me)
                    {
                        behindPoint = CalculatePointBehindTarget();
                        if (Navigator.CanNavigateFully(StyxWoW.Me.Location, behindPoint, 4))
                            if (BossList.AvoidRearBosses.Contains(currentTarget.Entry))
                                movebehind = true;
                    }

                    float range = StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.IsPlayer
                        ? 2f
                        : Unit.MeleeRange;
                    if (StyxWoW.Me.CurrentTarget != null &&
                        StyxWoW.Me.Location.Distance(StyxWoW.Me.CurrentTarget.Location) < range)
                    {
                        if (StyxWoW.Me.IsMoving)
                        {
                            if (!movebehind || (movebehind && currentTarget.MeIsSafelyBehind))
                                Navigator.PlayerMover.MoveStop();
                        }
                    }
                    else
                    {
                        if (StyxWoW.Me.CurrentTarget != null)
                        {
                            if (!movebehind)
                                Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                            if (movebehind)
                                Navigator.MoveTo(behindPoint);
                        }
                    }
                }
            }
            if (StyxWoW.Me.CurrentTarget != null)
            {
                if (StyxWoW.Me.IsCasting || StyxWoW.Me.IsChanneling)
                    return;
                if (NeedBear())
                {
                    BearPullHandler();
                    return;
                }
                if (NeedCat())
                {
                    CatPullHandler();
                    return;
                }
                if (NeedBear() || NeedCat()) return;
                Logging.Write(LogLevel.Diagnostic, "There is something wrong. Dont know which Form to choose.");
                Logging.Write(LogLevel.Diagnostic, "Choosing Cat Form");
                CatPullHandler();
            }
        }

        public static void CreateMoveBehindTargetBehavior()
        {
            if (Group.MeIsTank || StyxWoW.Me.IsCasting || StyxWoW.Me.IsChanneling)
                return;
            WoWUnit currentTarget = StyxWoW.Me.CurrentTarget;
            if (currentTarget != null)
            {
                if (currentTarget.MeIsSafelyBehind || !currentTarget.IsAlive ||
                    BossList.AvoidRearBosses.Contains(currentTarget.Entry))
                    return;
                if (currentTarget == StyxWoW.Me)
                    return;
                if (currentTarget.CurrentTarget == StyxWoW.Me)
                    return;
                WoWPoint behindPoint = CalculatePointBehindTarget();

                if (Navigator.CanNavigateFully(StyxWoW.Me.Location, behindPoint, 4))
                {
                    Navigator.MoveTo(behindPoint);
                }
            }
        }

        public override void Combat()
        {
            if (Paused)
                return;
            if (BotManager.Current.Name == "Gatherbuddy" &&
                (StyxWoW.Me.Shapeshift == ShapeshiftForm.EpicFlightForm ||
                 StyxWoW.Me.Shapeshift == ShapeshiftForm.FlightForm))
                return;
            if (SuperbadSettings.Instance.Suspend)
                if ((GetAsyncKeyState(Keys.LButton) != 0 && GetAsyncKeyState(Keys.RButton) != 0) ||
                    GetAsyncKeyState(Keys.W) != 0 ||
                    GetAsyncKeyState(Keys.S) != 0 ||
                    GetAsyncKeyState(Keys.D) != 0 ||
                    GetAsyncKeyState(Keys.A) != 0)
                    EnableMovement = false;
                else
                    EnableMovement = true;
            else
                EnableMovement = true;
            if (EnableMovement)
            {
                if (SuperbadSettings.Instance.UseTargeting)
                    EnsureTarget();
                //MoveToLos
                if (SuperbadSettings.Instance.UseMovement)
                {
                    if (StyxWoW.Me.CurrentTarget != null && !StyxWoW.Me.CurrentTarget.InLineOfSpellSight &&
                        StyxWoW.Me.CurrentTarget != StyxWoW.Me)
                        Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                }

                //FaceTarget
                if (SuperbadSettings.Instance.UseFacing)
                    if (StyxWoW.Me.CurrentTarget != null && !StyxWoW.Me.IsMoving && !StyxWoW.Me.CurrentTarget.IsMe &&
                        !StyxWoW.Me.IsSafelyFacing(StyxWoW.Me.CurrentTarget, 70f))
                        StyxWoW.Me.CurrentTarget.Face();
                //Movement
                if (SuperbadSettings.Instance.UseMovement)
                {
                    if (!StyxWoW.Me.IsCasting && !StyxWoW.Me.IsChanneling)
                    {
                        WoWUnit currentTarget = StyxWoW.Me.CurrentTarget;
                        if (currentTarget == null)
                            return;
                        if (currentTarget == StyxWoW.Me)
                            return;
                        bool movebehind = false;
                        WoWPoint behindPoint = StyxWoW.Me.GetPosition();

                        if (SuperbadSettings.Instance.PullStealth && SuperbadSettings.Instance.StealthOpener == 1
                            && NeedCat() && !Group.MeIsTank && currentTarget.CurrentTarget != StyxWoW.Me)
                        {
                            behindPoint = CalculatePointBehindTarget();
                            if (Navigator.CanNavigateFully(StyxWoW.Me.Location, behindPoint, 4))
                                if (BossList.AvoidRearBosses.Contains(currentTarget.Entry))
                                    movebehind = true;
                        }

                        float range = StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.IsPlayer
                            ? 2f
                            : Unit.MeleeRange;
                        if (StyxWoW.Me.CurrentTarget != null &&
                            StyxWoW.Me.Location.Distance(StyxWoW.Me.CurrentTarget.Location) < range)
                        {
                            if (StyxWoW.Me.IsMoving)
                            {
                                if (!movebehind || (movebehind && currentTarget.MeIsSafelyBehind))
                                    Navigator.PlayerMover.MoveStop();
                            }
                        }
                        else
                        {
                            if (StyxWoW.Me.CurrentTarget != null)
                            {
                                if (!movebehind)
                                    Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                                if (movebehind)
                                    Navigator.MoveTo(behindPoint);
                            }
                        }
                    }
                }
            }
            if (StyxWoW.Me.CurrentTarget != null)
            {
                if (StyxWoW.Me.IsCasting || StyxWoW.Me.IsChanneling)
                    return;
                if (InterruptHandler())
                    return;
                if (HealHandler())
                    return;
                if (EscapeHandler())
                    return;
                if (SpeedHandler())
                    return;
                auto_attack();
                if (NeedBear())
                {
                    BearHandler();
                    return;
                }
                if (NeedCat())
                {
                    CatHandler();
                    return;
                }
                Logging.Write(LogLevel.Diagnostic, "There is something wrong. Dont know which Form to choose.");
                Logging.Write(LogLevel.Diagnostic, "Choosing Cat Form");
                CatHandler();
            }
        }

        private static WoWPoint CalculatePointBehindTarget()
        {
            return
                StyxWoW.Me.CurrentTarget.Location.RayCast(
                    StyxWoW.Me.CurrentTarget.Rotation + WoWMathHelper.DegreesToRadians(150), Unit.MeleeRange - 2f);
        }

        public static void EnsureTarget()
        {
            WoWUnit target = grabTarget();

            if (target != null)
            {
                if (target.Guid != StyxWoW.Me.CurrentTargetGuid)
                {
                    target.Target();
                    TankManager.TargetingTimer.Reset();
                }
                return;
            }

            WoWUnit target2 = grabTarget2();
            if (target2 != null && target2.Guid != StyxWoW.Me.CurrentTargetGuid)
            {
                target2.Target();
            }
        }

        public static WoWUnit grabTarget()
        {
            if (CurrentShape == ShapeshiftForm.Bear && Group.MeIsTank && StyxWoW.Me.Combat &&
                TankManager.Instance.FirstUnit != null)
            {
                if (StyxWoW.Me.CurrentTarget != TankManager.Instance.FirstUnit)
                {
                    if (TankManager.TargetingTimer.IsFinished)
                    {
                        Logging.Write("TankTarget: switching to first unit of TankTargeting");
                        return TankManager.Instance.FirstUnit;
                    }

                    if (!Unit.ValidUnit(StyxWoW.Me.CurrentTarget))
                    {
                        Logging.Write("TankTarget: CurrentTarget invalid, switching to first unit of TankTargeting");
                        return TankManager.Instance.FirstUnit;
                    }
                }
                return StyxWoW.Me.CurrentTarget;
            }
            if (BotPoi.Current.Type == PoiType.Kill)
            {
                WoWUnit botpoi = BotPoi.Current.AsObject.ToUnit();
                if (Unit.ValidUnit(botpoi))
                {
                    if (StyxWoW.Me.CurrentTargetGuid != botpoi.Guid)
                        Logging.Write("Switching to BotPoi: " + botpoi.SafeName + "!");

                    return botpoi;
                }
                BotPoi.Clear("Superbad detected invalid mob as BotPoi");
            }
            if (StyxWoW.Me.CurrentTarget == null || StyxWoW.Me.CurrentTarget.IsDead)
                return null;
            if (!StyxWoW.Me.IsInGroup() && StyxWoW.Me.Combat &&
                ((!StyxWoW.Me.CurrentTarget.Combat && !StyxWoW.Me.CurrentTarget.Aggro &&
                  !StyxWoW.Me.CurrentTarget.PetAggro) || StyxWoW.Me.SpellDistance() > 30 ||
                 !StyxWoW.Me.CurrentTarget.InLineOfSpellSight))
            {
                // Look for agrroed mobs next. prioritize by IsPlayer, Relative Distance, then Health
                WoWUnit target = ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                    .Where(
                        p => p.SpellDistance() < 10
                             && Unit.ValidUnit(p)
                             && (p.Aggro || p.PetAggro)
                             && p.InLineOfSpellSight
                    )
                    // .OrderBy(u => CalcDistancePriority(u)).ThenBy(u => u.HealthPercent)
                    .OrderBy(u => u.HealthPercent)
                    .FirstOrDefault();

                if (target != null && target.Guid != StyxWoW.Me.CurrentTargetGuid)
                {
                    // Return the closest one to us
                    Logging.Write("Switching to aggroed mob pounding on me " + target.SafeName + "!");
                    return target;
                }
            }
            WoWUnit pOwner = Unit.GetPlayerParent(StyxWoW.Me.CurrentTarget);
            if (pOwner != null && Unit.ValidUnit(pOwner) && !Blacklist.Contains(pOwner, BlacklistFlags.Combat))
            {
                Logging.Write("Current target owned by a player.  Switching to " + pOwner.SafeName + "!");
                if (BotPoi.Current.Type == PoiType.Kill && BotPoi.Current.Guid == StyxWoW.Me.CurrentTarget.Guid)
                    BotPoi.Clear(string.Format("Superbad detected {0} as Player Owned Pet",
                        StyxWoW.Me.CurrentTarget.SafeName));

                return pOwner;
            }
            if (Blacklist.Contains(StyxWoW.Me.CurrentTargetGuid, BlacklistFlags.Combat))
            {
                if (StyxWoW.Me.CurrentTarget.Combat && StyxWoW.Me.CurrentTarget.IsTargetingMeOrPet)
                {
                    Logging.Write("Current target " + StyxWoW.Me.CurrentTarget.SafeName +
                                  " blacklisted and Bot has no other targets!  Fighting this one and hoping Bot wakes up if its Evade bugged!");
                    return StyxWoW.Me.CurrentTarget;
                }

                Logging.Write("CurrentTarget " + StyxWoW.Me.CurrentTarget.SafeName +
                              " blacklisted and not in combat with so clearing target!");
                StyxWoW.Me.ClearTarget();
                return null;
            }

            if (Unit.ValidUnit(StyxWoW.Me.CurrentTarget))
                return StyxWoW.Me.CurrentTarget;

            // at this point, stick with it if in Targetlist
            if (Targeting.Instance.TargetList.Contains(StyxWoW.Me.CurrentTarget))
            {
                Logging.Write("EnsureTarget: failed validation but {0} is in TargetList, continuing...",
                    StyxWoW.Me.CurrentTarget.SafeName);
                return StyxWoW.Me.CurrentTarget;
            }

            if (StyxWoW.Me.CurrentTarget.SafeName == "Dragonmaw War Banner" ||
                StyxWoW.Me.CurrentTarget.SafeName == "Healing Tide Totem")
                return StyxWoW.Me.CurrentTarget;

            // otherwise, let's get a new one
            Logging.Write("EnsureTarget: invalid target {0}, so forcing selection of a new one...",
                StyxWoW.Me.CurrentTarget == null ? "(null)" : StyxWoW.Me.CurrentTarget.SafeName);
            return null;
        }

        public static WoWUnit grabTarget2()
        {
            WoWPlayer rafLeader = RaFHelper.Leader;
            if (rafLeader != null && rafLeader.IsValid && !rafLeader.IsMe && rafLeader.Combat &&
                rafLeader.CurrentTarget != null && rafLeader.CurrentTarget.IsAlive &&
                !Blacklist.Contains(rafLeader.CurrentTarget, BlacklistFlags.Combat))
            {
                Logging.Write("Current target invalid. Switching to Tanks target " + rafLeader.CurrentTarget.SafeName +
                              "!");
                return rafLeader.CurrentTarget;
            }

            // if we have BotPoi then try it
            if (Context.SuperbadRoutine.CurrentWoWContext != WoWContext.Normal && BotPoi.Current.Type == PoiType.Kill)
            {
                var unit = BotPoi.Current.AsObject as WoWUnit;
                if (unit == null)
                {
                    Logging.Write("Current Kill POI invalid. Clearing POI!");
                    BotPoi.Clear("Superbad detected null POI");
                }
                else if (!unit.IsAlive)
                {
                    Logging.Write("Current Kill POI dead. Clearing POI " + unit.SafeName + "!");
                    BotPoi.Clear("Superbad detected Unit is dead");
                }
                else if (Blacklist.Contains(unit, BlacklistFlags.Combat))
                {
                    Logging.Write("Current Kill POI is blacklisted. Clearing POI " + unit.SafeName + "!");
                    BotPoi.Clear("Superbad detected Unit is Blacklisted");
                }
                else
                {
                    Logging.Write("Current target invalid. Switching to POI " + unit.SafeName + "!");
                    return unit;
                }
            }

            // Look for agrroed mobs next. prioritize by IsPlayer, Relative Distance, then Health
            WoWUnit target = Targeting.Instance.TargetList
                .Where(
                    p => !Blacklist.Contains(p, BlacklistFlags.Combat)
                         && Unit.ValidUnit(p)
                        // && p.DistanceSqr <= 40 * 40  // dont restrict check to 40 yds
                         &&
                         (p.Aggro || p.PetAggro ||
                          (p.Combat && p.GotTarget && (p.IsTargetingMeOrPet || p.IsTargetingMyRaidMember))))
                .OrderBy(u => u.IsPlayer)
                .ThenBy(CalcDistancePriority)
                .ThenBy(u => u.HealthPercent)
                .FirstOrDefault();

            if (target != null)
            {
                // Return the closest one to us
                Logging.Write("Current target invalid. Switching to aggroed mob " + target.SafeName + "!");
                return target;
            }

            // if we have BotPoi then try it
            if (Context.SuperbadRoutine.CurrentWoWContext == WoWContext.Normal && BotPoi.Current.Type == PoiType.Kill)
            {
                var unit = BotPoi.Current.AsObject as WoWUnit;
                if (unit == null)
                {
                    Logging.Write("Current Kill POI invalid. Clearing POI!");
                    BotPoi.Clear("Superbad detected null POI");
                }
                else if (!unit.IsAlive)
                {
                    Logging.Write("Current Kill POI dead. Clearing POI " + unit.SafeName + "!");
                    BotPoi.Clear("Superbad detected Unit is dead");
                }
                else if (Blacklist.Contains(unit, BlacklistFlags.Combat))
                {
                    Logging.Write("Current Kill POI is blacklisted. Clearing POI " + unit.SafeName + "!");
                    BotPoi.Clear("Superbad detected Unit is Blacklisted");
                }
                else
                {
                    Logging.Write("Current target invalid. Switching to POI " + unit.SafeName + "!");
                    return unit;
                }
            }

            // now anything in the target list or a Player
            target = Targeting.Instance.TargetList
                .Where(
                    p => !Blacklist.Contains(p, BlacklistFlags.Combat)
                         && p.IsAlive
                // && p.DistanceSqr <= 40 * 40 // don't restrict check to 40 yds
                )
                .OrderBy(u => u.IsPlayer)
                .ThenBy(u => u.DistanceSqr)
                .FirstOrDefault();

            if (target != null)
            {
                // Return the closest one to us
                Logging.Write("Current target invalid. Switching to TargetList mob " + target.SafeName + "!");
                return target;
            }
            return null;
        }


        private static int CalcDistancePriority(WoWUnit unit)
        {
            var prio = (int) StyxWoW.Me.SpellDistance(unit);
            if (prio <= 5)
                prio = 1;
            else if (prio <= 10)
                prio = 2;
            else if (prio <= 20)
                prio = 3;
            else
                prio = 4;
            return prio;
        }

        internal static bool CdChecker()
        {
            if (StyxWoW.Me.CurrentTarget == null)
                return true;
            if (StyxWoW.Me.IsInInstance)
            {
                return !Unit.IsBoss(StyxWoW.Me.CurrentTarget) && !StyxWoW.Me.CurrentTarget.IsBoss;
            }
            if (Unit.IsDummy(StyxWoW.Me.CurrentTarget))
                return false;
            if (target.time_to_die > 10)
                return false;
            return true;
        }

        private static void PrepareVariables()
        {
            Spell.GatherAuras();
            UnitList = Unit.NearbyUnfriendlyUnits;
            _unitCount = UnitList.Count(u => u.SpellDistance() <= 8);
            Facing = StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.IsFacing(StyxWoW.Me.CurrentTarget);
            _validtargetinrange = StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.IsWithinMeleeRange;
            _currentSpec = StyxWoW.Me.Specialization;
            CurrentShape = StyxWoW.Me.Shapeshift;
            _gcdTimeLeftTotalSeconds = 200 > GCD.GlobalCooldownLeft.TotalMilliseconds
                ? 0
                : GCD.GlobalCooldownLeft.TotalSeconds;
            _healthPercent = StyxWoW.Me.IsDead ? 0 : StyxWoW.Me.HealthPercent;
            _targetNotBoss = CdChecker();
            _targetAboveGround = StyxWoW.Me.CurrentTarget != null && Unit.IsAboveTheGround(StyxWoW.Me.CurrentTarget);
            _distance = StyxWoW.Me.CurrentTarget != null ? StyxWoW.Me.CurrentTarget.SpellDistance() : 99;
            buff.berserk.up = StyxWoW.Me.HasBuff(106951) || StyxWoW.Me.HasBuff(50334);
            buff.bloodlust.up = StyxWoW.Me.HasAnyBuff(Bloodlust);
            buff.darkflight.up = StyxWoW.Me.HasBuff(68992);
            buff.dash.up = StyxWoW.Me.HasBuff(1850);
            buff.flag.up = StyxWoW.Me.HasBuff("Alliance Flag") || StyxWoW.Me.HasBuff("Horde Flag");
            buff.mark_of_the_wild.down = !StyxWoW.Me.HasAnyBuff(Mdw);
            buff.might_or_ursoc.up = StyxWoW.Me.HasBuff(106922);
            buff.rejuvenation.up = StyxWoW.Me.HasAura(774);
            buff.resurrection_sickness.up = StyxWoW.Me.HasBuff("Resurrection Sickness");
            buff.rooted.up = StyxWoW.Me.HasAnyBuff(DebuffRootHs);
            buff.stampeding_roar.up = StyxWoW.Me.HasBuff(77764) || StyxWoW.Me.HasBuff(77761);
            debuff.cc.up = StyxWoW.Me.HasAnyBuff(Cc) || StyxWoW.Me.StackCountOnMe("Remorseless Winter") >= 3;
            target.health.pct = StyxWoW.Me.CurrentTarget == null ? 0 : StyxWoW.Me.CurrentTarget.HealthPercent;
            target.time_to_die = StyxWoW.Me.CurrentTarget != null
                ? DpsMeter.GetCombatTimeLeft(StyxWoW.Me.CurrentTarget).TotalSeconds
                : 10;

            _rage = LuaGetRage();
            _energy = _rage;
            buff.predatory_swiftness.up = StyxWoW.Me.HasAura(69369);
            buff.prowl.up = StyxWoW.Me.HasAura(5215);

            /* Bear only */
            if (CurrentShape == ShapeshiftForm.Bear)
            {
                _attackpower = StyxWoW.Me.AttackPower;
                _agi = StyxWoW.Me.Agility;
                _stamina = StyxWoW.Me.Stamina;
                _calcfrenziedheal = Math.Max((_attackpower - _agi*2), (_stamina*2.5));
                _healthmissing = StyxWoW.Me.MaxHealth - StyxWoW.Me.CurrentHealth;
                buff.savage_defense.down = !StyxWoW.Me.HasBuff(62606) && !StyxWoW.Me.HasBuff(132402);
                buff.savage_defense.up = StyxWoW.Me.HasBuff(62606) || StyxWoW.Me.HasBuff(132402);
                debuff.weakened_blows.remains = StyxWoW.Me.CurrentTarget == null
                    ? 0
                    : StyxWoW.Me.CurrentTarget.MyAuraRemains(115798);
                cooldown.berserk.remains = Math.Min(Spell.GetSpellCooldown(106951), Spell.GetSpellCooldown(50334));
            }

            /* Cat only */
            if (CurrentShape == ShapeshiftForm.Cat)
            {
                EnergyRegen = 10 + (StyxWoW.Me.GetCombatRating(WoWPlayerCombatRating.HasteMelee)/425*0.1);
                charges = talent.force_of_nature.enabled ? LuaGetSpellCharges() : 0;
                combo_points = StyxWoW.Me.ComboPoints;
                time_to_max = (100 - _energy)/EnergyRegen;
                buff.dream_of_cenarius_damage.up = talent.dream_of_cenarius.enabled && StyxWoW.Me.HasBuff(145152);
                buff.dream_of_cenarius.down = !buff.dream_of_cenarius_damage.up;
                buff.feral_fury.react = StyxWoW.Me.HasBuff(144865);
                buff.feral_rage.up = StyxWoW.Me.HasBuff(146874);
                buff.feral_rage.remains = StyxWoW.Me.MyBuffRemains(146874);
                buff.king_of_the_jungle.up = talent.incarnation.enabled &&
                                             StyxWoW.Me.HasBuff("Incarnation: King of the Jungle");
                buff.king_of_the_jungle.down = !buff.king_of_the_jungle.up;
                buff.omen_of_clarity.react = StyxWoW.Me.HasBuff(135700);
                buff.predatory_swiftness.down = !buff.predatory_swiftness.up;
                buff.predatory_swiftness.remains = StyxWoW.Me.MyBuffRemains(69369);
                buff.rune_of_reorigination.up = StyxWoW.Me.HasBuff(139120);
                buff.rune_of_reorigination.remains = buff.rune_of_reorigination.up
                    ? StyxWoW.Me.MyBuffRemains(139120)
                    : 0;
                buff.savage_roar.up = _currentSpec != WoWSpec.DruidFeral ||
                                      (StyxWoW.Me.HasBuff(127538) || StyxWoW.Me.HasBuff(52610));
                buff.savage_roar.down = !buff.savage_roar.up;
                buff.savage_roar.remains = _currentSpec != WoWSpec.DruidFeral
                    ? 99
                    : (buff.savage_roar.up
                        ? Math.Max(StyxWoW.Me.MyBuffRemains(127538),
                            StyxWoW.Me.MyBuffRemains(52610))
                        : 0);
                buff.stampede.up = StyxWoW.Me.HasBuff(81022);
                buff.tigers_fury.up = _currentSpec != WoWSpec.DruidFeral || StyxWoW.Me.HasBuff(5217);
                cooldown.incarnation.remains = Spell.GetSpellCooldownONGCD(102543);
                cooldown.tigers_fury.remains = Spell.GetSpellCooldown(5217);
                debuff.weakened_armor.stack = StyxWoW.Me.CurrentTarget != null
                    ? StyxWoW.Me.CurrentTarget.StackCount("Weakened Armor")
                    : 0;
                dot.rip.ticking = StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.HasMyDebuff(1079);
                dot.rip.remains = EventHandlers.RealRipTimeLeft;
                dot.rake.ticking = StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.HasMyDebuff(1822);
                dot.rake.remains = dot.rake.ticking ? StyxWoW.Me.CurrentTarget.MyAuraRemains(1822) : 0;
                dot.rake.ticks_remain = CalcRakeTicksRemaining();
                dot.thrash_cat.remains = StyxWoW.Me.CurrentTarget == null
                    ? 0
                    : StyxWoW.Me.CurrentTarget.MyAuraRemains("Thrash");
                buff.trinket.procagilityreact = getThatShit();
            }
        }

        private static bool getThatShit()
        {
            if (StyxWoW.Me.HasBuff(146310))
            {
                return true;
            }

            bool Renataki = false;
            bool RoR = false;
            int TrinketCount = 0;

            if (StyxWoW.Me.HasBuff(138756))
            {
                Renataki = true;

                if (StyxWoW.Me.MyBuffRemains(138756) < 2)
                {
                    return true;
                }
            }

            if (StyxWoW.Me.HasBuff(139120))
            {
                RoR = true;

                if (StyxWoW.Me.MyBuffRemains(139120) < 2)
                {
                    return true;
                }
            }


            if (StyxWoW.Me.HasBuff(148903))
            {
                TrinketCount ++;
                if (!RoR && !Renataki)
                    if (StyxWoW.Me.MyBuffRemains(148903) < 2)
                        return true;
            }

            if (StyxWoW.Me.HasBuff(146308))
            {
                TrinketCount++;
                if (!RoR && !Renataki)
                    if (StyxWoW.Me.MyBuffRemains(146308) < 2)
                        return true;
            }

            if (StyxWoW.Me.HasBuff(148896))
            {
                TrinketCount++;
                if (!RoR && !Renataki)
                    if (StyxWoW.Me.MyBuffRemains(148896) < 2)
                        return true;
            }

            if (StyxWoW.Me.HasBuff(138699))
            {
                TrinketCount++;
                if (!RoR && !Renataki)
                    if (StyxWoW.Me.MyBuffRemains(138699) < 2)
                        return true;
            }

            if (StyxWoW.Me.HasBuff(148896))
            {
                TrinketCount++;
                if (!RoR && !Renataki)
                    if (StyxWoW.Me.MyBuffRemains(148896) < 2)
                        return true;
            }

            if (TrinketCount == 2)
                return true;
            return false;
        }

        public override void Pulse()
        {
            if (!StyxWoW.IsInGame || !StyxWoW.IsInWorld)
                return;
            PrepareVariables();
            if (CurrentShape == ShapeshiftForm.Bear
                && StyxWoW.Me.GroupInfo.IsInParty)
            {
                TankManager.Instance.Pulse();
            }

            if (StyxWoW.Me.CurrentTarget != null)
                DpsMeter.Update();
            Spell.PulseDoubleCastEntries();
            Spell.PulseLogEntries();
            CooldownTracker.PulseSpellCooldownEntries();

            Spell.PulseItemEntries();
            if (CurrentShape == ShapeshiftForm.Cat)
                SnapShotStats();
            GCD.PulseGCDCache();
        }

        private void SnapShotStats()
        {
            bool DoCSID = buff.dream_of_cenarius_damage.up;
            AP = StyxWoW.Me.AttackPower;
            Multiplier = Lua.GetReturnVal<double>("return UnitDamage(\"player\");", 6);
            Mastery = 1 + (((StyxWoW.Me.GetCombatRating(WoWPlayerCombatRating.Mastery)/192) + 25)*0.01);
            Crit = 1 + (StyxWoW.Me.CritPercent/100 - LevelBasedCritSuppression);

            if (DoCSID)
                Multiplier = Multiplier*1.3;
        }

        public override void Death()
        {
            if (!releaseOnce)
            {
                var rnd = new Random();
                _delay = rnd.Next(2000, 7000);
                _watch.Start();
                releaseOnce = true;
                Logging.Write("We are dead! Waiting " + _delay + " milliseconds!");
            }

            if (!_watch.IsRunning || _watch.ElapsedMilliseconds <= _delay) return;
            Logging.Write("Releasing Ghost!");
            Lua.DoString(string.Format("RunMacroText(\"{0}\")", "/script RepopMe()"));
            releaseOnce = false;
            _watch.Reset();
        }

        public override void Rest()
        {
            if (!StyxWoW.Me.HasBuff("Drink") && !StyxWoW.Me.HasBuff("Food"))
            {
                if (buff.predatory_swiftness.up && HasSpellHealingTouch &&
                    _healthPercent <= SuperbadSettings.Instance.OoCReju)
                {
                    if (healing_touch())
                        return;
                }
                if (_healthPercent <= SuperbadSettings.Instance.OoCReju && !buff.rejuvenation.up)
                    if (rejuvenation())
                        return;
                if (_healthPercent <= SuperbadSettings.Instance.OoCHealingTouch)
                {
                    if (healing_touch_non_instant())
                        return;
                }
            }
            if (SuperbadSettings.Instance.UseRest &&
                !StyxWoW.Me.IsSwimming && _healthPercent <= SuperbadSettings.Instance.RestHealth &&
                !StyxWoW.Me.HasBuff("Food") &&
                Consumable.GetBestFood(false) != null)
            {
                if (StyxWoW.Me.IsMoving)
                    Navigator.PlayerMover.MoveStop();
                Styx.CommonBot.Rest.FeedImmediate();
            }

            if (SuperbadSettings.Instance.UseRest &&
                !StyxWoW.Me.IsSwimming && StyxWoW.Me.ManaPercent <= SuperbadSettings.Instance.RestMana &&
                !StyxWoW.Me.HasBuff("Drink") &&
                Consumable.GetBestDrink(false) != null)
            {
                if (StyxWoW.Me.IsMoving)
                    Navigator.PlayerMover.MoveStop();
                Styx.CommonBot.Rest.DrinkImmediate();
            }
        }

        private static void HandleModifierStateChanged(object sender, LuaEventArgs args)
        {
            if (SuperbadSettings.Instance.PauseKey == SuperbadSettings.Keypress.NONE &&
                SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.NONE &&
                SuperbadSettings.Instance.AoeKey == SuperbadSettings.Keypress.NONE &&
                SuperbadSettings.Instance.GrowlKey == SuperbadSettings.Keypress.NONE
                )
                return;

            if (args.Args[0].ToString() == SuperbadSettings.Instance.PauseKey.ToString() &&
                args.Args[1].ToString() == "1")
            {
                if (!Paused)
                {
                    Paused = true;
                    Logging.Write("Superbad PAUSED, press {0} in WOW to continue",
                        SuperbadSettings.Instance.PauseKey);
                    if (SuperbadSettings.Instance.PrintMsg)
                        Lua.DoString(
                            "RaidNotice_AddMessage(RaidWarningFrame, \"Superbad PAUSED\", ChatTypeInfo[\"RAID_WARNING\"]);");
                    return;
                }
                Paused = false;
                Logging.Write("Superbad Running....");
                if (SuperbadSettings.Instance.PrintMsg)
                    Lua.DoString(
                        "RaidNotice_AddMessage(RaidWarningFrame, \"Superbad Resumed\", ChatTypeInfo[\"RAID_WARNING\"]);");
            }
            if (args.Args[0].ToString() == SuperbadSettings.Instance.BurstKey.ToString() &&
                args.Args[1].ToString() == "1")
            {
                BurstMode = true;
                if (!BurstMode) return;
                Logging.Write("BurstMode activated");
                if (SuperbadSettings.Instance.PrintMsg)
                    Lua.DoString(
                        "RaidNotice_AddMessage(RaidWarningFrame, \"Burstmode activated\", ChatTypeInfo[\"RAID_WARNING\"]);");
            }

            if (args.Args[0].ToString() == SuperbadSettings.Instance.AoeKey.ToString() &&
                args.Args[1].ToString() == "1" && SuperbadSettings.Instance.UseAoeKey)
            {
                AoeMode = !AoeMode;

                if (AoeMode)
                {
                    Logging.Write("Aoe mode activated");
                    if (SuperbadSettings.Instance.PrintMsg)
                        Lua.DoString(
                            "RaidNotice_AddMessage(RaidWarningFrame, \"Aoe mode activated\", ChatTypeInfo[\"RAID_WARNING\"]);");
                }
                if (!AoeMode)
                {
                    Logging.Write("single target mode activated");
                    if (SuperbadSettings.Instance.PrintMsg)
                        Lua.DoString(
                            "RaidNotice_AddMessage(RaidWarningFrame, \"single target mode activated\", ChatTypeInfo[\"RAID_WARNING\"]);");
                }
            }

            if (args.Args[0].ToString() == SuperbadSettings.Instance.GrowlKey.ToString() &&
                args.Args[1].ToString() == "1")
            {
                SuperbadSettings.Instance.UseTaunt = !SuperbadSettings.Instance.UseTaunt;

                if (SuperbadSettings.Instance.UseTaunt)
                {
                    Logging.Write("Growl activated");
                    if (SuperbadSettings.Instance.PrintMsg)
                        Lua.DoString(
                            "RaidNotice_AddMessage(RaidWarningFrame, \"Aoe mode activated\", ChatTypeInfo[\"RAID_WARNING\"]);");
                }
                if (!SuperbadSettings.Instance.UseTaunt)
                {
                    Logging.Write("Growl deactivated");
                    if (SuperbadSettings.Instance.PrintMsg)
                        Lua.DoString(
                            "RaidNotice_AddMessage(RaidWarningFrame, \"single target mode activated\", ChatTypeInfo[\"RAID_WARNING\"]);");
                }
            }
        }
    }
}