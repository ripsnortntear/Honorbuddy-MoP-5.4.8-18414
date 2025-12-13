#region

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

#endregion

namespace Superbad
{
    public partial class Superbad
    {
        /*Energy functions*/

        public static bool HasEnergyForThrash
        {
            get { return _energy >= EnergyForThrash || buff.omen_of_clarity.react; }
        }

        public static double EnergyForThrash
        {
            get { return (buff.berserk.up && _currentSpec == WoWSpec.DruidFeral ? 25 : 50); }
        }

        public static bool HasEnergyForRake
        {
            get { return _energy >= EnergyForRake || buff.omen_of_clarity.react; }
        }

        public static double EnergyForRake
        {
            get { return (buff.berserk.up && _currentSpec == WoWSpec.DruidFeral ? 18 : 35); }
        }

        public static bool HasEnergyForRip
        {
            get { return _energy >= EnergyForRip || buff.omen_of_clarity.react; }
        }

        public static double EnergyForRip
        {
            get { return (buff.berserk.up && _currentSpec == WoWSpec.DruidFeral ? 15 : 30); }
        }

        public static bool HasEnergyForMangle
        {
            get { return _energy >= EnergyForMangle || buff.omen_of_clarity.react; }
        }

        public static double EnergyForMangle
        {
            get { return (buff.berserk.up && _currentSpec == WoWSpec.DruidFeral ? 18 : 35); }
        }

        public static bool HasEnergyForShred
        {
            get { return _energy >= EnergyForShred || buff.omen_of_clarity.react; }
        }

        public static double EnergyForShred
        {
            get { return (buff.berserk.up && _currentSpec == WoWSpec.DruidFeral ? 20 : 40); }
        }

        public static bool HasEnergyForFerociousBite
        {
            get { return _energy >= EnergyForFerociousBite || buff.omen_of_clarity.react; }
        }

        public static double EnergyForFerociousBite
        {
            get { return (buff.berserk.up && _currentSpec == WoWSpec.DruidFeral ? 25 : 50); }
        }

        public static bool HasEnergyForRavage
        {
            get { return _energy >= EnergyForRavage || buff.omen_of_clarity.react; }
        }

        public static double EnergyForRavage
        {
            get { return (buff.berserk.up && _currentSpec == WoWSpec.DruidFeral ? 23 : 45); }
        }

        public static bool HasEnergyForSwipe
        {
            get { return _energy >= EnergyForSwipe || buff.omen_of_clarity.react; }
        }

        public static double EnergyForSwipe
        {
            get { return (buff.berserk.up && _currentSpec == WoWSpec.DruidFeral ? 23 : 45); }
        }

        public static bool HasEnergyForPounce
        {
            get { return _energy >= EnergyForPounce || buff.omen_of_clarity.react; }
        }

        public static double EnergyForPounce
        {
            get { return (buff.berserk.up && _currentSpec == WoWSpec.DruidFeral ? 25 : 50); }
        }

        public static bool HasEnergyForSavageRoar
        {
            get { return _energy >= EnergyForSavageRoar; }
        }

        public static double EnergyForSavageRoar
        {
            get { return (buff.berserk.up && _currentSpec == WoWSpec.DruidFeral ? 13 : 25); }
        }

        public static bool HasEnergyForDeathCoil
        {
            get { return _energy >= 40; }
        }

        public static bool HasRageForMaul
        {
            get { return _rage >= 30; }
        }

        public static bool HasRageForSavageDefense
        {
            get { return _rage >= 60; }
        }

        public static bool HasRageForSpellReflection
        {
            get { return _rage >= 15; }
        }


        public static WoWUnit InterruptUnit { get; set; }

        protected static WoWUnit soulswaptarget
        {
            get
            {
                return
                    UnitList
                        .Where(
                            p =>
                                p.IsWithinMeleeRange && target.time_to_die_circle(p) > 10 &&
                                StyxWoW.Me.IsFacing(p) && p != StyxWoW.Me.CurrentTarget && !dot.rip.ticking_unit(p))
                        .ToList()
                        .FirstOrDefault();
            }
        }

        private static bool bear_form()
        {
            if (CurrentShape == ShapeshiftForm.Bear)
                return false;
            return SuperbadSettings.Instance.Form != SuperbadSettings.Shapeshift.MANUAL && Spell.Buff(5487);
        }

        private static void berserking()
        {
            if (BotbaseManualMode() && SuperbadSettings.Instance.UseBurst &&
                SuperbadSettings.Instance.BurstKey != SuperbadSettings.Keypress.NONE)
                if (SuperbadSettings.Instance.UseBurstBerserking)
                    return;
            if (!SuperbadSettings.Instance.UseRacial)
                return;
            if (buff.bloodlust.up)
                return;
            if (!_validtargetinrange)
                return;
            if (_targetNotBoss)
                return;
            if (StyxWoW.Me.Race != WoWRace.Troll)
                return;
            Spell.BuffOFFGCD(26297);
        }

        private static bool tigers_fury()
        {
            if (BotbaseManualMode() && SuperbadSettings.Instance.UseBurst &&
                SuperbadSettings.Instance.BurstKey != SuperbadSettings.Keypress.NONE)
                if (SuperbadSettings.Instance.UseBurstTigers)
                    return false;
            if (buff.berserk.up)
                return false;
            if (!_validtargetinrange)
                return false;
            if (talent.incarnation.enabled && cooldown.incarnation.remains <= _gcdTimeLeftTotalSeconds)
                return false;
            return !(target.time_to_die <= 3) && Spell.BuffOFFGCD(5217);
        }

        private static bool thrash_cat()
        {
            if (!SuperbadSettings.Instance.UseThrash)
                return false;
            if (SuperbadSettings.Instance.ThrashUsage == 1 && !buff.omen_of_clarity.react)
                return false;
            if (StyxWoW.Me.Specialization == WoWSpec.DruidGuardian)
                if (_unitCount > 0 && HasEnergyForThrash && Spell.CastnoFace(106830, true))
                {
                    return true;
                }
            if (_unitCount <= 0 || !HasEnergyForThrash || !Spell.CastnoFace(106832)) return false;
            return true;
        }

        private static bool rip()
        {
            return _validtargetinrange && HasEnergyForRip && Spell.Cast(1079);
        }

        private static bool rake()
        {
            if (!_validtargetinrange)
                return false;
            if (!HasEnergyForRake)
                return false;
            if (!Spell.Cast(1822)) return false;
            return true;
        }

        private static bool rake_cycle(WoWUnit unit)
        {
            if (!HasEnergyForRake)
                return false;
            if (!Spell.Cast(1822, unit)) return false;
            return true;
        }

