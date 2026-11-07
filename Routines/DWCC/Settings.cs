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
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool useArms { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool useFury { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool useProt { get; set; }
        #endregion
        #region Taunt
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool useTaunt { get; set; }
        #endregion
        #region HealthStone
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool useHealthStone { get; set; }

        [Setting, Styx.Helpers.DefaultValue(15)]
        public int HealthStonePercent { get; set; }
        #endregion
        #region Stances
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool AutoSwitchBattleStance { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoSwitchBerserkerStance { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoSwitchDefensiveStance { get; set; }
        #endregion
        #region PvP
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool usePvPRotation { get; set; }
        #endregion
        #region Pummel
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool usePummel { get; set; }
        #endregion
        #region Disarm Prot
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool useDisarmProt { get; set; }
        #endregion
        #region Vigilance
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool useVigilanceProt { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool useVigilanceOnRandom { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool useVigilanceOnSpecific { get; set; }

        [Setting, Styx.Helpers.DefaultValue("")]
        public string VigilanceSpecificName { get; set; }
        #endregion
        #region Potion
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool usePotion { get; set; }

        [Setting, Styx.Helpers.DefaultValue(20)]
        public int PotionPercent { get; set; }
        #endregion
        #region TrinketOne
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool UseTrinketOneOnCd { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool UseTrinketOneHero { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool UseTrinketOneBelow20 { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool DoNotUseTrinketOne { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool useTrinketOneCC { get; set; }
        #endregion
        #region TrinketTwo
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool UseTrinketTwoOnCd { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool UseTrinketTwoHero { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool UseTrinketTwoBelow20 { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool DoNotUseTrinketTwo { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool useTrinketTwoCC { get; set; }
        #endregion
        #region Movement
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool DisableMovement { get; set; }
        #endregion
        #region Pull
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool usePullBehaviour { get; set; }

        [Setting, Styx.Helpers.DefaultValue(20)]
        public int PullRange { get; set; }
        #endregion
        #region CombatDistance
        [Setting, Styx.Helpers.DefaultValue(10)]
        public int CombatDistance { get; set; }
        #endregion
        #region TankTargeting
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool useAutoTargetProt { get; set; }
        #endregion
        #region AoEPummel
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool usePummelAoEAuto { get; set; }
        #endregion
        #region Rest
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool UseRest { get; set; }

        [Setting, Styx.Helpers.DefaultValue(25)]
        public int RestPercent { get; set; }
        #endregion
        #region VR/IV
        [Setting, Styx.Helpers.DefaultValue(80)]
        public int IVHealth { get; set; }
        #endregion
        #region Racials
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool UseRacials { get; set; }
        #endregion
        #region CDs
        [Setting, Styx.Helpers.DefaultValue(40)]
        public int DbtSHealth { get; set; }

        #region Arms
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool ArmsAvaOnCD { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool ArmsAvaOnBoss { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool ArmsReckOnCD { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool ArmsReckOnBoss { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool ArmsSB { get; set; }

        [Setting, Styx.Helpers.DefaultValue(1)]
        public int HSStacks { get; set; }
        #endregion

        #region Fury
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool FuryAvaOnCD { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool FuryAvaOnBoss { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool FuryReckOnCD { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool FuryReckOnBoss { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool FurySB { get; set; }
        #endregion

        #region Prot
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool ProtAvaOnCD { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool ProtAvaOnBoss { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool ProtReckOnCD { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool ProtReckOnBoss { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool ProtSB { get; set; }

        [Setting, Styx.Helpers.DefaultValue(30)]
        public int SBarrHealth { get; set; }

        [Setting, Styx.Helpers.DefaultValue(30)]
        public int SBHealth { get; set; }

        [Setting, Styx.Helpers.DefaultValue(30)]
        public int SWHealth { get; set; }

        [Setting, Styx.Helpers.DefaultValue(30)]
        public int LStandHealth { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool useLStand { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool useSBarr { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool useSW { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool useSB { get; set; }
        #endregion

        #region Never
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool AvatarArmsNever { get; set; }
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool ArmsReckNever { get; set; }
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool AvatarFuryNever { get; set; }
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool FuryReckNever { get; set; }
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool AvatarProtNever { get; set; }
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool ProtReckNever { get; set; }
        #endregion

        #endregion
        #region AoE
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool MoveOutOfAoE { get; set; }
        #endregion
        #region SynapseSprigns
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool UseSynapseSpringsOnCD { get; set; }
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool UseSynapseSpringsOnBurst { get; set; }
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool DoNotUseSynapseSprings { get; set; }
        #endregion
        #region PureDPS
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool pureDPS { get; set; }
        #endregion

    }
}