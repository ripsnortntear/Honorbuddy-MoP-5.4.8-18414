#define MEASURE_PERFORMANCE_
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;
using System.IO;
using System.Data;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace RichieHolyPriestPvP {

    public partial class Main : CombatRoutine {

        #region GetAsyncKeyState

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        #endregion

        #region Basic Functions

        public override bool WantButton {
            get { return true; }
        }

        public override WoWClass Class {
            get {
                if (Me.Class == WoWClass.Priest && StyxWoW.Me.Specialization == WoWSpec.PriestHoly)
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

        public static LocalPlayer Me {
            get { return UnitManager.Me; }
        }

        public override void Initialize()
        {

            Lua.Events.AttachEvent("PLAYER_TALENT_UPDATE", onTalentAndGlyphUpdate);
            Lua.Events.AttachEvent("GLYPH_ADDED", onTalentAndGlyphUpdate);
            Lua.Events.AttachEvent("GLYPH_UPDATED", onTalentAndGlyphUpdate);
            
            BotEvents.OnBotStarted += BotEvents_OnBotStarted;
            BotEvents.OnBotStopped += BotEvents_OnBotStopped;

            Logging.Write("-----------------------------------------------------------------------------------------------------");
            Logging.Write("--- Welcome to " + RoutineName + " ---");
            Logging.Write("-----------------------------------------------------------------------------------------------------");
        }

        void BotEvents_OnBotStopped(EventArgs args)
        {
            GlyphManager.OnReloaded -= OnGlyphsReloaded;
            TalentManager.OnReloaded = null;

            Lua.Events.DetachEvent("PLAYER_TALENT_UPDATE", onTalentAndGlyphUpdate);
            Logging.WriteDiagnostic("Detached talent change tracker");
            Lua.Events.DetachEvent("GLYPH_ADDED", onTalentAndGlyphUpdate);
            Lua.Events.DetachEvent("GLYPH_UPDATED", onTalentAndGlyphUpdate);
            Logging.WriteDiagnostic("Detached glyph change tracker");

            SpecializedCombatLog.Detach();

            LoSer.Clear();

            PerfLogger.PrintAll();
            PerfLogger.ResetAll();
        }

        void BotEvents_OnBotStarted(EventArgs args) {
            
            if (HolySettings.Instance.RightClickMovementOff && TreeRoot.Current.Name != "BGBuddy")
                Lua.DoString("RunMacroText('/run ConsoleExec(\"Autointeract 0\")');");

            GlyphManager.OnReloaded += OnGlyphsReloaded;
            if (GlyphManager.Reload())
                GlyphManager.Print();

            if (TalentManager.Reload())
                TalentManager.Print();

            Spells.OnReloaded = () => {
                //added because it's not listed as a known spell
                Spells.AddAdditional(SpellIDs.HolyWordSerenity, "Holy Word: Serenity");
                Spells.AddAdditional(SpellIDs.HolyWordSanctuary, "Holy Word: Sanctuary");

                HolyCoolDown.Fake(SpellIDs.HolyWordSanctuary);
            };

            if (Spells.Reload())
                Spells.Print();

            // Will Preload Mana Costs
            UInt64 sum = 0;
            foreach (var spellId in Spells.GetKnownSpellIds())
                sum += (UInt64)ManaCosts[(SpellIDs)spellId];
            Console.Write(sum);

            LoSer.Clear();

            IsArena = Me.CurrentMap.IsArena;
            IsBG = Me.CurrentMap.IsBattleground;
            IsRBG = !IsArena && Me.IsFFAPvPFlagged;

            try
            {
                string PathASAPIDs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("Routines/RichieHolyPriestPvP/ASAPSpellIDs.xml"));

                XmlDocument doc = new XmlDocument();
                doc.Load(PathASAPIDs);
                XmlNodeList objects = doc.SelectNodes("//DispelASAP/spell");
                if (objects.Count > 0)
                {
                    Logging.Write("Loading Dispel ASAP override ids...");
                    HolySettings.Instance.BuffDispelASAPHS.Clear();
                    foreach (XmlNode o in objects)
                        HolySettings.Instance.BuffDispelASAPHS.Add(Convert.ToInt32(o.Attributes["id"].Value));
                }

                Logging.WriteDiagnostic("New values of BuffDispelASAPHS:");
                foreach (int i in HolySettings.Instance.BuffDispelASAPHS)
                    Logging.WriteDiagnostic("" + i);

                objects = doc.SelectNodes("//PurifyASAP/spell");
                if (objects.Count > 0)
                {
                    Logging.Write("Loading Purify ASAP override ids...");
                    HolySettings.DebuffCCPurifyASAPHS.Clear();
                    foreach (XmlNode o in objects)
                        HolySettings.DebuffCCPurifyASAPHS.Add(Convert.ToInt32(o.Attributes["id"].Value));
                }

                Logging.WriteDiagnostic("New values of DebuffCCPurifyASAPHS:");
                foreach (int i in HolySettings.DebuffCCPurifyASAPHS)
                    Logging.WriteDiagnostic("" + i);
            }
            catch (Exception e)
            {
                Logging.Write("Exception while loading ASAP Spell IDs");
                Logging.WriteException(e);
            }

            SpecializedCombatLog.Attach();

            HolySettings.Print();
        }

        private void OnGlyphsReloaded()
        {
            FearWardCooldown = GlyphManager.Has(Glyphs.GlyphOfFearWard) ? 120 : 180;
            UnitManager.HasSWDGlypth = GlyphManager.Has(Glyphs.GlyphOfShadowWordDeath);
        }

        #region Update Glyphs

        private static void onTalentAndGlyphUpdate(object sender, LuaEventArgs args)
        {
            UnitManager.Clear();

            if (GlyphManager.Reload())
                GlyphManager.Print();

            if (TalentManager.Reload())
                TalentManager.Print();

            Spells.Reload();

            SpecializedCombatLog.Attach();

            LoSer.Clear();

            IsArena = Me.CurrentMap.IsArena;
            IsBG = Me.CurrentMap.IsBattleground;
            IsRBG = !IsArena && Me.IsFFAPvPFlagged;

            Statistics.Print();
            Statistics.Clear();

            PerfLogger.PrintAll();
            PerfLogger.ResetAll();
        }

        #endregion

        public override void OnButtonPress()
        {
            var gui = new UI();
            gui.Text = Name;
            gui.ShowDialog();
        }

        #endregion

        #region Pulse

        public override void Pulse()
        {
            if (!Me.IsValid || !Me.IsAlive || !StyxWoW.IsInWorld)
                return;

            if (LastScan.AddMilliseconds(HolySettings.Instance.SearchInterval) <= DateTime.Now)
            {
                LastScan = DateTime.Now;

                using (StyxWoW.Memory.AcquireFrame())
                {
#if MEASURE_PERFORMANCE
                    using (var perf = PerfLogger.GetHelper("ObjectManager.Update"))
#endif
                        ObjectManager.Update();

#if MEASURE_PERFORMANCE
                    using (var perf = PerfLogger.GetHelper("RefreshUnits"))
#endif
                        UnitManager.Refresh();
                }
            }

            #region Debug
            /*
            Me.CurrentTarget.GetAllAuras().All(ret => {
                Logging.Write("All - " + ret.Name + ": " + ret.SpellId);
                return true;
            });

            Me.CurrentTarget.ActiveAuras.All(ret => {
                Logging.Write("Active - " + ret.Value.Name + ": " + ret.Value.SpellId);
                return true;
            });
            
            Logging.Write("" + Eval("Me.CurrentRage < 90", () => Me.CurrentRage < 90));
            Logging.Write("" + Eval("Mortal Wounds", () => MyAuraTimeLeft("Mortal Wounds", Me.CurrentTarget) < 750));
            Logging.Write("" + Eval("Mortal Wounds2", () => IsMyAuraFadingOnUnit(Me.CurrentTarget, 115804, 750)));           
            Logging.Write("" + Eval("Mortal Strike", () => SpellManager.Spells["Mortal Strike"].CooldownTimeLeft.TotalMilliseconds <= MyLatency));
            */
            //Logging.Write("asd: " + GetAsyncKeyState(Keys.LShiftKey));

            /*Logging.Write("getdistance: " + GetDistance(Me.CurrentTarget));
            Logging.Write("IsWithinMeleeRange: " + Me.CurrentTarget.IsWithinMeleeRange);
            Logging.Write("distance: " + Me.CurrentTarget.Distance);
            Logging.Write("CombatReach: " + Me.CurrentTarget.CombatReach);
            Logging.Write("DistanceSqr: " + Me.CurrentTarget.DistanceSqr);
            Logging.Write("Me.CombatReach: " + Me.CombatReach);*/

            /*            SpellFindResults results;
            if (SpellManager.FindSpell("Intervene", out results)) {
                Logging.Write("i: " + (results.Override != null ? results.Override.CooldownTimeLeft : results.Original.CooldownTimeLeft));
            }

            if (SpellManager.FindSpell("Safeguard", out results)) {
                Logging.Write("sg: " + (results.Override != null ? results.Override.CooldownTimeLeft : results.Original.CooldownTimeLeft));
            }*/

/*            DateTime started = DateTime.Now;
            Logging.Write(Colors.LightBlue, "InLineOfSpellSight enter: {0}", Name);
            bool a = Me.CurrentTarget.InLineOfSpellSight;
            Logging.Write(Colors.LightBlue, "InLineOfSpellSight leave: {0}, took {1} ms", Name, (ulong)(DateTime.Now - started).TotalMilliseconds);

            started = DateTime.Now;
            Logging.Write(Colors.LightBlue, "StopCasting enter: {0}", Name);
            StopCasting();
            Logging.Write(Colors.LightBlue, "StopCasting leave: {0}, took {1} ms", Name, (ulong)(DateTime.Now - started).TotalMilliseconds);

            started = DateTime.Now;
            Logging.Write(Colors.LightBlue, "StopCasting2 enter: {0}", Name);
            Lua.DoString("RunMacroText(\"/stopcasting\")");
            Logging.Write(Colors.LightBlue, "StopCasting2 leave: {0}, took {1} ms", Name, (ulong)(DateTime.Now - started).TotalMilliseconds);

            started = DateTime.Now;
            Logging.Write(Colors.LightBlue, "StopCasting3 enter: {0}", Name);
            SpellManager.StopCasting();
            Logging.Write(Colors.LightBlue, "StopCasting3 leave: {0}, took {1} ms", Name, (ulong)(DateTime.Now - started).TotalMilliseconds);

            started = DateTime.Now;
            Logging.Write(Colors.LightBlue, "GetUnits enter: {0}", Name);
            GetUnits();
            Logging.Write(Colors.LightBlue, "GetUnits leave: {0}, took {1} ms", Name, (ulong)(DateTime.Now - started).TotalMilliseconds);

            started = DateTime.Now;
            Logging.Write(Colors.LightBlue, "GetUnitHeal enter: {0}", Name);
            GetUnitHeal();
            Logging.Write(Colors.LightBlue, "GetUnitHeal leave: {0}, took {1} ms", Name, (ulong)(DateTime.Now - started).TotalMilliseconds);
            */


            //Logging.Write("Manacosts.Instance.PainSuppression: " + Manacosts.Instance.PainSuppression);

            /*Logging.Write("Manacosts.Instance.Dispel: " + Manacosts.Instance.Dispel);
            Logging.Write("LastDispel.AddSeconds(HolySettings.Instance.DispelDelay) <= DateTime.Now: " + (LastDispel.AddSeconds(HolySettings.Instance.DispelDelay) <= DateTime.Now));// &&
            Logging.Write("HealTarget.HealthPercent >= HolySettings.Instance.DispelAboveHp: " + (HealTarget.HealthPercent >= HolySettings.Instance.DispelAboveHp));// &&
            Logging.Write("Name: " + PlayerCastingPolyOrRepentance.Name);// &&
            Logging.Write("Name: " + PlayerCastingPolyOrRepentance.Name);//SpellManager.Spells["Dispel"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
            */


            /*if (PlayerCastingPolyOrRepentance != null && PlayerCastingPolyOrRepentance.IsValid && PlayerCastingPolyOrRepentance.IsCasting) {
                Logging.Write("Name: " + PlayerCastingPolyOrRepentance.Name);
                Logging.Write("CastingSpell.Name: " + PlayerCastingPolyOrRepentance.CastingSpell.Name);
                Logging.Write("CurrentCastTimeLeft: " + PlayerCastingPolyOrRepentance.CurrentCastTimeLeft.TotalMilliseconds);
            }*/


            /*Logging.Write("NearbyFriendlyUnits: " + NearbyFriendlyUnits.Count());
            NearbyFriendlyUnits.All(u => {
                if (!u.IsValid) {
                    Logging.Write(u.Name + " is not valid!");
                }
                Logging.Write(u.Name + ", " + u.IsPlayer);
                return true;
            });

            Logging.Write("NearbyUnFriendlyUnits: " + NearbyUnFriendlyUnits.Count());
            NearbyUnFriendlyUnits.All(u => {
                if (!u.IsValid) {
                    Logging.Write(u.Name + " is not valid!");
                }
                Logging.Write(u.Name + ", " + u.IsPlayer);
                return true;
            });

            Logging.Write("NearbyTotems: " + NearbyTotems.Count());
            NearbyTotems.All(u => {
                if (!u.IsValid) {
                    Logging.Write(u.Name + " is not valid!");
                }
                Logging.Write(u.Name + ", " + u.IsTotem);
                return true;
            });*/


            //Logging.Write("casting: " + Casting());
            //Logging.Write("gcd: " + GCDL());
            //Logging.Write("channeled: " + Me.ChanneledCastingSpellId);
            //Logging.Write("casted: " + Me.CastingSpellId);
            //Logging.Write("hold: " + (HolySettings.Instance.DontCancelDominateMind && Me.IsCasting && Me.CastingSpellId == 605));
            
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

            #endregion
        }

        #endregion
    }
}