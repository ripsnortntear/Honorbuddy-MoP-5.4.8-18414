using System.IO;
using Styx;
using Styx.Helpers;
using Styx.Common;

namespace DWCC
{
    public class DunatanksSettings : Settings
    {
        public static readonly DunatanksSettings Instance = new DunatanksSettings();

        public DunatanksSettings()
            : base(Path.Combine(Styx.Common.Utilities.AssemblyDirectory, "Settings", string.Format(@"DWCC-{0}-{1}.xml", StyxWoW.Me.Name, StyxWoW.Me.RealmName)))
        {
        }

        #region Specc
        [Setting, DefaultValue(true)]
        public bool useArms { get; set; }

        [Setting, DefaultValue(false)]
        public bool useFury { get; set; }

        [Setting, DefaultValue(false)]
        public bool useProt { get; set; }
        #endregion
        #region Taunt
        [Setting, DefaultValue(false)]
        public bool useTaunt { get; set; }
        #endregion
        #region HealthStone
        [Setting, DefaultValue(true)]
        public bool useHealthStone { get; set; }

        [Setting, DefaultValue(15)]
        public int HealthStonePercent { get; set; }
        #endregion
        #region Stances
        [Setting, DefaultValue(false)]
        public bool AutoSwitchBattleStance { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoSwitchBerserkerStance { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoSwitchDefensiveStance { get; set; }
        #endregion
        #region PvP
        [Setting, DefaultValue(false)]
        public bool usePvPRotation { get; set; }
        #endregion
        #region Pummel
        [Setting, DefaultValue(true)]
        public bool usePummel { get; set; }
        #endregion
        #region Disarm Prot
        [Setting, DefaultValue(true)]
        public bool useDisarmProt { get; set; }
        #endregion
        #region Vigilance
        [Setting, DefaultValue(false)]
        public bool useVigilanceProt { get; set; }

        [Setting, DefaultValue(true)]
        public bool useVigilanceOnRandom { get; set; }

        [Setting, DefaultValue(false)]
        public bool useVigilanceOnSpecific { get; set; }

        [Setting, DefaultValue("")]
        public string VigilanceSpecificName { get; set; }
        #endregion
        #region Potion
        [Setting, DefaultValue(true)]
        public bool usePotion { get; set; }

        [Setting, DefaultValue(20)]
        public int PotionPercent { get; set; }
        #endregion
        #region TrinketOne
        [Setting, DefaultValue(false)]
        public bool UseTrinketOneOnCd { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketOneHero { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketOneBelow20 { get; set; }

        [Setting, DefaultValue(true)]
        public bool DoNotUseTrinketOne { get; set; }

        [Setting, DefaultValue(false)]
        public bool useTrinketOneCC { get; set; }
        #endregion
        #region TrinketTwo
        [Setting, DefaultValue(false)]
        public bool UseTrinketTwoOnCd { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketTwoHero { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseTrinketTwoBelow20 { get; set; }

        [Setting, DefaultValue(true)]
        public bool DoNotUseTrinketTwo { get; set; }

        [Setting, DefaultValue(false)]
        public bool useTrinketTwoCC { get; set; }
        #endregion
        #region Movement
        [Setting, DefaultValue(false)]
        public bool DisableMovement { get; set; }
        #endregion
        #region Pull
        [Setting, DefaultValue(true)]
        public bool usePullBehaviour { get; set; }

        [Setting, DefaultValue(20)]
        public int PullRange { get; set; }
        #endregion
        #region CombatDistance
        [Setting, DefaultValue(10)]
        public int CombatDistance { get; set; }
        #endregion
        #region TankTargeting
        [Setting, DefaultValue(true)]
        public bool useAutoTargetProt { get; set; }
        #endregion
        #region AoEPummel
        [Setting, DefaultValue(false)]
        public bool usePummelAoEAuto { get; set; }
        #endregion
        #region Rest
        [Setting, DefaultValue(true)]
        public bool UseRest { get; set; }

        [Setting, DefaultValue(25)]
        public int RestPercent { get; set; }
        #endregion
        #region VR/IV
        [Setting, DefaultValue(80)]
        public int IVHealth { get; set; }
        #endregion
        #region Racials
        [Setting, DefaultValue(true)]
        public bool UseRacials { get; set; }
        #endregion
        #region CDs
        [Setting, DefaultValue(40)]
        public int DbtSHealth { get; set; }

        #region Arms
        [Setting, DefaultValue(true)]
        public bool ArmsAvaOnCD { get; set; }

        [Setting, DefaultValue(false)]
        public bool ArmsAvaOnBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool ArmsReckOnCD { get; set; }

        [Setting, DefaultValue(false)]
        public bool ArmsReckOnBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool ArmsSB { get; set; }

        [Setting, DefaultValue(1)]
        public int HSStacks { get; set; }
        #endregion

        #region Fury
        [Setting, DefaultValue(true)]
        public bool FuryAvaOnCD { get; set; }

        [Setting, DefaultValue(false)]
        public bool FuryAvaOnBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool FuryReckOnCD { get; set; }

        [Setting, DefaultValue(false)]
        public bool FuryReckOnBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool FurySB { get; set; }
        #endregion

        #region Prot
        [Setting, DefaultValue(true)]
        public bool ProtAvaOnCD { get; set; }

        [Setting, DefaultValue(false)]
        public bool ProtAvaOnBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool ProtReckOnCD { get; set; }

        [Setting, DefaultValue(false)]
        public bool ProtReckOnBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool ProtSB { get; set; }

        [Setting, DefaultValue(30)]
        public int SBarrHealth { get; set; }

        [Setting, DefaultValue(30)]
        public int SBHealth { get; set; }

        [Setting, DefaultValue(30)]
        public int SWHealth { get; set; }

        [Setting, DefaultValue(30)]
        public int LStandHealth { get; set; }

        [Setting, DefaultValue(true)]
        public bool useLStand { get; set; }

        [Setting, DefaultValue(true)]
        public bool useSBarr { get; set; }

        [Setting, DefaultValue(true)]
        public bool useSW { get; set; }

        [Setting, DefaultValue(true)]
        public bool useSB { get; set; }
        #endregion

        #region Never
        [Setting, DefaultValue(false)]
        public bool AvatarArmsNever { get; set; }
        [Setting, DefaultValue(false)]
        public bool ArmsReckNever { get; set; }
        [Setting, DefaultValue(false)]
        public bool AvatarFuryNever { get; set; }
        [Setting, DefaultValue(false)]
        public bool FuryReckNever { get; set; }
        [Setting, DefaultValue(false)]
        public bool AvatarProtNever { get; set; }
        [Setting, DefaultValue(false)]
        public bool ProtReckNever { get; set; }
        #endregion

        #endregion
        #region AoE
        [Setting, DefaultValue(true)]
        public bool MoveOutOfAoE { get; set; }
        #endregion
        #region SynapseSprigns
        [Setting, DefaultValue(false)]
        public bool UseSynapseSpringsOnCD { get; set; }
        [Setting, DefaultValue(false)]
        public bool UseSynapseSpringsOnBurst { get; set; }
        [Setting, DefaultValue(true)]
        public bool DoNotUseSynapseSprings { get; set; }
        #endregion
        #region PureDPS
        [Setting, DefaultValue(false)]
        public bool pureDPS { get; set; }
        #endregion

    }
}