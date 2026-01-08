
using System;
using Styx.Common.Helpers;
using Styx.CommonBot.Inventory;
using Styx.Helpers;
using Styx.Plugins;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
//using Styx.Plugins.PluginClass;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Helpers;
using Styx;
//using Styx.Logic.Pathing;

using Styx.Helpers;



using System.Windows.Forms;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
/***************************************************************
TODO

-PetsAlive EnemyPetsAlive



***************************************************************/

//using System.Linq;


//using Styx.Logic.BehaviorTree;

//using Styx.Logic;


//using Styx.Logic.Combat;
//using Styx.Logic.Inventory;
//using Styx.WoWInternals.WoWObjects;
//using Styx.Logic.Inventory.Frames.LootFrame;
//using Styx.Logic.Inventory.Frames.Gossip;
using Styx.WoWInternals.World;
//using Styx.Logic.Profiles;
//using Styx.Logic.AreaManagement;
using Styx.Plugins;
using Styx.WoWInternals.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Color = System.Drawing.Color;

using System.Text;
using Bots.BGBuddy.Helpers;
using Styx;

using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;
//using Microsoft.VisualBasic.Interaction;
using CalcEngine;

namespace Pokehbuddyplug
{
    public partial class Pokehbuddy : HBPlugin
    {
        private readonly WaitTimer _updateTimer = WaitTimer.TenSeconds;
        private ulong oldguid = 0;
        private int skipcounter = 0;
        private int blacklistcounter = 0;
        public static BBSettings MySettings;
        public int pvptimer;
        public bool disable = false;
        public bool oldlogging;
        public static PetBattleSettings PetSettings;
        public static PetBattleSettings DefaultLogicz;
        //private static readonly configForm Gui = new configForm();
        private configForm Gui = new configForm();
        private static Stopwatch timer = new Stopwatch();

        //private static readonly Form1 Gui2 = new Form1();
        public static List<string> theblacklist = new List<string>();
        public static List<string> thewhitelist = new List<string>();

        private string[] PetDefaultLogics = { "SWAPOUT Health(THISPET) ISLESSTHAN 30", "PASSTURN HASBUFF(822) EQUALS true", "PASSTURN HASBUFF(498) EQUALS true", "CASTSPELL(1) COOLDOWN(SKILL(1)) EQUALS false" };
        public Pokehbuddy()
        {

            try
            {
                MySettings = new BBSettings(Application.StartupPath + "\\Plugins\\Pokehbuddy\\Pokehbuddy.xml");
                LoadDefaultLogic("Default Logic");
                BlacklistLoad();
                WhitelistLoad();



            }


            catch (Exception ex) { BBLog(ex.ToString()); }
            //
            //BBLog("Pokehbuddy loaded");
            if (MySettings.AdFormula.Contains("DisFactor"))
            {
                MySettings.HPFormula = "petHP * HPFactor";
                MySettings.AdFormula = "advantage * 50 * AdFactor";
                MySettings.DisFormula = "disadvantage * 50 * DisFactor";
                MySettings.LevelFormula = "(petLevel - enemylevel) * 4 * LevelFactor";
            }
            PrintSettings();
            oldlogging = Pokehbuddy.MySettings.DetailedLogging;



            timer.Reset();
            timer.Start();
            //Random random = new Random();
            //pvptimer = (long)random.Next(Pokehbuddy.MySettings.PVPMinTime*60000, Pokehbuddy.MySettings.PVPMaxTime*60000);
        }
        public override void OnButtonPress()
        {
            //Gui.Load();
            Gui.Close();
            Gui = new configForm();
            Gui.Show();
            //Gui.Activate();
        }
		internal static void StatCounter()
        {
            if (MySettings.CheckAllowUsageTracking)
            {
                try
                {
                    var statcounterDate = DateTime.Now.DayOfYear.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    var statcounterPath = Application.StartupPath + "\\Plugins\\Pokehbuddy\\Images\\Statcounter\\";
                    var statcounterCleanup = Directory.GetFiles(statcounterPath);

                    if (!File.Exists(Path.Combine(statcounterPath, statcounterDate)))
                    {
                        new MemoryStream(new WebClient().DownloadData("http://c.statcounter.com/9192745/0/0966fc00/1/"));
                        if (!Directory.Exists(statcounterPath)) { Directory.CreateDirectory(statcounterPath); }
                        File.Create(Path.Combine(statcounterPath, statcounterDate));
                        foreach (var counterfile in statcounterCleanup)
                        {
                            var name = new FileInfo(counterfile).Name;
                            name = name.ToLower();
                            if (name != statcounterDate)
                            {
                                File.Delete(counterfile);
                            }
                        }
                    }
                }
                catch { } // Catch all, like 404's.
            }
        }



        public override void Pulse()
        {
			StatCounter();
            if (disable) return;
            List<string> petsequipped = Lua.GetReturnValues("local dummy=3 if C_PetJournal.GetPetLoadOutInfo(1)==nil then dummy=dummy-1 end if C_PetJournal.GetPetLoadOutInfo(2)==nil then dummy=dummy-1 end if C_PetJournal.GetPetLoadOutInfo(3)==nil then dummy=dummy-1 end return dummy");
            if (petsequipped[0] != "3")
            {
                BBLog("Pets Equipped " + petsequipped[0]);
                BBLog("This is NOT ENOUGH for this plugin to function. Equip 3 pets and learn the rez pet skill.");
                disable = true;
                return;
            }



            if (!oldlogging && Pokehbuddy.MySettings.DetailedLogging)
            {
                PrintSettings();

            }
            oldlogging = Pokehbuddy.MySettings.DetailedLogging;
            if (!InPetCombat()) Styx.Helpers.CharacterSettings.Instance.UseMount = true;
            if (InPetCombat()) Styx.Helpers.CharacterSettings.Instance.UseMount = false;

            if (petsequipped[0] == "3" && Styx.StyxWoW.Me.IsAlive && !Styx.StyxWoW.Me.Combat)
            {
                if (Pokehbuddy.MySettings.DoPVP && !InPetCombat())
                {
                    //check for pvp queue
                    List<string> cnt = Lua.GetReturnValues("if C_PetBattles.GetPVPMatchmakingInfo()=='queued' then return true end return false");

                    //niet queued? Moet ik queuen?
                    if (cnt[0] == "0")
                    {
                        Lua.DoString("if C_PetBattles.GetPVPMatchmakingInfo()=='proposal' then C_PetBattles.AcceptQueuedPVPMatch() end");

                        //BBLog("Queue after"+pvptimer+ "now at"+timer.ElapsedMilliseconds/600);
                        if (pvptimer < 1)
                        {

                            Random _ran = new Random();
                            //BBLog("Queue"+(Pokehbuddy.MySettings.PVPMaxTime+1)*100);
                            pvptimer = _ran.Next((Pokehbuddy.MySettings.PVPMinTime + 1) * 100, ((Pokehbuddy.MySettings.PVPMinTime + 1) + Pokehbuddy.MySettings.PVPMaxTime + 1) * 100);

                            BBLog("new timer set" + pvptimer.ToString() + "  " + Pokehbuddy.MySettings.PVPMinTime + "  " + 100 * (Pokehbuddy.MySettings.PVPMinTime + 1 + Pokehbuddy.MySettings.PVPMaxTime + 1));
                        }
                        if (timer.ElapsedMilliseconds > pvptimer * 600 && pvptimer != 0)
                        {
                            //BBLog("should Queue");
                            Lua.DoString("C_PetBattles.StartPVPMatchmaking()");
                            if (Pokehbuddy.MySettings.DetailedLogging) BBLog("Queued for PVP match");
                            timer.Reset();
                            timer.Start();

                            pvptimer = 0;

                        }
                    }

                    //Heb ik invite?



                }
                if (!InPetCombat() && WildBattleTarget() != null)
                {
                    //Bandages? Pet Rez Skill?
                    ///cast Revive Battle Pets           Pokehbuddy.MySettings.UseHealSkill
                    if (SpellManager.CanCast("Revive Battle Pets") || !SpellManager.CanCast("Revive Battle Pets"))
                    {
                        //do i actually need heals?
                        int dumdum2 = 0;
                        if (GetPetHPPreCombat(1) < 40) dumdum2++;
                        if (GetPetHPPreCombat(2) < 40) dumdum2++;
                        if (GetPetHPPreCombat(3) < 40) dumdum2++;
                        if (dumdum2 > 2)
                        {
                            BBLog(GetPetHPPreCombat(1) + "Injured pets quite high: " + dumdum2 + " Making sure its not due to lag");
                            Lua.DoString("local dummy = C_PetJournal.PetIsHurt(1)");
                            Lua.DoString("local dummy = C_PetJournal.PetIsHurt(2)");
                            Lua.DoString("local dummy = C_PetJournal.PetIsHurt(3)");
                            dumdum2 = 0;
                            if (GetPetHPPreCombat(1) < 40) dumdum2++;
                            if (GetPetHPPreCombat(2) < 40) dumdum2++;
                            if (GetPetHPPreCombat(3) < 40) dumdum2++;
                        }
                        //Heal
                        if (dumdum2 >= Pokehbuddy.MySettings.UseHealSkill)
                        {
                            if (!SpellManager.CanCast("Revive Battle Pets"))
                            {
                                if (SpellManager.Spells["Revive Battle Pets"].CooldownTimeLeft.TotalMilliseconds > 60000)
                                {
                                    if (dumdum2 >= Pokehbuddy.MySettings.UseBandagesToHeal && Pokehbuddy.MySettings.UseBandagesToHeal > 0)
                                    {
                                        Lua.DoString("RunMacroText(\"/use Battle Pet Bandage\");");
                                        BBLog("Enough pets injured, Using Bandages");
                                    }
                                }
                            }
                            SpellManager.Cast("Revive Battle Pets");
                            BBLog("Enough pets injured, Healing/Rezzing pets");
                            //Thread.Sleep(300);
                        }
                    }
                    int dumdum3 = 0;
                    if (GetPetHPPreCombat(1) < 20) dumdum3++;
                    if (GetPetHPPreCombat(2) < 20) dumdum3++;
                    if (GetPetHPPreCombat(3) < 20) dumdum3++;
                    if (Pokehbuddy.MySettings.DetailedLogging) BBLog("Pet HP's : " + GetPetHPPreCombat(1) + ":" + GetPetHPPreCombat(2) + ":" + GetPetHPPreCombat(3));
                    if (dumdum3 > 0) BBLog("Heavily injured pets : " + dumdum3);
                    if ((3 - dumdum3) >= Pokehbuddy.MySettings.MinPetsAlive)
                    {

                        if (WildBattleTarget().Distance > 8)
                        {
                            //Navigate to the pet


                            Styx.WoWInternals.WoWMovement.MoveStop();
                            //Thread.Sleep(200);

                            WoWPoint dummy;
                            while (WildBattleTarget().Distance > 8 && !Styx.StyxWoW.Me.Combat)
                            {
                                if (Pokehbuddy.MySettings.DetailedLogging) BBLog("Move to spot");
                                if (Styx.StyxWoW.Me.IsFlying) Styx.WoWInternals.WoWMovement.ClickToMove(WildBattleTarget().Location.X, WildBattleTarget().Location.Y, WildBattleTarget().Location.Z + 3);
                                if (!Styx.StyxWoW.Me.IsFlying) Navigator.MoveTo(WildBattleTarget().Location);
                                dummy = Styx.StyxWoW.Me.Location;
                                Thread.Sleep(500);
                                if (dummy.Distance(Styx.StyxWoW.Me.Location) < 1)
                                {
                                    if (!Styx.StyxWoW.Me.Combat)
                                    {
                                        blacklistcounter++;
                                        BBLog("STUCK : Blacklist counter :" + blacklistcounter);
                                        if (blacklistcounter > Pokehbuddy.MySettings.BlacklistCounterLimit)
                                        {
                                            BBLog("Could not get close enough to engage. Blacklisting " + WildBattleTarget().Name);
                                            Blacklist.Add(WildBattleTarget().Guid, TimeSpan.FromMinutes(1));
                                            blacklistcounter = 0;
                                        }
                                    }
                                }
                                //BBLog("dummy distance"+dummy.Distance(Styx.StyxWoW.Me.Location));
                            }


                        }
                        if (WildBattleTarget().Distance < 9)
                        {

                            //Preparation, like swapping pets for bonusses
                            SetPetAbilities();
                            BBLog("Battle Preparation");
                            if (!Styx.StyxWoW.Me.Combat && oldguid == WildBattleTarget().Guid)
                            {
                                blacklistcounter++;
                                BBLog("Trying to interact. Blacklist counter :" + blacklistcounter);
                                if (blacklistcounter > Pokehbuddy.MySettings.BlacklistCounterLimit)
                                {
                                    BBLog("Could not interact. Blacklisting" + WildBattleTarget().Name);
                                    Blacklist.Add(WildBattleTarget().Guid, TimeSpan.FromMinutes(1));
                                    blacklistcounter = 0;
                                    Styx.Helpers.CharacterSettings.Instance.UseMount = true;
                                }
                            }
                            oldguid = WildBattleTarget().Guid;

                            try
                            {
                                PetSwappingPreCombat();
                            }

                            catch (Exception ex) { BBLog(ex.ToString()); }
                            Styx.Helpers.CharacterSettings.Instance.UseMount = false;
                            //Thread.Sleep(100);
                            if (Styx.StyxWoW.Me.Mounted) Styx.CommonBot.Mount.Dismount();
                            if (Pokehbuddy.MySettings.DetailedLogging) BBLog("Attempting to start Wild Pet Battle");
                            WildBattleTarget().Interact();
                            Thread.Sleep(1000);

                        }



                        //Start Combat

                        //Styx.Helpers.CharacterSettings.Instance.UseMount=false;

                    }



                }
                if (InPetCombat())
                {
                    //Blacklist.Add(oldguid, TimeSpan.FromMinutes(10));
                    Styx.Helpers.CharacterSettings.Instance.UseMount = false;
                    Styx.Helpers.GlobalSettings.Instance.LogoutForInactivity = false;

                    Lua.DoString("if C_PetBattles.GetPVPMatchmakingInfo()=='proposal' then C_PetBattles.AcceptQueuedPVPMatch() end");
                    blacklistcounter = 0;
                    //Actual battle!
                    // BBLog("In Pet Combat against rarity "+GetRarity()+" and i can trap"+ CanTrap());
                    // BBLog("In Combat");
                    if (MustSelectNew())
                    {
                        BBLog("Have to select new one");
                        ForcedSwapping();
                    }
                    // BBLog("Past new pets");

                    int minrarity = Pokehbuddy.MySettings.GetRarity;
                    if (minrarity > 4)
                    {


                        minrarity = GetRarityBySpeciesID(GetCurrentEnemySpecies());

                        if (minrarity < 1) minrarity = 1;


                    }

                    if (GetRarity() > minrarity && CanTrap())
                    {
                        if (CanFight()) Lua.DoString("C_PetBattles.UseTrap()");

                    }
                    else
                    {
                        if (CanFight())
                        {
                            int getrari = Pokehbuddy.MySettings.GetRarity;
                            if (getrari > 4) getrari = 2;
                            if (Pokehbuddy.MySettings.ForfeitIfNotInteresting)
                            {
                                List<string> forfeit = Lua.GetReturnValues("if C_PetBattles.GetBreedQuality(2,1) <= " + getrari + " and C_PetBattles.GetBreedQuality(2,2) <= " + getrari + " and C_PetBattles.GetBreedQuality(2,3) <= " + getrari + " and  C_PetBattles.IsWildBattle() == true then C_PetBattles.ForfeitGame() return 1 end return 0");
                                //Lua.DoString("if C_PetBattles.GetBreedQuality(2,1) <= " + getrari + " and C_PetBattles.GetBreedQuality(2,2) <= " + getrari + " and C_PetBattles.GetBreedQuality(2,3) <= " + getrari + " and  C_PetBattles.IsWildBattle() == true then C_PetBattles.ForfeitGame() end");
                                //if (forfeit != null) { if (forfeit[0]=="1" BlackListNearby();}
                            }
                            // BBLog("Choosing attack");
                            /*if (PetCanCast(1)) {
                             Lua.DoString("C_PetBattles.UseAbility(1)");
                             }else {
                              Lua.DoString("C_PetBattles.SkipTurn()");
                             }*/
                            //private string PetDefaultLogics[] = {"SWAPOUT Health(THISPET) ISLESSTHAN 50","CASTSPELL(1) COOLDOWN(SKILL(1)) EQUALS false",""};
                            bool didlogic = false;
                            LoadPetSettings(ReadActiveSlot(), ReadActiveSlotSpecies());
                            string dumdum = DefaultLogicz.Logic + "@" + PetSettings.Logic;
                            string[] PetLogics = dumdum.Split('@');
                            BBLog("Doing Logic");
                            foreach (string alogic in PetLogics)
                            {

                                //BBLog("Logic : " + alogic);
                                bool gelukt = ParseLogic(alogic);
                                //BBLog("!!!Doing Logic " + alogic +" and it returned " + gelukt);
                                if (gelukt)
                                {
                                    //BBLog("TRUE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                    if ((String.Compare(alogic.Substring(0, 7), "SWAPOUT") == 0)) WantSwapping();
                                    if ((String.Compare(alogic.Substring(0, 12), "CASTSPELL(1)") == 0)) Lua.DoString("C_PetBattles.UseAbility(1)");
                                    if ((String.Compare(alogic.Substring(0, 12), "CASTSPELL(2)") == 0)) Lua.DoString("C_PetBattles.UseAbility(2)");
                                    if ((String.Compare(alogic.Substring(0, 12), "CASTSPELL(3)") == 0)) Lua.DoString("C_PetBattles.UseAbility(3)");
                                    if ((String.Compare(alogic.Substring(0, 8), "PASSTURN") == 0)) Lua.DoString("C_PetBattles.SkipTurn()");
                                    if ((String.Compare(alogic.Substring(0, 7), "FORFEIT") == 0)) { Lua.DoString("C_PetBattles.ForfeitGame()"); return; }

                                    didlogic = true;
                                    skipcounter = 0;

                                }

                            }
                            if (Pokehbuddy.MySettings.DetailedLogging)
                            {
                                BBLog("----------------------------------------------------------------");
                                BBLog("FINISHED Logics");
                                BBLog("----------------------------------------------------------------");
                            }
                            if (!didlogic)
                            {
                                if (!PetCanCast(1) && !PetCanCast(2) && !PetCanCast(3))
                                {
                                }
                                skipcounter++;
                                BBLog("SKIP count :" + skipcounter);
                                Thread.Sleep(1500);
                                if (skipcounter > Pokehbuddy.MySettings.SkipCounterLimit)
                                {
                                    Lua.DoString("C_PetBattles.SkipTurn()");
                                    skipcounter = 0;
                                    BBLog("SKIPPED!!!");
                                }

                            }






                        }
                    }


                }




                //if (Styx.StyxWoW.Me.CurrentTarget != null)  BBLog(""+Styx.StyxWoW.Me.CurrentTarget.CreatureType);
                /* if (IsBattlePet(Styx.StyxWoW.Me.CurrentTarget)) {
                   BBLog("COMBAT PET!!");
                  if (HasPet(Styx.StyxWoW.Me.CurrentTarget.Name))  BBLog("do not want");
                  if (!HasPet(Styx.StyxWoW.Me.CurrentTarget.Name))  BBLog("WANT!!GIEB!!");
                 }
                 /*if (HasPet(Styx.StyxWoW.Me.CurrentTarget.Name))  BBLog("do not want");
                  if (!HasPet(Styx.StyxWoW.Me.CurrentTarget.Name))  BBLog("WANT!!GIEB!!");
                 // BBLog(""+HasPet(Styx.StyxWoW.Me.CurrentTarget.Name));*/








            }
        }