        private static bool mangle_cat()
        {
            return _validtargetinrange && HasEnergyForMangle && Spell.Cast(33876);
        }

        private static bool shred()
        {
            if (!_validtargetinrange)
                return false;
            if (_currentSpec == WoWSpec.DruidGuardian)
                return mangle_cat();
            if (StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.MeIsBehind &&
                (!TalentManager.Shred || (TalentManager.Shred && !buff.tigers_fury.up && !buff.berserk.up)))
                return HasEnergyForShred && Spell.Cast(5221);
            if (TalentManager.Shred && (buff.tigers_fury.up ||
                                        buff.berserk.up))
            {
                return HasEnergyForShred && Spell.Cast("Shred");
            }
            return false;
        }

        private static void virmens_bite_potion()
        {
            if (SuperbadSettings.Instance.UseBurst &&
                SuperbadSettings.Instance.BurstKey != SuperbadSettings.Keypress.NONE)
                if (SuperbadSettings.Instance.UseBurstVirmens)
                    return;
            if (!SuperbadSettings.Instance.UsePotion)
                return;
            if (_targetNotBoss)
                return;
            if (!_validtargetinrange)
                return;
            WoWItem virmens = ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(item => item.Entry == 76089);
            if (virmens == null)
                return;
            if (virmens.Usable && virmens.CooldownTimeLeft.TotalSeconds <= 0)
            {
                Spell.LogAction(virmens.Name, Color.Yellow);
                virmens.Use();
            }
        }

        private static bool ferocious_bite()
        {
            if (!_validtargetinrange)
                return false;
            if (combo_points < 1)
                return false;
            if (!HasEnergyForFerociousBite)
                return false;
            return Spell.Cast(22568);
        }

        private static bool Swipe_Cat()
        {
            return _unitCount > 0 && HasEnergyForSwipe && Spell.CastnoFace(62078);
        }

        private static bool faerie_firecatcombat()
        {
            if (!SuperbadSettings.Instance.UseFff)
                return false;
            if (!_validtargetinrange)
                return false;
            return TalentManager.FaerieSwarm ? Spell.Cast(102355, true) : Spell.Cast(770, true);
        }

        private static bool fff_cycle(WoWUnit unit)
        {
            if (!SuperbadSettings.Instance.UseFff)
                return false;
            if (unit == null || unit == StyxWoW.Me.CurrentTarget)
                return false;
            return TalentManager.FaerieSwarm
                ? !Spell.SpellOnCooldown(102355) && Spell.Cast(102355, unit)
                : !Spell.SpellOnCooldown(770) && Spell.Cast(770, unit);
        }

        private static bool mark_of_the_wild()
        {
            if (!HasSpellMarkoftheWild)
                return false;
            if (StyxWoW.Me.CurrentTarget != null)
                StyxWoW.Me.ClearTarget();
            return Spell.Buff(1126);
        }

        private static bool Redirect()
        {
            if (!HasSpellRedirect)
                return false;
            if (!SuperbadSettings.Instance.SymbSpell)
                return false;
            if (combo_points != 0) return false;
            if (StyxWoW.Me.RawComboPoints == 0) return false;
            return target.time_to_die > 3 && Spell.CastSymbSpell(110730, StyxWoW.Me.CurrentTarget);
        }

        private static bool Soulswap()
        {
            if (!HasSpellSoulSwap)
                return false;
            if (!SuperbadSettings.Instance.SymbSpell)
                return false;
            if (!dot.rake.ticking || !dot.rip.ticking) return false;
            return soulswaptarget != null && Spell.CastSymbSpell(110810, soulswaptarget);
        }

        private static bool cat_form()
        {
            if (CurrentShape == ShapeshiftForm.Cat)
                return false;
            return SuperbadSettings.Instance.Form != SuperbadSettings.Shapeshift.MANUAL &&
                   Spell.Buff(768);
        }

        private static bool ravage()
        {
            if (!_validtargetinrange)
            {
                return false;
            }

            if (buff.stampede.up)
            {
                return Spell.CastHackFON(102545);
            }

            if (buff.king_of_the_jungle.up)
            {
                if (!HasEnergyForRavage)
                {
                    Spell.LogAction("Pooling energy for Ravage!", Color.DarkOrange);
                    return true;
                }
                return Spell.CastHackFON(102545);
            }


            if (buff.prowl.up &&
                StyxWoW.Me.CurrentTarget != null &&
                StyxWoW.Me.CurrentTarget.MeIsBehind)
                return HasEnergyForRavage && Spell.CastHackFON(6785);
            return false;
        }

        private static bool pounce()
        {
            return _validtargetinrange && HasEnergyForPounce &&
                   Spell.Cast(9005, true);
        }

        private static bool savage_roar()
        {
            if (CurrentShape != ShapeshiftForm.Cat)
                return false;
            if (!HasSpellSavageRoar)
                return false;
            if (!TalentManager.Savagery && StyxWoW.Me.RawComboPoints < 1)
                return false;
            if (TalentManager.Savagery)
            {
                if (HasEnergyForSavageRoar)
                    if (Spell.CastHack(127538))
                    {
                        return true;
                    }
            }
            if (!HasEnergyForSavageRoar) return false;
            if (!Spell.CastHack(52610)) return false;
            return true;
        }

        private static bool aquatic_form()
        {
            if (!HasSpellAquaticForm)
                return false;
            if (!SuperbadSettings.Instance.UseAquatic)
                return false;
            if (CurrentShape == ShapeshiftForm.Aqua)
                return false;
            if (StyxWoW.Me.CurrentTarget != null && _distance < 16)
                return false;
            return Spell.Buff(1066);
        }

        private static bool prowl()
        {
            if (BotPoi.Current.Type == PoiType.Loot || BotPoi.Current.Type == PoiType.Harvest ||
                BotPoi.Current.Type == PoiType.Skin ||
                ObjectManager.GetObjectsOfType<WoWUnit>()
                    .Any(
                        u =>
                            u.IsDead &&
                            ((CharacterSettings.Instance.LootMobs && u.CanLoot && u.Lootable) ||
                             (CharacterSettings.Instance.SkinMobs && u.Skinnable && u.CanSkin)) &&
                            u.Distance < CharacterSettings.Instance.LootRadius))
            {
                return false;
            }
            return SuperbadSettings.Instance.PullStealth && Spell.BuffOFFGCD(5215);
        }

        private static bool LightningShield()
        {
            if (!SuperbadSettings.Instance.SymbSpell)
                return false;
            return _currentSpec == WoWSpec.DruidGuardian && Spell.CastSymbSpell(110803);
        }

