using Action = Styx.TreeSharp.Action;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Diagnostics;
using Styx.Pathing;

namespace Boomkin
{
    public partial class Classname : CombatRoutine
    {
        public override string Name { get { return "Boomkin by Pasterke"; } }

        public override WoWClass Class { get { return WoWClass.Druid; } }

        LocalPlayer Me { get { return StyxWoW.Me; } }

        public override bool WantButton { get { return true; } }

        public override void OnButtonPress()
        {
            Form1 ConfigForm = new Form1();
            ConfigForm.ShowDialog();
        }

        public override void Initialize()
        { 
            Updater.CheckForUpdate();
            Lua.Events.AttachEvent("MODIFIER_STATE_CHANGED", HandleModifierStateChanged);
        }

        public bool RemoveDeadTargets = true;

        public override void Pulse()
        {
        }

        #region rest
        public override bool NeedRest
        {
            get
            {
                if (SpellManager.HasSpell("Rejuvenation")
                    && Me.HealthPercent <= CRSettings.myPrefs.Rejuvenation
                    && Me.HealthPercent > CRSettings.myPrefs.HealingTouch
                    && !buffExists("Rejuvenation", Me)
                    && !Me.Mounted
                    && !MeInGroup
                    && !Me.IsDead
                    && Me.IsGhost)
                {
                    return true;
                }
                if (SpellManager.HasSpell("Healing Touch")
                    && Me.HealthPercent <= CRSettings.myPrefs.HealingTouch
                    && Me.HealthPercent > CRSettings.myPrefs.Eat
                    && !Me.Mounted
                    && !MeInGroup
                    && !Me.IsDead
                    && Me.IsGhost)
                {
                    return true;
                }
                if (Me.HealthPercent <= CRSettings.myPrefs.Eat
                    && !Me.Mounted
                    && !MeInGroup
                    && !Me.IsDead
                    && Me.IsGhost
                    && !Me.IsSwimming)
                {
                    return true;
                }
                if (Me.ManaPercent <= CRSettings.myPrefs.Drink
                    && !Me.Mounted
                    && !MeInGroup
                    && !Me.IsDead
                    && Me.IsGhost
                    && !Me.IsSwimming)
                {
                    return true;
                }
                return false;
            }
        }

        public override void Rest()
        {
            if (SpellManager.HasSpell("Rejuvenation")
                    && Me.HealthPercent <= CRSettings.myPrefs.Rejuvenation
                    && Me.HealthPercent > CRSettings.myPrefs.HealingTouch
                    && !buffExists("Rejuvenation", Me)
                    && !Me.Mounted
                    && !MeInGroup
                    && !Me.IsDead
                    && Me.IsGhost)
            {
                SpellManager.Cast("Rejuvenation", Me);
                LogMsg("Rejuvenation", 2);
            }
            if (SpellManager.HasSpell("Healing Touch")
                && Me.HealthPercent <= CRSettings.myPrefs.HealingTouch
                && Me.HealthPercent > CRSettings.myPrefs.Eat
                && !Me.Mounted
                && !MeInGroup
                && !Me.IsDead
                && Me.IsGhost)
            {
                SpellManager.Cast("Healing Touch", Me);
                LogMsg("Healing Touch", 2);
            }
            if (Me.HealthPercent <= CRSettings.myPrefs.Eat
                && !Me.Mounted
                && !MeInGroup
                && !Me.IsDead
                && Me.IsGhost
                && !Me.IsSwimming)
            {
                Styx.CommonBot.Rest.Feed();
            }
            if (Me.ManaPercent <= CRSettings.myPrefs.Drink
                && !Me.Mounted
                && !MeInGroup
                && !Me.IsDead
                && Me.IsGhost
                && !Me.IsSwimming)
            {
                Styx.CommonBot.Rest.Feed();
            }
        }

        #endregion


        #region Pre Combat Buffs

        public override bool NeedPreCombatBuffs
        {
            get
            {
                if (!HaveStatsBuffs
                    && !Me.Mounted)
                {
                    CastBuff("Mark of the Wild");
                }
                if (!Me.Combat
                    && Me.IsSwimming
                    && !Me.Mounted
                    && Me.Shapeshift != ShapeshiftForm.Aqua)
                {
                    CastSpell("Aquatic Form");
                }
                return false;
            }
        }

        public override void PreCombatBuff() { }

        #endregion

