using FuryUnleashed.Core;
using FuryUnleashed.Core.Helpers;
using FuryUnleashed.Core.Managers;
using FuryUnleashed.Interfaces.Settings;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using System.Windows.Forms;
using G = FuryUnleashed.Rotations.Global;
using IS = FuryUnleashed.Interfaces.Settings.InternalSettings;
using Lua = FuryUnleashed.Core.Helpers.LuaClass;
using PG = FuryUnleashed.Rotations.Protection.ProtGlobal;
using U = FuryUnleashed.Core.Unit;

namespace FuryUnleashed.Rotations.Protection
{
    class ProtRel
    {
        private static LocalPlayer Me
        {
            get { return StyxWoW.Me; }
        }

        internal static Composite RelProtCombat
        {
            get
            {
                return new PrioritySelector(
                    new Switch<Enum.Mode>(ctx => SettingsH.Instance.ModeSelection,
                        new SwitchArgument<Enum.Mode>(Enum.Mode.Auto,
                            new PrioritySelector(
                                Rel_ProtDefensive(),
                                Rel_ProtOffensive(),
                                Rel_ProtRacials(),
                                Rel_ProtNonGcdUtility(),
                                Rel_ProtRageDump(),
                                Item.CreateItemBehaviour(),
                                new Decorator(ret => !Spell.IsGlobalCooldown(),
                                    new PrioritySelector(
                                        Rel_ProtGcdUtility(),
                                        new Decorator(ret => IS.Instance.Protection.CheckAoE && U.NearbyAttackableUnitsCount >= IS.Instance.Protection.CheckAoENum, Rel_ProtMt()),
                                        Rel_ProtSt()
                                )))),
                        new SwitchArgument<Enum.Mode>(Enum.Mode.SemiHotkey,
                            new PrioritySelector(
                                Rel_ProtDefensive(),
                                Rel_ProtNonGcdUtility(),
                                new Decorator(ret => HotKeyManager.IsCooldown,
                                    new PrioritySelector(
                                        Rel_ProtOffensive(),
                                        Rel_ProtRacials(),
                                        Item.CreateItemBehaviour()
                                        )),
                                Rel_ProtRageDump(),
                                new Decorator(ret => !Spell.IsGlobalCooldown(),
                                    new PrioritySelector(
                                        Rel_ProtGcdUtility(),
                                        new Decorator(ret => IS.Instance.Protection.CheckAoE && U.NearbyAttackableUnitsCount >= IS.Instance.Protection.CheckAoENum, Rel_ProtMt()),
                                        Rel_ProtSt()
                                )))),
                        new SwitchArgument<Enum.Mode>(Enum.Mode.Hotkey,
                            new PrioritySelector(
                                Rel_ProtDefensive(),
                                Rel_ProtNonGcdUtility(),
                                new Decorator(ret => HotKeyManager.IsCooldown,
                                    new PrioritySelector(
                                        Rel_ProtOffensive(),
                                        Rel_ProtRacials(),
                                        Item.CreateItemBehaviour()
                                        )),
                                Rel_ProtRageDump(),
                                new Decorator(ret => !Spell.IsGlobalCooldown(),
                                    new PrioritySelector(
                                        Rel_ProtGcdUtility(),
                                        new Decorator(ret => IS.Instance.Protection.CheckAoE && HotKeyManager.IsAoe && U.NearbyAttackableUnitsCount >= IS.Instance.Protection.CheckAoENum, Rel_ProtMt()),
                                        Rel_ProtSt()
                                ))))));
            }
        }

        internal static Composite Rel_ProtRageDump()
        {
            return new PrioritySelector(
                new Decorator(ret => !IS.Instance.Protection.CheckAoE || U.NearbyAttackableUnitsCount < IS.Instance.Protection.CheckAoENum,
                    Spell.Cast(SpellBook.HeroicStrike, ret => (Lua.PlayerPower >= Lua.PlayerPowerMax - 10 && G.NormalPhase) || G.UltimatumAura, true)),
                new Decorator(ret => IS.Instance.Protection.CheckAoE && U.NearbyAttackableUnitsCount >= IS.Instance.Protection.CheckAoENum,
                    Spell.Cast(SpellBook.Cleave, ret => (Lua.PlayerPower >= Lua.PlayerPowerMax - 10 && G.NormalPhase) || G.UltimatumAura, true))
                );
        }

