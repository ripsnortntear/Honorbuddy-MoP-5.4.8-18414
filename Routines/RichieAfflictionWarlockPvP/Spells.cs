using System;
using System.Linq;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace RichieAfflictionWarlock {

    public partial class Main {

        //Damage
        #region  Agony

        private static Composite Agony() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.Agony &&
                SpellManager.HasSpell("Agony") &&
                MyAuraTimeLeft("Agony", Me.CurrentTarget) <= AfflictionSettings.Instance.AgonyRefresh,
                new Action(
                    delegate {
                        CastSpell("Agony", Me.CurrentTarget);
                        return RunStatus.Success;
                    }
                )
            );
        }

        private static Composite AgonyMulti() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.Agony &&
                SpellManager.HasSpell("Agony") &&
                MyAuraTimeLeft("Agony", MultidotTarget) <= AfflictionSettings.Instance.AgonyRefresh,
                new Action(
                    delegate {
                        CastSpell("Agony",MultidotTarget, "Multi");
                        return RunStatus.Success;
                    }
                )
            );
        }

        private static Composite AgonyASAP() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.Agony &&
                SpellManager.HasSpell("Agony") &&
                MyAuraTimeLeft("Agony", ASAPAttackTarget) <= AfflictionSettings.Instance.AgonyRefresh,
                new Action(
                    delegate {
                        CastSpell("Agony", ASAPAttackTarget, "ASAP");
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region  Corruption

        private static Composite Corruption() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.Corruption &&
                SpellManager.HasSpell("Corruption") &&
                MyAuraTimeLeft(146739, Me.CurrentTarget) <= AfflictionSettings.Instance.CorruptionRefresh,
                new Action(
                    delegate {
                        CastSpell("Corruption", Me.CurrentTarget);
                        return RunStatus.Success;
                    }
                )
            );
        }

        private static Composite CorruptionMulti() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.Corruption &&
                SpellManager.HasSpell("Corruption") &&
                MyAuraTimeLeft(146739, MultidotTarget) <= AfflictionSettings.Instance.CorruptionRefresh,
                new Action(
                    delegate {
                        CastSpell("Corruption", MultidotTarget, "Multi");
                        return RunStatus.Success;
                    }
                )
            );
        }

        private static Composite CorruptionASAP() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.Corruption &&
                SpellManager.HasSpell("Corruption") &&
                MyAuraTimeLeft(146739, ASAPAttackTarget) <= AfflictionSettings.Instance.CorruptionRefresh,
                new Action(
                    delegate {
                        CastSpell("Corruption", ASAPAttackTarget, "ASAP");
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region   Unstable Affliction

        private static Composite UnstableAffliction() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.UnstableAffliction &&
                !Me.IsMoving &&
                SpellManager.HasSpell("Unstable Affliction") &&
                !(LastUATarget != null && LastUATarget == Me.CurrentTarget && LastUACast.AddSeconds(3) >= DateTime.Now) &&
                MyAuraTimeLeft("Unstable Affliction", Me.CurrentTarget) <= AfflictionSettings.Instance.UnstableAfflictionRefresh,
                new Action(
                    delegate {
                        CastSpell("Unstable Affliction", Me.CurrentTarget);
                        LastUATarget = Me.CurrentTarget;
                        LastUACast = DateTime.Now;
                        SoulSwapInhaleTarget = Me.CurrentTarget;
                        LastSoulSwapInhaleTargetSet = DateTime.Now;
                        return RunStatus.Success;
                    }
                )
            );
        }

        private static Composite UnstableAfflictionMulti() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.UnstableAffliction &&
                !Me.IsMoving &&
                SpellManager.HasSpell("Unstable Affliction") &&
                //!(LastUATarget != null && LastUATarget == MultidotTarget && LastUACast.AddSeconds(2) >= DateTime.Now) &&
                MyAuraTimeLeft("Unstable Affliction", MultidotTarget) <= AfflictionSettings.Instance.UnstableAfflictionRefresh,
                new Action(
                    delegate {
                        CastSpell("Unstable Affliction", MultidotTarget, "Multi");
                        //LastUATarget = MultidotTarget;
                        //LastUACast = DateTime.Now;
                        Blacklist.Add(MultidotTarget.Guid, BlacklistFlags.Combat, TimeSpan.FromSeconds(3));
                        SoulSwapInhaleTarget = MultidotTarget;
                        LastSoulSwapInhaleTargetSet = DateTime.Now;
                        MultidotTarget = null;
                        return RunStatus.Success;
                    }
                )
            );
        }

        private static Composite UnstableAfflictionASAP() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.UnstableAffliction &&
                !Me.IsMoving &&
                SpellManager.HasSpell("Unstable Affliction") &&
                !(LastUATarget != null && LastUATarget == ASAPAttackTarget && LastUACast.AddSeconds(3) >= DateTime.Now) &&
                MyAuraTimeLeft("Unstable Affliction", ASAPAttackTarget) <= AfflictionSettings.Instance.UnstableAfflictionRefresh,
                new Action(
                    delegate {
                        CastSpell("Unstable Affliction", ASAPAttackTarget, "ASAP");
                        LastUATarget = ASAPAttackTarget;
                        LastUACast = DateTime.Now;
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region   Haunt

        private static Composite Haunt() {
            return new Decorator(
                ret =>
                Me.CurrentSoulShards >= 1 &&
                LastHauntCast.AddSeconds(3) <= DateTime.Now &&
                SafelyFaceTarget(Me.CurrentTarget) &&
                !Me.IsMoving &&
                SpellManager.HasSpell("Haunt") &&
                MyAuraTimeLeft("Haunt", Me.CurrentTarget) <= 2500,
                new Action(
                    delegate {
                        CastSpell("Haunt", Me.CurrentTarget);
                        LastHauntCast = DateTime.Now;
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region   Malefic Grasp

        private static Composite MaleficGrasp() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.MaleficGrasp &&
                SafelyFaceTarget(Me.CurrentTarget) &&
                (!Me.IsMoving || TalentNames.Contains("Kil'jaeden's Cunning")) &&
                SpellManager.HasSpell("Malefic Grasp") &&
                !(Me.IsChanneling && Me.ChanneledCastingSpellId == 103103) &&
                ((Me.CurrentTarget.HasAura("Haunt") && Me.HasAura("Dark Soul: Misery")) || getNumberOfUndottedUnits() == 0),
                new Action(
                    delegate {
                        CastSpell("Malefic Grasp", Me.CurrentTarget);
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region   Drain Soul

        private static Composite DrainSoul() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.DrainSoul &&
                SafelyFaceTarget(Me.CurrentTarget) &&
                (Me.CurrentTarget.HealthPercent <= 20 || (AfflictionSettings.Instance.DrainSoulForShards && Me.CurrentSoulShards == 0)) &&
                !Me.IsMoving &&
                SpellManager.HasSpell("Drain Soul"),
                new Action(
                    delegate {
                        if (Me.CurrentTarget.HealthPercent <= 20) {
                            StopCasting("Drain Soul");
                        }
                        CastSpell("Drain Soul", Me.CurrentTarget);
                        DrainSoulTarget = Me.CurrentTarget;
                        return RunStatus.Success;
                    }
                )
            );
        }

        private static Composite DrainSoulASAP() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.DrainSoul &&
                SafelyFaceTarget(ASAPAttackTarget) &&
                !Me.IsMoving &&
                SpellManager.HasSpell("Drain Soul"),
                new Action(
                    delegate {
                        StopCasting("ASAP");
                        CastSpell("Drain Soul", ASAPAttackTarget, "ASAP");
                        DrainSoulTarget = ASAPAttackTarget;
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        //Utility        

        #region   Dark Soul

        private static Composite DarkSoul() {
            return new Decorator(
                ret =>                    
                SpellManager.HasSpell("Dark Soul") &&
                GetSpellCooldown(SpellIds.Instance.DarkSoul) <= MyLatency,
                new Action(
                    delegate {
                        CastSpell("Dark Soul", Me);
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        #region   Soulburn

        private static Composite SoulburnSoulSwap() {
            return new Decorator(
                ret =>
                    !CastingorGCDL() &&
                    Me.CurrentTarget.HealthPercent > 5 &&
                (Me.CurrentSoulShards >= 1 || Me.HasAura("Soulburn")) &&
                SpellManager.HasSpell("Soulburn") &&
                SpellManager.HasSpell("Soul Swap") &&
                SpellManager.Spells["Soul Swap"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                MyAuraTimeLeft("Agony", Me.CurrentTarget) == 0 &&
                MyAuraTimeLeft(146739, Me.CurrentTarget) == 0 &&
                MyAuraTimeLeft("Unstable Affliction", Me.CurrentTarget) == 0,
                new Action(
                    delegate {
                        if (!Me.HasAura("Soulburn")) {
                            CastSpell("Soulburn", Me, "Target");
                        }
                        CastSpell("Soul Swap", Me.CurrentTarget);
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region Soul Swap

        private static Composite SoulSwapInhale() {
            return new Decorator(
                ret =>
                !GCDL() &&
                SoulSwapInhaleTarget != null &&
                SoulSwapInhaleTarget.IsValid &&
                SoulSwapInhaleTarget.Distance <= 40 &&
                LastSoulSwapInhaleTargetSet.AddSeconds(5) >= DateTime.Now &&
                !Me.HasAura("Soul Swap"),
                new Action(
                    delegate {

                        CastSpell("Soul Swap", SoulSwapInhaleTarget, "Inhale");
                        return RunStatus.Success;
                    }
                )
            );
        }

        private static Composite SoulSwapExhale() {
            return new Decorator(
                ret =>
                !GCDL() &&
                Me.HasAura("Soul Swap"),
                new Action(
                    delegate {
                        CastSpell("Soul Swap", MultidotTarget, "Exhale");
                        MultidotTarget = null;
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region  Will of the Forsaken

        private static Composite WotF()
        {
            return new Decorator(
                ret =>
                AfflictionSettings.Instance.WotF &&
                Me.Race == WoWRace.Undead &&
                SpellManager.HasSpell("Will of the Forsaken") &&
                SpellManager.Spells["Will of the Forsaken"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                IsFearedOrCharmed(Me),
                new Action(
                    delegate { 
                        CastSpell("Will of the Forsaken", Me);
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        #region  DPS Trinket

        private static Composite DPSTrinket(String cause) {
            return new Decorator(
                ret =>
                AfflictionSettings.Instance.useTrinketWithDP &&
                ((AfflictionSettings.Instance.trinketSlotNumber == 13 && Me.Inventory.Equipped.Trinket1.CooldownTimeLeft.TotalMilliseconds < MyLatency) ||
                (AfflictionSettings.Instance.trinketSlotNumber == 14 && Me.Inventory.Equipped.Trinket2.CooldownTimeLeft.TotalMilliseconds < MyLatency)),
                new Action(
                    delegate {
                        Lua.DoString("RunMacroText('/use " + AfflictionSettings.Instance.trinketSlotNumber + "');");
                        Logging.Write("Using DPS trinket to " + cause + ".");
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        #region Healthstone

        private static Composite CreateHealthstone() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.CreateHealthstone &&
                LastHealthstoneCreated.AddSeconds(7) <= DateTime.Now &&
                !Me.Combat &&
                SpellManager.HasSpell("Create Healthstone") &&
                !CastingorGCDL() &&
                !Me.HasAura("Preparation") &&
                !Me.HasAura("Arena Preparation") &&
                Me.BagItems.FirstOrDefault(o => o.Entry == 5512) == null &&
                StyxWoW.Me.FreeNormalBagSlots >= 1,
                new Action(
                    delegate {
                        CastSpell("Create Healthstone", Me);
                        LastHealthstoneCreated = DateTime.Now;
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region     Dark Intent

        private static Composite DarkIntent() {
            return new Decorator(
                ret =>
                LastCastSpell != "Dark Intent" &&
                Me.CurrentMana >= Manacosts.Instance.DarkIntent &&
                SpellManager.HasSpell("Dark Intent") &&                
                !CastingorGCDL() &&
                !Me.HasAura("Dark Intent"),
                new Action(
                    delegate {
                        CastSpell("Dark Intent", Me);
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region     Soulstone

        private static Composite Soulstone() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.Soulstone &&
                !Me.Combat &&
                !Me.CurrentMap.IsArena &&
                !Me.IsFFAPvPFlagged &&
                AfflictionSettings.Instance.Soulstone &&
                SpellManager.HasSpell("Soulstone") &&
                SpellManager.Spells["Soulstone"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                !CastingorGCDL() &&
                !Me.HasAura("Soulstone"),
                new Action(
                    delegate {
                        CastSpell("Soulstone", Me);
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region     Sacrifice Pet

        private static Composite SacrificePet() {
            return new Decorator(
                ret =>
                Me.GotAlivePet &&
                TalentNames.Contains("Grimoire of Sacrifice") &&
                SpellManager.Spells["Grimoire of Sacrifice"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&    
                !CastingorGCDL() &&
                !Me.HasAura("Grimoire of Sacrifice"),
                new Action(
                    delegate {
                        CastSpell("Grimoire of Sacrifice", Me, "Sacrificed pet: " + Me.Pet.Name);                        
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion
        
        #region     Summon Pet

        private static Composite SummonPet() {
            return new Decorator(
                ret =>
                (!Me.GotAlivePet ||
                (Me.GotAlivePet && Me.Pet.CreatureFamilyInfo.Id != getPreferredPetId())) &&
                (!Me.IsMoving || Me.CurrentSoulShards >= 1 || !Me.HasAura("Soulburn")) &&
                LastSummon.AddSeconds(summonDelay) < DateTime.Now &&
                !Me.HasAura("Grimoire of Sacrifice") &&    
                !CastingorGCDL() &&
                SpellManager.HasSpell("Summon " + getPreferredPetName()) &&
                SpellManager.Spells["Summon " + getPreferredPetName()].CooldownTimeLeft.TotalMilliseconds <= MyLatency,
                new Action(
                    delegate {
                        if ((Me.Combat || Me.IsMoving) && Me.CurrentSoulShards >= 1 && !Me.HasAura("Soulburn")) {
                            CastSpell("Soulburn", Me, "Summoning");
                        }
                        
                        CastSpell("Summon " + getPreferredPetName(), Me);
                        LastSummon = DateTime.Now;
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region     Life Tap

        private static Composite LifeTap() {
            return new Decorator(
                ret =>
                Me.ManaPercent <= 50 &&
                Me.HealthPercent > AfflictionSettings.Instance.DontCastSpellsForHpBelowHp &&
                SpellManager.HasSpell("Life Tap") &&
                Me.Combat &&
                !CastingorGCDL(),
                new Action(
                    delegate {
                        CastSpell("Life Tap", Me);
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion


        #region     Dark Bargain

        private static Composite DarkBargain() {
            return new Decorator(
                ret =>
                Me.HealthPercent <= AfflictionSettings.Instance.DarkBargainHP &&
                SpellManager.HasSpell("Dark Bargain") &&
                SpellManager.Spells["Dark Bargain"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                Me.Combat &&
                LastDefensive.AddSeconds(defensiveDelay) < DateTime.Now,
                new Action(
                    delegate {
                        StopCasting("Low HP");
                        CastSpell("Dark Bargain", Me);
                        LastDefensive = DateTime.Now;
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        #region     Unending Resolve

        private static Composite UnendingResolve() {
            return new Decorator(
                ret =>
                    !GlyphNames.Contains("Eternal Resolve") &&                    
                Me.HealthPercent <= AfflictionSettings.Instance.UnendingResolveHp &&
                SpellManager.HasSpell("Unending Resolve") &&
                SpellManager.Spells["Unending Resolve"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                Me.Combat &&
                LastDefensive.AddSeconds(defensiveDelay) < DateTime.Now,
                new Action(
                    delegate {
                        StopCasting("Low HP");
                        CastSpell("Unending Resolve", Me);
                        LastDefensive = DateTime.Now;
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        #region     Sacrificial Pact

        private static Composite SacrificialPact() {
            return new Decorator(
                ret =>
                Me.HealthPercent <= AfflictionSettings.Instance.SacrificialPactHP &&
                SpellManager.HasSpell("Sacrificial Pact") &&
                SpellManager.Spells["Sacrificial Pact"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                Me.Combat &&
                (Me.GotAlivePet || Me.HealthPercent >= 26) &&
                LastDefensive.AddSeconds(defensiveDelay) < DateTime.Now,
                new Action(
                    delegate {
                        StopCasting("Low HP");
                        CastSpell("Sacrificial Pact", Me);
                        LastDefensive = DateTime.Now;
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        //Heals

        #region UseHealthstone

        private static Composite UseHealthstone() {
            return new Decorator(
                ret =>
                Me.Combat && Me.HealthPercent < AfflictionSettings.Instance.HealthstonePercent &&
                LastDefensive.AddSeconds(defensiveDelay) < DateTime.Now,
                new Action(
                        delegate {

                        WoWItem hs = Me.BagItems.FirstOrDefault(o => o.Entry == 5512); //5512 Healthstone
                        if (hs != null && hs.CooldownTimeLeft.TotalMilliseconds <= MyLatency) {
                            StopCasting("Low HP");
                            if (SpellManager.HasSpell("Dark Regeneration") &&
                                SpellManager.Spells["Dark Regeneration"].CooldownTimeLeft.TotalMilliseconds <= MyLatency) {
                                CastSpell("Dark Regeneration", Me, "With Healthstone");
                                hs.Use();
                                LastDefensive = DateTime.Now;
                                Logging.Write("Use Healthstone at " + Me.HealthPercent + "%");
                                return RunStatus.Success;
                            }

                            hs.Use();
                            LastDefensive = DateTime.Now;
                            Logging.Write("Use Healthstone at " + Me.HealthPercent + "%");
                            return RunStatus.Failure;
                        } else {
                            if (hs == null) { //we still have to use it even if we don't have HS
                                if (SpellManager.HasSpell("Dark Regeneration") &&
                                    SpellManager.Spells["Dark Regeneration"].CooldownTimeLeft.TotalMilliseconds <= MyLatency) {
                                    StopCasting("Low HP");
                                    CastSpell("Dark Regeneration", Me, "Without Healthstone");
                                    LastDefensive = DateTime.Now;
                                    return RunStatus.Success;
                                }
                            }
                        }
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        #region     Drain Life

        private static Composite DrainLife() {
            return new Decorator(
                ret =>
                Me.HealthPercent <= AfflictionSettings.Instance.DrainLifeBelowHp &&
                !Me.IsMoving &&
                !CastingorGCDL() &&
                SpellManager.HasSpell("Drain Life"),
                new Action(
                    delegate {
                        if (!Me.HasAura("Soulburn") && Me.HealthPercent <= 20) {
                            CastSpell("Soulburn", Me, "Low Health!");
                        }

                        CastSpell("Drain Life", Me.CurrentTarget);
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion 
    }
}