        public static void Travelform()
        {
            if (!SuperbadSettings.Instance.UseTravel)
                return;
            if (!StyxWoW.Me.IsCasting && !StyxWoW.Me.IsChanneling && !SpellManager.GlobalCooldown
                && Context.SuperbadRoutine.CurrentWoWContext != WoWContext.Instances && StyxWoW.Me.IsMoving
                && StyxWoW.Me.IsAlive
                && !StyxWoW.Me.OnTaxi && !StyxWoW.Me.InVehicle && !StyxWoW.Me.Mounted && !StyxWoW.Me.IsOnTransport
                && !StyxWoW.Me.IsIndoors && StyxWoW.Me.IsOutdoors
                && CurrentShape != ShapeshiftForm.Travel
                && SpellManager.HasSpell("Travel Form") && BotPoi.Current != null
                && !StyxWoW.Me.GotTarget
                && BotPoi.Current.Type != PoiType.Kill
                && BotPoi.Current.Location.Distance(StyxWoW.Me.Location) > 10
                &&
                (BotPoi.Current.Location.Distance(StyxWoW.Me.Location) < CharacterSettings.Instance.MountDistance ||
                 !CharacterSettings.Instance.UseMount ||
                 (StyxWoW.Me.GetSkill(SkillLine.Riding).CurrentValue == 0))
                && !_targetAboveGround)
            {
                Spell.Buff(783);
            }
        }

        private static WoWPlayer SymbiosisTargetGuardian()
        {
            if (!SpellManager.HasSpell("Symbiosis"))
                return null;
            return Unit.NearbyGroupMembers.FirstOrDefault(
                p => IsValidSymbiosisTarget(p) && p.Class == WoWClass.DeathKnight) // Bone Shield
                   ??
                   (Unit.NearbyGroupMembers.FirstOrDefault(
                       p => IsValidSymbiosisTarget(p) && p.Class == WoWClass.Paladin) // Consecration
                    ??
                    (Unit.NearbyGroupMembers.FirstOrDefault(
                        p => IsValidSymbiosisTarget(p) && p.Class == WoWClass.Shaman) // Lightning Shield
                     ??
                     Unit.NearbyGroupMembers.FirstOrDefault(
                         p => IsValidSymbiosisTarget(p) && p.Class == WoWClass.Warrior) // Spell Reflect
                     ?? (Unit.NearbyGroupMembers.FirstOrDefault(
                         p => IsValidSymbiosisTarget(p) && p.Class == WoWClass.Monk) // ElusiveBrew
                         )
                        )
                       );
        }

        private static WoWPlayer SymbiosisTargetFeral()
        {
            if (!SpellManager.HasSpell("Symbiosis"))
                return null;
            if (!StyxWoW.Me.IsInGroup())
                return null;
            return Unit.NearbyGroupMembers.FirstOrDefault(
                p => IsValidSymbiosisTarget(p) && p.Class == WoWClass.Shaman) // Feral Spirit
                   ??
                   (Unit.NearbyGroupMembers.FirstOrDefault(
                       p => IsValidSymbiosisTarget(p) && p.Class == WoWClass.Warrior) // ShatteringBlow
                    ??
                    (Unit.NearbyGroupMembers.FirstOrDefault(
                        p => IsValidSymbiosisTarget(p) && p.Class == WoWClass.Monk) // Clash
                     ??
                     Unit.NearbyGroupMembers.FirstOrDefault(
                         p => IsValidSymbiosisTarget(p) && p.Class == WoWClass.DeathKnight) // Deathcoil
                        )
                       );
        }

        private static void Symbiosis(WoWUnit unit)
        {
            if (!SuperbadSettings.Instance.SymbTarget)
                return;

            if (StyxWoW.Me.CurrentTarget != unit)
            {
                unit.Target();
                return;
            }
            Spell.Cast(110309, unit);
            if (StyxWoW.Me.HasBuff(110309))
                StyxWoW.Me.ClearTarget();
        }

        public static bool IsValidSymbiosisTarget(WoWPlayer p)
        {
            if (p == null)
                return false;

            if (p.IsHorde != StyxWoW.Me.IsHorde)
                return false;

            if (p.Class == WoWClass.Warrior && NeedCat())
                if (!StyxWoW.Me.GroupInfo.IsInRaid)
                    return false;

            if (Blacklist.Contains(p.Guid, BlacklistFlags.Combat))
                return false;

            if (!p.IsAlive)
                return false;

            if (p.Level < 87)
                return false;

            if (p.Combat)
                return false;

            if (p.Distance > 28)
                return false;

            return !p.HasAura("Symbiosis") && p.InLineOfSpellSight;
        }

        private static bool growl()
        {
            if (SuperbadSettings.Instance.UseTauntBosses)
            {
                if (StyxWoW.Me.FocusedUnit != null && StyxWoW.Me.GroupInfo.IsInRaid && StyxWoW.Me.CurrentTarget != null &&
                    (Unit.IsBoss(StyxWoW.Me.CurrentTarget) || StyxWoW.Me.CurrentTarget.IsBoss))
                {
                    if (TankManager.IsReconingCastDesired() && !Spell.SpellOnCooldownOffGCD(6795))
                        return Spell.Cast(6795, StyxWoW.Me.CurrentTarget);
                    return false;
                }
            }

            if(StyxWoW.Me.GroupInfo.IsInRaid && StyxWoW.Me.CurrentTarget != null &&
                    (Unit.IsBoss(StyxWoW.Me.CurrentTarget) || StyxWoW.Me.CurrentTarget.IsBoss))
                return false;


            if (!SuperbadSettings.Instance.UseTaunt)
                return false;
            WoWUnit firstOrDefault = TankManager.Instance.NeedToTaunt.FirstOrDefault();
            return firstOrDefault != null && ((!StyxWoW.Me.GroupInfo.IsInRaid ||
                                               (StyxWoW.Me.GroupInfo.IsInRaid && !firstOrDefault.IsBoss)) &&
                                              StyxWoW.Me.IsFacing(firstOrDefault) &&
                                              !Spell.SpellOnCooldownOffGCD(6795) &&
                                              Spell.Cast(6795, firstOrDefault));
        }

        private static bool bear_hug()
        {
            if (!SuperbadSettings.Instance.UseBearHug)
                return false;
            if (!_validtargetinrange)
                return false;
            if (!_targetNotBoss)
                return false;
            if (Context.SuperbadRoutine.CurrentWoWContext != WoWContext.Normal) return false;
            if (buff.berserk.up) return false;
            return _unitCount == 1 && Spell.Cast(102795, true);
        }