        #region pullbuffs
        public override bool NeedPullBuffs
        {
            get
            {
                if (SpellManager.HasSpell("Moonkin Form")
                    && Me.Shapeshift != ShapeshiftForm.Moonkin)
                {
                    CastBuff("Moonkin Form");
                }
                return false;
            }
        }
        #endregion pulbuffs


        #region pull
        public override void Pull()
        {
            if(gotTarget && !CRSettings.myPrefs.Movement && Me.CurrentTarget.Distance > 35) 
            {
                while (Me.CurrentTarget.Distance > 35)
                {
                    Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                }
                Navigator.PlayerMover.MoveStop();
            }
            if (gotTarget && !CRSettings.myPrefs.Movement && !Me.CurrentTarget.InLineOfSight)
            {
                while (!Me.CurrentTarget.InLineOfSight)
                {
                    Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                }
                Navigator.PlayerMover.MoveStop();
            }
            if (gotTarget && Me.CurrentTarget.Distance <= 35 && Me.IsMoving)
            {
                Navigator.PlayerMover.MoveStop();
            }
            
            Me.CurrentTarget.Face();

            if (gotTarget && !SpellManager.HasSpell("Moonfire"))
            {
                CastSpell("Wrath");
            }
            CastSpell("Moonfire");
        }
        #endregion pull

        #region Combat Buffs

        public override bool NeedCombatBuffs
        {
            get
            {
                if (gotTarget
                    && SpellManager.HasSpell("Moonkin Form")
                    && Me.Shapeshift != ShapeshiftForm.Moonkin)
                {
                    CastBuff("Moonkin Form");
                }
                if (gotTarget
                    && !HaveStatsBuffs
                    && !Me.Mounted)
                {
                    CastBuff("Mark of the Wild");
                }
                if (gotTarget
                    && !spellOnCooldown("Lifeblood")
                    && CRSettings.myPrefs.useLifeblood
                    && !HaveHasteBuff)
                {
                    CastSpell("Lifeblood");
                }
                if (!buffExists("Rejuvenation", Me)
                    && Me.HealthPercent <= CRSettings.myPrefs.Rejuvenation)
                {
                    CastBuff("Rejuvenation");
                }
                if (!spellOnCooldown("Renewal")
                    && Me.HealthPercent <= CRSettings.myPrefs.Renewal)
                {
                    CastSpell("Renewal");
                }
                if (!spellOnCooldown("Nature's Swiftness")
                    && Me.HealthPercent <= CRSettings.myPrefs.NaturesSwiftness)
                {
                    CastSpell("Nature's Swiftness");
                }
                if (buffExists("Nature's Swiftness", Me))
                {
                    CastBuff("Healing Touch");
                }
                if (Me.HealthPercent <= CRSettings.myPrefs.HealingTouch)
                {
                    CastBuff("Healing Touch");
                }
                if (!spellOnCooldown("Cenarion Ward")
                    && Me.HealthPercent <= CRSettings.myPrefs.CenarionWardPercent)
                {
                    CastBuff("Cenarion Ward");
                }
                if (!spellOnCooldown("Barkskin")
                    && Me.HealthPercent < CRSettings.myPrefs.BarskinPercent)
                {
                    CastSpell("Barkskin");
                }
                useAlchemistFlask();
                UseTrinket1();
                UseTrinket2();
                useFlask();
                useJadePotion();
                useHealthStone();
                return false;
            }
        }

        public override void CombatBuff() { }

        #endregion


        #region Combat

