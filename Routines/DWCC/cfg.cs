using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using System.IO;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Common;
using System.Reflection;

namespace DWCC
{
    public partial class cfg : Form
    {
        public cfg()
        {
            InitializeComponent();
        }

        private void cfg_Load(object sender, EventArgs e)
        {
            //pictureBox1.Image = Properties.Resources.logo;
            //pictureBox1.ImageLocation = Styx.Common.Utilities.AssemblyDirectory + @"\Routines\DWCC\utils\logo.jpg";
            Text = "Dunatank's Warrior CC v" + DWCC.Warrior.ver + " " + DWCC.Warrior.Tag;
            #region Trinkets
            if (StyxWoW.Me.Inventory.GetItemBySlot(12) != null)
            {
                TrinketOneGroupBox.Text = StyxWoW.Me.Inventory.GetItemBySlot(12).Name;
            }
            else
            {
                UseTrinketOneOnCd.Checked = false;
                UseTrinketOneOnCd.Enabled = false;
                UseTrinketOneHero.Checked = false;
                UseTrinketOneHero.Enabled = false;
                UseTrinketOneBelow20.Checked = false;
                UseTrinketOneBelow20.Enabled = false;
                DoNotUseTrinketOne.Checked = true;
                DoNotUseTrinketOne.Enabled = false;
                UseTrinketOneCC.Checked = false;
                UseTrinketOneCC.Enabled = false;
            }
            if (StyxWoW.Me.Inventory.GetItemBySlot(13) != null)
            {
                TrinketTwoGroupBox.Text = StyxWoW.Me.Inventory.GetItemBySlot(13).Name;
            }
            else
            {
                UseTrinketTwoOnCd.Checked = false;
                UseTrinketTwoOnCd.Enabled = false;
                UseTrinketTwoHero.Checked = false;
                UseTrinketTwoHero.Enabled = false;
                UseTrinketTwoBelow20.Checked = false;
                UseTrinketTwoBelow20.Enabled = false;
                DoNotUseTrinketTwo.Checked = true;
                DoNotUseTrinketTwo.Enabled = false;
                UseTrinketTwoCC.Checked = false;
                UseTrinketTwoCC.Enabled = false;
            }
            #endregion

            DunatanksSettings.Instance.Load();

            #region Specs
            UseArms.Checked = DunatanksSettings.Instance.useArms;
            UseFury.Checked = DunatanksSettings.Instance.useFury;
            UseProt.Checked = DunatanksSettings.Instance.useProt;
            #endregion
            #region PvP
            usePvPRota.Checked = DunatanksSettings.Instance.usePvPRotation;
            #endregion
            #region Healthstone
            UseHealthStone.Checked = DunatanksSettings.Instance.useHealthStone;
            HealthStonePercent.Value = new decimal(DunatanksSettings.Instance.HealthStonePercent);
            #endregion
            #region Pummel
            UsePummel.Checked = DunatanksSettings.Instance.usePummel;
            #endregion
            #region Taunt
            UseTaunt.Checked = DunatanksSettings.Instance.useTaunt;
            #endregion
            #region Potions
            UsePotion.Checked = DunatanksSettings.Instance.usePotion;
            PotionPercent.Value = new decimal(DunatanksSettings.Instance.PotionPercent);
            #endregion
            #region Trinket One
            UseTrinketOneOnCd.Checked = DunatanksSettings.Instance.UseTrinketOneOnCd;
            UseTrinketOneBelow20.Checked = DunatanksSettings.Instance.UseTrinketOneBelow20;
            UseTrinketOneHero.Checked = DunatanksSettings.Instance.UseTrinketOneHero;
            DoNotUseTrinketOne.Checked = DunatanksSettings.Instance.DoNotUseTrinketOne;
            #endregion
            #region Trinket Two
            UseTrinketTwoOnCd.Checked = DunatanksSettings.Instance.UseTrinketTwoOnCd;
            UseTrinketTwoBelow20.Checked = DunatanksSettings.Instance.UseTrinketTwoBelow20;
            UseTrinketTwoHero.Checked = DunatanksSettings.Instance.UseTrinketTwoHero;
            DoNotUseTrinketTwo.Checked = DunatanksSettings.Instance.DoNotUseTrinketTwo;
            #endregion
            #region Movement
            DisableMovement.Checked = DunatanksSettings.Instance.DisableMovement;
            #endregion
            #region Combat Distance
            CombatDistance.Value = new decimal(DunatanksSettings.Instance.CombatDistance);
            #endregion
            #region Auto-Target
            UseAutoTargetProt.Checked = DunatanksSettings.Instance.useAutoTargetProt;
            #endregion
            #region PvP Trinket
            UseTrinketOneCC.Checked = DunatanksSettings.Instance.useTrinketOneCC;
            UseTrinketTwoCC.Checked = DunatanksSettings.Instance.useTrinketTwoCC;
            #endregion
            #region Rest
            UseRest.Checked = DunatanksSettings.Instance.UseRest;
            RestPercent.Value = new decimal(DunatanksSettings.Instance.RestPercent);
            #endregion
            #region Racials
            useRacials.Checked = DunatanksSettings.Instance.UseRacials;
            #endregion
            #region CDs
            ArmsAvatarCD.Checked = DunatanksSettings.Instance.ArmsAvaOnCD;
            ArmsAvatarBoss.Checked = DunatanksSettings.Instance.ArmsAvaOnBoss;
            ArmsAvatarNever.Checked = DunatanksSettings.Instance.AvatarArmsNever;
            ArmsReckCD.Checked = DunatanksSettings.Instance.ArmsReckOnCD;
            ArmsReckBoss.Checked = DunatanksSettings.Instance.ArmsReckOnBoss;
            ArmsReckNever.Checked = DunatanksSettings.Instance.ArmsReckNever;
            AvatarFuryCD.Checked = DunatanksSettings.Instance.FuryAvaOnCD;
            AvatarFuryBoss.Checked = DunatanksSettings.Instance.FuryAvaOnBoss;
            AvatarFuryNever.Checked = DunatanksSettings.Instance.AvatarFuryNever;
            FuryReckCD.Checked = DunatanksSettings.Instance.FuryReckOnCD;
            FuryReckBoss.Checked = DunatanksSettings.Instance.FuryReckOnBoss;
            FursReckNever.Checked = DunatanksSettings.Instance.FuryReckNever;
            AvatarProtCD.Checked = DunatanksSettings.Instance.ProtAvaOnCD;
            AvatarProtBoss.Checked = DunatanksSettings.Instance.ProtAvaOnBoss;
            AvatarProtNever.Checked = DunatanksSettings.Instance.AvatarProtNever;
            ProtReckCD.Checked = DunatanksSettings.Instance.ProtReckOnCD;
            ProtReckBoss.Checked = DunatanksSettings.Instance.ProtReckOnBoss;
            ProtReckNever.Checked = DunatanksSettings.Instance.ProtReckNever;
            DBTSPercent.Value = new decimal(DunatanksSettings.Instance.DbtSHealth);
            useSkullBannerArms.Checked = DunatanksSettings.Instance.ArmsSB;
            useSkullBannerFury.Checked = DunatanksSettings.Instance.FurySB;
            useSkullBannerProt.Checked = DunatanksSettings.Instance.ProtSB;
            useShieldBarrier.Checked = DunatanksSettings.Instance.useSBarr;
            ShieldBarrierPercent.Value = new decimal(DunatanksSettings.Instance.SBarrHealth);
            useShieldBlock.Checked = DunatanksSettings.Instance.useSB;
            ShieldBlockPercent.Value = new decimal(DunatanksSettings.Instance.SBHealth);
            useShieldWall.Checked = DunatanksSettings.Instance.useSW;
            ShieldWallPercent.Value = new decimal(DunatanksSettings.Instance.SWHealth);
            useLastStand.Checked = DunatanksSettings.Instance.useLStand;
            LastStandPercent.Value = new decimal(DunatanksSettings.Instance.LStandHealth);
            #endregion
            #region MoveOutOfAoE
            MoveOutOfAoE.Checked = DunatanksSettings.Instance.MoveOutOfAoE;
            #endregion
            #region VR/IV
            VRPercent.Value = new decimal(DunatanksSettings.Instance.IVHealth);
            #endregion
            #region Synapse Springs
            UseSynapseSpringsOnCD.Checked = DunatanksSettings.Instance.UseSynapseSpringsOnCD;
            UseSynapseSpringsOnBurst.Checked = DunatanksSettings.Instance.UseSynapseSpringsOnBurst;
            DoNotUseSynapseSprings.Checked = DunatanksSettings.Instance.DoNotUseSynapseSprings;
            #endregion
            #region PureDPS
            pureDPS.Checked = DunatanksSettings.Instance.pureDPS;
            #endregion

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region specc
            DunatanksSettings.Instance.useArms = UseArms.Checked;
            DunatanksSettings.Instance.useFury = UseFury.Checked;
            DunatanksSettings.Instance.useProt = UseProt.Checked;
            #endregion
            #region PvP
            DunatanksSettings.Instance.usePvPRotation = usePvPRota.Checked;
            #endregion
            #region healthstone
            DunatanksSettings.Instance.useHealthStone = UseHealthStone.Checked;
            DunatanksSettings.Instance.HealthStonePercent = (int)HealthStonePercent.Value;
            #endregion
            #region Pummel
            DunatanksSettings.Instance.usePummel = UsePummel.Checked;
            #endregion
            #region Taunt
            DunatanksSettings.Instance.useTaunt = UseTaunt.Checked;
            #endregion
            #region Potions
            DunatanksSettings.Instance.usePotion = UsePotion.Checked;
            DunatanksSettings.Instance.PotionPercent = (int)PotionPercent.Value;
            #endregion
            #region Trinket One
            DunatanksSettings.Instance.UseTrinketOneOnCd = UseTrinketOneOnCd.Checked;
            DunatanksSettings.Instance.UseTrinketOneBelow20 = UseTrinketOneBelow20.Checked;
            DunatanksSettings.Instance.UseTrinketOneHero = UseTrinketOneHero.Checked;
            DunatanksSettings.Instance.DoNotUseTrinketOne = DoNotUseTrinketOne.Checked;
            #endregion
            #region Trinket Two
            DunatanksSettings.Instance.UseTrinketTwoOnCd = UseTrinketTwoOnCd.Checked;
            DunatanksSettings.Instance.UseTrinketTwoBelow20 = UseTrinketTwoBelow20.Checked;
            DunatanksSettings.Instance.UseTrinketTwoHero = UseTrinketTwoHero.Checked;
            DunatanksSettings.Instance.DoNotUseTrinketTwo = DoNotUseTrinketTwo.Checked;
            #endregion
            #region Movement
            DunatanksSettings.Instance.DisableMovement = DisableMovement.Checked;
            #endregion
            #region Combat Distance
            DunatanksSettings.Instance.CombatDistance = (int)CombatDistance.Value;
            #endregion
            #region Auto-Target
            DunatanksSettings.Instance.useAutoTargetProt = UseAutoTargetProt.Checked;
            #endregion
            #region PvP Trinket
            DunatanksSettings.Instance.useTrinketOneCC = UseTrinketOneCC.Checked;
            DunatanksSettings.Instance.useTrinketTwoCC = UseTrinketTwoCC.Checked;
            #endregion
            #region Rest
            DunatanksSettings.Instance.UseRest = UseRest.Checked;
            DunatanksSettings.Instance.RestPercent = (int)RestPercent.Value;
            #endregion
            #region Racials
            DunatanksSettings.Instance.UseRacials = useRacials.Checked;
            #endregion
            #region CDs
            DunatanksSettings.Instance.ArmsAvaOnCD = ArmsAvatarCD.Checked;
            DunatanksSettings.Instance.ArmsAvaOnBoss = ArmsAvatarBoss.Checked;
            DunatanksSettings.Instance.AvatarArmsNever = ArmsAvatarNever.Checked;
            DunatanksSettings.Instance.ArmsReckOnCD = ArmsReckCD.Checked;
            DunatanksSettings.Instance.ArmsReckOnBoss = ArmsReckBoss.Checked;
            DunatanksSettings.Instance.ArmsReckNever = ArmsReckNever.Checked;
            DunatanksSettings.Instance.FuryAvaOnCD = AvatarFuryCD.Checked;
            DunatanksSettings.Instance.FuryAvaOnBoss = AvatarFuryBoss.Checked;
            DunatanksSettings.Instance.AvatarFuryNever = AvatarFuryNever.Checked;
            DunatanksSettings.Instance.FuryReckOnCD = FuryReckCD.Checked;
            DunatanksSettings.Instance.FuryReckOnBoss = FuryReckBoss.Checked;
            DunatanksSettings.Instance.FuryReckNever = FursReckNever.Checked;
            DunatanksSettings.Instance.ProtAvaOnCD = AvatarProtCD.Checked;
            DunatanksSettings.Instance.ProtAvaOnBoss = AvatarProtBoss.Checked;
            DunatanksSettings.Instance.AvatarProtNever = AvatarProtNever.Checked;
            DunatanksSettings.Instance.ProtReckOnCD = ProtReckCD.Checked;
            DunatanksSettings.Instance.ProtReckOnBoss = ProtReckBoss.Checked;
            DunatanksSettings.Instance.ProtReckNever = ProtReckNever.Checked;
            DunatanksSettings.Instance.DbtSHealth = (int)DBTSPercent.Value;
            DunatanksSettings.Instance.ArmsSB = useSkullBannerArms.Checked;
            DunatanksSettings.Instance.FurySB = useSkullBannerFury.Checked;
            DunatanksSettings.Instance.ProtSB = useSkullBannerProt.Checked;
            DunatanksSettings.Instance.useSBarr = useShieldBarrier.Checked;
            DunatanksSettings.Instance.SBarrHealth = (int)ShieldBarrierPercent.Value;
            DunatanksSettings.Instance.useSB = useShieldBlock.Checked;
            DunatanksSettings.Instance.SBHealth = (int)ShieldBlockPercent.Value;
            DunatanksSettings.Instance.useSW = useShieldWall.Checked;
            DunatanksSettings.Instance.SWHealth = (int)ShieldWallPercent.Value;
            DunatanksSettings.Instance.useLStand = useLastStand.Checked;
            DunatanksSettings.Instance.LStandHealth = (int)LastStandPercent.Value;
            #endregion
            #region MoveOutOfAoE
            DunatanksSettings.Instance.MoveOutOfAoE = MoveOutOfAoE.Checked;
            #endregion
            #region VR/IV
            DunatanksSettings.Instance.IVHealth = (int)VRPercent.Value;
            #endregion
            #region Synapse Springs
            DunatanksSettings.Instance.UseSynapseSpringsOnCD = UseSynapseSpringsOnCD.Checked;
            DunatanksSettings.Instance.UseSynapseSpringsOnBurst = UseSynapseSpringsOnBurst.Checked;
            DunatanksSettings.Instance.DoNotUseSynapseSprings = DoNotUseSynapseSprings.Checked;
            #endregion
            #region PureDPS
            DunatanksSettings.Instance.pureDPS = pureDPS.Checked;
            #endregion
            DunatanksSettings.Instance.Save();
            Logging.Write("[DWCC]: Config saved");
            WriteSettingsToLog();
            Close();
        }


        private void UseRest_CheckedChanged(object sender, EventArgs e)
        {
            if (UseRest.Checked == true)
            {
                RestPercent.Enabled = true;
            }
            else
            {
                RestPercent.Enabled = false;
            }
        }

        #region Write Settings.xml to log
        public void WriteSettingsToLog()
        {
            TextReader SettingsReader = new StreamReader(Path.Combine(Styx.Common.Utilities.AssemblyDirectory, "Settings", string.Format(@"DWCC-{0}-{1}.xml", StyxWoW.Me.Name, StyxWoW.Me.RealmName)));
            string line;
            while ((line = SettingsReader.ReadLine()) != null)
                Logging.WriteDiagnostic(line);
        }
        #endregion
    }
}
