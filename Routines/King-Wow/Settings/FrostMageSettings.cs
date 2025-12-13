using System.IO;
using Styx;
using Styx.Helpers;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.Common;
using Styx.Common.Helpers;

namespace KingWoW
{
    public class FrostMageSettings : Settings
    {
        public static FrostMageSettings Instance = new FrostMageSettings();

        public string path_name = Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/FrostMage/FrostMage-Settings-{0}.xml", StyxWoW.Me.Class.ToString()));

        public FrostMageSettings()
            : base(Path.Combine(Styx.Common.Utilities.AssemblyDirectory, string.Format(@"KingWOWCurrentSettings/FrostMage/FrostMage-Settings-{0}.xml", StyxWoW.Me.Class.ToString())))
        {
        }

        public enum TargetType
        {
            MANUAL,
            SEMIAUTO,
            AUTO
        }

        [Setting]
        [DefaultValue(50)]
        [Category("Settings EVOCATION")]
        [DisplayName("EvocationHP")]
        [Description("Cast evocation if not rune and not evocation talent selected")]
        public int EvocationHP { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings EVOCATION")]
        [DisplayName("UseEvocation")]
        [Description("enable/disable use of evocation if not rune and not evocation talent selected")]
        public bool UseEvocation { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings EVOCATION")]
        [DisplayName("UseEvocationInCombat")]
        [Description("enable/disable use of evocation in combat if not rune and not evocation talent selected")]
        public bool UseEvocationInCombat { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings DECURSE")]
        [DisplayName("UseDecurse")]
        [Description("Autocast remove curse on friends")]
        public bool UseDecurse { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings INTERRUPT")]
        [DisplayName("autointerrupt")]
        [Description("Cast counterspell if target casting and interrumpible")]
        public bool AutoInterrupt { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings TARGETTING")]
        [DisplayName("AutoFacingTarget")]
        [Description("Enable/Disable autofacing current target")]
        public bool AutofaceTarget { get; set; }

        [Setting]
        [DefaultValue(TargetType.AUTO)]
        [Category("Settings TARGETTING")]
        [DisplayName("TargetTypeSelected")]
        [Description("AUTO/MANUAL enable/disable autotargeting SEMIAUTO: same logic of AUTO but no switch selected target")]
        public TargetType TargetTypeSelected { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Settings MULTIDOT")]
        [DisplayName("MultidotEnemyNumberMin")]
        [Description("Enable Multidot rotation if Enemy in range >= this value")]
        public int MultidotEnemyNumberMin { get; set; }

        [Setting]
        [DefaultValue(5)]
        [Category("Settings MULTIDOT")]
        [DisplayName("MultidotEnemyNumberMax")]
        [Description("Disable Multidot rotation if dotted Enemy > this value")]
        public int MultidotEnemyNumberMax { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings MULTIDOT")]
        [DisplayName("Enable Multidot Rotation")]
        [Description("Enable/Disable use of Multidot rotation")]
        public bool MultidotEnabled { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings MULTIDOT")]
        [DisplayName("Avoid Crowd Controlled in Multidot Rotation")]
        [Description("Avoid Crowd Controlled in Multidot Rotation")]
        public bool MultidotAvoidCC { get; set; }

        [Setting]
        [DefaultValue(28)]
        [Category("Settings PULL")]
        [DisplayName("PullDistance")]
        [Description("Distance to pull from")]
        public float PullDistance { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings PET")]
        [DisplayName("Use Pet")]
        [Description("enable/disable Pet")]
        public bool UsePet { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings PET")]
        [DisplayName("UsePetFreeze")]
        [Description("enable/disable Use of Pet Freeze spell")]
        public bool UsePetFreeze { get; set; }
        

        public enum ArmorType
        {
            FROST,
            MOLTEN,
            MAGE
        }

        [Setting]
        [DefaultValue(ArmorType.FROST)]
        [Category("Settings BUFF")]
        [DisplayName("Use Armor")]
        [Description("chose armor to use")]
        public ArmorType ArmorToUse { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings BUFF")]
        [DisplayName("UseIcebarrier")]
        [Description("chose if automatically cast Ice barrier on cooldown")]
        public bool UseIcebarrier { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings BUFF")]
        [DisplayName("Autobuff brillance")]
        [Description("chose if buff arcane brillance automatic or manual")]
        public bool AutoBuffBrillance { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings BUFF")]
        [DisplayName("IceWardOnTank")]
        [Description("chose if automaticallycast Ice ward on Tank")]
        public bool IceWardOnTank { get; set; }        

        [Setting]
        [DefaultValue(20)]
        [Category("Settings REST")]
        [DisplayName("Drink mana%")]
        [Description("Drink at % mana")]
        public int ManaPercent { get; set; }

        [Setting]
        [DefaultValue(0)]
        [Category("Settings REST")]
        [DisplayName("Eat hp%")]
        [Description("Eat at % hp")]
        public int HealthPercent { get; set; }

        [Setting]
        [DefaultValue(4)]
        [Category("Settings AOE")]
        [DisplayName("AOE on #mobs")]
        [Description("use AOE spells if mobs number >= this value")]
        public int AOECount { get; set; }

        public enum CDUseType
        {
            COOLDOWN,
            BOSS,
            MANUAL
        }

        [Setting]
        [DefaultValue(CDUseType.COOLDOWN)]
        [Category("Settings CD")]
        [DisplayName("Use Icy Veins on")]
        [Description("Chose when use Icy Veins CD")]
        public CDUseType CDUseIcyVeins { get; set; }

        [Setting]
        [DefaultValue(CDUseType.COOLDOWN)]
        [Category("Settings CD")]
        [DisplayName("Use Mirror Image on")]
        [Description("Chose when use your Mirror Image CD")]
        public CDUseType CDUseMirrorImage { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings CD")]
        [DisplayName("AutoCastFrozerOrb")]
        [Description("Chose if AutoCast FrozerOrb (you can disable it questing")]
        public bool AutoFrozerOrb { get; set; }
        

        [Setting]
        [DefaultValue(CDUseType.COOLDOWN)]
        [Category("Settings CD")]
        [DisplayName("Use AlterTime on")]
        [Description("Chose when use your AlterTime CD")]
        public CDUseType CDUseAlterTime { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings CC")]
        [DisplayName("Use Deep Freeze")]
        [Description("Chose to enable/disable Deep Freeze")]
        public bool UseDeepFreeze { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings CC")]
        [DisplayName("UseRingOfFrost")]
        [Description("Chose to enable/disable use of ring of frost")]
        public bool UseRingOfFrost { get; set; }
        

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("Use Blink")]
        [Description("Chose to enable/disable automatic Blink")]
        public bool UseBlink { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("Use FrostNova")]
        [Description("Chose to enable/disable automatic FrostNova")]
        public bool UseFrostNova { get; set; }
        

        [Setting]
        [DefaultValue(15)]
        [Category("Settings SPECIAL")]
        [DisplayName("IceBlockHP")]
        [Description("Use Ice Block if hp lower than this value")]
        public int IceBlockHP { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("Use Ice Block")]
        [Description("Chose to enable/disable automatic IceBlock")]
        public bool IceBlockUse { get; set; }
        

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("Use RuneOfPower")]
        [Description("Chose to enable/disable if talented RuneOfPower")]
        public bool UseRuneOfPower { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("EvocationBuffAutomatic")]
        [Description("Chose to enable/disable if talented automatic invocation buff")]
        public bool EvocationBuffAuto { get; set; }
        

        [Setting]
        [DefaultValue(true)]
        [Category("Settings SPECIAL")]
        [DisplayName("UseIncenterWardOnCD")]
        [Description("Chose to enable/disable if talented Incanter Ward on CD")]
        public bool UseIncenterWardOnCD { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings PVP")]
        [DisplayName("UseSpellSteal")]
        [Description("UseSpellSteal on focused target if exist or Current target")]
        public bool UseSpellSteal { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Settings PVP")]
        [DisplayName("UsePolymorf")]
        [Description("UsePolymorf on focused target")]
        public bool UsePolymorf { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings AOE")]
        [DisplayName("UseFlameStrike")]
        [Description("enable/disable use of UseFlameStrike")]
        public bool UseFlameStrike { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Settings AOE")]
        [DisplayName("UseArcaneExplosion")]
        [Description("enable/disable use of ArcaneExplosion")]
        public bool UseArcaneExplosion { get; set; }
        
        
    }
}
