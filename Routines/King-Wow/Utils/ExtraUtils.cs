using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using Styx.Common;
using System.Diagnostics;
using Styx.CommonBot;

namespace KingWoW
{
    #region MISHLIST
    public static class MiscLists
    {
        internal static HashSet<string> Racials
        {
            get
            {
                return _racials;
            }
        }

        #region _racials

        private static readonly HashSet<string> _racials = new HashSet<string> {
            "Stoneform",

            // Activate to remove poison, disease, and bleed effects; +10% Armor; Lasts 8 seconds. 2 minute cooldown.
            "Escape Artist",

            // Escape the effects of any immobilization or movement speed reduction effect. Instant cast. 1.45 min cooldown
            "Every Man for Himself",

            // Removes all movement impairing effects and all effects which cause loss of control of your character. This effect
            "Shadowmeld",

            // Activate to slip into the shadows, reducing the chance for enemies to detect your presence. Lasts until cancelled or upon
            "Gift of the Naaru",

            // Heals the target for 20% of their total health over 15 sec. 3 minute cooldown.
            "Darkflight",

            // Activates your true form, increasing movement speed by 40% for 10 sec. 3 minute cooldown.
            "Blood Fury",

            // Activate to increase attack power and spell damage by an amount based on level/class for 15 seconds. 2 minute cooldown.
            "War Stomp",

            // Activate to stun opponents - Stuns up to 5 enemies within 8 yards for 2 seconds. 2 minute cooldown.
            "Berserking",

            // Activate to increase attack and casting speed by 20% for 10 seconds. 3 minute cooldown.
            "Will of the Forsaken",

            // Removes any Charm, Fear and Sleep effect. 2 minute cooldown.
            "Cannibalize",

            // When activated, regenerates 7% of total health and mana every 2 seconds for 10 seconds. Only works on Humanoid or Undead corpses within 5 yards. Any movement, action, or damage taken while Cannibalizing will cancel the effect.
            "Arcane Torrent",

            // Activate to silence all enemies within 8 yards for 2 seconds. In addition, you gain 15 Energy, 15 Runic Power or 6% Mana. 2 min. cooldown.
            "Rocket Barrage",

            // Launches your belt rockets at an enemy, dealing X-Y fire damage. (24-30 at level 1; 1654-2020 at level 80). 2 min. cooldown.
        };

        #endregion _racials

        public static HashSet<int> GCDFreeAbilities
        {
            get
            {
                return _gcdFreeAbilities;
            }
        }

        #region _gcdFreeAbilities

