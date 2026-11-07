#region

using System.IO;
using Styx;
using Styx.Common;
using Styx.Helpers;

#endregion

namespace Superbad
{
    public class SuperbadSettings : Settings
    {
        public enum Keypress
        {
            NONE,
            LSHIFT,
            RSHIFT,
            LCTRL,
            RCTRL,
            LALT,
            RALT
        };

        public enum Shapeshift
        {
            CAT,
            BEAR,
            AUTO,
            MANUAL
        };

        public static readonly SuperbadSettings Instance = new SuperbadSettings();

        public SuperbadSettings()
            : base(
                Path.Combine(Utilities.AssemblyDirectory,
                    string.Format(@"Routines/Config/Superbad-Settings-{0}.xml", StyxWoW.Me.Name))
                )
        {
        }

        [Setting,
         Styx.Helpers.DefaultValue(Shapeshift.AUTO)]
        public Shapeshift Form { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool ShiftOnMobCount { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(3)]
        public int AddsBearSwitch { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool ShiftOnLowHealth { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(35)]
        public int HealthBearSwitch { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseMovement { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseTargeting { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseFacing { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool Suspend { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseTrinket1 { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseTrinket2 { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(0)]
        public int Trinket1Usage { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(30)]
        public int Trinket1Percent { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(0)]
        public int Trinket2Usage { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(30)]
        public int Trinket2Percent { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseGloves { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseLifeBlood { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(30)]
        public int LifebloodPercent { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseRest { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(35)]
        public int RestHealth { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(35)]
        public int RestMana { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseRacial { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool SymbTarget { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool SymbSpell { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseRebirth { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(0)]
        public int RebirthMode { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keypress.NONE)]
        public Keypress PauseKey { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(Keypress.NONE)]
        public Keypress BurstKey { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(Keypress.NONE)]
        public Keypress AoeKey { get; set; }

        //TODO: Unused so far!
        [Setting,
         Styx.Helpers.DefaultValue(Keypress.NONE)]
        public Keypress GrowlKey { get; set; }


        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool PrintMsg { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurst { get; set; }


        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseAoeKey { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool InterruptAnyone { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool InterruptRandomize { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(95)]
        public int InterruptFailPercentage { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseSkullBash { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurstTigers { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurstBerserk { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurstVigil { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurstIncar { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurstFeralSpirit { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurstBerserking { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurstHotw { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurstGloves { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurstTrinket1 { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurstTrinket2 { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurstVirmens { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBurstLifeBlood { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool Mdw { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool Mdwgroup { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseDash { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(0)]
        public int DashUsage { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseStampedingRoar { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(0)]
        public int StampedingRoarUsage { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseAquatic { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseTravel { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool Rooted { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(70)]
        public int Barkskin { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(35)]
        public int HealthStone { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(70)]
        public int LifeSpirit { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(25)]
        public int Survival { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(0)]
        public int HealingTouchCombat { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(0)]
        public int RejuvenationCombat { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(25)]
        public int OoCHealingTouch { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(70)]
        public int OoCReju { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseHotW { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool WrathSpam { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UseBlink { get; set; }


        [Setting,
         Styx.Helpers.DefaultValue(70)]
        public int Renewal { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseInca { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool HealOthers { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseWildCharge { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(95)]
        public int CenarionWard { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseFoN { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool MightyBash { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseVigil { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(80)]
        public int Predatory { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(3)]
        public int CatAoe { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool RakeCycle { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseBerserk { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseFff { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseThrash { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(0)]
        public int ThrashUsage { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool PullStealth { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(0)]
        public int StealthOpener { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool UsePotion { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool StayInStealth { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(false)]
        public bool SavageFarm { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(3)]
        public int BearAoe { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool LacerateCycle { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(80)]
        public int Frenzied { get; set; }


        [Setting,
         Styx.Helpers.DefaultValue(20)]
        public int MightofUrsoc { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseBerserkBear { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseFffBear { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseThrashBear { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseTaunt { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseTauntBosses { get; set; }
        
        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseBearHug { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseSavageDefense { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool UseFrenziedRegen { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool Update { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool Changelog { get; set; }

        [Setting,
         Styx.Helpers.DefaultValue(true)]
        public bool WaitSickness { get; set; }

        /* WING 1*/

        [Setting,
        Styx.Helpers.DefaultValue(1)]
        public int BossImmerseus { get; set; }

        [Setting,
        Styx.Helpers.DefaultValue(3)]
        public int BossAmalgam { get; set; }

        [Setting,
        Styx.Helpers.DefaultValue(1)]
        public int BossSha { get; set; }

        /* WING 2 */

        [Setting,
        Styx.Helpers.DefaultValue(3)]
        public int BossGalakras { get; set; }

        [Setting,
        Styx.Helpers.DefaultValue(2)]
        public int BossIronJuggernaut { get; set; }

        [Setting,
        Styx.Helpers.DefaultValue(5)]
        public int BossEarthBreakerHaromm { get; set; }

        [Setting,
        Styx.Helpers.DefaultValue(3)]
        public int BossNazgrim { get; set; }

        /* WING 3 */

        [Setting,
        Styx.Helpers.DefaultValue(12)]
        public int BossMalkorok { get; set; }

        [Setting,
        Styx.Helpers.DefaultValue(3)]
        public int BossThok { get; set; }

        /* WING 4 */

        [Setting,
        Styx.Helpers.DefaultValue(4)]
        public int BossBlackfuse { get; set; }

        [Setting,
        Styx.Helpers.DefaultValue(3)]
        public int BossGarrosh { get; set; }


        public static void printSettings()
        {
            Logging.Write(LogLevel.Diagnostic, "---------------");
            Logging.Write(LogLevel.Diagnostic, "Print Settings");
            Logging.Write(LogLevel.Diagnostic, "---------------");
            foreach (var variable in Instance.GetSettings())
            {
                Logging.Write(LogLevel.Diagnostic, variable.Key + " " + variable.Value);
            }
            Logging.Write(LogLevel.Diagnostic, "---------------");
            Logging.Write(LogLevel.Diagnostic, "Settings done");
            Logging.Write(LogLevel.Diagnostic, "---------------");
        }
    }
}