        private static void Maul()
        {
            if (!_validtargetinrange)
                return;
            if (_currentSpec == WoWSpec.DruidFeral)
                if ((_calcfrenziedheal/2 >= _healthmissing) || _rage > 89)
                {
                    if (!HasRageForMaul) return;
                    Spell.CastOFFGCD(6807);
                    return;
                }
            if (_currentSpec != WoWSpec.DruidGuardian) return;
            if ((_calcfrenziedheal/2 >= _healthmissing) && _rage > 89 &&
                buff.savage_defense.down)
            {
                if (!HasRageForMaul) return;
                Spell.CastOFFGCD(6807);
                return;
            }
            if (StyxWoW.Me.GotTarget && StyxWoW.Me.CurrentTarget.GotTarget && StyxWoW.Me.CurrentTarget.CurrentTarget != StyxWoW.Me &&
                Group.Tanks.Contains(StyxWoW.Me.CurrentTarget.CurrentTarget))
            {
                if (!HasRageForMaul) return;
                Spell.CastOFFGCD(6807);
            }
        }

        private static bool Frenzied_Regeneration()
        {
            if (!SuperbadSettings.Instance.UseFrenziedRegen)
                return false;
            if (_rage < 60)
                return false;
            if (_currentSpec == WoWSpec.DruidFeral)
                if (_healthPercent < SuperbadSettings.Instance.Frenzied)
                    return Spell.HealOFFGCD(22842);
            if ((_calcfrenziedheal/2 < _healthmissing) && _rage > 80 && buff.savage_defense.up)
                return Spell.HealOFFGCD(22842);
            return false;
        }

        private static void savage_defense()
        {
            if (!SuperbadSettings.Instance.UseSavageDefense)
                return;
            if (_currentSpec != WoWSpec.DruidGuardian)
                return;
            if (HasRageForSavageDefense)
                Spell.BuffOFFGCD(62606);
        }

        private static bool berserk()
        {
            if (BotbaseManualMode() && SuperbadSettings.Instance.UseBurst &&
                SuperbadSettings.Instance.BurstKey != SuperbadSettings.Keypress.NONE)
                if (SuperbadSettings.Instance.UseBurstBerserk)
                    return false;
            if (CurrentShape == ShapeshiftForm.Cat && !SuperbadSettings.Instance.UseBerserk)
                return false;
            if (CurrentShape == ShapeshiftForm.Bear && !SuperbadSettings.Instance.UseBerserkBear)
                return false;
            if (buff.bloodlust.up)
                return false;
            if (!_validtargetinrange)
                return false;
            if (_targetNotBoss)
                return false;
            if (target.time_to_die <= 25)
                return false;
            return Spell.BuffOFFGCD(CurrentShape == ShapeshiftForm.Cat ? 106951 : 50334);
        }

        private static bool incarnation()
        {
            if (BotbaseManualMode() && SuperbadSettings.Instance.UseBurst &&
                SuperbadSettings.Instance.BurstKey != SuperbadSettings.Keypress.NONE)
                if (SuperbadSettings.Instance.UseBurstIncar)
                    return false;
            if (!SuperbadSettings.Instance.UseInca)
                return false;
            if (_currentSpec == WoWSpec.DruidGuardian)
                if (CurrentShape != ShapeshiftForm.Bear)
                    return false;
            if (_currentSpec == WoWSpec.DruidFeral)
                if (CurrentShape != ShapeshiftForm.Cat)
                    return false;
            if (!_validtargetinrange)
                return false;
            if (_targetNotBoss)
                return false;
            return Spell.BuffHackInca(_currentSpec == WoWSpec.DruidGuardian ? 102558 : 102543, true);
        }

        private static void natures_vigil()
        {
            if (BotbaseManualMode() && SuperbadSettings.Instance.UseBurst &&
                SuperbadSettings.Instance.BurstKey != SuperbadSettings.Keypress.NONE)
                if (SuperbadSettings.Instance.UseBurstVigil)
                    return;
            if (!SuperbadSettings.Instance.UseVigil)
                return;
            if (!_validtargetinrange)
                return;
            if (_targetNotBoss)
                return;
            Spell.BuffOFFGCD(124974);
        }

        private static bool Might_of_Ursoc()
        {
            return Spell.HealOFFGCD(106922);
        }

        private static bool cenarion_ward()
        {
            return TalentManager.CenarionWard && Spell.Heal(102351, true);
        }

        private static void enrage()
        {
            if (_currentSpec != WoWSpec.DruidFeral)
                Spell.Buff(5229, true);
        }

        private static bool Mangle()
        {
            return _validtargetinrange && Spell.Cast(33878, true);
        }

        private static bool Lacerate(WoWUnit target)
        {
            return !Spell.SpellOnCooldown(33745, true) && Spell.Cast(33745, target);
        }

        private static bool thrash_bear()
        {
            if (!SuperbadSettings.Instance.UseThrashBear)
                return false;
            return _unitCount > 0 && Spell.CastnoFace(77758, true);
        }

        private static bool faerie_fire()
        {
            if (CurrentShape == ShapeshiftForm.Cat && !SuperbadSettings.Instance.UseFff)
                return false;
            if (CurrentShape == ShapeshiftForm.Bear && !SuperbadSettings.Instance.UseFffBear)
                return false;
            if (_distance > 35)
                return false;
            return TalentManager.FaerieSwarm ? Spell.Cast(102355, true) : Spell.Cast(770, true);
        }

        private static void lifeblood()
        {
            if (BotbaseManualMode() && SuperbadSettings.Instance.UseBurst &&
                SuperbadSettings.Instance.BurstKey != SuperbadSettings.Keypress.NONE)
                if (SuperbadSettings.Instance.UseBurstLifeBlood)
                    return;
            if (!SuperbadSettings.Instance.UseBurstLifeBlood)
                return;
            if (!buff.bloodlust.up && !buff.berserk.up)
                Spell.Buff("Lifeblood", true);
        }

        private static bool Consecration()
        {
            if (!SuperbadSettings.Instance.SymbSpell)
                return false;
            if (!HasSpellConsecration)
                return false;
            if (_currentSpec != WoWSpec.DruidGuardian)
                return false;
            return _unitCount > 0 && Spell.CastSymbSpell(110701);
        }

        private static void DeathCoil()
        {
            if (!SuperbadSettings.Instance.SymbSpell)
                return;
            if (_currentSpec != WoWSpec.DruidFeral)
                return;
            if (!HasEnergyForDeathCoil)
                return;
            if (StyxWoW.Me.CurrentTarget == null)
                return;
            if (_distance > 8)
                if (time_to_max < 1)
                {
                    Spell.CastSymbSpell(122282, StyxWoW.Me.CurrentTarget);
                }
        }

