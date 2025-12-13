using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace RichieDiscPriestPvP {

    public partial class Main : CombatRoutine {

        #region Delegates

        public delegate WoWPoint LocationRetriverDelegate(object context);

        #endregion

        #region Basic Functions

        private Composite _combatBehavior;

        public override bool WantButton {
            get { return true; }
        }

        public override WoWClass Class {
            get {
                if (Me.Class == WoWClass.Priest && StyxWoW.Me.Specialization == WoWSpec.PriestDiscipline)
                    return WoWClass.Priest;
                return WoWClass.None; 
            }
        }

        public override string Name {
            get { return RoutineName; }
        }

        public override Composite PreCombatBuffBehavior {
            get { return MainRotation(); }
        }

        public override Composite CombatBehavior {
            get { return MainRotation(); }

        }

        private static LocalPlayer Me {
            get { return StyxWoW.Me; }
        }

        public override void Initialize() {

            Lua.Events.AttachEvent("PLAYER_TALENT_UPDATE", UpdateMyTalentOrGlyphEvent);
            Lua.Events.AttachEvent("GLYPH_ADDED", UpdateMyTalentOrGlyphEvent);
            
            BotEvents.OnBotStarted += BotEvents_OnBotStarted;
            

            Logging.Write("-----------------------------------------------------------------------------------------------------");
            Logging.Write("--- Welcome to " + RoutineName + " ---");
            Logging.Write("-----------------------------------------------------------------------------------------------------");
        }

        void BotEvents_OnBotStarted(EventArgs args) {
            if (DiscSettings.Instance.rightClickMovementOff && TreeRoot.Current.Name != "BGBuddy") {
                Lua.DoString("RunMacroText('/run ConsoleExec(\"Autointeract 0\")');");
            }

            UpdateMyGlyph();
            UpdateMyTalent();

            if (GlyphNames.Contains("Glyph of Holy Fire")) {
                SmiteRange = 40;
            } else {
                SmiteRange = 30;
            }
        }

        public override void OnButtonPress() {
            var gui = new UI();
            gui.ShowDialog();

        }

        #endregion

        #region GetSpellCooldown

        private static TimeSpan GetSpellCooldown(string spell) {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results)) {
                return results.Override != null ? results.Override.CooldownTimeLeft : results.Original.CooldownTimeLeft;
            }

            return TimeSpan.MaxValue;
        }

        #endregion

        #region GetUnits

        private static IEnumerable<WoWUnit> GetAllUnits() {
            return
                ObjectManager.GetObjectsOfType<WoWUnit>(true, true)
                             .Where(u => ValidUnit(u) && u.Distance2DSqr < 60f * 60f && !u.IsCritter)
                             .ToList();
        }

        private static Composite GetUnits() {
		
            return new Action(
				delegate {
					NearbyFriendlyPlayers.Clear();
					NearbyUnFriendlyPlayers.Clear();
					NearbyFriendlyUnits.Clear();
					NearbyUnFriendlyUnits.Clear();
                    NearbyTotems.Clear();
					FarFriendlyPlayers.Clear();
					FarFriendlyUnits.Clear();

					NearbyFriendlyUnits.Add(Me);
					NearbyFriendlyPlayers.Add(Me);
					FarFriendlyUnits.Add(Me);
					FarFriendlyPlayers.Add(Me);

					foreach (WoWUnit unit in GetAllUnits()) {
						if (!unit.IsValid) {
							continue;
						}

						if (IsMyPartyRaidMember(unit)) {
						
							FarFriendlyUnits.Add(unit);

							var player = unit as WoWPlayer;
							if (player != null) {
								FarFriendlyPlayers.Add((WoWPlayer)unit);
							}

							if (unit.DistanceSqr < 1600) {
							
								NearbyFriendlyUnits.Add(unit);

								if (player != null) {
									NearbyFriendlyPlayers.Add(player);
								}
							}
						} else {
							if (IsEnemy(unit)) {
								if (unit.DistanceSqr > 1600) {
									continue;
								}

                                if (unit.IsPet || unit.IsPlayer || unit.Name.Contains("Dummy")) {
                                    NearbyUnFriendlyUnits.Add(unit);
                                } else {
                                    if (unit.Name.Contains("Totem")) {
                                        NearbyTotems.Add(unit);
                                    }
                                }

								var player = unit as WoWPlayer;
								if (player != null) {
									NearbyUnFriendlyPlayers.Add(player);
								}
							}
						}					
					}
				}
			);
        }

        #endregion

        #region GetUnitHeal

        private static Composite GetUnitHeal() {
            return new Action(
				delegate {
                    HealTarget = Me;
                    InjuredUnitCount = 0;

                    /*HealTarget = (from unit in NearbyFriendlyUnits
								where unit.IsValid
								where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
								orderby unit.HealthPercent ascending
								where unit.IsPlayer || Me.CurrentMap.IsArena && unit.IsPet
								where Healable(unit)
								select unit).FirstOrDefault();*/

                    (from unit in NearbyFriendlyUnits
                        where unit.IsValid
                        //where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                        //orderby unit.HealthPercent ascending
                        where unit.IsPlayer || Me.CurrentMap.IsArena && unit.IsPet && unit.HealthPercent <= 60
                        where Healable(unit)
                        select unit).ForEach(u => {

                            if (HealTarget.HealthPercent >= u.HealthPercent) {
                                HealTarget = u;
                            }

                            if (u.HealthPercent <= 90) {
                                InjuredUnitCount++;
                            }
                        });

					

                    if (HealTarget == null || !HealTarget.IsValid) {
                        HealTarget = Me;
                    }

					return RunStatus.Failure;
				}
			);
        }

        #endregion

        #region Hold

        private static Composite Hold() {
            
			return new Decorator(
				ret =>
					!Me.IsValid ||
					!StyxWoW.IsInWorld ||
					!Me.IsAlive ||
                    //!SpellManager.HasSpell("Meditation") ||
                    (Me.Mounted && TreeRoot.Current.Name != "BGBuddy") ||
					Me.HasAura("Food") ||
					Me.HasAura("Drink") ||
					(Me.HasAura("Spectral Guise") && (HealTarget == null || HealTarget.HealthPercent >= 40)) ||
					Me.HasAura("Resurrection Sickness"),
				new Action(
					delegate { 
						return RunStatus.Success; 
					}
				)
			);
        }

        #endregion

        #region Pulse

        public override void Pulse() {
            if (!Me.IsValid || !Me.IsAlive || !StyxWoW.IsInWorld) {
                return;
            }
            ObjectManager.Update();
            /*Logging.Write("casting: " + Casting());
            Logging.Write("gcd: " + GCDL());
            Logging.Write("channeled: " + Me.ChanneledCastingSpellId);
            Logging.Write("can cast MB: " + SpellManager.CanCast("Mind Blast"));*/
            //Logging.Write("MB manacost: " + SpellManager.Spells["Mind Blast"].PowerCost);    

            /*for (int i = 0; i < SpellManager.Spells.Count(); i++)
            {
                Logging.Write("" +  SpellManager.Spells.ElementAt(i).Key);
            }*/

            /*Me.GetAllAuras().All(ret => {
                Logging.Write(ret.Name + ": " + ret.SpellId);
                return true;
            });*/

            /*if (Me.IsChanneling) {
                Logging.Write("casted: " + Me.ChanneledSpell + "(" + Me.ChanneledCastingSpellId + ")");
            }

            if (Me.IsCasting) {
                Logging.Write("casted: " + Me.CastingSpell + "(" + Me.CastingSpellId + ")");
            }

            Logging.Write("InjuredUnitCount: " + InjuredUnitCount);*/
            //Logging.Write(TreeRoot.Current.Name);
            /*if (PWSTarget != null && PWSTarget.IsValid) {
                Logging.Write(PWSTarget.Name);
            } else {
                Logging.Write("no blanket target");
            }*/
        }

        #endregion

    }
}