        internal static Composite Rel_ProtSt()
        {
            return new PrioritySelector(
                //is this actions+=/run_action_list,name=dps_cds,if=buff.vengeance.value>health.max*0.20 
                Spell.Cast(SpellBook.Devastate, ret => G.WeakenedArmor3S && G.FadingSunderArmor(1500)), // Added to maintain debuff stacks - Not for applying - This happens later on.
                Spell.Cast(SpellBook.ThunderClap, ret => G.WeakenedBlowsAura && G.FadingWeakenedBlows(1500)), // Added to maintain debuff stacks - Not for applying - This happens later on.

                Spell.Cast(SpellBook.Execute, ret => G.ExecutePhase && Lua.PlayerPower >= Lua.PlayerPowerMax - 10),
                Spell.Cast(SpellBook.Execute, ret => G.ExecutePhase && PG.CustomExecuteUsage),

                Spell.Cast(SpellBook.ShieldSlam),
                Spell.Cast(SpellBook.Revenge, ret => Lua.PlayerPower <= Lua.PlayerPowerMax - 10),

                // Added to support and DPS increase.
                Spell.Cast(SpellBook.DragonRoar, ret => G.DragonRoarTalent && PG.Tier4AbilityUsage),
                Spell.Cast(SpellBook.Devastate, ret => !G.WeakenedArmor3S), // Builds up 3 sunder stacks - And keeps it up.
                Spell.Cast(SpellBook.StormBolt, ret => G.StormBoltTalent && PG.Tier6AbilityUsage),

                Spell.Cast(SpellBook.ThunderClap, ret => !G.WeakenedBlowsAura || G.ResonatingPowerGlyph),
                new Switch<Enum.Shouts>(ctx => IS.Instance.Protection.ShoutSelection,
                    new SwitchArgument<Enum.Shouts>(Enum.Shouts.BattleShout, Spell.Cast(SpellBook.BattleShout, ret => Lua.PlayerPower <= Lua.PlayerPowerMax - 25)),
                    new SwitchArgument<Enum.Shouts>(Enum.Shouts.CommandingShout, Spell.Cast(SpellBook.CommandingShout, ret => Lua.PlayerPower <= Lua.PlayerPowerMax - 25))),
                Spell.Cast(SpellBook.Devastate)
                );
        }

        internal static Composite Rel_ProtMt()
        {
            return new PrioritySelector(
                Spell.Cast(SpellBook.ThunderClap),

                Spell.Cast(SpellBook.Bladestorm, ret => G.BladestormTalent && PG.Tier4AbilityAoEUsage),
                Spell.Cast(SpellBook.DragonRoar, ret => G.DragonRoarTalent && PG.Tier4AbilityAoEUsage),
                Spell.Cast(SpellBook.Shockwave, ret => G.ShockwaveTalent && G.ShockwaveFacing && PG.Tier4AbilityAoEUsage),
                Spell.Cast(SpellBook.StormBolt, ret => G.StormBoltTalent && PG.Tier6AbilityAoEUsage),

                Rel_ProtSt()
                );
        }