        private static readonly HashSet<int> _gcdFreeAbilities = new HashSet<int> {
            108978, //= 0 GCD // ALTER TIME
            86659, // = 0 GCD // "Ancient Guardian",
            86669, //= 0 GCD  // "Ancient Guardian",
            86698, //= 0 GCD // "Ancient Guardian",
            71322, //= 0 GCD // "Annihilate",
            48707, //= 0 GCD // "Anti-Magic Shell",
            37538, //= 0 GCD // "Anti-Magic Shield",
            19645, //= 0 GCD // "Anti-Magic Shield",
            7121,  //= 0 GCD // "Anti-Magic Shield",
            51052, //= 0 GCD // "Anti-Magic Zone",
            12042, //= 0 GCD // "Arcane Power",
            31884, //= 0 GCD // "Avenging Wrath",
            22812, //= 0 GCD  // "Barkskin",
            41450, //= 0 GCD // "Blessing of Protection",
            45529, //= 0 GCD // "Blood Tap",
            84615, //= 0 GCD // "Blood and Thunder",
            2825,  //=  0 GCD // "Bloodlust",
            100,   //=  0 GCD // "Charge",
            845,   //=  0 GCD// "Cleave",
            11958, //=  0 GCD // "Cold Snap",
            11129, //=  0 GCD // "Combustion",
            2139,  //= 0 GCD // "Counterspell",

            //29893, //= 1.5 GCD  // "Create Soulwell",
            56222, //= 0 GCD // "Dark Command",
            77801, //= 0 GCD // "Dark Soul",
            49576, //= 0 GCD // "Death Grip",
            61595, //= 0 GCD // "Demonic Soul",
            19263, //= 0 GCD // "Deterrence",
            19505, //= 0 GCD // "Devour Magic",
            781, //= 0 GCD  // "Disengage",
            31842, //= 0 GCD // "Divine Favor",
            16166, //= 0 GCD // "Elemental Mastery",
            47568,  //= 0 GCD // "Empower Rune Weapon",
            5277, //= 0 GCD  // "Evasion",
            5384, //= 0 GCD // "Feign Death",
            87187, //= 0 GCD // "Feral Charge",
            79870, //= 0 GCD  // "Feral Charge",
            22842, //= 0 GCD // "Frenzied Regeneration",
            47788, //= 0 GCD // "Guardian Spirit",

            //24275, //= 1.5 GCD // "Hammer of Wrath",
            78, //= 0 GCD // "Heroic Strike",
            90255, //= 0 GCD  // "Hysteria",
            48792, //= 0 GCD // "Icebound Fortitude",
            12472, //= 0 GCD  // "Icy Veins",
            89485, //= 0 GCD  // "Inner Focus",
            50823, //= 0 GCD // "Intercept",
            78131, //= 0 GCD // "Intercept",
            27826, //= 0 GCD  // "Intercept",
            1766, //= 0 GCD // "Kick",
            34026, // "Kill Command", 1 second ???
            73325, // "Leap of Faith",
            49039, // "Lichborne",
            53271, // "Masters Call",
            6807, // "Maul",
            103958, // "Metamorphosis",
            47528, // "Mind Freeze",

            //16689, // "Nature's Grasp", GCD	1.5 seconds ??
            132158, // "Nature's Swiftness",
            51271, // "Pillar of Frost",
            10060, // "Power Infusion",
            14183, // "Premeditation",

            //14185, // "Preparation", GCD	1 second ???
            12043, // "Presence of Mind",
            6552, // "Pummel",
            3045, // "Rapid Fire",
            48982, // "Rune Tap",
            6358, // "Seduction",
            49572, // "Shadow Infusion",

            //30283, // "Shadowfury", GCD	500 milliseconds ??
            36554, // "Shadowstep",
            36988, // "Shield Bash",
            72194, // "Shield Bash",
            79732, // "Shield Bash",

            //101817, // "Shield Bash", GCD	1.5 seconds ??
            41197, // "Shield Bash",
            35178, // "Shield Bash",
            11972, // "Shield Bash",
            33871, // "Shield Bash",
            38233, // "Shield Bash",
            41180, // "Shield Bash",
            82800, // "Shield Bash",
            2565, // "Shield Block",
            53600, // "Shield of the Righteous",
            76008, // "Shock Blast",
            15487, // "Silence",
            34490, // "Silencing Shot",
            78675, // "Solar Beam",
            74434, // "Soulburn",
            19647, // "Spell Lock",
            23920, // "Spell Reflection",
            90361, // "Spirit Mend",
            79206, // "Spiritwalker's Grace",
            2983, // "Sprint",

            //48505, // "Starfall", GCD	1.5 seconds
            61336, // "Survival Instincts",
            12328, // Sweeping Strikes",
            80353, // "Time Warp",
            104773, // "Unending Resolve",
            55233, // "Vampiric Blood",
            1856, // "Vanish",
            6360, // "Whiplash",
            57994, // "Wind Shear",
            105809, // "Holy Avenger",
        };

        #endregion _gcdFreeAbilities

        internal static HashSet<string> spellsThatBreakCrowdControl
        {
            get
            {
                return _spellsThatBreakCrowdControl;
            }
        }

        #region _spellsThatBreakCrowdControl

        /// <summary>
        /// Please add spells which can break movement imparing effects here
        /// </summary>
        private static readonly HashSet<string> _spellsThatBreakCrowdControl = new HashSet<string>
        {
            "Berserker Rage",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
        };

        #endregion _spellsThatBreakCrowdControl
    }
    #endregion

