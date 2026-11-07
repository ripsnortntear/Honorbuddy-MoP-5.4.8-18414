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
            // Updater.CheckForUpdate(); // Disabled: defunct SVN endpoint
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
                    && !Me.IsGhost)
                {
                    return true;
                }
                if (SpellManager.HasSpell("Healing Touch")
                    && Me.HealthPercent <= CRSettings.myPrefs.HealingTouch
                    && Me.HealthPercent > CRSettings.myPrefs.Eat
                    && !Me.Mounted
                    && !MeInGroup
                    && !Me.IsDead
                    && !Me.IsGhost)
                {
                    return true;
                }
                if (Me.HealthPercent <= CRSettings.myPrefs.Eat
                    && !Me.Mounted
                    && !MeInGroup
                    && !Me.IsDead
                    && !Me.IsGhost
                    && !Me.IsSwimming)
                {
                    return true;
                }
                if (Me.ManaPercent <= CRSettings.myPrefs.Drink
                    && !Me.Mounted
                    && !MeInGroup
                    && !Me.IsDead
                    && !Me.IsGhost
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
                    && !Me.IsGhost)
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
                && !Me.IsGhost)
            {
                SpellManager.Cast("Healing Touch", Me);
                LogMsg("Healing Touch", 2);
            }
            if (Me.HealthPercent <= CRSettings.myPrefs.Eat
                && !Me.Mounted
                && !MeInGroup
                && !Me.IsDead
                && !Me.IsGhost
                && !Me.IsSwimming)
            {
                Styx.CommonBot.Rest.Feed();
            }
            if (Me.ManaPercent <= CRSettings.myPrefs.Drink
                && !Me.Mounted
                && !MeInGroup
                && !Me.IsDead
                && !Me.IsGhost
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
            if (!gotTarget || Me.CurrentTarget == null)
                return;

            if (!CRSettings.myPrefs.Movement && (Me.CurrentTarget.Distance > 35 || !Me.CurrentTarget.InLineOfSight))
            {
                Navigator.MoveTo(Me.CurrentTarget.Location);
                return;
            }

            if (Me.IsMoving)
                Navigator.PlayerMover.MoveStop();

            if (!CRSettings.myPrefs.Facing)
                Me.CurrentTarget.Face();

            if (!SpellManager.HasSpell("Moonfire"))
            {
                CastSpell("Wrath");
                return;
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
                UseEngiGloves();
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

            if (!gotTarget && !CRSettings.myPrefs.Targeting)
            {
                getTarget();
            }

            if (!gotTarget || Me.CurrentTarget == null)
                return;

            if (addCount >= CRSettings.myPrefs.startAoe && !DisableAoe)
            {
                AoeRotation();
                return;
            }

            if (!CRSettings.myPrefs.Movement && (Me.CurrentTarget.Distance > 35 || !Me.CurrentTarget.InLineOfSight))
            {
                Navigator.MoveTo(Me.CurrentTarget.Location);
                return;
            }

            if (Me.IsMoving)
                Navigator.PlayerMover.MoveStop();

            if (!CRSettings.myPrefs.Facing)
                Me.CurrentTarget.Face();
            bool inLunar = buffExists(48518, Me) || buffExists("Eclipse (Lunar)", Me);
            bool inSolar = buffExists(48517, Me) || buffExists("Eclipse (Solar)", Me);
            bool inEclipse = inLunar || inSolar;

            if (CRSettings.myPrefs.innervate > 0
                && Me.ManaPercent <= CRSettings.myPrefs.innervate
                && !spellOnCooldown("Innervate"))
            {
                CastSpell("Innervate");
                if (spellOnCooldown("Innervate")) return;
            }

            if (!debuffExists(FF, Me.CurrentTarget)
                && !spellOnCooldown(FF)
                && IsInRange(40, Me.CurrentTarget))
            {
                CastSpell(FF);
            }

            // Maintain DoTs (refresh if missing or about to expire)
            if ((!debuffExists("Moonfire", Me.CurrentTarget) || debuffTimeLeft("Moonfire", Me.CurrentTarget) < 3000)
                && IsInRange(40, Me.CurrentTarget))
            {
                CastSpell("Moonfire");
            }
            if ((!debuffExists("Sunfire", Me.CurrentTarget) || debuffTimeLeft("Sunfire", Me.CurrentTarget) < 3000)
                && IsInRange(40, Me.CurrentTarget))
            {
                CastSpell("Sunfire");
            }

            if (buffExists("Shooting Stars", Me) && IsInRange(40, Me.CurrentTarget))
            {
                CastSpell("Starsurge");
                if (spellOnCooldown("Starsurge")) return;
            }

            if (!spellOnCooldown("Nature's Vigil")
                && !buffExists("Nature's Vigil", Me)
                && (inEclipse || IsWoWBoss(Me.CurrentTarget) || addCount >= CRSettings.myPrefs.startAoe))
            {
                CastSpell("Nature's Vigil");
            }

            if (!spellOnCooldown("Incarnation")
                && IsWoWBoss(Me.CurrentTarget)
                && (SpellManager.HasSpell("Incarnation: Chosen of Elune") || SpellManager.HasSpell("Incarnation"))
                && IsInRange(40, Me.CurrentTarget))
            {
                if (SpellManager.CanCast("Incarnation: Chosen of Elune"))
                    SpellManager.Cast("Incarnation: Chosen of Elune");
                else
                    CastSpell("Incarnation");
            }

            if (CRSettings.myPrefs.celestial > 0
                && !spellOnCooldown("Celestial Alignment")
                && !buffExists("Celestial Alignment", Me)
                && (CRSettings.myPrefs.celestial == 1 || IsWoWBoss(Me.CurrentTarget)))
            {
                if (inEclipse || (Me.EclipsePercent <= 25 && Me.EclipsePercent >= -25))
                    CastSpell("Celestial Alignment");
            }

            if (SpellManager.HasSpell("Force of Nature")
                && !spellOnCooldown("Force of Nature")
                && (IsWoWBoss(Me.CurrentTarget) || addCount >= 2)
                && IsInRange(40, Me.CurrentTarget))
            {
                if (SpellManager.CanCast("Force of Nature"))
                {
                    SpellManager.Cast("Force of Nature");
                    SpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
                }
            }

            if (CRSettings.myPrefs.typhoon > 0
                && !spellOnCooldown("Typhoon")
                && UnfriendlyUnits.Count() >= CRSettings.myPrefs.typhoon)
            {
                CastSpell("Typhoon");
            }

            if (SpellManager.HasSpell("Berserking")
                && !HaveHasteBuff
                && !spellOnCooldown("Berserking")
                && (CRSettings.myPrefs.useBerserking == 1
                    || (CRSettings.myPrefs.useBerserking == 2 && IsWoWBoss(Me.CurrentTarget)))
                && IsInRange(40, Me.CurrentTarget))
            {
                CastSpell("Berserking");
            }

            if (!spellOnCooldown("Starfall")
                && (inLunar || buffExists("Celestial Alignment", Me)))
            {
                CastSpell("Starfall");
            }

            if (!spellOnCooldown("Starsurge") && IsInRange(40, Me.CurrentTarget))
            {
                CastSpell("Starsurge");
                if (spellOnCooldown("Starsurge")) return;
            }

            if (IsInRange(40, Me.CurrentTarget))
            {
                int eclipsePower = 0;
                try
                {
                    using (StyxWoW.Memory.AcquireFrame())
                    {
                        eclipsePower = Lua.GetReturnVal<int>("return UnitPower('player', 8)", 0);
                    }
                }
                catch { }

                // Prefer moving toward the correct eclipse when not in one
                if (inLunar)
                {
                    CastSpell("Starfire");
                }
                else if (inSolar)
                {
                    CastSpell("Wrath");
                }
                else if (eclipsePower > 0)
                {
                    CastSpell("Starfire");
                }
                else
                {
                    CastSpell("Wrath");
                }
            }
        }

        public void AoeRotation()
        {
            if (!gotTarget && !CRSettings.myPrefs.Targeting)
            {
                getTarget();
            }
            if (!gotTarget || Me.CurrentTarget == null)
                return;

            if (!CRSettings.myPrefs.Movement && (Me.CurrentTarget.Distance > 29 || !Me.CurrentTarget.InLineOfSight))
            {
                Navigator.MoveTo(Me.CurrentTarget.Location);
                return;
            }

            if (!CRSettings.myPrefs.Movement && Me.IsMoving)
                Navigator.PlayerMover.MoveStop();

            if (!CRSettings.myPrefs.Facing)
                Me.CurrentTarget.Face();

            bool inLunar = buffExists(48518, Me) || buffExists("Eclipse (Lunar)", Me);

            if (!spellOnCooldown("Starfall") && (inLunar || buffExists("Celestial Alignment", Me)))
            {
                CastSpell("Starfall");
            }

            if (buffExists("Shooting Stars", Me) && IsInRange(40, Me.CurrentTarget))
            {
                CastSpell("Starsurge");
                return;
            }

            if (!debuffExists("Moonfire", Me.CurrentTarget) && IsInRange(40, Me.CurrentTarget))
                CastSpell("Moonfire");
            if (!debuffExists("Sunfire", Me.CurrentTarget) && IsInRange(40, Me.CurrentTarget))
                CastSpell("Sunfire");

            if (IsInRange(35, Me.CurrentTarget) && !Me.IsChanneling)
            {
                CastSpell("Hurricane");
                SpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
            }
        }
        #endregion combat
    }
}