        internal static Composite Rel_ProtDefensive()
        {
            return new PrioritySelector(
                // HP Regeneration
                new Decorator(ret => G.EnragedRegenerationTalent && IS.Instance.Protection.CheckEnragedRegen && Me.HealthPercent <= IS.Instance.Protection.CheckEnragedRegenNum,
                    new PrioritySelector(
                        new Decorator(ret => G.EnrageAura,
                            Spell.Cast(SpellBook.EnragedRegeneration, on => Me)),
                        new Decorator(ret => !G.EnrageAura && !G.BerserkerRageOnCooldown,
                            G.EnragedRegenerationLogic()),
                        new Decorator(ret => !G.EnrageAura && G.BerserkerRageOnCooldown,
                            Spell.Cast(SpellBook.EnragedRegeneration, on => Me)))),

                Item.ProtUseHealthStone(),

                // Defensive
                Spell.CastOnGround(SpellBook.DemoralizingBanner, loc => Me.Location, ret => SettingsH.Instance.DemoBannerChoice == Keys.None && IS.Instance.Protection.CheckDemoBanner && Me.HealthPercent <= IS.Instance.Protection.CheckDemoBannerNum),
                Spell.Cast(SpellBook.DemoralizingShout, on => Me, ret => PG.DemoralizingShoutUsage && U.IsTargettingMe && Me.HealthPercent <= IS.Instance.Protection.DemoShoutNum),
                Spell.Cast(SpellBook.LastStand, on => Me, ret => IS.Instance.Protection.CheckLastStand && Me.HealthPercent <= IS.Instance.Protection.CheckLastStandNum),
                Spell.Cast(SpellBook.ShieldWall, on => Me, ret => IS.Instance.Protection.CheckShieldWall && Me.HealthPercent <= IS.Instance.Protection.CheckShieldWallNum),

                Spell.Cast(SpellBook.SpellReflection, ret => PG.SpellReflectionUsage && U.IsViable(Me.CurrentTarget) && U.IsTargettingMe && Me.CurrentTarget.IsCasting),
                Spell.Cast(SpellBook.MassSpellReflection, ret => G.MassSpellReflectionTalent && PG.MassSpellReflectionUsage && G.SpellReflectionSpellCooldown > 0 && U.IsViable(Me.CurrentTarget) && U.IsTargettingMe && Me.CurrentTarget.IsCasting),

                new Decorator(ret => IS.Instance.Protection.CheckShieldBbAdvancedLogics && IS.Instance.Protection.CheckShieldBbThreatLogics && Lua.PlayerPower >= IS.Instance.Protection.ShieldBarrierBlockNum,
                    new PrioritySelector(
                        new Decorator(ret => Me.CurrentTarget.ThreatInfo.RawPercent < 100 && Me.HealthPercent > IS.Instance.Protection.ShieldBarrierBlockThresholdNum,
                            new PrioritySelector(
                                Spell.Cast(SpellBook.Execute, ret => G.ExecutePhase, true),
                                Spell.Cast(SpellBook.HeroicStrike, ret => !IS.Instance.Protection.CheckAoE || U.NearbyAttackableUnitsCount < IS.Instance.Protection.CheckAoENum, true),
                                Spell.Cast(SpellBook.Cleave, ret => IS.Instance.Protection.CheckAoE && U.NearbyAttackableUnitsCount >= IS.Instance.Protection.CheckAoENum, true))),
                        new Decorator(ret => Me.CurrentTarget.ThreatInfo.RawPercent >= 100 || Me.HealthPercent <= IS.Instance.Protection.ShieldBarrierBlockThresholdNum,
                            new PrioritySelector(
                                new Decorator(ret => U.IsCastingAtMe,
                                    Spell.Cast(SpellBook.ShieldBarrier, on => Me, ret => Lua.PlayerPower > 30, true)),
                                Spell.Cast(SpellBook.ShieldBarrier, on => Me, ret => (DamageTracker.CalculateEstimatedAbsorbValue() > DamageTracker.CalculateEstimatedBlockValue()) || (Lua.PlayerPower > 90 && Me.HealthPercent < 100), true),
                                Spell.Cast(SpellBook.ShieldBlock, on => Me, ret => DamageTracker.CalculateEstimatedAbsorbValue() <= DamageTracker.CalculateEstimatedBlockValue(), true)
                                )))),

                new Decorator(ret => IS.Instance.Protection.CheckShieldBbAdvancedLogics && !IS.Instance.Protection.CheckShieldBbThreatLogics && Lua.PlayerPower >= IS.Instance.Protection.ShieldBarrierBlockNum,
                    new PrioritySelector(
                        Spell.Cast(SpellBook.ShieldBarrier, on => Me, ret => (DamageTracker.CalculateEstimatedAbsorbValue() > DamageTracker.CalculateEstimatedBlockValue()) || (Lua.PlayerPower > 90 && Me.HealthPercent < 100), true),
                        Spell.Cast(SpellBook.ShieldBlock, on => Me, ret => DamageTracker.CalculateEstimatedAbsorbValue() <= DamageTracker.CalculateEstimatedBlockValue(), true))));
        }