        private static bool ShatteringBlow()
        {
            if (!HasSpellShattering)
                return false;
            if (!SuperbadSettings.Instance.SymbSpell)
                return false;
            if (_currentSpec != WoWSpec.DruidFeral)
                return false;
            if (_targetNotBoss)
                return false;
            if (StyxWoW.Me.CurrentTarget == null)
                return false;
            return Spell.CastSymbSpell(112997, StyxWoW.Me.CurrentTarget);
        }

        private static bool FeralSpirit()
        {
            if (BotbaseManualMode() && SuperbadSettings.Instance.UseBurst &&
                SuperbadSettings.Instance.BurstKey != SuperbadSettings.Keypress.NONE)
                if (SuperbadSettings.Instance.UseBurstFeralSpirit)
                    return false;
            if (!SuperbadSettings.Instance.SymbSpell)
                return false;
            if (_currentSpec != WoWSpec.DruidFeral)
                return false;
            if (_targetNotBoss)
                return false;
            if (!_validtargetinrange)
                return false;
            return Spell.CastSymbSpell(110807);
        }

        private static bool Swipe()
        {
            return _unitCount > 0 && Spell.CastnoFace(779, true);
        }

        private static bool moonfire()
        {
            if (StyxWoW.Me.CurrentTarget != null && !StyxWoW.Me.CurrentTarget.InLineOfSpellSight)
                return false;
            return !(_distance > 40) && Spell.Cast(8921);
        }

        private static void auto_attack()
        {
            if (StyxWoW.Me.IsAutoAttacking)
                return;
            if (!_validtargetinrange)
                return;
            Lua.DoString("StartAttack()");
        }

        private static bool mighty_bash(WoWUnit interruptUnit)
        {
            if (!SuperbadSettings.Instance.MightyBash)
                return false;
            if (interruptUnit != null && interruptUnit.IsPet)
                return false;
            if (CurrentShape != ShapeshiftForm.Cat && CurrentShape != ShapeshiftForm.Bear)
                return false;
            if (!TalentManager.MightyBash) return false;
            if (interruptUnit != null && !interruptUnit.IsWithinMeleeRange)
                return false;
            if (Spell.DoubleCastEntries.ContainsKey("Interrupt")) return false;
            if (Spell.SpellOnCooldown(5211, true))
                return false;
            if (!Spell.Cast(5211, interruptUnit)) return false;
            Spell.UpdateDoubleCastEntries("Interrupt", 0.5);
            return true;
        }

        private static bool skull_bash_cat(WoWUnit interruptUnit)
        {
            if (!SuperbadSettings.Instance.UseSkullBash)
                return false;
            if (interruptUnit != null && interruptUnit.IsPet)
                return false;
            if (CurrentShape != ShapeshiftForm.Cat && CurrentShape != ShapeshiftForm.Bear)
                return false;
            if (interruptUnit != null
                && StyxWoW.Me.IsFacing(interruptUnit) &&
                interruptUnit.Distance > 13)
                return false;
            if (Spell.DoubleCastEntries.ContainsKey("Interrupt")) return false;
            if(CurrentShape == ShapeshiftForm.Cat)
                if (!Spell.CastOFFGCD(80965, interruptUnit)) return false;
            if (CurrentShape == ShapeshiftForm.Bear)
                if (!Spell.CastOFFGCD(80964, interruptUnit)) return false;
            Spell.UpdateDoubleCastEntries("Interrupt", 0.5);
            return true;
        }

        private static bool rebirth()
        {
            if (CurrentShape == ShapeshiftForm.Bear)
                return false;
            if (!SuperbadSettings.Instance.UseRebirth)
                return false;

            WoWPlayer woWPlayer;

            if (SuperbadSettings.Instance.RebirthMode == 0)
            {
                woWPlayer = Group.Tanks.FirstOrDefault(t => !t.IsMe && t.IsDead);
                if (woWPlayer == null)
                    return false;
                return !(woWPlayer.Distance >= 40) && !Spell.SpellOnCooldown(20484, true) &&
                       Spell.Cast(20484, woWPlayer);
            }
            if (SuperbadSettings.Instance.RebirthMode == 1)
            {
                woWPlayer = Group.Tanks.FirstOrDefault(t => !t.IsMe && t.IsDead) ??
                            Group.Healers.FirstOrDefault(h => !h.IsMe && h.IsDead);
                if (woWPlayer == null)
                    return false;
                return !(woWPlayer.Distance >= 40) && !Spell.SpellOnCooldown(20484, true) &&
                       Spell.Cast(20484, woWPlayer);
            }
            if (SuperbadSettings.Instance.RebirthMode == 2)
            {
                woWPlayer = (Group.Tanks.FirstOrDefault(t => !t.IsMe && t.IsDead) ??
                             Group.Healers.FirstOrDefault(h => !h.IsMe && h.IsDead)) ??
                            Group.Dps.FirstOrDefault(t => !t.IsMe && t.IsDead);
                if (woWPlayer == null)
                    return false;
                return !(woWPlayer.Distance >= 40) && !Spell.SpellOnCooldown(20484, true) &&
                       Spell.Cast(20484, woWPlayer);
            }
            if (SuperbadSettings.Instance.RebirthMode != 3) return false;
            woWPlayer = (WoWPlayer) StyxWoW.Me.FocusedUnit;
            if (woWPlayer == null)
                return false;
            return !(woWPlayer.Distance >= 40) && !Spell.SpellOnCooldown(20484, true) && Spell.Cast(20484, woWPlayer);
        }

        private static bool InterruptHandler()
        {
            if (InterruptUnit == null || (InterruptUnit != null && !InterruptUnit.IsValid) ||
                (InterruptUnit != null && InterruptUnit.IsValid && !InterruptUnit.IsCasting))
            {
                if (SuperbadSettings.Instance.InterruptAnyone)
                {
                    InterruptUnit =
                        UnitList.FirstOrDefault(
                            a =>
                                a.IsCasting && (a.CanInterruptCurrentSpellCast || a.CastingSpellId == 145599) &&
                                a.CastingSpellId != 144585);
                }

                if (!SuperbadSettings.Instance.InterruptAnyone)
                {
                    if (StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.IsCasting &&
                        (StyxWoW.Me.CurrentTarget.CanInterruptCurrentSpellCast ||
                         StyxWoW.Me.CurrentTarget.CastingSpellId == 145599) &&
                        StyxWoW.Me.CurrentTarget.CastingSpellId != 144585)
                    {
                        InterruptUnit = StyxWoW.Me.CurrentTarget;
                    }
                }
            }

            if (InterruptUnit != null && InterruptUnit.IsValid && InterruptUnit.IsCasting &&
                (InterruptUnit.CanInterruptCurrentSpellCast || InterruptUnit.CastingSpellId == 145599))
            {
                return mighty_bash(InterruptUnit) || skull_bash_cat(InterruptUnit);
            }
            return false;
        }

