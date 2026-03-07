/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Priest/Shadow.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade: $
 */

using System.Linq;
using JetBrains.Annotations;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx;
using CommonBehaviors.Actions;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace PureRotation.Classes.Priest
{
    [UsedImplicitly]
    internal class Shadow : RotationBase
    {
        //private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }
        private static PriestSettings PriestSettings { get { return PRSettings.Instance.Priest; } }
        private static readonly double ClientLagMs = StyxWoW.WoWClient.Latency;

        #region Cool-Downs
        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Fade", on => Me,
                           ret => Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 30) > 0 && !Spell.IsChanneling),
                Spell.Cast("Power Word: Shield",
                           ret => !Me.HasAura("Weakened Soul") && Me.HealthPercent < PowerWordShieldPercent),
                Spell.Cast("Dispersion", on => Me, ret => Me.HealthPercent < 15),
                Spell.Cast("Vampiric Embrace", ret => Me.HealthPercent < PriestSettings.ShadowVampiricEmbracePercent),

                new Decorator(ret => PriestSettings.ShadowUseVoidShift && Me.HealthPercent < 25 && !Me.HasAura("Dispersion"), HandleVoidShift()),

                new Decorator(
                    ret =>
                    Me.HealthPercent < 100 && PRSettings.Instance.EnableSelfHealing &&
                    TalentManager.HasGlyph("Dark Binding"),
                    new PrioritySelector(
                        Spell.Cast("Renew", on => Me, ret => !Me.HasAura("Renew") && Me.CurrentHealth <= 60),
                        Spell.Cast("Prayer of Mending", on => Me,
                                   ret => !Me.HasAura("Prayer of Mending") && Me.CurrentHealth <= 40)
                        )));
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Cascade", ret => TalentManager.HasTalent(16)),
                Spell.Cast("Divine Star", ret => TalentManager.HasTalent(17)),
                Spell.Cast("Halo", ret => PriestSettings.ShadowUseHalo && TalentManager.HasTalent(18) && Me.CurrentTarget != null && Me.CurrentTarget.Distance > 22 && Me.CurrentTarget.Distance <= 29), // See link for reasoning behind range: http://www.wowhead.com/spell=120517/halo#comments

                Spell.Cast("Dispersion", on => Me, ret => Me.ManaPercent < DispersionPercent),
                Spell.Cast("Power Infusion", on => Me, ret => TalentManager.HasTalent(14) && !Me.HasAnyAura(Spell.HeroismBuff) && NeedDevouringPlague),
                Spell.Cast("Shadowfiend", ret => !TalentManager.HasTalent(8)),
                Spell.Cast("Mindbender", ret => TalentManager.HasTalent(8)),
                Spell.PreventDoubleChannel("Hymn of Hope", 3, true, ret => Me.ManaPercent <= PriestSettings.HymnofHopeMana)
                );
        }
        #endregion

        #region Rotation Handles (Single / AoE)
        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                //MindFlayDebug(),

                        // Force mind flay when Insanity.
                        Spell.PreventDoubleChannel("Mind Flay", 0.5, false, on => Me.CurrentTarget, ret => Me.CurrentTarget!=null && Me.CurrentTarget.HasAura("Devouring Plague") && TalentManager.HasTalent(9), false),

                        // Force cast to get a 3rd insanity in.
                        Spell.PreventDoubleCastNoCanCast("Mind Flay", 0.5, on => Me.CurrentTarget, ret => Me.CurrentTarget != null && Me.CurrentTarget.HasAura("Devouring Plague") && Spell.GetAuraTimeLeft("Devouring Plague", Me.CurrentTarget) < 0.3 && TalentManager.HasTalent(9)),

                        // don't go any further into rotation when insanity.
                        new Decorator(a => Me.CurrentTarget != null && Me.CurrentTarget.HasAura("Devouring Plague") || Me.ChanneledCastingSpellId == 129197, new ActionAlwaysSucceed()), 

                        // stop MF cast
                        new Decorator(a => !CanMindFlay && Me.ChanneledCastingSpellId == 15407, new Action(delegate { SpellManager.StopCasting(); return RunStatus.Failure; })),
                        
                        //new Decorator(ret => PriestSettings.ReapplyDoTsWhenBuffed && CasterStatBuffActive, new Action(delegate { Logger.InfoLog("Stats Increased! Reapplying DoTs (Int[{0}], Mastery[{1}], SpellPower[{2}])", Lua._secondaryStats.Intellect, Lua._secondaryStats.Mastery, Lua._secondaryStats.SpellPower); return RunStatus.Failure; })),
                        Spell.PreventDoubleCast(SpellBook.ShadowWordPain, 1, ret => NeedShadowWordPain || (PriestSettings.ReapplyDoTsWhenBuffed && CasterStatBuffActive)),
                        Spell.PreventDoubleCast(SpellBook.VampiricTouch, 2.2, ret => NeedVampiricTouch || (PriestSettings.ReapplyDoTsWhenBuffed && CasterStatBuffActive)),

                        // SW:P must always be higher prio than dev plague
                        Spell.PreventDoubleCast("Devouring Plague", 0.5, on => Me.CurrentTarget, ret => NeedDevouringPlague, true),

                        Spell.PreventDoubleCast("Shadow Word: Death", 0.5, on => Me.CurrentTarget, ret => Me.CurrentTarget != null && Me.CurrentTarget.HealthPercent < 20, true),
                        Spell.PreventDoubleCast("Mind Blast", 0.5, on => Me.CurrentTarget, ret => Me.CurrentTarget!=null, Me.HasAura(124430/*Divine Insight*/)),

                        new Decorator(ret => PriestSettings.MultiDoTSingleTarget, HandleMultiDoT()),

                        Spell.Cast("Divine Star", ret => Me.IsSafelyFacing(Me.CurrentTarget, 45f)),
                        Spell.Cast("Mind Spike", ret => Me.HasAura("Surge of Darkness")),
                        Spell.PreventDoubleChannel("Mind Flay", 0.5, false, on => Me.CurrentTarget, ret => CanMindFlay, false),

                        // Moving
                        Spell.PreventDoubleCast("Shadow Word: Pain", 2, on => Me.CurrentTarget, ret => NeedShadowWordPainMoving, true)
                        );
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(ctx => GetExecuteUnit(),
                Spell.Cast("Devouring Plague", ret => NeedDevouringPlague),
                new Decorator(ret => ((WoWUnit)ret) != null, Spell.Cast("Shadow Word: Death", ret => ((WoWUnit)ret), ret => ((WoWUnit)ret) != null && _nearbyAoEUnitCount < 6)),
                Spell.PreventDoubleCast("Mind Blast", 0.5, on => Me.CurrentTarget, ret => Me.CurrentTarget != null && _nearbyAoEUnitCount < 6),
                Spell.PreventDoubleCast("Mind Spike", 0.5, on => Me.CurrentTarget, ret => Me.CurrentTarget != null && _nearbyAoEUnitCount < 6 && Me.HasAura("Surge of Darkness")),
                
                Spell.PreventDoubleChannel("Mind Sear", 0.5, true, ret => _nearbyAoEUnitCount >= PriestSettings.MindSearCount),
                HandleMultiDoT(),
                new Decorator(ret => _nearbyAoEUnitCount < PriestSettings.MindSearCount, HandleSingleTarget())
                
                );
        }
        #endregion

        #region Helpers / Aquire Unit / Other

        private static Composite HandleMultiDoT()
        {
            return new PrioritySelector(
                new Decorator(
                    ret =>
                    PriestSettings.MultiDoTStyle == MultiDoTMethod.AroundTarget,
                    new PrioritySelector(
                        Spell.PreventDoubleMultiDoT("Vampiric Touch", 3, Me.CurrentTarget, 15, 3,
                                       ret => Me.CurrentTarget != null && Me.ManaPercent < PriestSettings.ShadowManaPercentToMultiDoTVT && Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 50).Count(x => x.HasAura(SpellBook.VampiricTouch)) <= PriestSettings.ShadowMaxUnitsToMultiDoTVT),
                        Spell.PreventDoubleMultiDoT("Shadow Word: Pain", 2, Me.CurrentTarget, 15, 3,
                                       ret => Me.CurrentTarget != null && Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 50).Count(x => x.HasAura(SpellBook.ShadowWordPain)) <= PriestSettings.ShadowMaxUnitsToMultiDoTSWP)
                        )
                    ),

                new Decorator(
                    ret =>
                    PriestSettings.MultiDoTStyle == MultiDoTMethod.EverythingAroundMe,
                    new PrioritySelector(
                        Spell.PreventDoubleMultiDoT("Vampiric Touch", 3, Me, 40, 3,
                                       ret => Me.ManaPercent < PriestSettings.ShadowManaPercentToMultiDoTVT && Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 50).Count(x => x.HasAura(SpellBook.VampiricTouch)) <= PriestSettings.ShadowMaxUnitsToMultiDoTVT),
                        Spell.PreventDoubleMultiDoT("Shadow Word: Pain", 2, Me, 40, 3,
                                       ret => Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 50).Count(x => x.HasAura(SpellBook.ShadowWordPain)) <= PriestSettings.ShadowMaxUnitsToMultiDoTSWP)
                        )
                    ));

        }

        private static Composite HandleVoidShift()
        {
            WoWUnit voidShiftUnit = null;
            return new PrioritySelector(ctx => voidShiftUnit = VoidShiftTarget(),
                Spell.Cast("Void Shift", on => voidShiftUnit, ret => voidShiftUnit != null && voidShiftUnit.HealthPercent > Me.HealthPercent)
                );

        }

        private static WoWUnit VoidShiftTarget()
        {
            return Unit.GroupMembers.Where(u => u.CanSelect && !u.IsDead && !u.IsMe && u.IsFriendly
                        && u != Me.CurrentTarget.CurrentTarget && !Unit.Tanks.Contains(u))
                    .OrderByDescending(u => u.HealthPercent)
                    .FirstOrDefault(); // try to not switch with the tank!!
        }

        private static WoWUnit GetExecuteUnit()
        {
            return
                Unit.CachedNearbyAttackableUnitsAttackingUs(Me.CurrentTarget.Location, 30)
                    .Where(x => x.HealthPercent < 20)
                    .OrderByDescending(x => x.HealthPercent)
                    .FirstOrDefault();
        }