    public class ExtraUtils
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public Stopwatch KeyBinds_sw = new Stopwatch();     //for Keybinds

        private KingWoWUtility utils = new KingWoWUtility();

        #region UseHealthstone

        public bool UseHealthstone()
        {
            if(Me.Combat && Me.HealthPercent <= ExtraUtilsSettings.Instance.HealthsStoneHP)
            {
                WoWItem hs = Me.BagItems.FirstOrDefault(o => o.Entry == 5512); //5512 Healthstone
                if (hs != null && hs.CooldownTimeLeft.TotalMilliseconds <= 0)
                {
                    hs.Use();
                    Logging.Write("Use Healthstone at " + Me.HealthPercent + "%");
                    return true;
                }
            }
            return false;
        }

        #endregion




        #region Alchemist

        private const uint INTELLECT_1000 = 76085; //"Flask of the Warm Sun"
        private const uint STRENGHT_1000 = 76088;//"Flask of Winter's Bite"
        private const uint STAMINA_1500 = 76087;//"Flask of the Earth"
        private const uint AGILITY_1000 = 76084;//"Flask of Spring Blossoms"
        private const uint SPIRIT_1000 = 76086;//"Flask of Falling Leaves"
        private const uint BATTLE_MOP = 65455;//"Flask of Battle"

        private const uint AGILITY_300 = 58087;//"Flask of the Winds";
        private const uint STRENGHT_300 = 58088;//"Flask of Titanic Strenght"
        private const uint INTELLECT_300 = 58086;//"Flask of the Draconic Mind"
        private const uint SPIRIT_300 = 67438;//"Flask of Flowing Water"
        private const uint STAMINA_450 = 58085;//"Flask of Steelskin"