        private static bool healing_touch()
        {
            Lua.DoString("RunMacroText(\"/console autounshift 0\")");
            if (!StyxWoW.Me.GroupInfo.IsInParty || !SuperbadSettings.Instance.HealOthers)
                if (Spell.Heal(5185))
                {
                    Lua.DoString("RunMacroText(\"/console autounshift 1\")");
                    return true;
                }



            WoWPlayer healTarget = GetHealTarget();
            if (healTarget != null)
                if (Spell.Heal(5185, healTarget))
                {
                    Lua.DoString("RunMacroText(\"/console autounshift 1\")");
                    return true;
                }

            if (Spell.Heal(5185))
            {
                Lua.DoString("RunMacroText(\"/console autounshift 1\")");
                return true;
            }

            Lua.DoString("RunMacroText(\"/console autounshift 1\")");
            return false;
        }

        private static bool healing_touch_non_instant()
        {
            if (!StyxWoW.Me.GroupInfo.IsInParty || !SuperbadSettings.Instance.HealOthers)
                if (Spell.Heal(5185))
                {
                    return true;
                }
            WoWPlayer healTarget = GetHealTarget();
            if (healTarget != null)
                if (Spell.Heal(5185, healTarget))
                {
                    return true;
                }
            if (!Spell.Heal(5185)) return false;
            return true;
        }


        private static WoWPlayer GetHealTarget()
        {
            IEnumerable<WoWPlayer> x = ObjectManager.GetObjectsOfType<WoWPlayer>(true, true).Where(
                unit => (unit.IsInMyParty || unit.IsInMyRaid || unit.IsMe)
                        && unit.DistanceSqr < 1600 && unit.CurrentHealth > 1 && !unit.IsGhost
                        && unit.IsFriendly && unit.InLineOfSight && unit.HealthPercent < 80
                );

            IOrderedEnumerable<WoWPlayer> xx = x.OrderBy(u => u.HealthPercent);
            return xx.FirstOrDefault();
        }

        private static bool Use_Health_Stone()
        {
            WoWItem stone = StyxWoW.Me.BagItems.FirstOrDefault(i => i.Entry == 5512);
            if (stone == null)
                return false;
            if (stone.Cooldown != 0) return false;
            stone.Use();
            Spell.LogAction("Using Health Stone", Color.LimeGreen);
            return true;
        }

        private static bool life_spirit()
        {
            if (!SuperbadSettings.Instance.UsePotion)
                return false;
            WoWItem spirit = ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(item => item.Entry == 89640);
            if (spirit == null)
                return false;
            if (!spirit.Usable || !(spirit.Cooldown <= 0)) return false;
            Spell.LogAction("Using Life Spirit", Color.LimeGreen);
            spirit.Use();
            return true;
        }

        private static bool renewal()
        {
            return TalentManager.Renewal && Spell.Heal(108238, true);
        }

        private static bool Barkskin()
        {
            return Spell.HealOFFGCD(22812);
        }

        private static bool Survival_Instincts()
        {
            return Spell.HealOFFGCD(61336);
        }

        private static bool rejuvenation()
        {
            return Spell.Heal(774);
        }

        private static bool HealHandler()
        {
            if (rebirth())
                return true;
            if (buff.predatory_swiftness.up && _healthPercent < SuperbadSettings.Instance.Predatory)
                if (healing_touch())
                    return true;
            if (_healthPercent < SuperbadSettings.Instance.HealthStone)
                if (Use_Health_Stone())
                    return true;
            if (_healthPercent < SuperbadSettings.Instance.LifeSpirit)
                if (life_spirit())
                    return true;
            if (_healthPercent < SuperbadSettings.Instance.Renewal)
                if (renewal())
                    return true;
            if (_healthPercent < SuperbadSettings.Instance.CenarionWard)
                if (cenarion_ward())
                    return true;
            if (_healthPercent < SuperbadSettings.Instance.Barkskin)
                if (Barkskin())
                    return true;
            if (_healthPercent < SuperbadSettings.Instance.Survival)
                if (Survival_Instincts())
                    return true;
            if (_healthPercent < SuperbadSettings.Instance.HealingTouchCombat)
                if (healing_touch_non_instant())
                    return true;
            if (!(_healthPercent < SuperbadSettings.Instance.RejuvenationCombat) || buff.rejuvenation.up)
                return false;
            return rejuvenation();
        }

        private static bool EscapeHandler()
        {
            if (trinket1())
                return true;
            if (trinket2())
                return true;
            if (buff.rooted.up)
                if (escapespeed())
                    return true;
            if (!buff.rooted.up) return buff.flag.up && natures_grasp();
            if (escape())
                return true;
            return buff.flag.up && natures_grasp();
        }

        private static bool escapespeed()
        {
            if (CurrentShape != ShapeshiftForm.Cat)
            {
                if (SuperbadSettings.Instance.StampedingRoarUsage == 0 ||
                    SuperbadSettings.Instance.StampedingRoarUsage == 2)
                    return stampeding_roar();
                return false;
            }
            if (SuperbadSettings.Instance.DashUsage == 0 || SuperbadSettings.Instance.DashUsage == 2)
                return dash();
            if (SuperbadSettings.Instance.StampedingRoarUsage == 0 || SuperbadSettings.Instance.StampedingRoarUsage == 2)
                return stampeding_roar();
            return false;
        }

        private static bool natures_grasp()
        {
            return UnitList.Count(u => u.IsWithinMeleeRange) > 0 && Spell.Buff(16689);
        }

        private static bool escape()
        {
            if (!SuperbadSettings.Instance.Rooted)
                return false;
            if (!Battlegrounds.IsInsideBattleground) return false;
            if (English)
            {
                if (CurrentShape == ShapeshiftForm.Cat)
                    Lua.DoString("RunMacroText(\"/Cast !Cat Form\")");
                if (CurrentShape == ShapeshiftForm.Bear)
                    Lua.DoString("RunMacroText(\"/Cast !Bear Form\")");
                Spell.LogAction("Shift because i'm rooted!", Color.Orange);
                return true;
            }
            if (!Russian) return false;
            if (CurrentShape == ShapeshiftForm.Cat)
                Lua.DoString("RunMacroText(\"/Cast !Облик кошки\")");
            if (CurrentShape == ShapeshiftForm.Bear)
                Lua.DoString("RunMacroText(\"/Cast !Облик медведя\")");
            Spell.LogAction("Shift because i'm rooted!", Color.Orange);
            return true;
        }