// ReSharper disable UnusedMember.Local
        private static Composite MindFlayDebug()
        {
            double clipMs = (ClientLagMs * 2) + 150;
            return new PrioritySelector(
                new Decorator(ret => !Me.IsChanneling, new Action(delegate { Logger.InfoLog("[Allow Cast] Not Channeling"); return RunStatus.Failure; })),
                new Decorator(ret => Spell.CanClipChannel() && Me.CurrentChannelTimeLeft.TotalMilliseconds >= clipMs, new Action(delegate { Logger.InfoLog("[Prevent Clip] Channeling [{0}] - Time Remaining on Channel[{1}] - Clip When Under[{2}]", Me.ChanneledSpell.Name, Me.CurrentChannelTimeLeft.TotalMilliseconds, clipMs); return RunStatus.Failure; })),
                new Decorator(ret => Spell.CanClipChannel() && Me.CurrentChannelTimeLeft.TotalMilliseconds < clipMs, new Action(delegate { Logger.InfoLog("[Allow Clip] Channeling [{0}] - Time Remaining on Channel[{1}] - Clip When Under[{2}]", Me.ChanneledSpell.Name, Me.CurrentChannelTimeLeft.TotalMilliseconds, clipMs); return RunStatus.Failure; }))
                );
        }
// ReSharper restore UnusedMember.Local

        private static int _nearbyAoEUnitCount;
        //private static int _allUnitsAttackingUsCount;
        private static Composite SetCounts
        {
            get
            {
                return new Action(delegate
                {
                    _nearbyAoEUnitCount = Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count();
                    //_allUnitsAttackingUsCount = Unit.NearbyAttackableUnitsAttackingUs(Me.Location, 40).Count();
                    //Logger.InfoLog("Units Near Target [{0}] - Units In Range [{1}]", _nearbyAoEUnitCount, _allUnitsAttackingUsCount);
                    return RunStatus.Failure;
                });

            }
        }
        #endregion

        #region DoT Calculations

        // buffer (start casting when it hits this many seconds)
        // traveltime (1.5 seconds default..but some spells have a longer time..see Traveltime(double travelSpeed).)
        // casttime (provide the casttime of the spell)
        // Latency?? hrm.. see Clientlag

        private static readonly double Clientlag = StyxWoW.WoWClient.Latency * 2 / 1000.0;

        private static double DoTRefreshTotalSeconds(double buffer, double traveltime, double casttime)
        {
            {
                //Logger.DebugLog("DoTRefreshTotalSeconds: Buffer: {0} + traveltime: {1} + casttime: {2} + Clientlag: {3}", buffer, traveltime, casttime, Clientlag);
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
                    Logger.DebugLog("Travel time: {0}", t);
                    return t;
                }

                return 0;
            }
        }

        #endregion

        #region booleans

        private static bool CanMindFlay
        {
            get
            {

                //if (Spell.GetSpellCooldown("Mind Blast").TotalSeconds < (Spell.GetSpellCastTime("Mind Blast") - Clientlag)) 
                //    return false;

                if (Me.CurrentTarget != null && Me.CurrentTarget.HealthPercent < 20 && Spell.GetSpellCooldown("Shadow Word: Death").TotalSeconds < (0.5 - Clientlag))
                    return false;

                if (Spell.GetSpellCooldown("Mind Blast").TotalSeconds < (0.5 - Clientlag))
                    return false;

                //if (Me.HasAura("Divine Insight") && !Spell.SpellOnCooldown("Mind Blast"))
                if (SpellManager.CanCast("Mind Blast"))
                    return false;

                if (Me.ActiveAuras.ContainsKey("Surge of Darkness"))
                    return false;

                if (NeedDevouringPlague)
                    return false;

                if (Spell.GetAuraTimeLeft(SpellBook.ShadowWordPain, Me.CurrentTarget) < 3)
                    return false;

                if (Spell.GetAuraTimeLeft("Vampiric Touch", Me.CurrentTarget) < 4)
                    return false;


                return true;
            }
        }
        private static uint CurrentShadowOrbs { get { return Me.GetCurrentPower(WoWPowerType.ShadowOrbs); } }
        private static bool NeedShadowWordPain
        {
            get
            {
                return
                    Me.CurrentTarget != null && !Me.CurrentTarget.HasMyAura("Shadow Word: Pain")
                    ||
                    (
                        Me.CurrentTarget != null && Me.CurrentTarget.HasMyAura("Shadow Word: Pain")
                        &&
                        Spell.GetMyAuraTimeLeft("Shadow Word: Pain", Me.CurrentTarget) <
                        DoTRefreshTotalSeconds(2.67, Traveltime(0), Spell.GetSpellCastTime("Shadow Word: Pain"))
                    );
            }
        }
        private static bool NeedShadowWordPainMoving { get { return Me.CurrentTarget != null && Me.IsMoving() && (!Me.CurrentTarget.HasMyAura("Shadow Word: Pain") || (Me.CurrentTarget.HasMyAura("Shadow Word: Pain") && Spell.GetMyAuraTimeLeft("Shadow Word: Pain", Me.CurrentTarget) < DoTRefreshTotalSeconds(9, Traveltime(0), Spell.GetSpellCastTime("Shadow Word: Pain")))); } }
        private static bool CasterStatBuffActive
        {
            get
            {

                // DoTTracker returns the value you last cast the spell at
                //   We don't want it to refresh if you gained 10 intellect, so add on a decent figure (i.e. 500 - 5000)
                //   and then check it against your current stats. If your current stats are higher than what it was applied
                //   at plus the 'extra' - then you've got an active buff. We'd then want to refresh the DoT.
                return Me.CurrentTarget != null && DoTTracker.SpellStats(Me.CurrentTarget, SpellBook.ShadowWordPain).Intellect + PriestSettings.Shadow_ReapplyValue < Me.Intellect
                    ||
                    Me.CurrentTarget != null && DoTTracker.SpellStats(Me.CurrentTarget, SpellBook.ShadowWordPain).Mastery + PriestSettings.Shadow_ReapplyValue < Me.Mastery
                    ||
                    Me.CurrentTarget != null && DoTTracker.SpellStats(Me.CurrentTarget, SpellBook.ShadowWordPain).SpellPower + PriestSettings.Shadow_ReapplyValue < Me.SpellPower()
                    ;
            }
        }

        private static bool NeedDevouringPlague
        {
            get
            {
                return Me.CurrentTarget != null &&
                       CurrentShadowOrbs == 3 &&
                       Me.CurrentTarget.HasAllAuras(SpellBook.ShadowWordPain, SpellBook.VampiricTouch);
            }
        }
        private static bool NeedVampiricTouch { get { return !Me.IsMoving() && Me.CurrentTarget != null && (!Me.CurrentTarget.HasMyAura("Vampiric Touch") || (Me.CurrentTarget.HasMyAura("Vampiric Touch") && Spell.GetMyAuraTimeLeft("Vampiric Touch", Me.CurrentTarget) < DoTRefreshTotalSeconds(2.5, Traveltime(0), Spell.GetSpellCastTime("Vampiric Touch")))); } }

        #endregion

        #region Settings
        private static int PowerWordShieldPercent { get { return PriestSettings.ShadowPowerWordShieldPercent; } }
        private static int DispersionPercent { get { return PriestSettings.ShadowDispersionPercent; } }
        #endregion

        #region Overrides of RotationBase

        internal override string Help
        {
            get
            {
                return
@"
-----------------------------------------------
Special Key: Unassigned

Be sure to set up Hot Keys / Automatic mode
    and check the settings for how you want the
    rotation to run!

Latest Changes [24-Jun-2013]
- Changed priority to not cast MB or SW:D when 3 orbs
- Will shutdown DoT tracker correctly when setting disabled
- Added setting for DoT tracker reapplying DoTs
- Moving detection now ignores turning on the spot

Enjoy! ~ Millz
-----------------------------------------------
";
            }
        }

        internal override void OnPulse()
        {
            if (!DoTTracker.Initialized && PriestSettings.ReapplyDoTsWhenBuffed)
            {
                DoTTracker.Initialize();
            }
            
            if (DoTTracker.Initialized && !PriestSettings.ReapplyDoTsWhenBuffed)
            {
                DoTTracker.Shutdown();
            }
        }

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.PriestShadow; }
        }

        public override string Name
        {
            get { return "Shadow Priest"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                     new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                     CachedUnits.Pulse,
                     Racials.UseRacials(),
                     EncounterSpecific.HandleActionBarInterrupts(),
                     HandleDefensiveCooldowns(), // Stay Alive!
                     Item.HandleItems(),         // Pop Trinkets, Drink potions...
                     new Decorator(ret => Me.CurrentTarget != null, SetCounts),
                     new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget != null && Me.CurrentTarget.IsBoss() && !EncounterSpecific.HasBadAura, HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && _nearbyAoEUnitCount >= PriestSettings.AoECount && PRSettings.Instance.UseAoEAbilities, HandleAoeCombat()),
                                      new Decorator(ret => Me.CurrentTarget != null && _nearbyAoEUnitCount < PriestSettings.AoECount, HandleSingleTarget()))),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => Me.CurrentTarget != null && HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && _nearbyAoEUnitCount >= PriestSettings.AoECount && PRSettings.Instance.UseAoEAbilities, HandleAoeCombat()),
                                      new Decorator(ret => Me.CurrentTarget != null && _nearbyAoEUnitCount < PriestSettings.AoECount, HandleSingleTarget()))),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => Me.CurrentTarget != null && HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && HotKeyManager.IsAoe, HandleAoeCombat()),
                                      new Decorator(ret => Me.CurrentTarget != null && !HotKeyManager.IsAoe, HandleSingleTarget()))));
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
                return new PrioritySelector(HandleDefensiveCooldowns());
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(
                    ret => !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    new PrioritySelector(
                        Spell.Cast("Inner Fire", on => Me, ret => !Me.HasAura("Inner Fire")),
                        Spell.Cast("Shadowform", on => Me, ret => !Me.HasAura("Shadowform")),
                        Spell.CastRaidBuff("Power Word: Fortitude", ret => true)));
            }
        }
        #endregion
    }
}
