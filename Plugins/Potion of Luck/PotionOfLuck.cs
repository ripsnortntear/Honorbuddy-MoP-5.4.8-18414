using System;
using System.Linq;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.Plugins;
using Styx.WoWInternals.WoWObjects;

namespace PotionOfLuck {
    public class PotionOfLuck : HBPlugin {

        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        public static LocalPlayer Me = StyxWoW.Me;

        public static WoWItem CrystalOfInsanityItem;

        // ===========================================================
        // Constructors
        // ===========================================================

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        public override string Name {
            get { return "Potion of Luck"; }
        }

        public override string Author {
            get { return "Wigglez- edited by Stuartroad"; }
        }

        public override Version Version {
            get { return new Version(1, 0); }
        }

        public override void Initialize() {
            CustomNormalLog("Initialization complete.");
            
            base.Initialize();
        }

        public override void Dispose() {
            CustomNormalLog("Shutdown complete.");

            base.Dispose();
        }

        public override void Pulse() {
            if(!HasCrystalOfInsanityItem()) {
                return;
            }

            if(!CanUseCrystalOfInsanity()) {
                return;
            }

            if(HasCrystalOfInsanityBuff()) {
                return;
            }

            if(IsCrystalOfInsanityOnCooldown()) {
                return;
            }

            UseCrystalOfInsanity();

            CustomNormalLog("Crystal of Insanity buff is now active.");
        }

        // ===========================================================
        // Methods
        // ===========================================================

        public void CustomNormalLog(string message, params object[] args) {
            Logging.Write(Colors.DeepSkyBlue, "[Crystal of Insanity]: " + message, args);
        }

        public static bool IsViable(WoWObject pWoWObject) {
            return (pWoWObject != null) && pWoWObject.IsValid;
        }

        public static bool HasCrystalOfInsanityItem() {
            CrystalOfInsanityItem = Me.BagItems.FirstOrDefault(item => item.Entry == 93351);

            return CrystalOfInsanityItem != null;
        }

        public static bool CanUseCrystalOfInsanity() {
            return IsViable(Me) && !Me.Mounted && !Me.IsDead && !Me.InVehicle && !Me.IsChanneling && !Me.IsFlying && !Me.IsCasting && !Me.OnTaxi;
        }

        public static bool HasCrystalOfInsanityBuff() {
            return Me.HasAura(105699);
        }

        public static void UseCrystalOfInsanity() {
            CrystalOfInsanityItem.Use();
        }

        public static bool IsCrystalOfInsanityOnCooldown() {
            return CrystalOfInsanityItem.Cooldown > 0;
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================


    }
}