        private static void force_of_nature()
        {
            if (!SuperbadSettings.Instance.UseFoN)
                return;
            if (!_validtargetinrange)
                return;
            if (!buff.trinket.procagilityreact && Spell.DoubleCastEntries.ContainsKey("FoN") && target.time_to_die >= 20)
                return;
            if (Spell.CastHackFON(102703))
            {
                Spell.UpdateDoubleCastEntries("FoN", 0.5);
            }
        }

        private static bool dash()
        {
            if (!SuperbadSettings.Instance.UseDash)
                return false;
            if (buff.darkflight.up || buff.stampeding_roar.up)
                return false;
            if (CurrentShape != ShapeshiftForm.Cat)
                return false;
            if (Spell.DoubleCastEntries.ContainsKey("Speed")) return false;
            if (!Spell.BuffOFFGCD(1850)) return false;
            Spell.UpdateDoubleCastEntries("Speed", 0.5);
            return true;
        }

        private static bool stampeding_roar()
        {
            if (!SuperbadSettings.Instance.UseStampedingRoar)
                return false;
            if (buff.dash.up || buff.darkflight.up)
                return false;
            if (Spell.DoubleCastEntries.ContainsKey("Speed")) return false;
            if (CurrentShape == ShapeshiftForm.Cat)
                if (Spell.Buff(77764))
                {
                    Spell.UpdateDoubleCastEntries("Speed", 0.5);
                    return true;
                }
            if (CurrentShape != ShapeshiftForm.Bear) return false;
            if (!Spell.Buff(77761, true)) return false;
            Spell.UpdateDoubleCastEntries("Speed", 0.5);
            return true;
        }

        private static bool SpeedHandler()
        {
            if (StyxWoW.Me.CurrentTarget == null || StyxWoW.Me.CurrentTarget.IsDead)
                return false;
            if (_targetAboveGround)
                return false;
            if (Clash())
                return true;
            if (Wild_Charge())
                return true;
            if (Displacer_Beast())
                return true;
            if (StyxWoW.Me.CurrentTarget != null && (_distance > Unit.MeleeRange + 10f))
                if (darkflight())
                    return true;
            if (SuperbadSettings.Instance.DashUsage == 0 || SuperbadSettings.Instance.DashUsage == 1)
                if (StyxWoW.Me.CurrentTarget != null && (_distance > Unit.MeleeRange + 10f))
                    if (dash())
                        return true;
            if (SuperbadSettings.Instance.StampedingRoarUsage == 0 || SuperbadSettings.Instance.StampedingRoarUsage == 1)
                if (StyxWoW.Me.CurrentTarget != null && (_distance > Unit.MeleeRange + 10f))
                    return stampeding_roar();
            return false;
        }

        private static bool Clash()
        {
            if (!SuperbadSettings.Instance.SymbSpell)
                return false;
            if (_currentSpec != WoWSpec.DruidFeral)
                return false;
            if (Spell.DoubleCastEntries.ContainsKey("Speed"))
                return false;
            if (!StyxWoW.Me.GotTarget) return false;
            if (!(_distance > 8) ||
                !(_distance < 25) ||
                !StyxWoW.Me.CurrentTarget.InLineOfSpellSight || !StyxWoW.Me.CurrentTarget.InLineOfSight) return false;
            if (_targetAboveGround || _validtargetinrange) return false;
            if (!Spell.CastSymbSpell(126449, StyxWoW.Me.CurrentTarget)) return false;
            Spell.UpdateDoubleCastEntries("Speed", 0.5);
            return true;
        }

        private static bool Wild_Charge()
        {
            if (Spell.DoubleCastEntries.ContainsKey("Speed"))
                return false;
            if (buff.dash.up || buff.darkflight.up || buff.stampeding_roar.up)
                return false;
            if (!SuperbadSettings.Instance.UseWildCharge)
                return false;
            if (!TalentManager.WildCharge) return false;
            if (CurrentShape != ShapeshiftForm.Cat && CurrentShape != ShapeshiftForm.Bear)
                return false;
            if (StyxWoW.Me.CurrentTarget == null) return false;
            if (!(_distance > 8) ||
                !(_distance < 25) ||
                !StyxWoW.Me.CurrentTarget.InLineOfSpellSight || !StyxWoW.Me.CurrentTarget.InLineOfSight) return false;
            if (_targetAboveGround || _validtargetinrange) return false;
            if (!Spell.CastOFFGCD(CurrentShape == ShapeshiftForm.Cat ? 49376 : 16979)) return false;
            Spell.UpdateDoubleCastEntries("Speed", 0.5);
            return true;
        }

        private static bool Displacer_Beast()
        {
            if (Spell.DoubleCastEntries.ContainsKey("Speed"))
                return false;
            if (buff.dash.up || buff.darkflight.up || buff.stampeding_roar.up)
                return false;
            if (!SuperbadSettings.Instance.UseBlink)
                return false;
            if (!TalentManager.DisplacerBeast) return false;
            if (CurrentShape != ShapeshiftForm.Cat && CurrentShape != ShapeshiftForm.Bear) return false;
            if (StyxWoW.Me.CurrentTarget == null) return false;
            if (_targetAboveGround || _validtargetinrange) return false;
            if (_distance < 20)
                return false;
            if (!Spell.Cast(102280, true)) return false;
            Spell.UpdateDoubleCastEntries("Speed", 0.5);
            return true;
        }

        private static bool darkflight()
        {
            if (!SuperbadSettings.Instance.UseRacial)
                return false;
            if (buff.dash.up || buff.stampeding_roar.up)
                return false;
            if (StyxWoW.Me.Race != WoWRace.Worgen || Spell.DoubleCastEntries.ContainsKey("Speed")) return false;
            if (!Spell.Cast(68992, true)) return false;
            Spell.UpdateDoubleCastEntries("Speed", 0.5);
            return true;
        }

        private static void BoneShield()
        {
            if (!SuperbadSettings.Instance.SymbSpell)
                return;
            if (_currentSpec != WoWSpec.DruidGuardian)
                return;
            Spell.CastSymbSpell(122285);
        }

        private static void ElusiveBrew()
        {
            if (!SuperbadSettings.Instance.SymbSpell)
                return;
            if (_currentSpec != WoWSpec.DruidGuardian)
                return;
            if (_healthPercent <= 60)
                Spell.CastSymbSpell(126453);
        }

