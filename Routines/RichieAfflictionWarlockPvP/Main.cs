using JetBrains.Annotations;
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

namespace RichieAfflictionWarlock {

    public partial class Main : CombatRoutine {

        #region Delegates

        public delegate WoWPoint LocationRetriverDelegate(object context);

        #endregion

        #region Basic Functions

        public override bool WantButton {
            get { return true; }
        }

        public override WoWClass Class {
            get {
                if (Me.Class == WoWClass.Warlock && StyxWoW.Me.Specialization == WoWSpec.WarlockAffliction)
                    return WoWClass.Warlock;
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
            BotEvents.OnBotStopped += BotEvents_OnBotStopped;

            Logging.Write("-----------------------------------------------------------------------------------------------------");
            Logging.Write("--- Welcome to " + RoutineName + " ---");
            Logging.Write("-----------------------------------------------------------------------------------------------------");
        }

        void BotEvents_OnBotStarted(EventArgs args) {
            if (AfflictionSettings.Instance.rightClickMovementOff && TreeRoot.Current.Name != "BGBuddy") {
                Lua.DoString("RunMacroText('/run ConsoleExec(\"Autointeract 0\")');");
            }

            UpdateMyGlyph();
            UpdateMyTalent();

            if (AfflictionSettings.Instance.FakeCast) {
                AttachCombatLogEvent();
            } else {
                DetachCombatLogEvent();
            }

            printSettings();
        }


        void BotEvents_OnBotStopped(EventArgs args) {

            Lua.Events.DetachEvent("PLAYER_TALENT_UPDATE", UpdateMyTalentOrGlyphEvent);
            Logging.WriteDiagnostic("Detached talent change tracker");
            Lua.Events.DetachEvent("GLYPH_ADDED", UpdateMyTalentOrGlyphEvent);
            Logging.WriteDiagnostic("Detached glyph change tracker");

            DetachCombatLogEvent();
        }

        public override void OnButtonPress() {
            var gui = new UI();
            gui.Text = Name;
            gui.ShowDialog();

        }

        #endregion



        #region GetUnits

        private static IEnumerable<WoWUnit> GetAllUnits() {

            return ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(u => ValidUnit(u) && u.Distance2D <= 40 && !u.IsCritter)
                             .ToList();
            /*return
                ObjectManager.GetObjectsOfType<WoWUnit>(true, true)
                             .Where(u => ValidUnit(u) && u.Distance2DSqr < 40f * 40f && !u.IsCritter)
                             .ToList();*/
        }

        private static Composite GetUnits() {
		
            return new Action(
				delegate {
					NearbyFriendlyPlayers.Clear();
					NearbyUnFriendlyPlayers.Clear();
					NearbyFriendlyUnits.Clear();
					NearbyUnFriendlyUnits.Clear();
                    NearbyTotems.Clear();

					NearbyFriendlyUnits.Add(Me);
					NearbyFriendlyPlayers.Add(Me);

					foreach (WoWUnit unit in GetAllUnits()) {
						if (!unit.IsValid) {
							continue;
						}

						if (IsMyPartyRaidMember(unit)) {
						
							var player = unit as WoWPlayer;
							
							NearbyFriendlyUnits.Add(unit);

							if (player != null) {
								NearbyFriendlyPlayers.Add(player);
							}
						} else {
							if (IsEnemy(unit)) {

                                if (unit.IsPet || unit.IsPlayer || unit.Name.Contains("Dummy")
                                    // || Me.CurrentMap.Name.Contains("Alterac Valley")
                                    ) {
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

        #region Hold

        private static Composite Hold() {
            
			return new Decorator(
				ret =>
					!Me.IsValid ||
					!StyxWoW.IsInWorld ||
					!Me.IsAlive ||
                    (Me.Mounted && TreeRoot.Current.Name != "BGBuddy") ||
                    Me.IsFlying ||
					Me.HasAura("Food") ||
					Me.HasAura("Drink") ||
                    Me.HealthPercent == 0 ||
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



            //Logging.Write("getPreferredPetSpellId(): " + (getPreferredPetSpellId()));
            //Logging.Write("Me.CastingSpellId: " + (Me.CastingSpellId));
            
            //Logging.Write("Me.HasAura(): " + (Me.HasAura("Grimoire of Sacrifice")));
            //Logging.Write("GetSacrificeAbility(): " + (GetSacrificeAbility()));
            //Logging.Write("icon: " + (WoWSpell.FromId(SpellIds.Instance.CommandDemon).Icon.ToLower()));
            //Logging.Write("Me.CurrentPendingCursorSpell: " + (Me.CurrentPendingCursorSpell));
            
            //Logging.Write("hasaura s : " + Me.HasAura("Symbiosis"));
            //Logging.Write("hasaura s : " + MyAuraTimeLeft("Rejuvenation", Me));            
            //Logging.Write("asd : " + SpellManager.Spells["Optical Blast"].CooldownTimeLeft.TotalMilliseconds);
            //Logging.Write("bsd : " + SpellManager.Spells["Spell Lock"].CooldownTimeLeft.TotalMilliseconds);

            /*if (fearTracker.TryGetValue(Me.CurrentTarget.Guid, out tempFearTime)) {
                Logging.Write(tempFearTime.ToString("ss:fff"));
            }*/

            //Logging.Write("" + Me.Pet.SubName);
            //Logging.Write("" + Me.Pet.);
            /*Me.GetAllAuras().All(ret => {
                Logging.Write(ret.Name + ": " + ret.SpellId);
                return true;
            });
            Logging.Write("Soul Link: " + Me.HasAura("Soul Link"));
            Logging.Write("Dark Bargain: " + Me.HasAura("Dark Bargain"));
            Logging.Write("Magicdot: " + DebuffMagicDot(Me));
            Logging.Write("TBMDD: " + TargetedByMagicDamageDealer());*/
            

            //Logging.Write("" + Me.HasAura("Burning Rush"));
            /*Logging.Write("casting: " + Casting());
            Logging.Write("gcd: " + GCDL());
            Logging.Write("channeled: " + Me.ChanneledCastingSpellId);*/
            //Logging.Write("MB manacost: " + SpellManager.Spells["Mind Blast"].PowerCost);    

            /*for (int i = 0; i < SpellManager.Spells.Count(); i++)
            {
                Logging.Write("" +  SpellManager.Spells.ElementAt(i).Key);
            }*/

            //Logging.Write("--" + Me.MovementInfo.TimeMoved);

            /*if (Me.IsCasting) {

                Logging.Write("casting: " + Me.IsCasting + ", " + Me.CastingSpell.Name + ", " + Me.CastingSpellId);
                Logging.Write("casting: " + (Me.CastingSpell.Name.Contains("Opening") || Me.CastingSpell.Name.Contains("Capturing")));
//                            (unit.CastingSpellId == 98322 || unit.CastingSpellId == 98323 || unit.CastingSpellId == 98324 || 
  //                          (unit.CastingSpell != null && unit.CastingSpell.Name != null && ()))
            }*/
        }

        #endregion

        #region YBMoP BT - LockSelector
        [UsedImplicitly]
        private class LockSelector : PrioritySelector {
            public LockSelector(params Composite[] children)
                : base(children) {
            }
            public override RunStatus Tick(object context) {
                using (StyxWoW.Memory.AcquireFrame()) {
                    return base.Tick(context);
                }
            }
        }
        #endregion

        /*#region YBMoP BT - LockSelector
        [UsedImplicitly]
        private class LockSelector : Decorator {
             public LockSelector(CanRunDecoratorDelegate func, Composite decorated)
                : base(func,decorated) {
            }
            public override RunStatus Tick(object context) {
                using (StyxWoW.Memory.AcquireFrame()) {
                    return base.Tick(context);
                }
            }
        }
       #endregion*/

    }
}