        public override void Combat()
        {
            if (Paused) { return; }

            if (gotTarget && addCount >= CRSettings.myPrefs.startAoe && !DisableAoe) { AoeRotation(); }

            if (!gotTarget && !CRSettings.myPrefs.Targeting)
            {
                getTarget();
            }
            if (gotTarget && !CRSettings.myPrefs.Movement && Me.CurrentTarget.Distance > 35)
            {
                while (Me.CurrentTarget.Distance > 35)
                {
                    Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                }
                Navigator.PlayerMover.MoveStop();
            }
            if (gotTarget && !CRSettings.myPrefs.Movement && !Me.CurrentTarget.InLineOfSight)
            {
                while (!Me.CurrentTarget.InLineOfSight)
                {
                    Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                }
                Navigator.PlayerMover.MoveStop();
            }
            if (gotTarget && Me.CurrentTarget.Distance <= 35 && Me.IsMoving)
            {
                Navigator.PlayerMover.MoveStop();
            }
            if (gotTarget && !CRSettings.myPrefs.Facing)
            {
                Me.CurrentTarget.Face();
            }
            if (gotTarget
                    && Me.ManaPercent <= CRSettings.myPrefs.innervate
                    && !spellOnCooldown("Innervate"))
            {
                CastBuff("Innervate");
            }
            if (gotTarget
                    && !debuffExists(FF, Me.CurrentTarget)
                    && !spellOnCooldown(FF)
                    && IsInRange(39, Me.CurrentTarget))
            {
                CastSpell(FF);
            }
            if (gotTarget
                && buffExists("Shooting Stars", Me))
            {
                CastSpell("Starsurge");
            }
            if (gotTarget
                    && !debuffExists("Moonfire", Me.CurrentTarget)
                    && IsInRange(39, Me.CurrentTarget))
            {
                CastSpell("Moonfire");
            }
            if (gotTarget
                && !spellOnCooldown("Nature's Vigil")
                && !buffExists("Nature's Vigil", Me)
                && (buffExists(48518, Me) // lunar = 48518
                || buffExists(48517, Me))) // solar = 48517
            {
                CastSpell("Nature's Vigil");
            }
            if (gotTarget
                    && !spellOnCooldown("Incarnation")
                    && IsWoWBoss(Me.CurrentTarget)
                    && buffExists("Nature's Vigil", Me)
                    && IsInRange(39, Me.CurrentTarget))
            {
                CastSpell("Incarnation");
            }
            if (gotTarget
                    && !debuffExists("Sunfire", Me.CurrentTarget)
                    && IsInRange(39, Me.CurrentTarget))
            {
                CastSpell("Sunfire");
            }
            if (gotTarget
                    && !spellOnCooldown("Celestial Alignment")
                    && !buffExists("Nature's Vigil", Me)
                    && (Me.EclipsePercent <= 20 && Me.EclipsePercent >= -20)
                    && (CRSettings.myPrefs.celestial == 1
                    || (CRSettings.myPrefs.celestial == 2 && IsWoWBoss(Me.CurrentTarget))))
            {
                CastSpell("Celestial Alignment");
            }
            if (gotTarget
                    && !spellOnCooldown("Typhoon")
                    && UnfriendlyUnits.Count() > CRSettings.myPrefs.typhoon)
            {
                CastSpell("Typhoon");
            }
            if (gotTarget
                    && !spellOnCooldown("Starsurge")
                    && IsInRange(39, Me.CurrentTarget))
            {
                CastSpell("Starsurge");
            }
            if (gotTarget
                    && !HaveHasteBuff
                    && SpellManager.HasSpell("Berserking")
                    && !spellOnCooldown("Berserking")
                    && (CRSettings.myPrefs.useBerserking == 1
                    || (CRSettings.myPrefs.useBerserking == 2 && IsWoWBoss(Me.CurrentTarget)))
                    && IsInRange(39, Me.CurrentTarget))
            {
                CastSpell("Berserking");
            }
            if (gotTarget
                    && !spellOnCooldown("Starfall")
                    && buffExists(48518, Me)) //Eclipse (Lunar)
            {
                CastSpell("Starfall");
            }
            if (gotTarget && buffExists("Eclipse (Lunar)", Me)
                    && IsInRange(39, Me.CurrentTarget))
            {
                CastSpell("Starfire");
            }
            if (gotTarget
                    && IsInRange(39, Me.CurrentTarget))
            {
                CastSpell("Wrath");
            }
        }

        public void AoeRotation()
        {
            if (!gotTarget && CRSettings.myPrefs.Targeting)
            {
                getTarget();
            }
            if (gotTarget && !CRSettings.myPrefs.Movement && Me.CurrentTarget.Distance > 29)
            {
                Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                return;
            }
            if (gotTarget && !CRSettings.myPrefs.Movement)
            {
                Navigator.PlayerMover.MoveStop();
            }
            if (gotTarget && !CRSettings.myPrefs.Facing)
            {
                Me.CurrentTarget.Face();
            }

            if (gotTarget
                && !spellOnCooldown("Starfall")
                && buffExists(48518, Me))
            {
                CastSpell("Starfall");
            }

            if (gotTarget
                    && IsInRange(29, Me.CurrentTarget)
                    && !Me.IsChanneling)
            {
                CastSpell("Hurricane");
                SpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
            }
            return;
        }
        #endregion combat
    }
}