        internal static Composite Rel_ProtGcdUtility()
        {
            return new PrioritySelector(
                //138279 Victorious - T15 Proc ID (Victory Rush & Impending Victory).
                //32216	Victorious - Regular Kill Proc ID (Victory Rush & Impending Victory).
                Spell.Cast(SpellBook.ImpendingVictory, ret => G.ImpendingVictoryTalent && Me.HealthPercent <= IS.Instance.Protection.ImpendingVictoryNum),
                Spell.Cast(SpellBook.VictoryRush, ret => G.VictoriousAura && Me.HealthPercent <= IS.Instance.Protection.VictoryRushNum),
                Spell.Cast(SpellBook.ShatteringThrow, ret => PG.ShatteringThrowUsage));
        }

        internal static Composite Rel_ProtNonGcdUtility()
        {
            return new PrioritySelector(
                // Need to improve hamstring method
                Spell.Cast(SpellBook.BerserkerRage, on => Me, ret => (!G.EnrageAura || G.FadingEnrage(1500)) && PG.BerserkerRageUsage, true),
                Spell.Cast(SpellBook.Hamstring, ret => !U.IsTargetBoss && !G.HamstringAura && (IS.Instance.Protection.HamString == Enum.Hamstring.Always || IS.Instance.Protection.HamString == Enum.Hamstring.AddList && U.IsHamstringTarget), true),
                Spell.Cast(SpellBook.IntimidatingShout, ret => IS.Instance.Protection.CheckIntimidatingShout && G.IntimidatingShoutGlyph && !U.IsTargetBoss, true),
                Spell.Cast(SpellBook.Taunt, ret => (IS.Instance.Protection.CheckAutoTaunt && !IS.Instance.Protection.CheckSmartTaunt && !U.IsTargettingMe) || (IS.Instance.Protection.CheckSmartTaunt && Unit.IsAutoTauntDesired()), true),
                Spell.Cast(SpellBook.RallyingCry, on => Me, ret => U.RaidMembersNeedCryCount > 0 && !G.LastStandAura && IS.Instance.Protection.CheckRallyingCry, true),
                Spell.Cast(SpellBook.StaggeringShout, ret => G.StaggeringShoutTalent && IS.Instance.Protection.CheckStaggeringShout && U.NearbyAttackableUnitsCount >= IS.Instance.Protection.CheckStaggeringShoutNum, true),
                Spell.Cast(SpellBook.PiercingHowl, ret => G.PiercingHowlTalent && IS.Instance.Protection.CheckPiercingHowl && U.NearbyAttackableUnitsCount >= IS.Instance.Protection.CheckPiercingHowlNum, true),
                new Decorator(ret => U.VigilanceTarget != null,
                    Spell.Cast(SpellBook.Vigilance, on => U.VigilanceTarget, ret => true, true)));
        }

        internal static Composite Rel_ProtRacials()
        {
            return new PrioritySelector(
                new Decorator(ret => PG.RacialUsage,
                    Spell.Cast(G.SelectRacialSpell(), ret => G.SelectRacialSpell() != null && G.RacialUsageSatisfied(G.SelectRacialSpell()))));
        }

        internal static Composite Rel_ProtOffensive()
        {
            return new PrioritySelector(
                Spell.Cast(SpellBook.Avatar, on => Me, ret => G.AvatarTalent && PG.Tier6AbilityUsage, true),
                Spell.Cast(SpellBook.Bloodbath, on => Me, ret => G.BloodbathTalent && PG.Tier6AbilityUsage, true),
                Spell.Cast(SpellBook.Recklessness, on => Me, ret => PG.RecklessnessUsage, true),
                Spell.Cast(SpellBook.SkullBanner, ret => !G.SkullBannerAura && PG.SkullBannerUsage, true));
        }
    }
}