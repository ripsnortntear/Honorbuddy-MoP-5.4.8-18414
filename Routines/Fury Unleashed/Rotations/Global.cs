using CommonBehaviors.Actions;
using FuryUnleashed.Core;
using FuryUnleashed.Core.Helpers;
using FuryUnleashed.Core.Managers;
using FuryUnleashed.Core.Utilities;
using FuryUnleashed.Interfaces.Settings;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Linq;
using Action = Styx.TreeSharp.Action;
using Enum = FuryUnleashed.Core.Helpers.Enum;
using IS = FuryUnleashed.Interfaces.Settings.InternalSettings;
using U = FuryUnleashed.Core.Unit;


namespace FuryUnleashed.Rotations
{
    class Global
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        #region Global Used Composites
        internal static Composite InitializeCaching()
        {
            return new PrioritySelector(
                new Action(delegate { Spell.GetCachedAuras(); return RunStatus.Failure; }),
                new Action(delegate { U.GetNearbyAttackableUnitsCount(); return RunStatus.Failure; }),

                new Decorator(ret => IS.Instance.General.Vigilance != Enum.VigilanceTrigger.Never,
                    new Action(delegate { U.GetVigilanceTarget(); return RunStatus.Failure; })),

                new Switch<WoWSpec>(ret => Me.Specialization,
                    // Arms Caching Tree
                    new SwitchArgument<WoWSpec>(WoWSpec.WarriorArms,
                        new PrioritySelector(
                            new Decorator(ret => IS.Instance.Arms.CheckAoE && IS.Instance.Arms.CheckAoEThunderclap && U.NearbyAttackableUnitsCount > 1,
                                new Action(delegate { U.GetNeedThunderclapUnitsCount(); return RunStatus.Failure; })),
                            new Decorator(ret => IS.Instance.Arms.CheckInterruptsAoE && U.NearbyAttackableUnitsCount > 1,
                                new Action(delegate { U.GetInterruptableUnitsCount(); return RunStatus.Failure; })),
                            new Decorator(ret => IS.Instance.Arms.CheckRallyingCry,
                                new Action(delegate { U.GetRaidMembersNeedCryCount(); return RunStatus.Failure; })),
                            new Action(delegate { U.GetNearbySlamCleaveUnitsCount(); return RunStatus.Failure; }))),
                    
                    // Fury Caching Tree
                    new SwitchArgument<WoWSpec>(WoWSpec.WarriorFury,
                        new PrioritySelector(
                            new Decorator(ret => IS.Instance.Fury.CheckInterruptsAoE && U.NearbyAttackableUnitsCount > 1,
                                new Action(delegate { U.GetInterruptableUnitsCount(); return RunStatus.Failure; })),
                            new Decorator(ret => IS.Instance.Fury.CheckRallyingCry,
                                new Action(delegate { U.GetRaidMembersNeedCryCount(); return RunStatus.Failure; })))),

                    // Protection Caching Tree
                    new SwitchArgument<WoWSpec>(WoWSpec.WarriorProtection,
                        new PrioritySelector(
                            new Decorator(ret => IS.Instance.Protection.CheckAoE && U.NearbyAttackableUnitsCount > 1,
                                new Action(delegate { U.GetNeedThunderclapUnitsCount(); return RunStatus.Failure; })),
                            new Decorator(ret => IS.Instance.Protection.CheckInterruptsAoE && U.NearbyAttackableUnitsCount > 1,
                                new Action(delegate { U.GetInterruptableUnitsCount(); return RunStatus.Failure; })),
                            new Decorator(ret => IS.Instance.Protection.CheckRallyingCry,
                                new Action(delegate { U.GetRaidMembersNeedCryCount(); return RunStatus.Failure; }))))
                                ));
        }