        public static void BlacklistLoad()
        {
            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\blacklist.txt";

            theblacklist.Clear();

            try
            {
                StreamReader Read = new StreamReader(Convert.ToString(filename));
                while (Read.Peek() >= 0)
                {
                    string pline = Read.ReadLine();
                    if (pline != null)
                    {
                        theblacklist.Add(pline.ToLower());

                    }
                }
                Read.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex.Message));
                return;
            }

        }

        public static void WhitelistLoad()
        {
            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\whitelist.txt";

            thewhitelist.Clear();

            try
            {
                StreamReader Read = new StreamReader(Convert.ToString(filename));
                while (Read.Peek() >= 0)
                {
                    string pline = Read.ReadLine();
                    if (pline != null)
                    {
                        thewhitelist.Add(pline.ToLower());

                    }
                }
                Read.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex.Message));
                return;
            }

        }


        public void PrintSettings()
        {
            if (Pokehbuddy.MySettings.DetailedLogging)
            {
                BBLog("**************************************************************************************");
                BBLog("Settings :");
                BBLog("Distance " + Pokehbuddy.MySettings.Distance);
                BBLog("HPFactor " + Pokehbuddy.MySettings.HPFactor);
                BBLog("LevelFactor " + Pokehbuddy.MySettings.LevelFactor);
                BBLog("Advantage Factor " + Pokehbuddy.MySettings.AdFactor);
                BBLog("Disadvantage Factor " + Pokehbuddy.MySettings.DisFactor);
                BBLog("GetRarity " + Pokehbuddy.MySettings.GetRarity);
                BBLog("BlacklistCounterLimit " + Pokehbuddy.MySettings.BlacklistCounterLimit);
                BBLog("SkipCounterLimit " + Pokehbuddy.MySettings.SkipCounterLimit);
                BBLog("DoPVP " + Pokehbuddy.MySettings.DoPVP);
                BBLog("PVPMinTime " + Pokehbuddy.MySettings.PVPMinTime);
                BBLog("PVPMaxTime " + Pokehbuddy.MySettings.PVPMaxTime);
                BBLog("BelowLevel " + Pokehbuddy.MySettings.BelowLevel);
                BBLog("AboveLevel " + Pokehbuddy.MySettings.AboveLevel);
                BBLog("UseBandagesToHeal " + Pokehbuddy.MySettings.UseBandagesToHeal);
                BBLog("MinPetsAlive " + Pokehbuddy.MySettings.MinPetsAlive);
                BBLog("UseHealSkill " + Pokehbuddy.MySettings.UseHealSkill);


                BBLog("DetailedLogging " + Pokehbuddy.MySettings.DetailedLogging);
                List<string> cnt = Lua.GetReturnValues("local dummy=3 if C_PetJournal.GetPetLoadOutInfo(1)==nil then dummy=dummy-1 end if C_PetJournal.GetPetLoadOutInfo(2)==nil then dummy=dummy-1 end if C_PetJournal.GetPetLoadOutInfo(3)==nil then dummy=dummy-1 end return dummy");
                BBLog("Pets Equipped " + cnt[0]);
                BBLog("**************************************************************************************");
            }
        }
        public void LoadPetSettings(string petID, string speciesID)
        {
            if (Pokehbuddy.MySettings.DetailedLogging) BBLog("Attempting to load " + petID + ", if not existing, reverting to species " + speciesID);

            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + petID + ".xml";
            if (!File.Exists(filename))
            {
                if (Pokehbuddy.MySettings.DetailedLogging) BBLog("Error : " + petID + " not found, attempt to revert to " + speciesID);
                string filename2 = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + speciesID + ".xml";
                if (File.Exists(filename2))
                {
                    File.Copy(filename2, filename);
                }
            }

            try
            {
                PetSettings = new PetBattleSettings(filename);


            }
            catch (Exception ex) { BBLog(ex.ToString()); }
            PetSettings.Logic = ConvertFromOldFile(PetSettings.Logic);
            PetSettings.Save();
        }
        public void LoadPetSettingsBN(string petID)
        {

            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + petID + ".xml";


            try
            {
                PetSettings = new PetBattleSettings(filename);


            }
            catch (Exception ex) { BBLog(ex.ToString()); }
            PetSettings.Logic = ConvertFromOldFile(PetSettings.Logic);
            PetSettings.Save();
        }

        public static void LoadDefaultLogic(string filez)
        {

            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\" + filez + ".xml";


            try
            {
                DefaultLogicz = new PetBattleSettings(filename);


            }
            catch (Exception ex) { }


        }
        public bool ActualCalc(string s)
        {
            if (s == "" || s == null) return false;
            var ce = new CalcEngine.CalcEngine();

            var x = ce.Parse(s);
            //Logging.Write("Evaluating");
            var value = (bool)x.Evaluate();
            return value;


        }
        public bool Calc(string s)
        {

            char[] delimit = new char[] { '&' };
            string s10 = s;
            foreach (string substr in s10.Split(delimit))
            {
                if (!ActualCalc(substr)) return false;
            }
            //BBLog("returning true");
            return true;


        }



        public void BBLog(string msg)
        {
            Color clr = Color.Red;
            System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(clr.A, clr.R, clr.G, clr.B);
            Logging.Write(newColor, "[PB] " + msg);

        }


        public void WantSwapping()
        {
            int slot1rating = 0;
            int slot2rating = 0;
            int slot3rating = 0;
            BBLog("Forced swap");
            slot1rating = BattleRating(GetPetLevel(1), GetPetHealth(1), ReadSlot(1), GetCurrentEnemyType(), GetCurrentEnemyLevel());

            slot2rating = BattleRating(GetPetLevel(2), GetPetHealth(2), ReadSlot(2), GetCurrentEnemyType(), GetCurrentEnemyLevel());
            slot3rating = BattleRating(GetPetLevel(3), GetPetHealth(3), ReadSlot(3), GetCurrentEnemyType(), GetCurrentEnemyLevel());

            if (!CanSelect(1)) slot1rating = slot1rating - 100000;
            if (!CanSelect(2)) slot2rating = slot2rating - 100000;
            if (!CanSelect(3)) slot3rating = slot3rating - 100000;


            BBLog("Pet Ratings - Slot 1 : " + slot1rating + " " + "Slot 2 : " + slot2rating + " " + "Slot 3 : " + slot3rating);
            if (slot1rating < slot2rating || slot1rating < slot3rating)
            {
                //swap pet 
                BBLog("Swapping something");
                if (slot2rating >= slot3rating) CombatCallPet(2);
                if (slot2rating < slot3rating) CombatCallPet(3);
                //Thread.Sleep(1000);

            }
            else
            {
                CombatCallPet(1);
            }
        }

        public void ForcedSwapping()
        {
            int slot1rating = 0;
            int slot2rating = 0;
            int slot3rating = 0;

            if (CanSelect(1)) slot1rating = BattleRating(GetPetLevel(1), GetPetHealth(1), ReadSlot(1), GetCurrentEnemyType(), GetCurrentEnemyLevel());

            if (CanSelect(2)) slot2rating = BattleRating(GetPetLevel(2), GetPetHealth(2), ReadSlot(2), GetCurrentEnemyType(), GetCurrentEnemyLevel());
            if (CanSelect(3)) slot3rating = BattleRating(GetPetLevel(3), GetPetHealth(3), ReadSlot(3), GetCurrentEnemyType(), GetCurrentEnemyLevel());

            if (!CanSelect(1)) slot1rating = slot1rating - 100000;
            if (!CanSelect(2)) slot2rating = slot2rating - 100000;
            if (!CanSelect(3)) slot3rating = slot3rating - 100000;
            BBLog("Pet Ratings - Slot 1 : " + slot1rating + " " + "Slot 2 : " + slot2rating + " " + "Slot 3 : " + slot3rating);
            if (slot1rating < slot2rating || slot1rating < slot3rating)
            {
                //swap pet 
                BBLog("Swapping something");
                if (slot2rating >= slot3rating) CombatCallPet(2);
                if (slot2rating < slot3rating) CombatCallPet(3);
                //Thread.Sleep(1000);

            }
            else
            {
                CombatCallPet(1);
            }
        }

        //rating=rating* (GetPetHealth(
        public void PetSwappingPreCombat()
        {
            if (Pokehbuddy.MySettings.DetailedLogging) BBLog("Entered Pet Swapping function");
            // (Health% * healthfactor) * 10 + (Advantage(1 or 2) * 100 * advantagefactor) * 10 + (Disadvantage(-1 or -2) *100 * disadvantagefactor)*10 + (leveldifference * 4 * levelfactor) *10

            int tartype = GetTypeByTarget();
            int tarlevel = GetLevelByTarget();
            int slot1rating = BattleRating(GetPetLevelPreCombat(1), GetPetHPPreCombat(1), ReadSlot(1), tartype, tarlevel);
            int slot2rating = BattleRating(GetPetLevelPreCombat(2), GetPetHPPreCombat(2), ReadSlot(2), tartype, tarlevel);
            int slot3rating = BattleRating(GetPetLevelPreCombat(3), GetPetHPPreCombat(3), ReadSlot(3), tartype, tarlevel);
            BBLog("Pet Ratings - Slot 1 : " + slot1rating + " " + "Slot 2 : " + slot2rating + " " + "Slot 3 : " + slot3rating);

            if (slot1rating < slot2rating || slot1rating < slot3rating)
            {
                //swap pet 
                BBLog("Swapping something");
                if (slot2rating >= slot3rating) SetSlot(1, 2);
                if (slot2rating < slot3rating) SetSlot(1, 3);
                //Thread.Sleep(1000);

            }
            else
            {
                BBLog("No swap needed");
            }
        }
        public bool IsBattlePet(WoWUnit tar)
        {
            if (tar.CreatureType.ToString() == "14") return true;
            return false;
        }
        public int GetTypeByTarget()
        {
            List<string> cnt = Lua.GetReturnValues("local dummy=UnitBattlePetType('target') if dummy==nil then dummy=0 end return dummy");
            int getal = 0;
            try
            {
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }

            return getal;
        }

        public int GetLevelByTarget()
        {
            List<string> cnt = Lua.GetReturnValues("local dummy=UnitBattlePetLevel('target') if dummy==nil then dummy=0 end return dummy");
            int getal = 0;
            try
            {
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }
            return getal;
        }


        public void SetSlot(int slot, int fromslot)
        {
            Lua.DoString("local petID= C_PetJournal.GetPetLoadOutInfo(" + fromslot + ") C_PetJournal.SetPetLoadOutInfo(" + slot + ", petID)");

        }

        public void CombatCallPet(int petnum)
        {
            Lua.DoString("if C_PetBattles.GetActivePet(1) ~= " + petnum + " then C_PetBattles.ChangePet(" + petnum + ") end");
        }

        public int GetCurrentPetHealth()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetHealth(1,C_PetBattles.GetActivePet(1))");
            try
            {
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }
            int getal2 = 0;
            List<string> cnt2 = Lua.GetReturnValues("return C_PetBattles.GetMaxHealth(1,C_PetBattles.GetActivePet(1))");
            try
            {
                getal2 = Convert.ToInt32(cnt2[0]);
            }
            catch (Exception exc)
            {

            }




            getal = getal * 100;
            getal = getal / getal2;
            return getal;
        }


        public static int GetPetHPPreCombat(int petnum)
        {

            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("local petID= C_PetJournal.GetPetLoadOutInfo(" + petnum + ") local health, maxHealth, attack, speed, rarity = C_PetJournal.GetPetStats(petID) local dummy = (health / maxHealth) * 100 return dummy");
            try
            {
                cnt[0].Replace(",", ".");
                int i = cnt[0].IndexOf('.');

                // Remainder of string starting at 'c'.
                if (i > -1) cnt[0] = cnt[0].Substring(0, i);
                //BBLog("Lua received HP :"+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }
        public static int GetPetHealth(int petnum)
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetHealth(1," + petnum + ")");
            try
            {
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }
            int getal2 = 0;
            List<string> cnt2 = Lua.GetReturnValues("return C_PetBattles.GetMaxHealth(1," + petnum + ")");
            try
            {
                getal2 = Convert.ToInt32(cnt2[0]);
            }
            catch (Exception exc)
            {

            }




            getal = getal * 100;
            getal = getal / getal2;
            return getal;
        }

        public int GetEnHealth()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("local dummy=0 if (C_PetBattles.GetActivePet(2) > 0) then dummy= C_PetBattles.GetHealth(2,C_PetBattles.GetActivePet(2)) end return dummy");
            try
            {
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }
            int getal2 = 0;
            List<string> cnt2 = Lua.GetReturnValues("local dummy=0 if (C_PetBattles.GetActivePet(2) > 0) then dummy= C_PetBattles.GetMaxHealth(2,C_PetBattles.GetActivePet(2)) end return dummy");
            try
            {
                getal2 = Convert.ToInt32(cnt2[0]);
            }
            catch (Exception exc)
            {

            }




            getal = getal * 100;
            getal = getal / getal2;
            return getal;
        }


        public string GetCurrentEnemyName()
        {

            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetName(2,C_PetBattles.GetActivePet(2))");

            return cnt[0];
        }
        //C_PetBattles.GetPetType
        public int GetCurrentEnemyType()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetPetType(2,C_PetBattles.GetActivePet(2))");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }
        public string GetCurrentEnemySpecies()
        {

            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetPetSpeciesID(2,C_PetBattles.GetActivePet(2))");
            return cnt[0];
        }




        public int GetCurrentSpeed()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("local dummy = C_PetBattles.GetSpeed(1,C_PetBattles.GetActivePet(1)) if dummy > 0 then return dummy end return 0");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }


        public int GetCurrentEnemySpeed()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("local dummy = C_PetBattles.GetSpeed(2,C_PetBattles.GetActivePet(2)) if dummy > 0 then return dummy end return 0");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }



        public int GetCurrentEnemyLevel()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetLevel(2,C_PetBattles.GetActivePet(2))");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }
        public int GetPetLevel(int petnum)
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetLevel(1," + petnum + ")");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }

        public int GetCurrentPetLevel()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetLevel(1,C_PetBattles.GetActivePet(1))");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }
        public int GetCurrentEnemyPetLevel()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetLevel(2,C_PetBattles.GetActivePet(2))");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }


        public static int GetPetLevelPreCombat(int petnum)
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("local petID= C_PetJournal.GetPetLoadOutInfo(" + petnum + ") local speciesID, customName, level, xp, maxXp, displayID, petName, petIcon, petType, creatureID = C_PetJournal.GetPetInfoByPetID(petID) return level;");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }





        public int GetCurrentEnemyHealth()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetHealth(2,C_PetBattles.GetActivePet(2))");
            try
            {
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }
        public int GetCurrentEnemyMaxHealth()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetMaxHealth(2,C_PetBattles.GetActivePet(2))");
            try
            {
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }


        public int GetRarity()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetBreedQuality(2,C_PetBattles.GetActivePet(2))");
            try
            {
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }
        public int GetRarityByNum(int petnum)
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.GetBreedQuality(2," + petnum + ")");
            try
            {
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }
        public void SetPetAbilities()
        {
            //For each pet
            for (int i = 1; i < 4; i++)
            {
                //if (MySettings.DetailedLogging) BBLog("Checking for custom abilities for pet slot: " + i + " ID:" + ReadSlot(i));
                LoadPetSettings(ReadSlot(i), ReadSlotSpecies(i));
                if (PetSettings.SpellLayout == "") PetSettings.SpellLayout = "ASSIGNABILITY1(0)@ASSIGNABILITY2(0)@ASSIGNABILITY3(0)";
                //parse settings looking for AssignAbility1, AssignAbility2, AssignAbility3
                string dumdum = PetSettings.SpellLayout;

                string[] PetLogics = dumdum.Split('@');

                foreach (string alogic in PetLogics)
                {
                    if ((String.Compare(alogic.Substring(0, 15), "ASSIGNABILITY1(") == 0))
                    {
                        int FirstChr = alogic.IndexOf("ASSIGNABILITY1(") + 15;
                        int SecondChr = alogic.IndexOf(")", FirstChr);
                        string strTemp = alogic.Substring(FirstChr, SecondChr - FirstChr);

                        if (strTemp.ToInt32() > 0) SetAbility(i, 1, strTemp.ToInt32());
                    }
                    if ((String.Compare(alogic.Substring(0, 15), "ASSIGNABILITY2(") == 0))
                    {
                        int FirstChr = alogic.IndexOf("ASSIGNABILITY2(") + 15;
                        int SecondChr = alogic.IndexOf(")", FirstChr);
                        string strTemp = alogic.Substring(FirstChr, SecondChr - FirstChr);

                        if (strTemp.ToInt32() > 0) SetAbility(i, 2, strTemp.ToInt32());
                    }
                    if ((String.Compare(alogic.Substring(0, 15), "ASSIGNABILITY3(") == 0))
                    {
                        int FirstChr = alogic.IndexOf("ASSIGNABILITY3(") + 15;
                        int SecondChr = alogic.IndexOf(")", FirstChr);
                        string strTemp = alogic.Substring(FirstChr, SecondChr - FirstChr);

                        if (strTemp.ToInt32() > 0) SetAbility(i, 3, strTemp.ToInt32());
                    }
                }
            }
        }

        public void SetAbility(int petSlot, int abilitySlot, int spellID)
        {
            if (Pokehbuddy.MySettings.DetailedLogging)
                if (MySettings.DetailedLogging) BBLog("Setting custom ability Pet slot: " + petSlot + " Ability slot:" + abilitySlot + " Spell ID:" + spellID + ")");


            Lua.DoString("petGUID = C_PetJournal.GetPetLoadOutInfo(" + petSlot + ") speciesID, customName, level = C_PetJournal.GetPetInfoByPetID(petGUID) if (" + abilitySlot + " == 1 and level > 9) then C_PetJournal.SetAbility(" + petSlot + "," + abilitySlot + "," + spellID + ") end if (" + abilitySlot + " == 2 and level > 14) then C_PetJournal.SetAbility(" + petSlot + "," + abilitySlot + "," + spellID + ") end if (" + abilitySlot + " == 3 and level > 19) then C_PetJournal.SetAbility(" + petSlot + "," + abilitySlot + "," + spellID + ") end");
        }

        public bool ParseLogic(string theLogic)
        {
            //if (String.Compare(theLogic.Substring(0, 13), "ASSIGNABILITY") == 0) return false;

            //private string PetDefaultLogics[] = {"SWAPOUT Health(THISPET) ISLESSTHAN 50","CASTSPELL(1) COOLDOWN(SKILL(1)) EQUALS false",""};
            theLogic = theLogic.Replace("ISLESSTHAN", "<");
            theLogic = theLogic.Replace("ISGREATERTHAN", ">");
            theLogic = theLogic.Replace("EQUALS", "=");
            theLogic = theLogic.Replace("ISNOT", "<>");

            theLogic = theLogic.Replace("$", "&");
            string oldlogic = theLogic;


            theLogic = theLogic.Replace("SWAPOUT", "");
            theLogic = theLogic.Replace("FORFEIT", "");
            theLogic = theLogic.Replace("CASTSPELL(1)", "");
            theLogic = theLogic.Replace("CASTSPELL(2)", "");
            theLogic = theLogic.Replace("CASTSPELL(3)", "");
            theLogic = theLogic.Replace("PASSTURN", "");






            theLogic = theLogic.Replace("COOLDOWN(SKILL(1))", (!PetCanCast(1)).ToString());
            theLogic = theLogic.Replace("COOLDOWN(SKILL(2))", (!PetCanCast(2)).ToString());
            theLogic = theLogic.Replace("COOLDOWN(SKILL(3))", (!PetCanCast(3)).ToString());
            //Logging.Write("Ok so far!!!!!!!!!");
            theLogic = theLogic.Replace("Health(THISPET)", (GetCurrentPetHealth()).ToString());
            theLogic = theLogic.Replace("Health(ENEMYPET)", (GetEnHealth()).ToString());

            theLogic = theLogic.Replace("MyPetLevel", (GetCurrentPetLevel()).ToString());
            theLogic = theLogic.Replace("EnemyPetLevel", (GetCurrentEnemyPetLevel()).ToString());

            theLogic = theLogic.Replace("MyPetsAlive", (GetPetsAlive()).ToString());
            theLogic = theLogic.Replace("EnemyPetsAlive", (GetEnemyPetsAlive()).ToString());





            theLogic = theLogic.Replace("ENEMYSPEED", (GetCurrentEnemySpeed()).ToString());
            theLogic = theLogic.Replace("MYPETSPEED", (GetCurrentSpeed()).ToString());

            //



            theLogic = theLogic.Replace("ENEMYTYPE", (GetCurrentEnemyType()).ToString());
            theLogic = theLogic.Replace("HUMANOID", "1");
            theLogic = theLogic.Replace("DRAGONKIN", "2");
            theLogic = theLogic.Replace("FLYING", "3");
            theLogic = theLogic.Replace("UNDEAD", "4");
            theLogic = theLogic.Replace("CRITTER", "5");
            theLogic = theLogic.Replace("MAGIC", "6");
            theLogic = theLogic.Replace("ELEMENTAL", "7");
            theLogic = theLogic.Replace("BEAST", "8");
            theLogic = theLogic.Replace("AQUATIC", "9");
            theLogic = theLogic.Replace("MECHANICAL", "10");

            //id = ["hasbuff", "wheatherbuff", ...]
            //action = [checkforbuff, checkwheaterbuff,...]
            //action[id.indexof("hasbuff)]();

            while (theLogic.IndexOf("HASBUFF(") > -1)
            {
                int FirstChr = theLogic.IndexOf("HASBUFF(") + 8;
                int SecondChr = theLogic.IndexOf(")", FirstChr);
                string dumdumdum = theLogic.Substring(FirstChr, SecondChr - FirstChr);
                //BBLog(dumdumdum+" "+FirstChr+" "+SecondChr+""+CheckForBuff(dumdumdum));
                theLogic = theLogic.Replace("HASBUFF(" + dumdumdum + ")", (CheckForBuff(dumdumdum)).ToString());
            }
            while (theLogic.IndexOf("WEATHERBUFF(") > -1)
            {
                int FirstChr = theLogic.IndexOf("WEATHERBUFF(") + 12;
                int SecondChr = theLogic.IndexOf(")", FirstChr);
                string dumdumdum = theLogic.Substring(FirstChr, SecondChr - FirstChr);
                //BBLog(dumdumdum+" "+FirstChr+" "+SecondChr+""+CheckForBuff(dumdumdum));
                theLogic = theLogic.Replace("WEATHERBUFF(" + dumdumdum + ")", (CheckWeatherBuff(dumdumdum)).ToString());
            }
            while (theLogic.IndexOf("HASENEMYBUFF(") > -1)
            {
                int FirstChr = theLogic.IndexOf("HASENEMYBUFF(") + 13;
                int SecondChr = theLogic.IndexOf(")", FirstChr);
                string dumdumdum = theLogic.Substring(FirstChr, SecondChr - FirstChr);
                //BBLog(dumdumdum+" "+FirstChr+" "+SecondChr+""+CheckForBuff(dumdumdum));
                theLogic = theLogic.Replace("HASENEMYBUFF(" + dumdumdum + ")", (CheckEnemyForBuff(dumdumdum)).ToString());
            }
            while (theLogic.IndexOf("HASTEAMBUFF(") > -1)
            {
                int FirstChr = theLogic.IndexOf("HASTEAMBUFF(") + 12;
                int SecondChr = theLogic.IndexOf(")", FirstChr);
                string dumdumdum = theLogic.Substring(FirstChr, SecondChr - FirstChr);
                //BBLog(dumdumdum+" "+FirstChr+" "+SecondChr+""+CheckForBuff(dumdumdum));
                theLogic = theLogic.Replace("HASTEAMBUFF(" + dumdumdum + ")", (CheckTeamBuff(dumdumdum, 1)).ToString());
            } //HASTEAMBUFF ENEMYTEAMBUFF
            while (theLogic.IndexOf("ENEMYTEAMBUFF(") > -1)
            {
                int FirstChr = theLogic.IndexOf("ENEMYTEAMBUFF(") + 14;
                int SecondChr = theLogic.IndexOf(")", FirstChr);
                string dumdumdum = theLogic.Substring(FirstChr, SecondChr - FirstChr);
                //BBLog(dumdumdum+" "+FirstChr+" "+SecondChr+""+CheckForBuff(dumdumdum));
                theLogic = theLogic.Replace("ENEMYTEAMBUFF(" + dumdumdum + ")", (CheckTeamBuff(dumdumdum, 2)).ToString());
            }

            bool dumdum = Calc(theLogic);
            if (Pokehbuddy.MySettings.DetailedLogging)
            {
                BBLog("----------------------------------------------------------------");
                BBLog("Logic " + oldlogic);
                BBLog(theLogic + " //// returned " + dumdum);
            }

            return dumdum;
        }

        public int BattleRating(int petLevel, int petHP, string petID, int enemytype, int enemylevel)
        {
            int advantage = 0;
            int disadvantage = 0;
            // (petHP * Pokehbuddy.MySettings.HPFactor) * 10 + (advantage * 100 * Pokehbuddy.MySettings.AdFactor) * 10 + (disadvantage *100 * Pokehbuddy.MySettings.DisFactor)*10 + ((petLevel - enemylevel) * 4 * Pokehbuddy.MySettings.LevelFactor) *10
            //Pokehbuddy.MySettings.HPFactor
            //Logging.Write("Pet ID : " + petID);
            int mypet = GetTypeByID(petID);
            //Logging.Write("Pet Type : " + mypet);
            int rating = 0;
            //Logging.Write("target type " + enemytype);
            if (mypet == DumbChoiceTakeMoreDMG(enemytype)) disadvantage = -2;
            if (mypet == DumbChoiceDealLessDMG(enemytype)) disadvantage = disadvantage - 1;//rating -1;
            if (mypet == SmartChoiceTakeLessDMG(enemytype)) advantage = 1;
            if (mypet == SmartChoiceDealMoreDMG(enemytype)) advantage = advantage + 2;

            /*****************************************/
            var ce = new CalcEngine.CalcEngine();
            string s = "1 + 1 * 3";
            var x = ce.Parse(s);

            var value = x.Evaluate();
            var total = 0;

            //pet 1


            s = MySettings.HPFormula;
            s = s.Replace("petHP", petHP.ToString()).Replace("HPFactor", Pokehbuddy.MySettings.HPFactor.ToString());
            x = ce.Parse(s);
            var HPresult = x.Evaluate();
            total = int.Parse(HPresult.ToString());



            s = MySettings.AdFormula;
            s = s.Replace("advantage", advantage.ToString()).Replace("AdFactor", MySettings.AdFactor.ToString());
            x = ce.Parse(s);
            var Adresult = x.Evaluate();
            total = total + int.Parse(Adresult.ToString());


            s = MySettings.DisFormula;
            s = s.Replace("disadvantage", disadvantage.ToString()).Replace("DisFactor", MySettings.DisFactor.ToString());
            x = ce.Parse(s);
            var Disresult = x.Evaluate();
            total = total + int.Parse(Disresult.ToString());

            s = MySettings.LevelFormula;
            s = s.Replace("petLevel", petLevel.ToString()).Replace("enemylevel", enemylevel.ToString()).Replace("LevelFactor", MySettings.LevelFactor.ToString());
            x = ce.Parse(s);
            var Levelresult = x.Evaluate();
            total = total + int.Parse(Levelresult.ToString());


            /***************************************/

            //do more rating stuff with health percentages, level difference
            rating = total; /*(petHP * Pokehbuddy.MySettings.HPFactor) +
                     (advantage * 50 * Pokehbuddy.MySettings.AdFactor) +
                     (disadvantage * 50 * Pokehbuddy.MySettings.DisFactor) +
                     ((petLevel - enemylevel) * 4 * Pokehbuddy.MySettings.LevelFactor);*/
            int oldrating = rating;
            if (petHP < 30) rating = rating - 10000;
            if (petHP < 15) rating = rating - 40000;
            if (petHP < 5) rating = rating - 50000;
            if (!CanSummon(petID)) rating = -100000000;
            //BBLog("" + petHP+ " "+ petLevel + " "+ enemylevel );
            if (Pokehbuddy.MySettings.DetailedLogging)
            {
                BBLog("~~~~~~~~~~~~~~~~~~~~~Rating info~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                BBLog("HP            " + petHP.ToString("000") + " x " + Pokehbuddy.MySettings.HPFactor + " = " + HPresult.ToString());
                BBLog("Advantage     " + advantage.ToString("000") + " x " + Pokehbuddy.MySettings.AdFactor + " = " + Adresult.ToString());
                BBLog("Disadvantage  " + disadvantage.ToString("000") + " x " + Pokehbuddy.MySettings.DisFactor + " = " + Disresult.ToString());
                BBLog("Level  " + (petLevel - enemylevel).ToString("00") + " x 4 x " + Pokehbuddy.MySettings.LevelFactor + " = " + Levelresult.ToString());
                BBLog("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                BBLog("Total Rating : " + oldrating + "      ");
                BBLog("Final Rating : " + rating + "    (after extreme low HP penalty)  ");
                BBLog("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }
            return rating;
        }
        public bool InPetCombat()
        {
            List<string> cnt = Lua.GetReturnValues("dummy,reason=C_PetBattles.IsTrapAvailable() return dummy,reason");

            if (cnt != null) { if (cnt[1] != "0") return true; }
            return false;
        }


        public bool CanSelect(int petnum)
        {
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.CanPetSwapIn(" + petnum + ")");

            if (cnt != null) { if (cnt[0] == "1") return true; }
            return false;
        }
        public bool MustSelectNew()
        {

            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.ShouldShowPetSelect()");
            //Lua.DoString("print(C_PetBattles.ShouldShowPetSelect())");

            if (cnt != null) { if (cnt[0] == "1") return true; }
            return false;
        }


        public bool CanTrap()
        {
            List<string> cnt = Lua.GetReturnValues("dummy,reason=C_PetBattles.IsTrapAvailable() return dummy,reason");

            if (cnt != null) { if (cnt[0] == "1") return true; }
            return false;
        }


        public bool CanSummon(string petID)
        {
            List<string> cnt = Lua.GetReturnValues("return C_PetJournal.PetIsSummonable(" + petID + ");");

            if (cnt != null) { if (cnt[0] == "1") return true; }
            return false;
        }
        //local petID= C_PetJournal.GetPetLoadOutInfo(2) return C_PetJournal.PetIsSummonable(petID);




        public bool PetCanCast(int skillnum)
        {
            List<string> cnt = Lua.GetReturnValues("local isUsable, currentCooldown = C_PetBattles.GetAbilityState(LE_BATTLE_PET_ALLY, C_PetBattles.GetActivePet(LE_BATTLE_PET_ALLY), " + skillnum + "); if isUsable == nil then isUsable=0 end return isUsable,currentCooldown");
            //		Logging.Write("skill"+skillnum+" returned "+cnt[0]);
            if (cnt != null) { if (cnt[0] == "1") return true; }
            return false;
        }

        public bool CanFight()
        {
            List<string> cnt = Lua.GetReturnValues("return C_PetBattles.IsWaitingOnOpponent()");

            if (cnt != null) { if (cnt[0] == "0") return true; }
            return false;
        }


        public static string ReadSlot(int slotnr)
        {
            List<string> cnt = Lua.GetReturnValues("dummy={} for i = 1, 3   do  local petID= C_PetJournal.GetPetLoadOutInfo(i); dummy[i]=petID  end return tonumber(dummy[1],16),tonumber(dummy[2],16),tonumber(dummy[3],16);");
            //Logging.Write("Slot has ID : " + cnt[slotnr - 1]);
            return cnt[slotnr - 1];

        }
        public string ReadActiveSlot()
        {
            List<string> cnt = Lua.GetReturnValues("dummy={} for i = 1, 3   do  local petID= C_PetJournal.GetPetLoadOutInfo(i); dummy[i]=petID  end return tonumber(dummy[C_PetBattles.GetActivePet(1)],16);");
            //Logging.Write(cnt[0]);
            //int decAgain = int.Parse(, System.Globalization.NumberStyles.HexNumber);
            return cnt[0];//decAgain.ToString();

        }
        public string ReadActiveSlotSpecies()
        {
            List<string> cnt = Lua.GetReturnValues("local petID = C_PetJournal.GetPetLoadOutInfo(C_PetBattles.GetActivePet(1)); local speciesID = C_PetJournal.GetPetInfoByPetID(petID) return speciesID");
            //Logging.Write(cnt[0]);
            //int decAgain = int.Parse(, System.Globalization.NumberStyles.HexNumber);
            //return cnt[0];//decAgain.ToString();
            for (int i = 0; i < allpetz.Length; i++)
            {
                if (allpetz[i] == cnt[0]) return allpetz[i + 1];
            }
            return "";

        }

        public static string ReadSlotSpecies(int slotnum)
        {
            List<string> cnt = Lua.GetReturnValues("local petID = C_PetJournal.GetPetLoadOutInfo(" + slotnum + "); local speciesID = C_PetJournal.GetPetInfoByPetID(petID) return speciesID");
            //Logging.Write(cnt[0]);
            //int decAgain = int.Parse(, System.Globalization.NumberStyles.HexNumber);
            //return cnt[0];//decAgain.ToString();
            for (int i = 0; i < allpetz.Length; i++)
            {
                if (allpetz[i] == cnt[0]) return allpetz[i + 1];
            }
            return "";

        }


        public string ActiveSlotIcon()
        {
            List<string> cnt = Lua.GetReturnValues("local petID = C_PetJournal.GetPetLoadOutInfo(C_PetBattles.GetActivePet(1)); local speciesID = C_PetJournal.GetPetInfoByPetID(petID) return speciesID");
            //Logging.Write(cnt[0]);
            //int decAgain = int.Parse(, System.Globalization.NumberStyles.HexNumber);
            //return cnt[0];//decAgain.ToString();
            for (int i = 0; i < allpetz.Length; i++)
            {
                if (allpetz[i] == cnt[0]) return allpetz[i + 2];
            }
            return "";

        }

        public static string SlotIcon(int slotnr)
        {
            List<string> cnt = Lua.GetReturnValues("local petID = C_PetJournal.GetPetLoadOutInfo(" + slotnr + "); local speciesID = C_PetJournal.GetPetInfoByPetID(petID) return speciesID");
            //Logging.Write(cnt[0]);
            //int decAgain = int.Parse(, System.Globalization.NumberStyles.HexNumber);
            //return cnt[0];//decAgain.ToString();
            for (int i = 0; i < allpetz.Length; i++)
            {
                if (allpetz[i] == cnt[0]) return allpetz[i + 2];
            }
            return "";

        }



        public static int SmartChoiceTakeLessDMG(int enemytype)
        {
            if (enemytype > 10 || enemytype < 0) enemytype = 1;
            int[] smart = new int[] { 0, 8, 4, 2, 9, 1, 10, 5, 3, 6, 7 };
            return smart[enemytype];
        }
        //return {0,8,4,2,9,1,10,5,3,6,7}[enemytype]
        public static int SmartChoiceDealMoreDMG(int enemytype)
        {
            if (enemytype > 10 || enemytype < 0) enemytype = 1;
            int[] smart = new int[] { 0, 4, 1, 6, 5, 8, 2, 9, 10, 3, 7 };
            return smart[enemytype];
        }
        public static int DumbChoiceDealLessDMG(int enemytype)
        {
            if (enemytype > 10 || enemytype < 0) enemytype = 1;
            int[] smart = new int[] { 0, 5, 3, 8, 2, 7, 9, 10, 1, 4, 6 };
            return smart[enemytype];
        }
        public static int DumbChoiceTakeMoreDMG(int enemytype)
        {
            if (enemytype > 10 || enemytype < 0) enemytype = 1;
            int[] smart = new int[] { 0, 2, 6, 9, 1, 4, 3, 10, 5, 7, 8 };
            return smart[enemytype];
        }

        //for i=1, C_PetBattles.GetNumAuras(0,0) do local auraID = C_PetBattles.GetAuraInfo(LE_BATTLE_PET_WEATHER, PET_BATTLE_PAD_INDEX, i) if (auraID == 596) then return true end end return false
        //
        public bool CheckWeatherBuff(string buffnum)
        {

            List<string> cnt = Lua.GetReturnValues("for i=1, C_PetBattles.GetNumAuras(0,0) do local auraID = C_PetBattles.GetAuraInfo(LE_BATTLE_PET_WEATHER, PET_BATTLE_PAD_INDEX, i) if (auraID == " + buffnum + ") then return true end end return false");

            if (cnt[0] == "1") return true;
            return false;
        }

        public bool CheckTeamBuff(string buffnum, int teamnum)
        {

            List<string> cnt = Lua.GetReturnValues("for i=1, C_PetBattles.GetNumAuras(" + teamnum + ",0) do local auraID = C_PetBattles.GetAuraInfo(" + teamnum + ", PET_BATTLE_PAD_INDEX, i) if (auraID == " + buffnum + ") then return true end end return false");

            if (cnt != null) { if (cnt[0] == "1") return true; }
            return false;
        }

        ///for i=1, C_PetBattles.GetNumAuras("+teamnum+",0) do local auraID = C_PetBattles.GetAuraInfo("+teamnum+", PET_BATTLE_PAD_INDEX, i) if (auraID == "+buffnum+") then return true end end return false
        public bool CheckForBuff(string buffnum)
        {

            List<string> cnt = Lua.GetReturnValues("for j=1,C_PetBattles.GetNumAuras(1,C_PetBattles.GetActivePet(1)) do  local buffid = C_PetBattles.GetAuraInfo(1,C_PetBattles.GetActivePet(1),j)  if buffid == " + buffnum + " then return (true) end end return( false) ");

            if (cnt != null) { if (cnt[0] == "1") return true; }
            return false;
        }

        public bool CheckEnemyForBuff(string buffnum)
        {

            List<string> cnt = Lua.GetReturnValues("for j=1,C_PetBattles.GetNumAuras(2,C_PetBattles.GetActivePet(2)) do  local buffid = C_PetBattles.GetAuraInfo(2,C_PetBattles.GetActivePet(2),j)  if buffid == " + buffnum + " then return (true) end end return( false) ");

            if (cnt != null) { if (cnt[0] == "1") return true; }
            return false;
        }




        public int GetTypeByName(string petname)
        {
            Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
            Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
            ////Lua.DoString("C_PetJournal.SetSearchFilter('" + petname + "')");
            petname = GetSpeciesByName(petname);
            List<string> cnt = Lua.GetReturnValues("local dummy=-1 for i=1,C_PetJournal.GetNumPets(false) do     local petID, speciesID, isOwned, customName, level, favorite, isRevoked, name, icon, petType  = C_PetJournal.GetPetInfoByIndex(i, false)  if speciesID == " + petname + " then dummy=petType ; end end return dummy;");

            if (cnt[0] != null && cnt[0] != "-1")
            {

                int numValue = -1;
                try
                {
                    numValue = Convert.ToInt32(cnt[0]);
                }
                catch (Exception exc)
                {

                }






                return numValue;

            }
            return -1;

        }
        public int GetRarityBySpeciesID(string speciesid)
        {
            Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
            Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
            ////Lua.DoString("C_PetJournal.SetSearchFilter('" + petname + "')");

            string lustring = "local dummy=-1 for i=1,C_PetJournal.GetNumPets(false) do     local petID, speciesID, isOwned, customName, level, favorite, isRevoked, name, icon, petType  = C_PetJournal.GetPetInfoByIndex(i, false)  if isOwned then local _, _, _, _, rarity = C_PetJournal.GetPetStats(petID) if (speciesID == " + speciesid + ")then if (rarity > dummy) then dummy=rarity ; end end end end return dummy;";

            List<string> cnt = Lua.GetReturnValues(lustring);


            if (cnt[0] != null && cnt[0] != "-1")
            {

                int numValue = -1;
                try
                {
                    numValue = Convert.ToInt32(cnt[0]);
                }
                catch (Exception exc)
                {

                }






                return numValue;

            }
            return -1;

        }



        public int GetRarityByName(string petname)
        {
            Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
            Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
            ////Lua.DoString("C_PetJournal.SetSearchFilter('" + petname + "')");
            petname = GetSpeciesByName(petname);
            string lustring = "local dummy=-1 for i=1,C_PetJournal.GetNumPets(false) do     local petID, speciesID, isOwned, customName, level, favorite, isRevoked, name, icon, petType  = C_PetJournal.GetPetInfoByIndex(i, false)  if isOwned then local _, _, _, _, rarity = C_PetJournal.GetPetStats(petID) if (speciesID == " + petname + ")then if (rarity > dummy) then dummy=rarity ; end end end end return dummy;";

            List<string> cnt = Lua.GetReturnValues(lustring);


            if (cnt[0] != null && cnt[0] != "-1")
            {

                int numValue = -1;
                try
                {
                    numValue = Convert.ToInt32(cnt[0]);
                }
                catch (Exception exc)
                {

                }






                return numValue;

            }
            return -1;

        }

        public static int GetTypeByID(string thepetID)
        {
            Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
            Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
            ////Lua.DoString("C_PetJournal.SetSearchFilter('" + petname + "')");

            List<string> cnt = Lua.GetReturnValues("local dummy=-1 for i=1,C_PetJournal.GetNumPets(false) do     local petID, speciesID, isOwned, customName, level, favorite, isRevoked, name, icon, petType  = C_PetJournal.GetPetInfoByIndex(i, false) if tonumber(petID,16) == " + thepetID + " then return petType end end  print('finished') return dummy;");
            //Logging.Write("Returned : " + cnt[0]);
            if (cnt != null)
            {
                if (cnt[0] != null && cnt[0] != "-1")
                {


                    int numValue = -1;
                    try
                    {
                        numValue = Convert.ToInt32(cnt[0]);
                    }
                    catch (Exception exc)
                    {

                    }


                    return numValue;

                }
            }
            return -1;
        }
        public string GetNameByID(string thepetID)
        {
            Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
            Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
            ////Lua.DoString("C_PetJournal.SetSearchFilter('" + petname + "')");

            List<string> cnt = Lua.GetReturnValues("local dummy='' for i=1,C_PetJournal.GetNumPets(false) do     local petID, speciesID, isOwned, customName, level, favorite, isRevoked, name, icon, petType  = C_PetJournal.GetPetInfoByIndex(i, false)  if tonumber(petID,16) == " + thepetID + " then dummy=name ; end end return dummy;");

            if (cnt[0] != null && cnt[0] != "")
            {



                return cnt[0];
            }
            return "Error";
        }

        public string GetNameBySpeciesID(string speciesID)
        {

            for (int i = 0; i < allpetz.Length; i++)
            {
                if (allpetz[i] == speciesID) return allpetz[i + 1];

            }
            return "Error";
        }

        public string GetSpeciesByName(string speciesID)
        {

            for (int i = 0; i < allpetz.Length; i++)
            {
                if (allpetz[i] == speciesID) return allpetz[i - 1];

            }
            return "Error";
        }







        /*public bool HasPet(string petname, int rarity=0){
           bool dummy=false;
           Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, false) ");
           Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
           ////Lua.DoString("C_PetJournal.SetSearchFilter('" + petname + "')");
           petname=GetSpeciesByName(petname);
           List<string> cnt = Lua.GetReturnValues("local s = false;for i=1,C_PetJournal.GetNumPets(false) do     local petID, speciesID, isOwned, customName, level, favorite, isRevoked, name  = C_PetJournal.GetPetInfoByIndex(i, false)  local health, maxHealth, attack, speed, rarity = C_PetJournal.GetPetStats(petID);   if speciesID == '"+petname+"' and rarity > "+rarity+" then s = true ; end end return s;");




           // BBLog(cnt[0]);
           if (cnt[0]=="1") dummy=true;
           return dummy;
		
        }*/

        public static WoWUnit WildBattleTarget()
        {

            WoWUnit ret = (from unit in ObjectManager.GetObjectsOfType<WoWUnit>(true, true)
                           orderby unit.Distance ascending

                           where !Blacklist.Contains(unit.Guid)
                           //where unit.CreatureType.ToString() == "14"
                           where unit.IsPetBattleCritter
                           where !unit.IsDead
                           where (MySettings.UseWhiteList && thewhitelist.Contains(unit.Name.ToLower()) || !MySettings.UseWhiteList)
                           where (MySettings.UseBlackList && !theblacklist.Contains(unit.Name.ToLower()) || !MySettings.UseBlackList)




                           where unit.Distance < Pokehbuddy.MySettings.Distance
                           select unit).FirstOrDefault();
            //where unit.Distance < Pokehbuddy.MySettings.Distance 
            //if (ret != null)  BBLog("BB "+ret);
            if (ret != null)
            {
                ret.Target();
                //Logging.Write(""+GetWildLevel()+" "+(GetLowLevel() - (Pokehbuddy.MySettings.BelowLevel + 1))+" "+(GetHighLevel()+ Pokehbuddy.MySettings.AboveLevel+1) );
                if (Pokehbuddy.MySettings.DetailedLogging)
                {
                    //BBLog("-----********-----********-----********");
                    //BBLog("Wild battle target level " + GetWildLevel());
                    //BBLog("-----********-----********-----********");
                }
                int dumlevel = GetWildLevel();
                if (dumlevel > GetLowLevel() - (Pokehbuddy.MySettings.BelowLevel + 1) && dumlevel < GetHighLevel() + Pokehbuddy.MySettings.AboveLevel + 1)
                {
                    Logging.Write("Attacking " + ret.Guid);
                    return ret;
                }
                else
                {
                    if (dumlevel > 0)
                    {
                        if (MySettings.DetailedLogging) Logging.Write("Blacklisted target because level is " + dumlevel + ", the min is " + (GetLowLevel() - (Pokehbuddy.MySettings.BelowLevel + 1)) + " and the max is " + (GetHighLevel() + Pokehbuddy.MySettings.AboveLevel + 1));
                        Blacklist.Add(ret.Guid, TimeSpan.FromMinutes(1));
                    }
                }

                //Logging.Write(""+ret.Name+" range " + ret.Guid);


            }
            return null;
        }

        public static int GetLowLevel()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("local dummy = 99 for j=1,3 do local petID= C_PetJournal.GetPetLoadOutInfo(j) local speciesID, customName, level = C_PetJournal.GetPetInfoByPetID(petID) if level < dummy then dummy=level end end return dummy");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }
        public static int GetPetsAlive()
        {
            int getal = 0;

            List<string> cnt = Lua.GetReturnValues("dummy=0 for i = 1,C_PetBattles.GetNumPets(1)    do health, maxhealth = C_PetBattles.GetHealth(1, i) if health > 0 then dummy=dummy+1 end end return dummy");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }
        public static int GetEnemyPetsAlive()
        {
            int getal = 0;

            List<string> cnt = Lua.GetReturnValues("dummy=0 for i = 1,C_PetBattles.GetNumPets(2)    do health, maxhealth = C_PetBattles.GetHealth(2, i) if health > 0 then dummy=dummy+1 end end return dummy");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }





        public static int GetHighLevel()
        {
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("local dummy = 0 for j=1,3 do local petID= C_PetJournal.GetPetLoadOutInfo(j) local speciesID, customName, level = C_PetJournal.GetPetInfoByPetID(petID) if level > dummy then dummy=level end end return dummy");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }
        public static int GetWildLevel()
        {
            //Logging.Write(GUID);
            int getal = 0;
            List<string> cnt = Lua.GetReturnValues("return UnitBattlePetLevel('target')");
            try
            {
                //BBLog(""+cnt[0]);
                getal = Convert.ToInt32(cnt[0]);
            }
            catch (Exception exc)
            {

            }


            return getal;
        }







        public override string Name { get { return "Pokébuddy"; } }

        public override string Author { get { return "maybe"; } }

        public override Version Version { get { return new Version(1, 0, 0, 0); } }
        public override bool WantButton { get { return true; } }
    }

}



/***********************************************************\
*															*
*				Settings part!!								*
*															*
\***********************************************************/

namespace Pokehbuddyplug
{  // credits to Apoc for showing how to use the HB settings. & also credits from me, the guy that stole this from the AutoAngler Addon
    //DefaultLogic[0]={0,32000,""};


    public class BBSettings : Settings
    {

        public BBSettings(string settingsPath)
            : base(settingsPath)
        {
            Load();
        }

        [Setting, DefaultValue(1)]
        public int HPFactor { get; set; }

        [Setting, DefaultValue(1)]
        public int LevelFactor { get; set; }

        [Setting, DefaultValue(1)]
        public int AdFactor { get; set; }

        [Setting, DefaultValue(1)]
        public int DisFactor { get; set; }

        [Setting, DefaultValue(250)]
        public int Distance { get; set; }

        [Setting, DefaultValue(3)]
        public int GetRarity { get; set; }

        [Setting, DefaultValue(5)]
        public int BlacklistCounterLimit { get; set; }

        [Setting, DefaultValue(5)]
        public int SkipCounterLimit { get; set; }

        [Setting, DefaultValue(false)]
        public bool DoPVP { get; set; }

        [Setting, DefaultValue(5)]
        public int PVPMinTime { get; set; }

        [Setting, DefaultValue(10)]
        public int PVPMaxTime { get; set; }

        [Setting, DefaultValue(1)]
        public int MinPetsAlive { get; set; }

        [Setting, DefaultValue(3)]
        public int BelowLevel { get; set; }

        [Setting, DefaultValue(3)]
        public int AboveLevel { get; set; }

        [Setting, DefaultValue(0)]
        public int UseBandagesToHeal { get; set; }

        [Setting, DefaultValue(1)]
        public int UseHealSkill { get; set; }

        [Setting, DefaultValue(false)]
        public bool ForfeitIfNotInteresting { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseWhiteList { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseBlackList { get; set; }

        [Setting, DefaultValue(false)]
        public bool DetailedLogging { get; set; }

        [Setting, DefaultValue(true)]
        public bool AmHorde { get; set; }

        [Setting, DefaultValue("petHP * HPFactor")]
        public string HPFormula { get; set; }

        [Setting, DefaultValue("advantage * 50 * AdFactor")]
        public string AdFormula { get; set; }

        [Setting, DefaultValue("disadvantage * 50 * DisFactor")]
        public string DisFormula { get; set; }

        [Setting, DefaultValue("(petLevel - enemylevel) * 4 * LevelFactor")]
        public string LevelFormula { get; set; }
		
		[Setting, DefaultValue(true)]
		public bool CheckAllowUsageTracking { get; set; }



        [Setting, DefaultValue(false)]
        public bool IBSupport { get; set; }

    }

    public class PetBattleSettings : Settings
    {

        public PetBattleSettings(string settingsPath)
            : base(settingsPath)
        {
            Load();
        }

        [Setting, DefaultValue("SWAPOUT Health(THISPET) ISLESSTHAN 30@CASTSPELL(1) COOLDOWN(SKILL(1)) EQUALS false")]
        public string Logic { get; set; }

        [Setting, DefaultValue("ASSIGNABILITY1(0)@ASSIGNABILITY2(0)@ASSIGNABILITY3(0)")]
        public string SpellLayout { get; set; }



    }

}




/***********************************************************\
*															*
*				Config Screen part!!						*
*															*
\***********************************************************/



/*

namespace Pokehbuddyplug
{
    partial class configForm
    {
        int initdone = 0;

        private void configForm_Load(object sender, EventArgs e)
        {

            checkBox2.Checked = Pokehbuddy.MySettings.ForfeitIfNotInteresting;
            checkBox3.Checked = Pokehbuddy.MySettings.UseBandages;
            checkBox4.Checked = Pokehbuddy.MySettings.DetailedLogging;

            trackBar1.Value = Pokehbuddy.MySettings.HPFactor;
            trackBar2.Value = Pokehbuddy.MySettings.LevelFactor;
            trackBar3.Value = Pokehbuddy.MySettings.AdFactor;
            trackBar4.Value = Pokehbuddy.MySettings.DisFactor;
            trackBar5.Value = Pokehbuddy.MySettings.Distance;

            checkBox1.Checked = Pokehbuddy.MySettings.DoPVP;
            comboBox5.SelectedIndex = Pokehbuddy.MySettings.BelowLevel;
            comboBox6.SelectedIndex = Pokehbuddy.MySettings.AboveLevel;
            comboBox7.SelectedIndex = Pokehbuddy.MySettings.PVPMinTime;

            comboBox8.Items.Clear();
            int dummy = 0;
            for (int i = 1; i < 60; i++)
            {
                dummy = Pokehbuddy.MySettings.PVPMinTime + 1;
                comboBox8.Items.Add(dummy + i);
            }


            comboBox8.SelectedIndex = Pokehbuddy.MySettings.PVPMaxTime;


            comboBox1.SelectedIndex = Pokehbuddy.MySettings.GetRarity - 1;
            initdone = 1;
            //Logging.Write("LAlalala");

            /* TheDungeonComboBox.SelectedIndex = Pokehbuddy.MySettings.TheDungeon;
             HeartstoneOutSetting.Text = Pokehbuddy.MySettings.HeartstoneAfter.ToString();
             WalkoutTimeSetting.Text = Pokehbuddy.MySettings.WalkOutAfter.ToString();
             MailEveryResetCheck.Checked = Pokehbuddy.MySettings.MailEveryReset;
             HordeCheck.Checked = Pokehbuddy.MySettings.AmHorde;
             IBSupport.Checked = Pokehbuddy.MySettings.IBSupport;*
        }

        private void MailEveryResetCheck_CheckedChanged(object sender, EventArgs e)
        {
            //   Pokehbuddy.MySettings.MailEveryReset = MailEveryResetCheck.Checked;
        }
        private void HordeCheck_CheckedChanged(object sender, EventArgs e)
        {
            //   Pokehbuddy.MySettings.AmHorde = HordeCheck.Checked;
        }
        private void IBSupport_CheckedChanged(object sender, EventArgs e)
        {
            //   Pokehbuddy.MySettings.IBSupport = IBSupport.Checked;
        }



        private void WalkoutTimeSetting_Leave(object sender, EventArgs e)
        {
            //  WalkoutTimeSetting.Text = Pokehbuddy.MySettings.WalkOutAfter.ToString();
        }

        private void WalkoutTimeSetting_TextChanged(object sender, EventArgs e)
        {
            //   int n;
            //   int.TryParse(WalkoutTimeSetting.Text, out n);
            //   if (n < 1)
            //       n = 1;
            //   else if (n > 50)
            //       n = 50;
            //   Pokehbuddy.MySettings.WalkOutAfter = n;
        }

        private void HeartstoneOutSetting_TextChanged(object sender, EventArgs e)
        {
            //   int n;
            //   int.TryParse(HeartstoneOutSetting.Text, out n);
            //  if (n < 20)
            //       n = 20;
            //   else if (n > 100)
            //      n = 100;
            //    Pokehbuddy.MySettings.HeartstoneAfter = n;
        }

        private void HeartstoneOutSetting_Leave(object sender, EventArgs e)
        {
            //  HeartstoneOutSetting.Text = Pokehbuddy.MySettings.HeartstoneAfter.ToString();
        }

        private void configForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Pokehbuddy.MySettings.Save();
        }
    }

}

namespace Pokehbuddyplug
{
    public partial class configForm : Form
    {
        string[] options = { "COOLDOWN(SKILL(1))","EQUALS", "true,false",
	"COOLDOWN(SKILL(2))","EQUALS", "true,false", 
	"COOLDOWN(SKILL(3))","EQUALS", "true,false", 
	"Health(THISPET)","EQUALS,ISLESSTHAN,ISGREATERTHAN", "NUMBER,Health(ENEMYPET)", 
	"Health(ENEMYPET)","EQUALS,ISLESSTHAN,ISGREATERTHAN", "NUMBER,Health(THISPET)", 
	"MyPetLevel","EQUALS,ISLESSTHAN,ISGREATERTHAN", "NUMBER,EnemyPetLevel,EnemyPetLevel + NUMBER", 
	"EnemyPetLevel","EQUALS,ISLESSTHAN,ISGREATERTHAN", "NUMBER,MyPetLevel,MyPetLevel + NUMBER", 
	"ENEMYTYPE","EQUALS,ISNOT", "HUMANOID,DRAGONKIN,FLYING,UNDEAD,CRITTER,MAGIC,ELEMENTAL,BEAST,AQUATIC,MECHANICAL", 
	"HASBUFF(X)","EQUALS", "true,false", 
	"HASENEMYBUFF(X)","EQUALS","true,false",  
	"WEATHERBUFF(X)","EQUALS", "true,false", 
	"HASTEAMBUFF(X)","EQUALS", "true,false", 
	"ENEMYTEAMBUFF(X)","EQUALS","true,false", 
	"MYPETSPEED","EQUALS,ISLESSTHAN,ISGREATERTHAN","NUMBER,ENEMYSPEED", 
	"ENEMYSPEED","EQUALS,ISLESSTHAN,ISGREATERTHAN","NUMBER,MYPETSPEED",
"MyPetsAlive","EQUALS,ISLESSTHAN,ISGREATERTHAN","NUMBER,EnemyPetsAlive",
"EnemyPetsAlive","EQUALS,ISLESSTHAN,ISGREATERTHAN","NUMBER,MyPetsAlive"};


        /*MyPetsAlive
,
"","",""};
*


        public configForm()
        {
            InitializeComponent();
            listBox2.Visible = false;
            this.pictureBox1.Image = new Bitmap(Application.StartupPath + "\\Plugins\\Pokehbuddy\\Images\\pb.jpg");
        }

        private void TheDungeonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //      Pokehbuddy.MySettings.TheDungeon = TheDungeonComboBox.SelectedIndex;
            //BBLog("Dungeon " + TheDungeonComboBox.SelectedIndex);



        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.GetRarity = comboBox1.SelectedIndex + 1;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
            Pokehbuddy.MySettings.HPFactor = trackBar1.Value;



        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            label2.Text = trackBar2.Value.ToString();
            Pokehbuddy.MySettings.LevelFactor = trackBar2.Value;



        }
        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = trackBar3.Value.ToString();
            Pokehbuddy.MySettings.AdFactor = trackBar3.Value;



        }
        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            label4.Text = trackBar4.Value.ToString();
            Pokehbuddy.MySettings.DisFactor = trackBar4.Value;



        }
        private void trackBar5_ValueChanged(object sender, EventArgs e)
        {
            label10.Text = trackBar5.Value.ToString();
            Pokehbuddy.MySettings.Distance = trackBar5.Value;



        }
        private void ListBuffs_Clicked(object sender, EventArgs e)
        {
            Lua.DoString("for j=1,C_PetBattles.GetNumAuras(1,C_PetBattles.GetActivePet(1)) do  local buffid = C_PetBattles.GetAuraInfo(1,1,j)  print (buffid) end");


        }


        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox11.SelectedIndex == -1)
            {
                button3.Enabled = false;
                button4.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox2.Items.Clear();
                comboBox3.Items.Clear();
                comboBox4.Items.Clear();
            }
            if (comboBox11.SelectedIndex > -1)
            {
                comboBox2.Enabled = true;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;

                comboBox2.Items.Clear();

                for (int i = 0; i < options.Count(); i++)
                {
                    comboBox2.Items.Add(options[i]);
                    i++;
                    i++;
                }
            }




        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = true;
            comboBox3.Text = "";
            comboBox3.Items.Clear();

            comboBox4.Enabled = true;
            comboBox4.Text = "";
            comboBox4.Items.Clear();


            string dumdumdum = comboBox2.Text;
            //Logging.Write("im here!!!"+dumdumdum);
            for (int i = 0; i < options.Count(); i++)
            {
                if (dumdumdum.Contains(options[i]))
                {

                    string[] equalizers = options[i + 1].Split(',');

                    foreach (string equalizer in equalizers)
                    {
                        comboBox3.Items.Add(equalizer);
                    }
                    string[] targetz = options[i + 2].Split(',');

                    foreach (string targ in targetz)
                    {
                        comboBox4.Items.Add(targ);
                    }

                }
                i++;
                i++;
            }






        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox4.Text.Contains("NUMBER"))
            {
                if (comboBox11.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && comboBox4.Text != "")
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                }


            }




            if (comboBox4.Text.Contains("true"))
            {

                if (comboBox11.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && comboBox4.Text != "")
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                }


            }
            if (comboBox4.Text.Contains("false"))
            {


                if (comboBox11.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && comboBox4.Text != "")
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                }


            }

            if (comboBox11.SelectedIndex > -1)
            {
                if (comboBox11.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && comboBox4.Text != "")
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                }

            }




        }
        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //AND
            string dummy = comboBox2.Text.Replace("(X)", "(" + textBox101.Text + ")") + " " + comboBox3.Text + " " + comboBox4.Text.Replace("NUMBER", "" + textBox102.Text + "") + " $ ";
            textBox1.Text = textBox1.Text + dummy;
            //comboBox11.Enabled = false;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            button3.Enabled = false;
            button4.Enabled = false;
            button2.Enabled = false;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            //FINISH
            string dummy = comboBox11.Text + " " + textBox1.Text + comboBox2.Text.Replace("(X)", "(" + textBox101.Text + ")") + " " + comboBox3.Text + " " + comboBox4.Text.Replace("NUMBER", "" + textBox102.Text + "");
            textBox2.Text = dummy;
            textBox1.Text = "";
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox11.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            button2.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox2.Text);
            textBox2.Text = "";
            button2.Enabled = false;

        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            Lua.DoString("for j=1,C_PetBattles.GetNumAuras(1,C_PetBattles.GetActivePet(1)) do  local buffid = C_PetBattles.GetAuraInfo(1,C_PetBattles.GetActivePet(1),j)  print (buffid) end");
            List<string> cnt2 = Lua.GetReturnValues("local dummy = '' for j=1,C_PetBattles.GetNumAuras(1,C_PetBattles.GetActivePet(1)) do  local buffid = C_PetBattles.GetAuraInfo(1,C_PetBattles.GetActivePet(1),j)  dummy = tostring(dummy) .. tostring(buffid) .. ',' end return dummy");
            string[] buffz = cnt2[0].Split(',');

            foreach (string buff in buffz)
            {
                listBox2.Items.Add(buff);
            }
            if (listBox2.Items.Count > 0) listBox2.Visible = true;


        }
        private void button99_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            Lua.DoString("for i=1, C_PetBattles.GetNumAuras(0,0) do local auraID = C_PetBattles.GetAuraInfo(LE_BATTLE_PET_WEATHER, PET_BATTLE_PAD_INDEX, i) print(auraID) end");
            List<string> cnt2 = Lua.GetReturnValues("local dummy = '' for j=1,C_PetBattles.GetNumAuras(LE_BATTLE_PET_WEATHER,PET_BATTLE_PAD_INDEX) do  local buffid = C_PetBattles.GetAuraInfo(LE_BATTLE_PET_WEATHER,PET_BATTLE_PAD_INDEX,j)  dummy = tostring(dummy) .. tostring(buffid) .. ',' end return dummy");
            string[] buffz = cnt2[0].Split(',');
            listBox2.Items.Clear();
            foreach (string buff in buffz)
            {
                listBox2.Items.Add(buff);
            }
            if (listBox2.Items.Count > 0) listBox2.Visible = true;
        }

        private void button919_Click(object sender, EventArgs e)
        {//               for i=1, C_PetBattles.GetNumAuras(1,0) do local auraID = C_PetBattles.GetAuraInfo(1, PET_BATTLE_PAD_INDEX, i) print(auraID) end
            listBox2.Items.Clear();
            Lua.DoString("for i=1, C_PetBattles.GetNumAuras(1,0) do local auraID = C_PetBattles.GetAuraInfo(1, PET_BATTLE_PAD_INDEX, i) print(auraID) end");
            List<string> cnt2 = Lua.GetReturnValues("local dummy = '' for j=1,C_PetBattles.GetNumAuras(1,0) do  local buffid = C_PetBattles.GetAuraInfo(1,PET_BATTLE_PAD_INDEX,j)  dummy = tostring(dummy) .. tostring(buffid) .. ',' end return dummy");
            string[] buffz = cnt2[0].Split(',');
            listBox2.Items.Clear();
            foreach (string buff in buffz)
            {
                listBox2.Items.Add(buff);
            }
            if (listBox2.Items.Count > 0) listBox2.Visible = true;


        }
        private void button929_Click(object sender, EventArgs e)
        {
            //get whole pet list
            /*		for (int intI = 1; intI < 597; intI++) {
                    List<string> cntlist = Lua.GetReturnValues("local stor = '' local petID, speciesID, _, _, _, _, _, name, icon  = C_PetJournal.GetPetInfoByIndex("+ intI +", false); stor = stor .. '*'..speciesID .. '*,*' .. name ..'*,*' .. icon .. '*,'  return stor");

                    Logging.Write(cntlist[0]);
                    }*

            //


            listBox2.Items.Clear();
            Lua.DoString("for i=1, C_PetBattles.GetNumAuras(2,0) do local auraID = C_PetBattles.GetAuraInfo(2, PET_BATTLE_PAD_INDEX, i) print(auraID) end");
            List<string> cnt2 = Lua.GetReturnValues("local dummy = '' for j=1,C_PetBattles.GetNumAuras(2,0) do  local buffid = C_PetBattles.GetAuraInfo(2,PET_BATTLE_PAD_INDEX,j)  dummy = tostring(dummy) .. tostring(buffid) .. ',' end return dummy");
            string[] buffz = cnt2[0].Split(',');
            listBox2.Items.Clear();
            foreach (string buff in buffz)
            {
                listBox2.Items.Add(buff);
            }
            if (listBox2.Items.Count > 0) listBox2.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            //tabControl1.Visible=false;

            Lua.DoString("for j=1,C_PetBattles.GetNumAuras(2,C_PetBattles.GetActivePet(2)) do  local buffid = C_PetBattles.GetAuraInfo(2,C_PetBattles.GetActivePet(2),j)  print (buffid) end");
            List<string> cnt2 = Lua.GetReturnValues("local dummy = '' for j=1,C_PetBattles.GetNumAuras(2,C_PetBattles.GetActivePet(2)) do  local buffid = C_PetBattles.GetAuraInfo(2,C_PetBattles.GetActivePet(2),j)  dummy = tostring(dummy) .. tostring(buffid) .. ',' end return dummy");
            string[] buffz = cnt2[0].Split(',');
            listBox2.Items.Clear();
            foreach (string buff in buffz)
            {
                listBox2.Items.Add(buff);
            }
            if (listBox2.Items.Count > 0) listBox2.Visible = true;
        }
        public void MoveItem(int direction)
        {
            // Checking selected item
            if (listBox1.SelectedItem == null || listBox1.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = listBox1.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= listBox1.Items.Count)
                return; // Index out of range - nothing to do

            object selected = listBox1.SelectedItem;

            // Removing removable element
            listBox1.Items.Remove(selected);
            // Insert it in new position
            listBox1.Items.Insert(newIndex, selected);
            // Restore selection
            listBox1.SetSelected(newIndex, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + label71.Text + ".xml";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            listBox1.Items.Clear();
            Pokehbuddy pok = new Pokehbuddy();
            pok.LoadPetSettings(label71.Text, label22.Text);

            //string dumdum = "";
            string dumdum = Pokehbuddy.PetSettings.Logic;
            string[] PetLogics = dumdum.Split('@');
            foreach (string alogic in PetLogics)
            {
                listBox1.Items.Add(alogic);
            }


        }
        private void button5_Click(object sender, EventArgs e)
        {

            MoveItem(-1);

        }

        private void button6_Click(object sender, EventArgs e)
        {

            MoveItem(1);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1) listBox1.Items.Remove(listBox1.SelectedItem);

        }
        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            textBox101.Text = listBox2.Text;
            listBox2.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox9.SelectedIndex > -1)
            {
                listBox1.Items.Clear();
                Pokehbuddy pok = new Pokehbuddy();
                pok.LoadPetSettings(pok.ReadSlot(comboBox9.SelectedIndex + 1), pok.ReadSlotSpecies(comboBox9.SelectedIndex + 1));
                label22.Text = pok.ReadSlotSpecies(comboBox9.SelectedIndex + 1);
                label71.Text = pok.ReadSlot(comboBox9.SelectedIndex + 1);
                //string dumdum = "";

                string dumdum = Pokehbuddy.PetSettings.Logic;
                string[] PetLogics = dumdum.Split('@');
                foreach (string alogic in PetLogics)
                {
                    listBox1.Items.Add(alogic);
                }

                string theicon = pok.SlotIcon(comboBox9.SelectedIndex + 1);
                string baseurl = "http://wow.zamimg.com/images/wow/icons/large/";
                string replace1 = @"INTERFACE\ICONS\";

                string image = theicon.Replace(replace1, "").Replace(".BLP", "").ToLower();
                image = baseurl + image + ".jpg";
                Logging.Write("loading image :" + image);
                pictureBox2.ImageLocation = image;
                /*5.1//
                label20.Text=pok.GetNameByID(label71.Text);
                List<string> cnt3 = Lua.GetReturnValues("local speciesID, customName, _, _, _,_, name, icon  = C_PetJournal.GetPetInfoByPetID(string.format('%X',"+label71.Text+")) if customName==nil then return 'No custom name' end	return customName");
                label22.Text=cnt3[0];
                //List<string> cnt4 = Lua.GetReturnValues("local speciesID, customName, _, _, _,_, name, icon, petType  = C_PetJournal.GetPetInfoByPetID(string.format('%X',"+label71.Text+")) return petType");
                //label22.Text=cnt4[0];
                5.1*

            }
        }
        private string GetPetImage(int slot)
        {
            List<string> cnt2 = Lua.GetReturnValues("local petid = C_PetJournal.GetPetLoadOutInfo(" + slot + ") local speciesID, customName, _, _, _,_, name, icon  = C_PetJournal.GetPetInfoByPetID(petid) return icon");
            string baseurl = "http://wow.zamimg.com/images/wow/icons/large/";
            string replace1 = @"INTERFACE\ICONS\";

            string image = cnt2[0].Replace(replace1, "").Replace(".BLP", "").ToLower();
            image = baseurl + image + ".jpg";
            return (image);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (label71.Text != "")
            {
                string dummy = "";
                Pokehbuddy pok = new Pokehbuddy();
                int i = 0;
                foreach (object item in listBox1.Items)
                {
                    dummy = dummy + item.ToString();
                    if (i < listBox1.Items.Count - 1) dummy = dummy + "@";
                    i++;
                }
                pok.LoadPetSettings(label71.Text, label22.Text);
                Pokehbuddy.PetSettings.Logic = dummy;
                Pokehbuddy.PetSettings.Save();

            }

        }

        private void button44_Click(object sender, EventArgs e)
        {
            Pokehbuddy pok = new Pokehbuddy();
            if (label71.Text != "")
            {
                string dummy = "";

                int i = 0;
                foreach (object item in listBox1.Items)
                {
                    dummy = dummy + item.ToString();
                    if (i < listBox1.Items.Count - 1) dummy = dummy + "@";
                    i++;
                }
                pok.LoadPetSettingsBN(label22.Text);





                Pokehbuddy.PetSettings.Logic = dummy;
                Pokehbuddy.PetSettings.Save();







                Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
                Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
                ////Lua.DoString("C_PetJournal.SetSearchFilter('" + petname + "')");
                List<string> cnt1 = Lua.GetReturnValues("local teller=0 local retdata={} retdata[0]='nothing' retdata[1]='nothing'  local dummy1 = '" + pok.GetSpeciesByName(label22.Text) + "' local numpets = C_PetJournal.GetNumPets(false) local skillist = C_PetJournal.GetPetAbilityList(dummy1); for j = 1, numpets do  local _, dummy2 = C_PetJournal.GetPetInfoByIndex(j,false); local skillist2 = C_PetJournal.GetPetAbilityList(dummy2); if skillist[1] == skillist2[1] and skillist[2] == skillist2[2] and skillist[3] == skillist2[3] then  local _,_,_,_,_,_,_,ass = C_PetJournal.GetPetInfoByIndex(j,false) teller=teller+1 retdata[teller]=ass end end return teller");
                int getal = 0;
                try
                {
                    getal = Convert.ToInt32(cnt1[0]);
                }
                catch (Exception exc)
                {

                }
                for (int intI = 1; intI < getal; intI++)
                {
                    List<string> cnt = Lua.GetReturnValues("local teller=0 local retdata={} retdata[0]='nothing' retdata[1]='nothing'  local dummy1 = '" + pok.GetSpeciesByName(label22.Text) + "' local numpets = C_PetJournal.GetNumPets(false) local skillist = C_PetJournal.GetPetAbilityList(dummy1); for j = 1, numpets do  local _, dummy2 = C_PetJournal.GetPetInfoByIndex(j,false); local skillist2 = C_PetJournal.GetPetAbilityList(dummy2); if skillist[1] == skillist2[1] and skillist[2] == skillist2[2] and skillist[3] == skillist2[3] then  local _,speciesID,_,_,_,_,_,ass = C_PetJournal.GetPetInfoByIndex(j,false) teller=teller+1 retdata[teller]=speciesID end end return (retdata[" + intI + "])");
                    cnt[0] = pok.GetNameBySpeciesID(cnt[0]);
                    string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + cnt[0] + ".xml";
                    if (!File.Exists(filename))
                    {

                        string filename2 = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + label22.Text + ".xml";
                        if (File.Exists(filename2))
                        {
                            File.Copy(filename2, filename);
                        }
                    }



                    Logging.Write(cnt[0]);
                }




                // BBLog(cnt[0]);
                //if (cnt[0]=="1") dummy=true;






            }

        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.ForfeitIfNotInteresting = checkBox2.Checked;

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.UseBandages = checkBox3.Checked;

        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.DetailedLogging = checkBox4.Checked;

        }




        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.BelowLevel = comboBox5.SelectedIndex;


            /*            comboBox6.Items.Clear();
                        int dummy=0;
                        for (int i = 1; i < 60; i++)
                        {
                            dummy=comboBox5.SelectedIndex+1;
                            comboBox6.Items.Add(dummy + i);
                        }*
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.AboveLevel = comboBox6.SelectedIndex;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.PVPMinTime = comboBox7.SelectedIndex;
            if (initdone == 1)
            {
                comboBox8.Items.Clear();
                int dummy = 0;
                for (int i = 1; i < 60; i++)
                {
                    dummy = comboBox7.SelectedIndex + 1;
                    comboBox8.Items.Add(dummy + i);
                }
                comboBox8.SelectedIndex = 0;
            }

        }
        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initdone == 1) Pokehbuddy.MySettings.PVPMaxTime = comboBox8.SelectedIndex;
            if (initdone == 0) comboBox8.SelectedIndex = Pokehbuddy.MySettings.PVPMaxTime;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Pokehbuddy.MySettings.DoPVP = checkBox1.Checked;
        }
        private void button13_Click(object sender, EventArgs e)
        {
            listBox2.Visible = false;
            groupBox4.Visible = false;

        }

        private void button12_Click(object sender, EventArgs e)
        {
            /*5.1
            //Pokehbuddy pok = new Pokehbuddy();
            int i=0;
               for (i=1;i<4;i++){
                List<string> cnt2 = Lua.GetReturnValues("local dummy1 = C_PetJournal.GetPetInfoByPetID(string.format('%X',"+label71.Text+")); local skillist = C_PetJournal.GetPetAbilityList(dummy1) name, icon = C_PetJournal.GetPetAbilityInfo(skillist["+i+"]); return icon");
                string baseurl="http://wow.zamimg.com/images/wow/icons/large/";
                string replace1=@"INTERFACE\ICONS\";
			
                string image = cnt2[0].Replace(replace1,"").Replace(".BLP","").ToLower();
                image=baseurl+image+".jpg";
                if (i==1) pictureBox3.ImageLocation=image;
                if (i==2) pictureBox4.ImageLocation=image;
                if (i==3) pictureBox5.ImageLocation=image;
                //Logging.Write("loading image" + image);
                List<string> cnt3 = Lua.GetReturnValues("local dummy1 = C_PetJournal.GetPetInfoByPetID(string.format('%X',"+label71.Text+")); local skillist = C_PetJournal.GetPetAbilityList(dummy1) name, icon = C_PetJournal.GetPetAbilityInfo(skillist["+i+"]); return name");
                if (i==1) label23.Text=cnt3[0];
                if (i==2) label28.Text=cnt3[0];
                if (i==3) label32.Text=cnt3[0];
			
            }
            label24.Visible=false;
            label25.Visible=false;
            label26.Visible=false;
            label27.Visible=false;
            label30.Visible=false;
            label29.Visible=false;
		
			
			
                //List<string> cnt3 = Lua.GetReturnValues("local speciesID, customName, _, _, _,_, name, icon  = C_PetJournal.GetPetInfoByPetID("+label71.Text+") if customName==nil then return 'No custom name' end	return customName");
                //label22.Text=cnt3[0];
                groupBox4.Visible=true;
            5.1*
        }


        private void button45_Click(object sender, EventArgs e)
        {
            Pokehbuddy pok = new Pokehbuddy();
            if (label71.Text != "")
            {
                string dummy = "";

                int i = 0;
                foreach (object item in listBox1.Items)
                {
                    dummy = dummy + item.ToString();
                    if (i < listBox1.Items.Count - 1) dummy = dummy + "@";
                    i++;
                }

                pok.LoadPetSettingsBN(label22.Text);





                Pokehbuddy.PetSettings.Logic = dummy;
                Pokehbuddy.PetSettings.Save();







                Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
                Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
                ////Lua.DoString("C_PetJournal.SetSearchFilter('" + petname + "')");
                List<string> cnt1 = Lua.GetReturnValues("local teller=0 local retdata={} retdata[0]='nothing' retdata[1]='nothing'  local dummy1 = C_PetJournal.GetPetInfoByPetID(string.format('%X'," + label71.Text + ")); local numpets = C_PetJournal.GetNumPets(false) local skillist = C_PetJournal.GetPetAbilityList(dummy1); for j = 1, numpets do  local _, dummy2 = C_PetJournal.GetPetInfoByIndex(j,false); local skillist2 = C_PetJournal.GetPetAbilityList(dummy2); if skillist[1] == skillist2[1] and skillist[2] == skillist2[2] and skillist[3] == skillist2[3] then  local _,_,_,_,_,_,_,ass = C_PetJournal.GetPetInfoByIndex(j,false) teller=teller+1 retdata[teller]=ass end end return teller");
                int getal = 0;
                try
                {
                    getal = Convert.ToInt32(cnt1[0]);
                }
                catch (Exception exc)
                {

                }
                Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
                Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
                for (int intI = 1; intI < getal; intI++)
                {
                    List<string> cnt = Lua.GetReturnValues("local teller=0 local retdata={} retdata[0]='nothing' retdata[1]='nothing'  local dummy1 = '" + pok.GetSpeciesByName(label22.Text) + "' local numpets = C_PetJournal.GetNumPets(false) local skillist = C_PetJournal.GetPetAbilityList(dummy1); for j = 1, numpets do  local _, dummy2 = C_PetJournal.GetPetInfoByIndex(j,false); local skillist2 = C_PetJournal.GetPetAbilityList(dummy2); if skillist[1] == skillist2[1] and skillist[2] == skillist2[2] and skillist[3] == skillist2[3] then  local _,speciesID,_,_,_,_,_,ass = C_PetJournal.GetPetInfoByIndex(j,false) teller=teller+1 retdata[teller]=speciesID  end end return (retdata[" + intI + "])");
                    //Logging.Write(cnt[0]);
                    cnt[0] = pok.GetNameBySpeciesID(cnt[0]);
                    string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + cnt[0] + ".xml";
                    string filename2 = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + label22.Text + ".xml";
                    //Logging.Write("File 1 : "+filename+ " File 2 :"+filename2);
                    if (File.Exists(filename) && filename != filename2) File.Delete(filename);

                    //string filename2=Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\"+pok.GetNameByID(label71.Text)+".xml";
                    if (File.Exists(filename2) && filename != filename2)
                    {
                        File.Copy(filename2, filename);
                    }




                    Logging.Write(cnt[0]);
                }




                // BBLog(cnt[0]);
                //if (cnt[0]=="1") dummy=true;






            }

        }





    }









}





*/