        private static void SpellReflection()
        {
            if (!SuperbadSettings.Instance.SymbSpell)
                return;
            if (_currentSpec != WoWSpec.DruidGuardian)
                return;
            if (!HasRageForSpellReflection)
                return;
            if (
                UnitList.Any(
                    u =>
                        u.IsCasting && u.CurrentTargetGuid == StyxWoW.Me.Guid &&
                        u.CurrentCastTimeLeft.TotalMilliseconds >= 200 &&
                        u.CurrentCastTimeLeft.TotalMilliseconds <= 2000))
                Spell.CastSymbSpell(113002);
        }

        private static void use_item()
        {
            if (!Synapse) return;
            if (BotbaseManualMode() && SuperbadSettings.Instance.UseBurst &&
                SuperbadSettings.Instance.BurstKey != SuperbadSettings.Keypress.NONE)
                if (SuperbadSettings.Instance.UseBurstGloves)
                    return;
            if (!SuperbadSettings.Instance.UseGloves)
                return;
            if (!_validtargetinrange)
                return;
            if ((Spell.GetItemCooldown(99) > 0 || !Gloves.Usable)) return;
            Spell.LogAction(Gloves.Name, Color.Yellow);
            Gloves.Use();
        }

        private static void DPSButtonPressed()
        {
            if (!SuperbadSettings.Instance.UseBurst ||
                SuperbadSettings.Instance.BurstKey == SuperbadSettings.Keypress.NONE
                || !BotbaseManualMode())
                return;
            if (SuperbadSettings.Instance.UseBurstTigers)
                Spell.BuffOFFGCD(5217);
            if (SuperbadSettings.Instance.UseBurstBerserk)
                Spell.BuffOFFGCD(_currentSpec == WoWSpec.DruidFeral ? 106951 : 50334);
            if (SuperbadSettings.Instance.UseBurstVigil)
                Spell.BuffOFFGCD(124974);
            if (SuperbadSettings.Instance.UseBurstIncar)
                Spell.Buff(_currentSpec == WoWSpec.DruidGuardian ? 102558 : 102543, true);
            if (SuperbadSettings.Instance.UseBurstFeralSpirit)
                Spell.CastSymbSpell(110807, StyxWoW.Me);
            if (SuperbadSettings.Instance.UseBurstBerserking)
                if (StyxWoW.Me.Race == WoWRace.Troll)
                    Spell.BuffOFFGCD(26297);
            if (SuperbadSettings.Instance.UseBurstHotw)
                Spell.BuffOFFGCD(108288);
            if (SuperbadSettings.Instance.UseBurstGloves)
                use_item();
            if (SuperbadSettings.Instance.UseBurstTrinket1)
                trinket1();
            if (SuperbadSettings.Instance.UseBurstTrinket2)
                trinket2();
            if (SuperbadSettings.Instance.UseBurstVirmens)
            {
                WoWItem firstOrDefault =
                    ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(item => item.Entry == 76089);
                if (firstOrDefault != null)
                    firstOrDefault.Use();
            }
            if (SuperbadSettings.Instance.UseBurstLifeBlood)
                Spell.Buff("Lifeblood", true);
            BurstMode = false;
        }

        private static bool trinket1()
        {
            if (BotbaseManualMode() && SuperbadSettings.Instance.UseBurst &&
                SuperbadSettings.Instance.BurstKey != SuperbadSettings.Keypress.NONE)
                if (SuperbadSettings.Instance.UseBurstTrinket1)
                    return false;
            if (!SuperbadSettings.Instance.UseTrinket1)
                return false;
            if (SuperbadSettings.Instance.Trinket1Usage == 0 && debuff.cc.up)
                if (Trinket.UseTrinketOne())
                    return true;
            if (SuperbadSettings.Instance.Trinket1Usage == 1 && buff.tigers_fury.up)
                if (Trinket.UseTrinketOne())
                    return true;
            if (SuperbadSettings.Instance.Trinket1Usage == 2 &&
                StyxWoW.Me.HealthPercent < SuperbadSettings.Instance.Trinket1Percent)
                if (Trinket.UseTrinketOne())
                    return true;
            return SuperbadSettings.Instance.Trinket1Usage == 3 && Trinket.UseTrinketOne();
        }

        private static bool trinket2()
        {
            if (BotbaseManualMode() && SuperbadSettings.Instance.UseBurst &&
                SuperbadSettings.Instance.BurstKey != SuperbadSettings.Keypress.NONE)
                if (SuperbadSettings.Instance.UseBurstTrinket2)
                    return false;
            if (!SuperbadSettings.Instance.UseTrinket2)
                return false;
            if (SuperbadSettings.Instance.Trinket2Usage == 0 && debuff.cc.up)
                if (Trinket.UseTrinketTwo())
                    return true;
            if (SuperbadSettings.Instance.Trinket2Usage == 1 && buff.tigers_fury.up)
                if (Trinket.UseTrinketTwo())
                    return true;
            if (SuperbadSettings.Instance.Trinket2Usage == 2 &&
                StyxWoW.Me.HealthPercent < SuperbadSettings.Instance.Trinket2Percent)
                if (Trinket.UseTrinketTwo())
                    return true;
            return SuperbadSettings.Instance.Trinket2Usage == 3 && Trinket.UseTrinketTwo();
        }

        private static void DoCHealBear()
        {
            if (talent.dream_of_cenarius.enabled && StyxWoW.Me.HasBuff(145162))
            {
                if (!StyxWoW.Me.GroupInfo.IsInParty || !SuperbadSettings.Instance.HealOthers)
                    if (StyxWoW.Me.HealthPercent < 80)
                    {
                        Lua.DoString("RunMacroText(\"/console autounshift 0\")");
                        Spell.Heal(5185);
                        Lua.DoString("RunMacroText(\"/console autounshift 1\")");
                        return;
                    }
                WoWPlayer healTarget = GetHealTarget();
                if (healTarget != null)
                {
                    Lua.DoString("RunMacroText(\"/console autounshift 0\")");
                    Spell.Heal(5185,healTarget);
                    Lua.DoString("RunMacroText(\"/console autounshift 1\")");
                    return;
                }

                if (StyxWoW.Me.MyBuffRemains(145162) < 3 &&
                    StyxWoW.Me.HealthPercent < 95)
                {
                    Lua.DoString("RunMacroText(\"/console autounshift 0\")");
                    Spell.Heal(5185);
                    Lua.DoString("RunMacroText(\"/console autounshift 1\")");
                }
            }
        }
    }
}