        public bool UseAlchemyFlask()
        {
            if (ExtraUtilsSettings.Instance.UseAlchemyFlask)
            {

                if (!StyxWoW.Me.Auras.Any(aura => aura.Key.StartsWith("Enhanced ") || aura.Key.StartsWith("Flask of ")) && !Me.Mounted)
                {
                    foreach (WoWItem item in Me.BagItems)
                    {
                        if (item != null)
                        {
                            switch (ExtraUtilsSettings.Instance.FlaskTypeToUse)
                            {
                                case ExtraUtilsSettings.FlaskType.Intellect:
                                    {
                                        WoWItem flask = Me.BagItems.FirstOrDefault(o => o.Entry == INTELLECT_1000);
                                        if(flask==null)
                                            flask = Me.BagItems.FirstOrDefault(o => o.Entry == INTELLECT_300);          
                                        if (flask != null)
                                        {
                                            flask.Use();
                                            Logging.Write("Use "+ flask.Name);
                                            return true;
                                        }
                                    }
                                    break;
                                case ExtraUtilsSettings.FlaskType.Spirit:
                                    {
                                        WoWItem flask = Me.BagItems.FirstOrDefault(o => o.Entry == SPIRIT_1000);
                                        if (flask == null)
                                            flask = Me.BagItems.FirstOrDefault(o => o.Entry == SPIRIT_300);                      
                                        if (flask != null)
                                        {
                                            flask.Use();
                                            Logging.Write("Use " + flask.Name);
                                            return true;
                                        }
                                    }
                                    break;
                                case ExtraUtilsSettings.FlaskType.Strenght:
                                    {
                                        WoWItem flask = Me.BagItems.FirstOrDefault(o => o.Entry == STRENGHT_1000);
                                        if (flask == null)
                                            flask = Me.BagItems.FirstOrDefault(o => o.Entry == STRENGHT_300);                           
                                        if (flask != null)
                                        {
                                            flask.Use();
                                            Logging.Write("Use " + flask.Name);
                                            return true;
                                        }
                                    }
                                    break;
                                case ExtraUtilsSettings.FlaskType.Agility:
                                    {
                                        WoWItem flask = Me.BagItems.FirstOrDefault(o => o.Entry == AGILITY_1000);
                                        if (flask == null)
                                            flask = Me.BagItems.FirstOrDefault(o => o.Entry == AGILITY_300);                                
                                        if (flask != null)
                                        {
                                            flask.Use();
                                            Logging.Write("Use " + flask.Name);
                                            return true;
                                        }
                                    }
                                    break;
                                case ExtraUtilsSettings.FlaskType.Stamina:
                                    {
                                        WoWItem flask = Me.BagItems.FirstOrDefault(o => o.Entry == STAMINA_1500);
                                        if (flask == null)
                                            flask = Me.BagItems.FirstOrDefault(o => o.Entry == STAMINA_450);                                   
                                        if (flask != null)
                                        {
                                            flask.Use();
                                            Logging.Write("Use " + flask.Name);
                                            return true;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region Trinket

        public bool IsTargetBoss()
        {
            return (Me.CurrentTarget != null && Me.CurrentTarget.IsAlive && Me.CurrentTarget.IsBoss);
            
        }

        public bool UseTrinket1()
        {
            if (Me.Combat && ExtraUtilsSettings.Instance.UseTrinket_1_OnCondition == ExtraUtilsSettings.TrinketUseType.OnBoss && IsTargetBoss())
            {
                if (StyxWoW.Me.Inventory.Equipped.Trinket1 != null && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                {
                    Lua.DoString("RunMacroText('/use 13');");
                    Logging.Write("OnBoss Using 1st Trinket");
                    return true;
                }
            }
            else if (Me.Combat && ExtraUtilsSettings.Instance.UseTrinket_1_OnCondition == ExtraUtilsSettings.TrinketUseType.Always)
            {
                if (StyxWoW.Me.Inventory.Equipped.Trinket1 != null && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                {
                    Lua.DoString("RunMacroText('/use 13');");
                    Logging.Write("OnCD Using 1st Trinket");
                    return true;
                }
            }
            else if (Me.Combat && ExtraUtilsSettings.Instance.UseTrinket_1_OnCondition == ExtraUtilsSettings.TrinketUseType.At_HP &&
                Me.HealthPercent <= ExtraUtilsSettings.Instance.Trinket_1_HP)
            {
                if (StyxWoW.Me.Inventory.Equipped.Trinket1 != null && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                {
                    Lua.DoString("RunMacroText('/use 13');");
                    Logging.Write("At HP% condition Using 1st Trinket");
                    return true;
                }
            }
            else if (Me.Combat && ExtraUtilsSettings.Instance.UseTrinket_1_OnCondition == ExtraUtilsSettings.TrinketUseType.At_MANA &&
            Me.ManaPercent <= ExtraUtilsSettings.Instance.Trinket_1_MANA)
            {
                if (StyxWoW.Me.Inventory.Equipped.Trinket1 != null && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                {
                    Lua.DoString("RunMacroText('/use 13');");
                    Logging.Write("At Mana% condition Using 1st Trinket");
                    return true;
                }
            }
            else if (Me.Combat && ExtraUtilsSettings.Instance.UseTrinket_1_OnCondition == ExtraUtilsSettings.TrinketUseType.OnCrowdControlled &&
                utils.IsCrowdControlledPlayer(Me))
            {
                if (StyxWoW.Me.Inventory.Equipped.Trinket1 != null && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                {
                    Lua.DoString("RunMacroText('/use 13');");
                    Logging.Write("CrowdControlled Using 1st Trinket");
                    return true;
                }
            }

            else if (Me.Combat && ExtraUtilsSettings.Instance.UseTrinket_1_OnCondition == ExtraUtilsSettings.TrinketUseType.Manual) { return false; }
            return false;
        }

        public bool UseTrinket2()
        {
            if (Me.Combat && ExtraUtilsSettings.Instance.UseTrinket_2_OnCondition == ExtraUtilsSettings.TrinketUseType.OnBoss && IsTargetBoss())
            {
                if (StyxWoW.Me.Inventory.Equipped.Trinket2 != null && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown <= 0)
                {
                    Lua.DoString("RunMacroText('/use 14');");
                    Logging.Write("OnBoss Using 2nd Trinket");
                    return true;
                }
            }
            else if (Me.Combat && ExtraUtilsSettings.Instance.UseTrinket_2_OnCondition == ExtraUtilsSettings.TrinketUseType.Always)
            {
                if (StyxWoW.Me.Inventory.Equipped.Trinket2 != null && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown <= 0)
                {
                    Lua.DoString("RunMacroText('/use 14');");
                    Logging.Write("OnCD Using 2nd Trinket");
                    return true;
                }
            }
            else if (Me.Combat && ExtraUtilsSettings.Instance.UseTrinket_2_OnCondition == ExtraUtilsSettings.TrinketUseType.At_HP &&
                Me.HealthPercent <= ExtraUtilsSettings.Instance.Trinket_2_HP)
            {
                if (StyxWoW.Me.Inventory.Equipped.Trinket2 != null && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown <= 0)
                {
                    Lua.DoString("RunMacroText('/use 14');");
                    Logging.Write("At HP% condition Using 2nd Trinket");
                    return true;
                }
            }
            else if (Me.Combat && ExtraUtilsSettings.Instance.UseTrinket_2_OnCondition == ExtraUtilsSettings.TrinketUseType.At_MANA &&
            Me.ManaPercent <= ExtraUtilsSettings.Instance.Trinket_2_MANA)
            {
                if (StyxWoW.Me.Inventory.Equipped.Trinket2 != null && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown <= 0)
                {
                    Lua.DoString("RunMacroText('/use 14');");
                    Logging.Write("At Mana% condition Using 2nd Trinket");
                    return true;
                }
            }
            else if (Me.Combat && ExtraUtilsSettings.Instance.UseTrinket_2_OnCondition == ExtraUtilsSettings.TrinketUseType.OnCrowdControlled &&
            utils.IsCrowdControlledPlayer(Me))
            {
                if (StyxWoW.Me.Inventory.Equipped.Trinket2 != null && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown <= 0)
                {
                    Lua.DoString("RunMacroText('/use 14');");
                    Logging.Write("CrowdControlled Using 2nd Trinket");
                    return true;
                }
            }
            else if (Me.Combat && ExtraUtilsSettings.Instance.UseTrinket_2_OnCondition == ExtraUtilsSettings.TrinketUseType.Manual) { return false; }
            return false;
        }

        #endregion

        #region Racials

        private bool RacialUsageSatisfied(WoWSpell racial)
        {
            if (racial != null)
            {
                switch (racial.Name)
                {
                    case "Stoneform":
                        return StyxWoW.Me.GetAllAuras().Any(a => a.Spell.Mechanic == WoWSpellMechanic.Bleeding || a.Spell.DispelType == WoWDispelType.Disease || a.Spell.DispelType == WoWDispelType.Poison);
                    case "Escape Artist":
                        return StyxWoW.Me.Rooted;
                    case "Every Man for Himself":
                        return utils.IsCrowdControlledPlayer(StyxWoW.Me);
                    case "Shadowmeld":
                        return false;
                    case "Gift of the Naaru":
                        return StyxWoW.Me.HealthPercent <= ExtraUtilsSettings.Instance.GigtOfTheNaaruHP;
                    case "Darkflight":
                        return false;
                    case "Blood Fury":
                        return true;
                    case "War Stomp":
                        return true;
                    case "Berserking":
                        return true;
                    case "Will of the Forsaken":
                        return utils.IsCrowdControlledPlayer(StyxWoW.Me);
                    case "Cannibalize":
                        return false;
                    case "Arcane Torrent":
                        return StyxWoW.Me.ManaPercent < ExtraUtilsSettings.Instance.ArcaneTorrentMana && StyxWoW.Me.Class != WoWClass.DeathKnight;
                    case "Rocket Barrage":
                        return true;

                    default:
                        return false;
                }
            }

            return false;
        }

        public static IEnumerable<WoWSpell> CurrentRacials
        {
            get
            {
                //lil bit hackish ... but HB is broken ... maybe -- edit by wulf.
                var listPairs = SpellManager.Spells.Where(racial => MiscLists.Racials.Contains(racial.Value.Name)).ToList();
                return listPairs.Select(s => s.Value).ToList();
            }
        }

        public bool UseRacials()
        {
            if (ExtraUtilsSettings.Instance.UseRacials)
            {
                foreach (WoWSpell r in CurrentRacials.Where(racial => SpellManager.CanCast(racial.Name) && RacialUsageSatisfied(racial)))
                {
                    Logging.Write(DateTime.Now.ToString("ss:fff") + "[Racial Abilitie] " + r.Name);
                    return SpellManager.Cast(r.Name);
                }
            }
            return false;
        }
        #endregion

        public bool LifeSpirit()
        {
            if (Me.Combat && Me.HealthPercent <= ExtraUtilsSettings.Instance.LifeSpiritHP)
            {
                WoWItem lspirit = Me.BagItems.FirstOrDefault(h => h.Entry == 89640);
                if (lspirit != null && lspirit.CooldownTimeLeft.TotalMilliseconds <= 0)
                {
                    lspirit.Use();
                    Logging.Write("Using Life Spirit");
                }
            }
            return false;
        }
        
        public bool WaterSpirit()
        {
            if (Me.Combat && Me.ManaPercent <= ExtraUtilsSettings.Instance.WaterSpiritMANA)
            {
                WoWItem wspirit = Me.BagItems.FirstOrDefault(h => h.Entry == 89641);
                if (wspirit != null && wspirit.CooldownTimeLeft.TotalMilliseconds <= 0)
                {
                    wspirit.Use();
                    Logging.Write("Using Water Spirit");
                }
            }
            return false;
        }

        private bool CanUseEquippedItem(WoWItem item)
        {
            // Check for engineering tinkers!
            string itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;


            return item.Usable && item.Cooldown <= 0;
        }

        public bool UseEngineeringGloves()
        {
            if (Me.Combat && StyxWoW.Me.Inventory.Equipped.Hands!=null && CanUseEquippedItem(StyxWoW.Me.Inventory.Equipped.Hands) && StyxWoW.Me.Inventory.Equipped.Hands.Usable && StyxWoW.Me.Inventory.Equipped.Hands.CooldownTimeLeft.Milliseconds <= 0)
            {
                if (ExtraUtilsSettings.Instance.UseGloves_OnCondition == ExtraUtilsSettings.GenericUseType.OnBoss && IsTargetBoss())
                {
                    StyxWoW.Me.Inventory.Equipped.Hands.Use();
                    return true;
                }
                else if (ExtraUtilsSettings.Instance.UseGloves_OnCondition == ExtraUtilsSettings.GenericUseType.Always)
                {
                    StyxWoW.Me.Inventory.Equipped.Hands.Use();
                    return true;
                }
            }
            return false;
        }

        public bool UseLifeblood()
        {
            if (Me.Combat && SpellManager.CanCast("Lifeblood") && SpellManager.Spells["Lifeblood"].CooldownTimeLeft.Milliseconds <= 0)
            {
                if (ExtraUtilsSettings.Instance.UseLifeblood_OnCondition == ExtraUtilsSettings.GenericUseType.OnBoss && IsTargetBoss())
                {
                    return SpellManager.Cast("Lifeblood");
                }
                else if (ExtraUtilsSettings.Instance.UseLifeblood_OnCondition == ExtraUtilsSettings.GenericUseType.Always)
                {
                    return SpellManager.Cast("Lifeblood");
                }
            }
            return false;
        }

        //KeyBinds
        public void AnyKeyBindsPressed()
        {
            if (KeyBinds_sw.Elapsed.TotalSeconds > 1)
            {
                Keybinds.Pulse();
                KeyBinds_sw.Reset();
                KeyBinds_sw.Start();
            }
        }

        public void GoStopWatchUpdateKeyBinds()
        {
            if (KeyBinds_sw != null)
            {
                KeyBinds_sw.Start();
            }
        }

        public void InterruptStopWatchUpdateKeyBinds()
        {
            if (KeyBinds_sw != null)
            {
                KeyBinds_sw.Stop();
            }
        }
    }
}
