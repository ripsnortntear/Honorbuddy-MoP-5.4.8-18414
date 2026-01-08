using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;


namespace MobileBanking {
    public class MobileBanking : HBPlugin {

        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        public static LocalPlayer Me = StyxWoW.Me;
        public static WoWObject MobileBank;

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
            get { return "Mobile Banking"; }
        }

        public override string Author {
            get { return "Wigglez updated by Naxor"; }
        }

        public override Version Version {
            get { return new Version(2, 0); }
        }

        public override void Pulse() {
            if(Me.IsDead) {
                return;
            }

            if(Me.Combat) {
                return;
            }

            if(!IsViable(Me)) {
                return;
            }

            if(Me.Mounted) {
                return;
            }

            if(Me.Gold < 1000) {
                return;
            }

            if(!HasMobileBanking()) {
                return;
            }

            if(!MobileBankExists()) {
                FindMobileBank();

                if(!CanCastMobileBanking()) {
                    return;
                }

                CastMobileBanking();
            } else {
                if(!MobileBank.WithinInteractRange) {

                    var mobileBankLocation = WoWMovement.CalculatePointFrom(MobileBank.Location, 5f);

                    Navigator.MoveTo(mobileBankLocation);
                }
            
                MobileBank.Interact();

                var depositCopperAmount = Me.Copper - 1000000;
                var depositGoldAmount = Me.Gold - 100;

                DepositGuildBankMoney(depositCopperAmount);

                CustomNormalLog("Deposited " + depositGoldAmount + " gold and closed the bank frame.");

            }
        }

        // ===========================================================
        // Methods
        // ===========================================================

        public void CustomNormalLog(string message, params object[] args) {
            Logging.Write(Colors.DeepSkyBlue, "[Mobile Banking]: " + message, args);
        }

        public static bool IsViable(WoWObject pWoWObject) {
            return (pWoWObject != null) && pWoWObject.IsValid;
        }

        public static bool HasMobileBanking() {
            return SpellManager.HasSpell(83958);
        }

        public static bool CanCastMobileBanking() {
            return SpellManager.CanCast(83958);
        }

        public static void CastMobileBanking() {
            SpellManager.Cast(83958);
        }

        public static void FindMobileBank() {
            MobileBank = ObjectManager.GetObjectsOfTypeFast<WoWObject>().FirstOrDefault(bank => bank.IsValid && bank.Entry == 206603);
        }

        public static bool MobileBankExists() {
            return MobileBank != null;
        }

        public static void DepositGuildBankMoney(ulong pCopper) {
            Lua.DoString(string.Format("DepositGuildBankMoney({0})", pCopper));
        }

        public static void CloseGuildBankFrame() {
            Lua.DoString("CloseGuildBankFrame()");
        }

        public static List<string> GetFactionInfoByID(int pFactionID) {
            return Lua.GetReturnValues(string.Format("return GetFactionInfoByID({0})", pFactionID));
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

    }
}