        internal static Composite InitializeOnKeyActions()
        {
            return new PrioritySelector(
                new Decorator(ret => IS.Instance.General.AutoDetectManualCast,
                    ManualCastPauseLogic()),
                new Decorator(ret => HotKeyManager.IsKeyAsyncDown(SettingsH.Instance.Tier4Choice),
                    new PrioritySelector(
                        Spell.Cast(SpellBook.Bladestorm, ret => BladestormTalent),
                        Spell.Cast(SpellBook.DragonRoar, ret => DragonRoarTalent),
                        Spell.Cast(SpellBook.Shockwave, ret => ShockwaveTalent))),
                new Decorator(ret => HotKeyManager.IsKeyAsyncDown(SettingsH.Instance.ShatteringThrowChoice),
                    Spell.Cast(SpellBook.ShatteringThrow)),
                new Decorator(ret => HotKeyManager.IsKeyDown(SettingsH.Instance.HeroicLeapChoice),
                    new Action(ret =>
                    {
                        SpellManager.Cast(SpellBook.HeroicLeap);
                        Lua.DoString("if SpellIsTargeting() then CameraOrSelectOrMoveStart() CameraOrSelectOrMoveStop() end");
                        Logger.CombatLogPu("Casting: Heroic Leap - On Mousecursor Location");
                    })),
                new Decorator(ret => HotKeyManager.IsKeyDown(SettingsH.Instance.DemoBannerChoice),
                    new Action(ret =>
                    {
                        SpellManager.Cast(SpellBook.DemoralizingBanner);
                        Lua.DoString("if SpellIsTargeting() then CameraOrSelectOrMoveStart() CameraOrSelectOrMoveStop() end");
                        Logger.CombatLogPu("Casting: Demoralizing Banner - On Mousecursor Location");
                    })),
                new Decorator(ret => HotKeyManager.IsKeyDown(SettingsH.Instance.MockingBannerChoice),
                    new Action(ret =>
                    {
                        SpellManager.Cast(SpellBook.MockingBanner);
                        Lua.DoString("if SpellIsTargeting() then CameraOrSelectOrMoveStart() CameraOrSelectOrMoveStop() end");
                        Logger.CombatLogPu("Casting: Mocking Banner - On Mousecursor Location");
                    })));
        }

        internal static Composite CancelBladestormLogic()
        {
            return new Action(ctx =>
            {
                Me.CancelAura(AuraBook.Bladestorm);
                Spell.Cast(SpellBook.Bloodthirst);
                return RunStatus.Failure;
            });
        }

        internal static Composite EnragedRegenerationLogic()
        {
            return new Action(ctx =>
            {
                Logger.CombatLogWh("Using Berserker Rage to Enrage - Required for Emergency Enraged Regeneration");
                Spell.Cast(SpellBook.BerserkerRage, on => Me);
                Spell.Cast(SpellBook.EnragedRegeneration, on => Me);
                return RunStatus.Failure;
            });
        }

        internal static Composite InterruptLogic()
        {
            return new PrioritySelector(
                new ThrottlePasses(1, TimeSpan.FromSeconds(15), RunStatus.Failure,
                    Spell.Cast(SpellBook.DisruptingShout, ret => DisruptingShoutTalent && U.InterruptableUnitsCount > 1)),
                new ThrottlePasses(1, TimeSpan.FromSeconds(15), RunStatus.Failure,
                    Spell.Cast(SpellBook.Pummel)));
        }

        internal static Composite ManualCastPauseLogic()
        {
            return new Sequence(
                new Decorator(ret => IS.Instance.General.AutoDetectManualCast && HotKeyManager.AnyKeyPressed(), new ActionAlwaysSucceed()),
                new WaitContinue(TimeSpan.FromMilliseconds(IS.Instance.General.ResumeTime), ret => false,
                    new ActionAlwaysSucceed()));
        }
        #endregion

        #region Racials
        internal static string SelectRacialSpell()
        {
            switch (Me.Race)
            {
                case WoWRace.BloodElf: return "Arcane Torrent";
                case WoWRace.Draenei: return "Gift of the Naaru";
                case WoWRace.Dwarf: return "Stoneform";
                case WoWRace.Gnome: return "Escape Artist";
                case WoWRace.Goblin: return "Rocket Barrage";
                case WoWRace.Human: return "Every Man for Himself";
                case WoWRace.NightElf: return "Shadowmeld";
                case WoWRace.Orc: return "Blood Fury";
                case WoWRace.Pandaren: return null;
                case WoWRace.Tauren: return "War Stomp";
                case WoWRace.Troll: return "Berserking";
                case WoWRace.Undead: return "Will of the Forsaken";
                case WoWRace.Worgen: return "Darkflight";
                default: return null;
            }
        }

