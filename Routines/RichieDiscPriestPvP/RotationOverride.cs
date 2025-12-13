using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace RichieDiscPriestPvP
{
    public partial class Main {

        private static string RoutineName = "Richie's Discipline Priest PvP Combat Routine";

        //Main Rotation
		#region MainRotation

        private static Composite MainRotation() {
            return new PrioritySelector(
                Hold(),
                new Throttle(TimeSpan.FromMilliseconds(DiscSettings.Instance.SearchInterval), GetUnits()),
                GetUnitHeal(),
                MiscSetups(),
                PainSuppression(),
                Fade(),                
                UseHealthstone(),
                Barrier(),
                WotF(),
                AngelicFeather(),
                Shackle(),
                PurifyLowPrio(),
                Cascade(),
                Halo(),
                new Decorator(ret => !CastingorGCDL() && Healable(HealTarget),
                    new PrioritySelector(
                        PrayerOfHealing(),
                        new Decorator(ret => HealTarget.HealthPercent <= 30,
                            new PrioritySelector(
                                DPSTrinket("heal"),
                                Berserking("heal")
                            )
                        ),
                        PWS(),
                        PoM(),
                        Renew(),
                        Penance(),
                        FlashHealInstant(),
                        BindingHeal(),
                        FlashHeal(),                                                
                        DivineStar()
                    )
                ),
                InnerFire(),
                PowerWordFortitude(),
                FearWardSelf(),
                SetBasicVariables()
            );
        }


		private static Composite SetBasicVariables() {			

            return new Action(
                delegate {
                    return RunStatus.Success;
                }
            );
		}

        #endregion

    }
}
