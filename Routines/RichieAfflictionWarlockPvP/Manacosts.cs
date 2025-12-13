using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;

namespace RichieAfflictionWarlock
{
    class Manacosts {

        public static readonly Manacosts Instance = new Manacosts();
        private static SpellFindResults spell;

        private Manacosts() {
            init();
        }

        public void init() {

            //spells
            if (SpellManager.HasSpell(SpellIds.Instance.SummonVoidwalker)) {
                SpellManager.FindSpell(SpellIds.Instance.SummonVoidwalker, out spell);
                if (spell.Override == null) {
                    SummonVoidwalker = spell.Original.PowerCost;
                } else {
                    SummonVoidwalker = spell.Override.PowerCost;
                }                
            }
            if (SpellManager.HasSpell(SpellIds.Instance.SummonSuccubus)) {
                SpellManager.FindSpell(SpellIds.Instance.SummonSuccubus, out spell);
                if (spell.Override == null) {
                    SummonSuccubus = spell.Original.PowerCost;
                } else {
                    SummonSuccubus = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.SummonFelhunter)) {
                SpellManager.FindSpell(SpellIds.Instance.SummonFelhunter, out spell);
                if (spell.Override == null) {
                    SummonFelhunter = spell.Original.PowerCost;
                } else {
                    SummonFelhunter = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.SummonImp)) {
                SpellManager.FindSpell(SpellIds.Instance.SummonImp, out spell);
                if (spell.Override == null) {
                    SummonImp = spell.Original.PowerCost;
                } else {
                    SummonImp = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.UnstableAffliction)) {
                SpellManager.FindSpell(SpellIds.Instance.UnstableAffliction, out spell);
                if (spell.Override == null) {
                    UnstableAffliction = spell.Original.PowerCost;
                } else {
                    UnstableAffliction = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.UnendingResolve)) {
                SpellManager.FindSpell(SpellIds.Instance.UnendingResolve, out spell);
                if (spell.Override == null) {
                    UnendingResolve = spell.Original.PowerCost;
                } else {
                    UnendingResolve = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.UnendingBreath)) {
                SpellManager.FindSpell(SpellIds.Instance.UnendingBreath, out spell);
                if (spell.Override == null) {
                    UnendingBreath = spell.Original.PowerCost;
                } else {
                    UnendingBreath = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.TwilightWard)) {
                SpellManager.FindSpell(SpellIds.Instance.TwilightWard, out spell);
                if (spell.Override == null) {
                    TwilightWard = spell.Original.PowerCost;
                } else {
                    TwilightWard = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.SummonDoomguard)) {
                SpellManager.FindSpell(SpellIds.Instance.SummonDoomguard, out spell);
                if (spell.Override == null) {
                    SummonDoomguard = spell.Original.PowerCost;
                } else {
                    SummonDoomguard = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.SummonInfernal)) {
                SpellManager.FindSpell(SpellIds.Instance.SummonInfernal, out spell);
                if (spell.Override == null) {
                    SummonInfernal = spell.Original.PowerCost;
                } else {
                    SummonInfernal = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.Soulstone)) {
                SpellManager.FindSpell(SpellIds.Instance.Soulstone, out spell);
                if (spell.Override == null) {
                    Soulstone = spell.Original.PowerCost;
                } else {
                    Soulstone = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.SoulSwap)) {
                SpellManager.FindSpell(SpellIds.Instance.SoulSwap, out spell);
                if (spell.Override == null) {
                    SoulSwap = spell.Original.PowerCost;
                } else {
                    SoulSwap = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.SeedOfCorruption)) {
                SpellManager.FindSpell(SpellIds.Instance.SeedOfCorruption, out spell);
                if (spell.Override == null) {
                    SeedOfCorruption = spell.Original.PowerCost;
                } else {
                    SeedOfCorruption = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.MaleficGrasp)) {
                SpellManager.FindSpell(SpellIds.Instance.MaleficGrasp, out spell);
                if (spell.Override == null) {
                    MaleficGrasp = spell.Original.PowerCost;
                } else {
                    MaleficGrasp = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.FelFlame)) {
                SpellManager.FindSpell(SpellIds.Instance.FelFlame, out spell);
                if (spell.Override == null) {
                    FelFlame = spell.Original.PowerCost;
                } else {
                    FelFlame = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.Fear)) {
                SpellManager.FindSpell(SpellIds.Instance.Fear, out spell);
                if (spell.Override == null) {
                    Fear = spell.Original.PowerCost;
                } else {
                    Fear = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.DrainSoul)) {
                SpellManager.FindSpell(SpellIds.Instance.DrainSoul, out spell);
                if (spell.Override == null) {
                    DrainSoul = spell.Original.PowerCost;
                } else {
                    DrainSoul = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.DemonicCircleTeleport)) {
                SpellManager.FindSpell(SpellIds.Instance.DemonicCircleTeleport, out spell);
                if (spell.Override == null) {
                    DemonicCircleTeleport = spell.Original.PowerCost;
                } else {
                    DemonicCircleTeleport = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.DemonicCircleSummon)) {
                SpellManager.FindSpell(SpellIds.Instance.DemonicCircleSummon, out spell);
                if (spell.Override == null) {
                    DemonicCircleSummon = spell.Original.PowerCost;
                } else {
                    DemonicCircleSummon = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.DarkSoul)) {
                SpellManager.FindSpell(SpellIds.Instance.DarkSoul, out spell);
                if (spell.Override == null) {
                    DarkSoul = spell.Original.PowerCost;
                } else {
                    DarkSoul = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.DarkIntent)) {
                SpellManager.FindSpell(SpellIds.Instance.DarkIntent, out spell);
                if (spell.Override == null) {
                    DarkIntent = spell.Original.PowerCost;
                } else {
                    DarkIntent = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.CurseOftheElements)) {
                SpellManager.FindSpell(SpellIds.Instance.CurseOftheElements, out spell);
                if (spell.Override == null) {
                    CurseOftheElements = spell.Original.PowerCost;
                } else {
                    CurseOftheElements = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.CurseOfExhaustion)) {
                SpellManager.FindSpell(SpellIds.Instance.CurseOfExhaustion, out spell);
                if (spell.Override == null) {
                    CurseOfExhaustion = spell.Original.PowerCost;
                } else {
                    CurseOfExhaustion = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.CurseOfEnfeeblement)) {
                SpellManager.FindSpell(SpellIds.Instance.CurseOfEnfeeblement, out spell);
                if (spell.Override == null) {
                    CurseOfEnfeeblement = spell.Original.PowerCost;
                } else {
                    CurseOfEnfeeblement = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.CreateSoulwell)) {
                SpellManager.FindSpell(SpellIds.Instance.CreateSoulwell, out spell);
                if (spell.Override == null) {
                    CreateSoulwell = spell.Original.PowerCost;
                } else {
                    CreateSoulwell = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.CreateHealthstone)) {
                SpellManager.FindSpell(SpellIds.Instance.CreateHealthstone, out spell);
                if (spell.Override == null) {
                    CreateHealthstone = spell.Original.PowerCost;
                } else {
                    CreateHealthstone = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.Corruption)) {
                SpellManager.FindSpell(SpellIds.Instance.Corruption, out spell);
                if (spell.Override == null) {
                    Corruption = spell.Original.PowerCost;
                } else {
                    Corruption = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.Banish)) {
                SpellManager.FindSpell(SpellIds.Instance.Banish, out spell);
                if (spell.Override == null) {
                    Banish = spell.Original.PowerCost;
                } else {
                    Banish = spell.Override.PowerCost;
                }
            }
            if (SpellManager.HasSpell(SpellIds.Instance.Agony)) {
                SpellManager.FindSpell(SpellIds.Instance.Agony, out spell);
                if (spell.Override == null) {
                    Agony = spell.Original.PowerCost;
                } else {
                    Agony = spell.Override.PowerCost;
                }                
            }            
        }


        //spells
        public int SummonVoidwalker { get; set; }
        public int SummonSuccubus { get; set; }
        public int SummonFelhunter { get; set; }
        public int SummonImp { get; set; }
        public int UnstableAffliction { get; set; }
        public int UnendingResolve { get; set; }
        public int UnendingBreath { get; set; }
        public int TwilightWard { get; set; }
        public int SummonDoomguard { get; set; }
        public int SummonInfernal { get; set; }
        public int Soulstone { get; set; }
        public int SoulSwap { get; set; }
        public int SeedOfCorruption { get; set; }
        public int MaleficGrasp { get; set; }
        public int FelFlame { get; set; }
        public int Fear { get; set; }
        public int DrainSoul { get; set; }
        public int DemonicCircleTeleport { get; set; }
        public int DemonicCircleSummon { get; set; }
        public int DarkSoul { get; set; }
        public int DarkIntent { get; set; }
        public int CurseOftheElements { get; set; }
        public int CurseOfExhaustion { get; set; }
        public int CurseOfEnfeeblement { get; set; }
        public int CreateSoulwell { get; set; }
        public int CreateHealthstone { get; set; }
        public int Corruption { get; set; }
        public int Banish { get; set; }
        public int Agony { get; set; }


    }
}
