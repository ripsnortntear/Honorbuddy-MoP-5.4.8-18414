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

namespace RichieAfflictionWarlock
{
    public partial class Main {

        private static string RoutineName = "Richie's Affliction Warlock PvP Combat Routine";

		#region MainRotation

        private static Composite MainRotation() {
            return new PrioritySelector(
                Hold(),
                new Throttle(TimeSpan.FromMilliseconds(AfflictionSettings.Instance.SearchInterval), GetUnits()),
                MiscSetups(),
                WotF(),
                new Decorator(ret => Me.HealthPercent <= 90,
                    new PrioritySelector(
                        UseHealthstone(),
                        DarkBargain(),
                        SacrificialPact(),
                        UnendingResolve()
                    )
                ),
                SummonPet(),
                SacrificePet(),
                new Decorator(ret => ValidUnit(Me.CurrentTarget) && !InvulnerableSpell(Me.CurrentTarget) &&
                    IsEnemy(Me.CurrentTarget) &&
                    (!CastingorGCDL() || Me.CurrentTarget.HealthPercent <= 20) &&
                    Me.CurrentTarget.Distance2D < 40 &&
                    Me.CurrentTarget.InLineOfSpellSight &&
                    !Me.CurrentTarget.IsTotem,
                    new PrioritySelector(
                //checkforburst
                        new Decorator(ret => AfflictionSettings.Instance.BurstOnCD,
                            new PrioritySelector(
                                DPSTrinket("burst"),
                                DarkSoul()
                            )
                        ),
                //spells
                        SoulburnSoulSwap(),
                        DrainLife(),
                        Agony(),
                        Corruption(),
                        UnstableAffliction(),
                        DrainSoul(),
                        Haunt(),
                        MaleficGrasp()
                    )
                ),
                LifeTap(),
                new Decorator(ret => AfflictionSettings.Instance.Multidot &&
                    NearbyUnFriendlyUnits.Count > 0 &&
                    !CastingorGCDL() &&
                    GetMultidotTarget(),
                    new PrioritySelector(
                        SoulSwapExhale(),
                        SoulSwapInhale(),
                        AgonyMulti(),
                        CorruptionMulti(),
                        UnstableAfflictionMulti()
                    )
                ),
                DarkIntent(),
                CreateHealthstone(),
                Soulstone()
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