        //actions+=/blood_fury,if=buff.cooldown_reduction.down&(buff.bloodbath.up|(!talent.bloodbath.enabled&debuff.colossus_smash.up))|buff.cooldown_reduction.up&buff.recklessness.up
        //actions+=/berserking,if=buff.cooldown_reduction.down&(buff.bloodbath.up|(!talent.bloodbath.enabled&debuff.colossus_smash.up))|buff.cooldown_reduction.up&buff.recklessness.up
        //actions+=/arcane_torrent,if=buff.cooldown_reduction.down&(buff.bloodbath.up|(!talent.bloodbath.enabled&debuff.colossus_smash.up))|buff.cooldown_reduction.up&buff.recklessness.up
        //actions+=/use_item,slot=hands,if=!talent.bloodbath.enabled&debuff.colossus_smash.up|buff.bloodbath.up

        internal static bool RacialUsageSatisfied(string racial)
        {
            if (racial != null)
            {
                switch (racial)
                {
                    case "Stoneform": return IsSick;
                    case "Escape Artist": return Me.Rooted;
                    case "Every Man for Himself": return IsImpaired;
                    case "Shadowmeld": return Targeting.GetAggroOnMeWithin(Me.Location, 15) >= 1 && Me.HealthPercent < IS.Instance.General.RacialNum && !Me.IsMoving;
                    case "Gift of the Naaru": return Me.HealthPercent <= IS.Instance.General.RacialNum;
                    case "Darkflight": return Me.IsMoving;
                    case "Blood Fury": return true;
                    case "War Stomp": return Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 8) >= 1;
                    case "Berserking": return !HasteAbilities;
                    case "Will of the Forsaken": return IsLazy;
                    case "Arcane Torrent": return Me.ManaPercent < IS.Instance.General.RacialNum && Me.Class != WoWClass.DeathKnight;
                    case "Rocket Barrage": return true;
                    default: return false;
                }
            }
            return false;
        }
        #endregion

        #region Multi Category
        internal static bool AlmostDead
        {
            get { return Me.CurrentTarget.HealthPercent <= 5; }
        }

        internal static bool DumpAllRage
        {
            get { return Me.CurrentTarget.HealthPercent <= 2.5; }
        }

        internal static bool ExecutePhase
        {
            get { return Me.CurrentTarget.HealthPercent <= 20; }
        }

        internal static bool HasteAbilities
        {
            get { return (AncientHysteriaAura || BloodLustAura || HeroismAura || TimeWarpAura); }
        }

        internal static bool HotkeyMode
        {
            get { return SettingsH.Instance.ModeSelection == Enum.Mode.Hotkey || SettingsH.Instance.ModeSelection == Enum.Mode.SemiHotkey; }
        }

        internal static bool NormalPhase
        {
            get { return Me.CurrentTarget.HealthPercent > 20; }
        }

        internal static bool ShockwaveFacing
        {
            get { return Me.IsSafelyFacing(Me.CurrentTarget); }
        }

        internal static bool WieldsTwoHandedWeapons
        {
            get { return Item.WieldsTwoHandedWeapons; }
        }

        // Arms Specific
        internal static bool SlamViable
        {
            get
            {
                double wwcost = WoWSpell.FromId(SpellBook.Whirlwind).PowerCost;
                double slamcost = WoWSpell.FromId(SpellBook.Slam).PowerCost;

                return (U.NearbySlamCleaveUnitsFloat * 3.4) / slamcost >= U.NearbyAttackableUnitsFloat / wwcost;
            }
        }

        internal static bool WhirlwindViable
        {
            get
            {
                double wwcost = WoWSpell.FromId(SpellBook.Whirlwind).PowerCost;
                double slamcost = WoWSpell.FromId(SpellBook.Slam).PowerCost;

                return U.NearbyAttackableUnitsFloat / wwcost >= (U.NearbySlamCleaveUnitsFloat * 3.4) / slamcost;
            }
        }

        // Major CD Variables
        internal static bool ColossusSmashTracker
        {
            get
            {
                return ColossusSmashAura && Spell.AuraTimeLeft(StyxWoW.Me.CurrentTarget, AuraBook.ColossusSmash) > 4500 || !ColossusSmashOnCooldown;
            }
        }

        internal static bool FadingOffensiveCooldowns(int timeLeft = 50000)
        {
            return (RecklessnessOnCooldown && RecklessnessSpellCooldown < timeLeft) ||
                       (SkullBannerOnCooldown && SkullBannerSpellCooldown < timeLeft) ||
                       (AvatarOnCooldown && AvatarSpellCooldown < timeLeft);
        }

        internal static bool RunningOffensiveCoolDowns
        {
            get
            {
                return (AvatarAura && Spell.AuraTimeLeft(StyxWoW.Me, AuraBook.Avatar) > 5000) ||
                       (RecklessnessAura && Spell.AuraTimeLeft(StyxWoW.Me, AuraBook.Recklessness) > 5000) ||
                       (SkullBannerAura && Spell.AuraTimeLeft(StyxWoW.Me, AuraBook.SkullBannerNormal) > 5000 ||
                       (SkullBannerAura && Spell.AuraTimeLeft(StyxWoW.Me, AuraBook.SkullBannerT15) > 5000));
            }
        }

        #endregion

        #region HasAura
        // StyxWoW.Me - (WoWUnit unit, int auraId, int stacks, int msuLeft = 0, bool isFromMe = true, bool cached = true)
        internal static bool AncientHysteriaAura
        {
            get { return Spell.HasAura(Me, AuraBook.AncientHysteria, 0, 0, false); }
        }

        internal static bool AvatarAura
        {
            get { return Spell.HasAura(Me, AuraBook.Avatar); }
        }

        internal static bool BattleShoutAura
        {
            get { return Spell.HasAura(Me, AuraBook.BattleShout, 0, 0, false); }
        }

        internal static bool BattleStanceAura
        {
            get { return Spell.HasAura(Me, AuraBook.BattleStance, 0, 0, true, false, true); }
        }

        internal static bool BerserkerStanceAura
        {
            get { return Spell.HasAura(Me, AuraBook.BerserkerStance, 0, 0, true, false, true); }
        }

        internal static bool BladestormAura
        {
            get { return Spell.HasAura(Me, AuraBook.Bladestorm); }
        }

        internal static bool BloodbathAura
        {
            get { return Spell.HasAura(Me, AuraBook.Bloodbath); }
        }

        internal static bool BloodLustAura
        {
            get { return Spell.HasAura(Me, AuraBook.Bloodlust, 0, 0, false); }
        }

        internal static bool BloodsurgeAura
        {
            get { return Spell.HasAura(Me, AuraBook.Bloodsurge); }
        }

        internal static bool CommandingShoutAura
        {
            get { return Spell.HasAura(Me, AuraBook.CommandingShout, 0, 0, false); }
        }

        internal static bool DeathSentenceAuraT16
        {
            get { return Spell.HasAura(Me, AuraBook.DeathSentence); }
        }

        internal static bool DefensiveStanceAura
        {
            get { return Spell.HasAura(Me, AuraBook.DefensiveStance); }
        }

        internal static bool DeterminationAura
        {
            get { return Spell.HasAura(Me, AuraBook.Determination); }
        }

        internal static bool EnrageAura
        {
            get { return Spell.HasAura(Me, AuraBook.EnrageUnknown) || Spell.HasAura(Me, AuraBook.EnrageNormal); }
        }

        internal static bool HeroismAura
        {
            get { return Spell.HasAura(Me, AuraBook.Heroism, 0, 0, false); }
        }

        internal static bool LastStandAura
        {
            get { return Spell.HasAura(Me, AuraBook.LastStand); }
        }

        internal static bool MeatCleaverAura
        {
            get { return Spell.HasAura(Me, AuraBook.MeatCleaver); }
        }

        internal static bool OutrageAura
        {
            get { return Spell.HasAura(Me, AuraBook.Outrage); }
        }

        internal static bool RagingBlowAura
        {
            get { return Spell.HasAura(Me, AuraBook.RagingBlow); }
        }

        internal static bool RagingWindAura
        {
            get { return Spell.HasAura(Me, AuraBook.RagingWind); }
        }

        internal static bool RallyingCryAura
        {
            get { return Spell.HasAura(Me, AuraBook.RallyingCry, 0, 0, false); }
        }

        internal static bool ReadinessAura
        {
            get { return Spell.HasAura(Me, AuraBook.ReadinessAddPctModifier, 0, 0, false, false, true); }
        }

        internal static bool RecklessnessAura
        {
            get { return Spell.HasAura(Me, AuraBook.Recklessness); }
        }

        internal static bool ShieldBarrierAura
        {
            get { return Spell.HasAura(Me, AuraBook.ShieldBarrier); }
        }

        internal static bool ShieldBlockAura
        {
            get { return Spell.HasAura(Me, AuraBook.ShieldBlock); }
        }

        internal static bool SkullBannerAura
        {
            get { return Spell.HasAura(Me, AuraBook.SkullBannerNormal, 0, 0, false, false, true); }
        }

        internal static bool SkullBannerAuraT15
        {
            get { return Spell.HasAura(Me, AuraBook.SkullBannerT15); }
        }

        internal static bool SweepingStrikesAura
        {
            get { return Spell.HasAura(Me, AuraBook.SweepingStrikes); }
        }

        internal static bool SuddenExecAura
        {
            get { return Spell.HasAura(Me, AuraBook.SuddenExecute); }
        }

        internal static bool TasteforBloodAura
        {
            get { return Spell.HasAura(Me, AuraBook.TasteforBlood); }
        }

        internal static bool Tier16TwoPieceBonus
        {
            get { return Spell.HasAura(Me, "Item - Warrior T16 DPS 2P Bonus", 0, 0, true, false, true); }
        }

        internal static bool Tier16FourPieceBonus
        {
            get { return Spell.HasAura(Me, "Item - Warrior T16 DPS 4P Bonus", 0, 0, true, false, true); }
        }

        internal static bool Tier16TwoPieceBonusT
        {
            get { return Spell.HasAura(Me, "Item - Warrior T16 Protection 2P Bonus", 0, 0, true, false, true); }
        }

        internal static bool Tier16FourPieceBonusT
        {
            get { return Spell.HasAura(Me, "Item - Warrior T16 Protection 4P Bonus", 0, 0, true, false, true); }
        }

        internal static bool TimeWarpAura
        {
            get { return Spell.HasAura(Me, AuraBook.Timewarp, 0, 0, false); }
        }

        internal static bool UltimatumAura
        {
            get { return Spell.HasAura(Me, AuraBook.Ultimatum); }
        }

        internal static bool VictoriousAura
        {
            get { return Spell.HasAura(Me, AuraBook.Victorious) || Spell.HasAura(Me, AuraBook.VictoriousT15); }
        }

        internal static bool VictoriousAuraT15
        {
            get { return Spell.HasAura(Me, AuraBook.VictoriousT15); }
        }

        // StyxWoW.Me.CurrentTarget
        internal static bool ColossusSmashAura
        {
            get { return Spell.HasAura(Me.CurrentTarget, AuraBook.ColossusSmash); }
        }

        internal static bool DeepWoundsAura
        {
            get { return Spell.HasAura(Me.CurrentTarget, AuraBook.DeepWounds); }
        }

        internal static bool HamstringAura
        {
            get { return Spell.HasAura(Me.CurrentTarget, AuraBook.Hamstring, 0, 0, false); }
        }

        internal static bool SunderArmorAura
        {
            get { return Spell.HasAura(Me.CurrentTarget, AuraBook.SunderArmor, 0, 0, false); }
        }

        internal static bool WeakenedBlowsAura
        {
            get { return Spell.HasAura(Me.CurrentTarget, AuraBook.WeakenedBlows, 0, 0, false); }
        }
        #endregion

        #region Remaining & Fading Aura's
        // StyxWoW.Me.CurrentTarget - Remaining
        internal static bool RemainingColossusSmash(int remainingtime)
        {
            return Spell.RemainingAura(Me.CurrentTarget, AuraBook.ColossusSmash, remainingtime);
        }

        // StyxWoW.Me - Remaining
        internal static bool RemainingEnrage(int remainingtime)
        {
            return Spell.RemainingAura(Me, AuraBook.EnrageNormal, remainingtime);
        }

        // StyxWoW.Me.CurrentTarget - Fading
        internal static bool FadingColossusSmash(int fadingtime)
        {
            return Spell.FadingAura(Me.CurrentTarget, AuraBook.ColossusSmash, fadingtime);
        }

        internal static bool FadingDeepWounds(int fadingtime)
        {
            return Spell.FadingAura(Me.CurrentTarget, AuraBook.DeepWounds, fadingtime);
        }

        internal static bool FadingSunderArmor(int fadingtime)
        {
            return Spell.FadingAura(Me.CurrentTarget, AuraBook.SunderArmor, fadingtime);
        }

        internal static bool FadingWeakenedBlows(int fadingtime)
        {
            return Spell.FadingAura(Me.CurrentTarget, AuraBook.WeakenedBlows, fadingtime);
        }

        // StyxWoW.Me - Fading
        internal static bool FadingDeathSentence(int fadingtime)
        {
            return Spell.FadingAura(Me, AuraBook.DeathSentence, fadingtime);
        }

        internal static bool FadingEnrage(int fadingtime)
        {
            return Spell.FadingAura(Me, AuraBook.EnrageUnknown, fadingtime) || Spell.FadingAura(Me, AuraBook.EnrageNormal, fadingtime);
        }

        internal static bool FadingRagingBlow(int fadingtime)
        {
            return Spell.FadingAura(Me, AuraBook.RagingBlow, fadingtime);
        }

        internal static bool FadingVictoryRush(int fadingtime)
        {
            return Spell.FadingAura(Me, AuraBook.Victorious, fadingtime) || Spell.FadingAura(Me, AuraBook.VictoriousT15, fadingtime);
        }
        #endregion

        #region StackCounts
        // StyxWoW.Me.CurrentTarget
        internal static bool WeakenedArmor1S
        {
            get { return Spell.HasAura(Me.CurrentTarget, AuraBook.WeakenedArmor, 1, 0, false); }
        }

        internal static bool WeakenedArmor2S
        {
            get { return Spell.HasAura(Me.CurrentTarget, AuraBook.WeakenedArmor, 2, 0, false); }
        }

        internal static bool WeakenedArmor3S
        {
            get { return Spell.HasAura(Me.CurrentTarget, AuraBook.WeakenedArmor, 3, 0, false); }
        }

        // StyxWoW.Me
        internal static bool MeatCleaverAuraS1
        {
            get { return Spell.HasAura(Me, AuraBook.MeatCleaver, 1); }
        }

        internal static bool MeatCleaverAuraS2
        {
            get { return Spell.HasAura(Me, AuraBook.MeatCleaver, 2); }
        }

        internal static bool MeatCleaverAuraS3
        {
            get { return Spell.HasAura(Me, AuraBook.MeatCleaver, 3); }
        }

        internal static bool RagingBlow1S
        {
            get { return Spell.HasAura(Me, AuraBook.RagingBlow, 1); }
        }

        internal static bool RagingBlow2S
        {
            get { return Spell.HasAura(Me, AuraBook.RagingBlow, 2); }
        }

        internal static bool TasteForBloodS3
        {
            get { return Spell.HasAura(Me, AuraBook.TasteForBlood, 3); }
        }

        internal static bool TasteForBloodS4
        {
            get { return Spell.HasAura(Me, AuraBook.TasteForBlood, 4); }
        }

        internal static bool TasteForBloodS5
        {
            get { return Spell.HasAura(Me, AuraBook.TasteForBlood, 5); }
        }
        #endregion

        #region Character States
        internal static bool IsImpaired
        {
            get
            {
                return Me.GetAllAuras().Any(a =>
                        a.Spell.Mechanic == WoWSpellMechanic.Dazed || a.Spell.Mechanic == WoWSpellMechanic.Disoriented ||
                        a.Spell.Mechanic == WoWSpellMechanic.Frozen || a.Spell.Mechanic == WoWSpellMechanic.Incapacitated ||
                        a.Spell.Mechanic == WoWSpellMechanic.Rooted || a.Spell.Mechanic == WoWSpellMechanic.Slowed ||
                        a.Spell.Mechanic == WoWSpellMechanic.Snared);
            }
        }

        internal static bool IsLazy
        {
            get
            {
                return Me.GetAllAuras().Any(a =>
                        a.Spell.Mechanic == WoWSpellMechanic.Fleeing || a.Spell.Mechanic == WoWSpellMechanic.Asleep ||
                        a.Spell.Mechanic == WoWSpellMechanic.Charmed);
            }
        }

        internal static bool IsSick
        {
            get
            {
                return Me.GetAllAuras().Any(a =>
                        a.Spell.Mechanic == WoWSpellMechanic.Bleeding || a.Spell.DispelType == WoWDispelType.Disease ||
                        a.Spell.DispelType == WoWDispelType.Poison);
            }
        }
        #endregion

        #region TalentManager Functions
        // Talentmanager - HasGlyphs
        internal static bool ColossusSmashGlyph
        {
            get { return TalentManager.HasGlyph("Colossus Smash"); }
        }

        internal static bool HinderingStrikesGlyph
        {
            get { return TalentManager.HasGlyph("Hindering Strikes"); }
        }

        internal static bool UnendingRageGlyph
        {
            get { return TalentManager.HasGlyph("Unending Rage"); }
        }

        internal static bool IntimidatingShoutGlyph
        {
            get { return TalentManager.HasGlyph("Intimidating Shout"); }
        }

        internal static bool ResonatingPowerGlyph
        {
            get { return TalentManager.HasGlyph("Resonating Power"); }
        }

        internal static bool WhirlwindGlyph
        {
            get { return TalentManager.HasGlyph("Whirlwind"); }
        }

        // Talentmanager CurrentSpec
        internal static bool IsArmsSpec
        {
            get { return TalentManager.CurrentSpec == WoWSpec.WarriorArms; }
        }

        internal static bool IsFurySpec
        {
            get { return TalentManager.CurrentSpec == WoWSpec.WarriorFury; }
        }

        internal static bool IsProtSpec
        {
            get { return TalentManager.CurrentSpec == WoWSpec.WarriorProtection; }
        }

        // Talentmanager - HasTalents
        // Tier 1 Talents
        internal static bool JuggernautTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.Juggernaut); }
        }

        internal static bool DoubleTimeTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.DoubleTime); }
        }

        internal static bool WarbringerTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.Warbringer); }
        }

        // Tier 2 Talents
        internal static bool EnragedRegenerationTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.EnragedRegeneration); }
        }

        internal static bool SecondWindTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.SecondWind); }
        }

        internal static bool ImpendingVictoryTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.ImpendingVictory); }
        }

        // Tier 3 Talents
        internal static bool StaggeringShoutTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.StaggeringShout); }
        }

        internal static bool PiercingHowlTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.PiercingHowl); }
        }

        internal static bool DisruptingShoutTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.DisruptingShout); }
        }

        // Tier 4 Talents
        internal static bool BladestormTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.Bladestorm); }
        }

        internal static bool ShockwaveTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.Shockwave); }
        }

        internal static bool DragonRoarTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.DragonRoar); }
        }

        // Tier 5 Talents
        internal static bool MassSpellReflectionTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.MassSpellReflection); }
        }

        internal static bool SafeguardTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.Safeguard); }
        }

        internal static bool VigilanceTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.Vigilance); }
        }

        // Tier 6 Talents
        internal static bool AvatarTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.Avatar); }
        }

        internal static bool BloodbathTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.Bloodbath); }
        }

        internal static bool StormBoltTalent
        {
            get { return TalentManager.HasTalent(Enum.WarriorTalents.StormBolt); }
        }
        #endregion

        #region Cooldowntracker
        // Cooldowntracker - Timeleft in TotalMilliseconds
        internal static double AvatarSpellCooldown
        {
            get { return CooldownTracker.GetSpellCooldown(SpellBook.Avatar).TotalMilliseconds; } // Avatar
        }

        internal static double BloodbathSpellCooldown
        {
            get { return CooldownTracker.GetSpellCooldown(SpellBook.Bloodbath).TotalMilliseconds; } // Bloodbath
        }

        internal static double BerserkerRageSpellCooldown
        {
            get { return CooldownTracker.GetSpellCooldown(SpellBook.BerserkerRage).TotalMilliseconds; }
        }

        internal static double BloodthirstSpellCooldown
        {
            get { return CooldownTracker.GetSpellCooldown(SpellBook.Bloodthirst).TotalMilliseconds; } // Bloodthirst
        }

        internal static double ColossusSmashSpellCooldown
        {
            get { return CooldownTracker.GetSpellCooldown(SpellBook.ColossusSmash).TotalMilliseconds; } // Colossus Smash
        }

        internal static double PummelSpellCooldown
        {
            get { return CooldownTracker.GetSpellCooldown(SpellBook.Pummel).TotalMilliseconds; } // Pummel
        }

        internal static double ShieldSlamSpellCooldown
        {
            get { return CooldownTracker.GetSpellCooldown(SpellBook.ShieldSlam).TotalMilliseconds; } // Shield Slam
        }

        internal static double SkullBannerSpellCooldown
        {
            get { return CooldownTracker.GetSpellCooldown(SpellBook.SkullBanner).TotalMilliseconds; } // Skull Banner
        }

        internal static double SpellReflectionSpellCooldown
        {
            get { return CooldownTracker.GetSpellCooldown(SpellBook.SpellReflection).TotalMilliseconds; } // Spell Reflection
        }

        internal static double RecklessnessSpellCooldown
        {
            get { return CooldownTracker.GetSpellCooldown(SpellBook.Recklessness).TotalMilliseconds; } // Recklessness
        }

        // Spells on Cooldown
        internal static bool AvatarOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.Avatar); } // Berserker Rage
        }
        internal static bool BerserkerRageOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.BerserkerRage); } // Berserker Rage
        }

        internal static bool BladeStormOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.Bladestorm); } // Bladestorm
        }

        internal static bool BloodThirstOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.Bloodthirst); } // Bloodthirst
        }

        internal static bool ColossusSmashOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.ColossusSmash); } // Colossus Smash
        }

        internal static bool DemoralizingBannerOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.DemoralizingBanner); } // Demoralizing Banner
        } 

        internal static bool DragonRoarOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.DragonRoar); } // Dragon Roar
        }

        internal static bool DisruptingShoutOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.DisruptingShout); } // Disrupting Shout
        }

        internal static bool HeroicLeapOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.HeroicLeap); } // Heroic Leap
        }

        internal static bool ImpendingVictoryOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.ImpendingVictory); } // Impending Victory
        }

        internal static bool MortalStrikeOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.MortalStrike); } // Mortal Strike
        }

        internal static bool PummelOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.Pummel); } // Pummel
        }

        internal static bool RagingBlowOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.RagingBlow); } // Raging Blow
        }

        internal static bool RecklessnessOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.Recklessness); } // Revenge
        }

        internal static bool RevengeOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.Revenge); } // Revenge
        }

        internal static bool SkullBannerOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.SkullBanner); } // Demoralizing Banner
        } 

        internal static bool StormBoltOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.StormBolt); } // Storm Bolt
        }

        internal static bool SlamOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.Slam); } // Slam
        }

        internal static bool ShieldSlamOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.ShieldSlam); }// Shield Slam
        }

        internal static bool ShockwaveOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.Shockwave); } // Shockwave
        }

        internal static bool VictoryRushOnCooldown
        {
            get { return CooldownTracker.SpellOnCooldown(SpellBook.VictoryRush); } // Victory Rush
        }
        #endregion
    }
}
