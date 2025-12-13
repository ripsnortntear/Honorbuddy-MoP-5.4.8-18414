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
    class SpellIds {

        public static readonly SpellIds Instance = new SpellIds();

        private SpellIds() {
            init();
        }

        public void init() {

            Lifeblood = 55500;
            SummonVoidwalker = 697;
            SummonSuccubus = 712;
            SummonFelhunter = 691;
            SummonImp = 688;
            UnstableAffliction = 30108;
            UnendingResolve = 104773;
            UnendingBreath = 5697;
            TwilightWard = 6229;
            SummonDoomguard = 18540;
            SummonInfernal = 1122;
            Soulstone = 20707;
            Soulburn = 74434;
            SoulSwap = 86121;
            SeedOfCorruption = 27243;
            MaleficGrasp = 103103;
            LifeTap = 1454;
            HealthFunnel = 755;
            Haunt = 48181;
            DrainLife = 689;
            FelFlame = 77799;
            Fear = 5782;
            DrainSoul = 1120;
            DemonicCircleTeleport = 48020;
            DemonicCircleSummon = 48018;
            DarkSoul = 77801;
            DarkIntent = 109773;
            CurseOftheElements = 1490;
            CurseOfExhaustion = 18223;
            CurseOfEnfeeblement = 109466;
            CreateSoulwell = 29893;
            CreateHealthstone = 6201;
            Corruption = 172;
            CommandDemon = 119898;
            Banish = 710;
            Agony = 980;
            WillOfTheForsaken = 7744;
            Berserking = 26297;

            //talents
            HowlOfTerror = 5484;
            MortalCoil = 6789;
            Shadowfury = 30283;
            DarkRegeneration = 108359;
            SoulLeech = 108370;
            HarvestLife = 108371;
            SoulLink = 108415;
            SacrificialPact = 108416;
            UnboundWill = 108482;
            GrimoireOfService = 108501;
            GrimoireOfSacrifice = 108503;
            ArchimondesVengeance = 108505;
            DarkBargain = 110913;
            BloodHorror = 111397;
            BurningRush = 111400;

            //pets
            PetId_Imp = 23;
            PetId_Felguard = 29;
            PetId_Voidwalker = 16;
            PetId_Felhunter = 15;
            PetId_Succubus = 17;
            PetId_FelImp = 100;
            PetId_Wrathguard = 104;
            PetId_Voidlord = 101;
            PetId_Observer = 103;
            PetId_Shivarra = 102;
        }


        //spells
        public int Lifeblood { get; set; }
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
        public int Soulburn { get; set; }
        public int SoulSwap { get; set; }
        public int SeedOfCorruption { get; set; }
        public int MaleficGrasp { get; set; }
        public int LifeTap { get; set; }
        public int HealthFunnel { get; set; }
        public int Haunt { get; set; }
        public int DrainLife { get; set; }
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
        public int CommandDemon { get; set; }
        public int Banish { get; set; }
        public int Agony { get; set; }
        public int WillOfTheForsaken { get; set; }
        public int Berserking { get; set; }

        //talents
        public int HowlOfTerror { get; set; }
        public int MortalCoil { get; set; }
        public int Shadowfury { get; set; }
        public int DarkRegeneration { get; set; }
        public int SoulLeech { get; set; }
        public int HarvestLife { get; set; }
        public int SoulLink { get; set; }
        public int SacrificialPact { get; set; }
        public int UnboundWill { get; set; }
        public int GrimoireOfService { get; set; }
        public int GrimoireOfSacrifice { get; set; }
        public int ArchimondesVengeance { get; set; }
        public int DarkBargain { get; set; }
        public int BloodHorror { get; set; }
        public int BurningRush { get; set; }


        //pets
        public int PetId_Imp { get; set; }
        public int PetId_Felguard { get; set; }
        public int PetId_Voidwalker { get; set; }
        public int PetId_Felhunter { get; set; }
        public int PetId_Succubus { get; set; }
        public int PetId_FelImp { get; set; }
        public int PetId_Wrathguard { get; set; }
        public int PetId_Voidlord { get; set; }
        public int PetId_Observer { get; set; }
        public int PetId_Shivarra { get; set; }

    }
}


       /* //spells
        public static int Lifeblood = 55500;
        public static int SummonVoidwalker = 697;
        public static int SummonSuccubus = 712;
        public static int SummonFelhunter = 691;
        public static int SummonImp = 688;
        public static int FelArmor = 104938;
        public static int ControlDemon = 93375;
        public static int UnstableAffliction = 30108;
        public static int UnendingResolve = 104773;
        public static int UnendingBreath = 5697;
        public static int TwilightWard = 6229;
        public static int SummonDoomguard = 18540;
        public static int SummonInfernal = 1122;
        public static int Soulstone = 20707;
        public static int Soulburn = 74434;
        public static int SoulSwap = 86121;
        public static int SeedOfCorruption = 27243;
        public static int MaleficGrasp = 103103;
        public static int LifeTap = 1454;
        public static int HealthFunnel = 755;
        public static int Haunt = 48181;
        public static int DrainLife = 689;
        public static int FelFlame = 77799;
        public static int Fear = 5782;
        public static int DrainSoul = 1120;
        public static int DemonicCircleTeleport = 48020;
        public static int DemonicCircleSummon = 48018;
        public static int DarkSoul = 77801;
        public static int DarkIntent = 109773;
        public static int CurseOftheElements = 1490;
        public static int CurseOfExhaustion = 18223;
        public static int CurseOfEnfeeblement = 109466;
        public static int CreateSoulwell = 29893;
        public static int CreateHealthstone = 6201;
        public static int Corruption = 172;
        public static int CommandDemon = 119898;
        public static int Banish = 710;
        public static int Agony = 980;
        public static int WillOfTheForsaken = 7744;

        //talents
        public static int HowlOfTerror = 5484;
        public static int MortalCoil = 6789;
        public static int Shadowfury = 30283;
        public static int DarkRegeneration = 108359;
        public static int SoulLeech = 108370;
        public static int HarvestLife = 108371;
        public static int SoulLink = 108415;
        public static int SacrificialPact = 108416;
        public static int UnboundWill = 108482;
        public static int GrimoireOfService = 108501;
        public static int GrimoireOfSacrifice = 108503;
        public static int ArchimondesVengeance = 108505;
        public static int DarkBargain = 110913;
        public static int BloodHorror = 111397;
        public static int BurningRush = 111400;*/

