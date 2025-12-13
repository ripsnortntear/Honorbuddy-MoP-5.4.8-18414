using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;

namespace RichieDiscPriestPvP
{
    class Manacosts {

        public static readonly Manacosts Instance = new Manacosts();

        private Manacosts() {
            init();
        }

        public void init() {

            if (SpellManager.HasSpell("Holy Fire")) {
                HolyFire = SpellManager.Spells["Holy Fire"].PowerCost;
            }
            
            if (SpellManager.HasSpell("Heal")) {
                Heal = SpellManager.Spells["Heal"].PowerCost;
            }
            
            if (SpellManager.HasSpell("Greater Heal")) {
                GreaterHeal = SpellManager.Spells["Greater Heal"].PowerCost;
            }
            
            if (SpellManager.HasSpell("Power Word: Barrier")) {
                Barrier = SpellManager.Spells["Power Word: Barrier"].PowerCost;
            }
            
            if (SpellManager.HasSpell("Penance")) {
                Penance = SpellManager.Spells["Penance"].PowerCost;
            } 
            
            if (SpellManager.HasSpell("Pain Suppression")) {
                PainSuppression = SpellManager.Spells["Pain Suppression"].PowerCost;
            }
            
            if (SpellManager.HasSpell("Purify")) {
                Purify = SpellManager.Spells["Purify"].PowerCost;
            }

            if (SpellManager.HasSpell("Smite")) {
                Smite = SpellManager.Spells["Smite"].PowerCost;
            }
            
            if (SpellManager.HasSpell("Renew")) {
                Renew = SpellManager.Spells["Renew"].PowerCost;
            }

            if (SpellManager.HasSpell("Prayer of Mending")) {
                PoM = SpellManager.Spells["Prayer of Mending"].PowerCost;
            }

            if (SpellManager.HasSpell("Power Word: Shield")) {
                PWS = SpellManager.Spells["Power Word: Shield"].PowerCost;
            }

            if (SpellManager.HasSpell("Flash Heal")) {
                FlashHeal = SpellManager.Spells["Flash Heal"].PowerCost;
            }

            if (SpellManager.HasSpell("Binding Heal")) {
                BindingHeal = SpellManager.Spells["Binding Heal"].PowerCost;
            }

            if (SpellManager.HasSpell("Fade")) {
                Fade = SpellManager.Spells["Fade"].PowerCost;
            }

            if (SpellManager.HasSpell("Dispel Magic")) {
                Dispel = SpellManager.Spells["Dispel Magic"].PowerCost;
            }

            if (SpellManager.HasSpell("Shadow Word: Death")) {
                SWD = SpellManager.Spells["Shadow Word: Death"].PowerCost;
            }

            if (SpellManager.HasSpell("Halo")) {
                Halo = SpellManager.Spells["Halo"].PowerCost;
            }

            if (SpellManager.HasSpell("Fear Ward")) {
                FearWard = SpellManager.Spells["Fear Ward"].PowerCost;
            }

            if (SpellManager.HasSpell("Power Word: Fortitude")) {
                PWF = SpellManager.Spells["Power Word: Fortitude"].PowerCost;
            }

            if (SpellManager.HasSpell("Shackle Undead")) {
                Shackle = SpellManager.Spells["Shackle Undead"].PowerCost;
            }

            if (SpellManager.HasSpell("Psyfiend")) {
                Psyfiend = SpellManager.Spells["Psyfiend"].PowerCost;
            }

            if (SpellManager.HasSpell("Mass Dispel")) {
                MassDispel = SpellManager.Spells["Mass Dispel"].PowerCost;
            }

            if (SpellManager.HasSpell("Psychic Scream")) {
                PsychicScream = SpellManager.Spells["Psychic Scream"].PowerCost;
            }

            if (SpellManager.HasSpell("Leap of Faith")) {
                LeapOfFaith = SpellManager.Spells["Leap of Faith"].PowerCost;
            }
            
            if (SpellManager.HasSpell("Shadow Word: Pain")) {
                SWP = SpellManager.Spells["Shadow Word: Pain"].PowerCost;
            }

            if (SpellManager.HasSpell("Void Tendrils")) {
                VoidTendrils = SpellManager.Spells["Void Tendrils"].PowerCost;
            }

            if (SpellManager.HasSpell("Cascade")) {
                Cascade = SpellManager.Spells["Cascade"].PowerCost;
            }

            if (SpellManager.HasSpell("Divine Star")) {
                DivineStar = SpellManager.Spells["Divine Star"].PowerCost;
            }

            if (SpellManager.HasSpell("Prayer of Healing")) {
                PrayerOfHealing = SpellManager.Spells["Prayer of Healing"].PowerCost;
            }
        }

        public int HolyFire { get; set; }
        public int Heal { get; set; }
        public int GreaterHeal { get; set; }
        public int Barrier { get; set; }
        public int Penance { get; set; }
        public int PainSuppression { get; set; }
        public int Purify { get; set; }
        public int Smite { get; set; }
        public int Renew { get; set; }
        public int PoM { get; set; }
        public int PWS { get; set; }
        public int FlashHeal { get; set; }
        public int BindingHeal { get; set; }
        public int Fade { get; set; }
        public int Dispel { get; set; }
        public int SWD { get; set; }       
        public int Halo { get; set; }
        public int FearWard { get; set; }
        public int PWF { get; set; }
        public int Shackle { get; set; }
        public int Psyfiend { get; set; }
        public int MassDispel { get; set; }
        public int PsychicScream { get; set; }
        public int LeapOfFaith { get; set; }
        public int SWP { get; set; }
        public int VoidTendrils { get; set; }
        public int Cascade { get; set; }
        public int DivineStar { get; set; }
        public int PrayerOfHealing { get; set; }
    }
}
