using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Inventory;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace KingWoW
{
    class HealInterruptDispellsCleanse_Manager
    {

        #region HEAL

        #region DO_NOT_HEAL
        public bool TargetDontHeal(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsAlive || !target.IsPlayer && !target.IsPet)
                return true;

            return target.HasAura("Dominate Mind") ||
                       target.HasAura("Cyclone") ||
                       target.HasAura(123184) || //Dissonance Field
                       target.HasAura(121949) || // / Parasitic Growth
                       target.HasAura("Reshape Life") ||
                       target.HasAura(137341) || //Beast of Nightmares
                       target.HasAura("Beast of Nightmares") || //Not work above??
                       target.HasAura(137360); //Corrupted Healing
        }
        #endregion
        
        #endregion

        #region DISPELL

        #region DISPELL_ENEMY
        public readonly HashSet<string> DispellEnemyASAPnHashSet = new HashSet<string>
            {
                "Power Word: Shield",
                "Ice Barrier",
                "Divine Aegis",
                "Hand of Freedom", 
                "Hand of Protection",
                /*"Elemental Mastery",
                "Bloodlust", 
                "Time Warp",
                "Rejuvenation",
                "Regrowth",
                "Renew",*/
                "Illuminated Healing"
            };

        public bool NeedDispellEnemyASAPTarget(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsPlayer && !target.IsPet)
            {
                return false;
            }

            if (target.ActiveAuras.Any(a => DispellEnemyASAPnHashSet.Contains(a.Value.Name)))
            {
                return true;
            }
            return false;

        }
        #endregion

        #region DO_NOT_DISPELL
        public bool TargetDontDispell(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsAlive || !target.IsPlayer && !target.IsPet)
                return false;

            //using (StyxWoW.Memory.AcquireFrame())
            //{
                return target.HasAura("Flame Shock") ||
                       target.HasAura(136184) || //Thick Bones
                       target.HasAura(136186) || //Clear Mind
                       target.HasAura(136180) || //Keen Eyesight
                       target.HasAura(136182) || //Improved Synapses
                       target.HasAura("Unstable Affliction") ||
                       target.HasAura(138609) || // Matter Swap
                       target.HasAura(138732) || //Ionization
                       target.HasAura("Vampiric Touch");
            //}
        }
#endregion

        #region DISPELL_ASAP

        public readonly HashSet<int> Dispell_ASAP_HashSet = new HashSet<int>
            {
                105421, //Blinding Light
                123393, //Breath of Fire (Glyph of Breath of Fire)
                44572, //Deep Freeze
                605, //Dominate Mind
                31661, //Dragon's Breath
                5782, // target.Class != WoWClass.Warrior || //Fear
                118699, // target.Class != WoWClass.Warrior || //Fear
                130616, // target.Class != WoWClass.Warrior || //Fear (Glyph of Fear)
                3355, //Freezing Trap
                853, //Hammer of Justice
                110698, //Hammer of Justice (Paladin)
                2637, //Hibernate
                88625, //Holy Word: Chastise
                119072, //Holy Wrath
                5484, // target.Class != WoWClass.Warrior || //Howl of Terror
                115268, //Mesmerize (Shivarra)
                6789, //Mortal Coil
                115078, //Paralysis
                113953, //Paralysis (Paralytic Poison)
                126355, //Paralyzing Quill (Porcupine)
                118, //Polymorph
                61305, //Polymorph: Black Cat
                28272, //Polymorph: Pig
                61721, //Polymorph: Rabbit
                61780, //Polymorph: Turkey
                28271, //Polymorph: Turtle
                64044, // target.Class != WoWClass.Warrior || //Psychic Horror
                8122, // target.Class != WoWClass.Warrior || //Psychic Scream
                113792, // target.Class != WoWClass.Warrior || //Psychic Terror (Psyfiend)
                107079, //Quaking Palm
                115001, //Remorseless Winter
                20066, // target.Class != WoWClass.Warrior || //Repentance
                82691, //Ring of Frost
                1513, //Scare Beast
                132412, //Seduction (Grimoire of Sacrifice)
                6358, //Seduction (Succubus)
                9484, //Shackle Undead
                30283, //Shadowfury
                87204, //Sin and Punishment
                104045, //Sleep (Metamorphosis)
                118905, //Static Charge (Capacitor Totem)
                10326, //Turn Evil
                19386, //Wyvern Sting //Thank bp423
                118552, //Flesh to Stone" //Thank bp423
                119985, //Dread Spray" //Thank mnipper
                117436, //Lightning Prison
                124863, //Visions of Demise
                123011, //Terrorize (10%)
                123012, //Terrorize (5%)
//Farraki
                136708, //Stone Gaze - Thank Clubwar
                136719, //Blazing Sunlight - Thank Clubwar && bp423
//Gurubashi
                136587, //Venom Bolt Volley - Thank Clubwar
//Drakaki
                136710, //Deadly Plague - Thank Clubwar
//Amani
                136512, //Hex of Confusion - Thank Clubwar
                136857, //Entrapped - Thank amputations
                136185, //Fragile Bones - Thank Sk1vvy 
                136187, //Clouded Mind - Thank Sk1vvy 
                136183, //Dulled Synapses - Thank Sk1vvy 
                136181, //Impaired Eyesight - Thank Sk1vvy 
                138040, //Horrific Visage - Thank macVsog
                117949 //Closed Curcuit
            };

        public readonly HashSet<int> Dispell_Warrior_ASAP_HashSet = new HashSet<int>
            {
                5782,
                118699,
                130616,
                64044,
                8122,
                113792
            };

        public bool TargetDispellASAP(WoWUnit target,double time)
        {
            if (target == null || !target.IsValid || !target.IsPlayer)
            {
                return false;
            }

            if (target.IsPlayer &&
                target.Class == WoWClass.Warrior &&
                target.ActiveAuras.Any(
                    a =>
                    Dispell_Warrior_ASAP_HashSet.Contains(a.Value.SpellId)))
            {
                return false;
            }
            
            if (target.ActiveAuras.Any(
                a =>
                Dispell_ASAP_HashSet.Contains(a.Value.SpellId) &&
                a.Value.TimeLeft.TotalMilliseconds > time))
            {
                return true;
            }
            return false;

        }

        #endregion

        #endregion

        #region INVULNERABLE

        public readonly HashSet<string> InvulnerableHashSet = new HashSet<string>
            {
                //"Bladestorm",
                "Cyclone",
                //"Desecrated Ground",
                "Deterrence",
                "Dispersion",
                "Divine Shield", //Grapple Weapon (Monk)
                "Ice Block"
            };

        public bool InvulnerableTarget(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsPlayer && !target.IsPet)
            {
                return false;
            }

            if (target.ActiveAuras.Any(a => InvulnerableHashSet.Contains(a.Value.Name)))
            {
                return true;
            }
            return false;

        }

        #endregion

        #region INVULNERABLE_PHYSIC

        public bool InvulnerableToPhysicTarget(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsPlayer && !target.IsPet)
            {
                return false;
            }

            return target.HasAura("Hand of Protection");
        }

        #endregion

        #region INVULNERABLE_SPELL

        public readonly HashSet<string> InvulnerableToSpellHashSet = new HashSet<string>
            {
                "Anti-Magic Shell",
                "Cloak of Shadows",
                "Glyph of Ice Block",
                "Grounding Totem Effect",
                "Mass Spell Reflection",
                "Phantasm",
                "Spell Reflection",
                "Zen Meditation"
            };

        public bool InvulnerableToSpellTarget(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsPlayer && !target.IsPet)
            {
                return false;
            }

            if (target.ActiveAuras.Any(a => InvulnerableToSpellHashSet.Contains(a.Value.Name)))
            {
                //Logging.Write(target.Name + " got DebuffDisarm");
                return true;
            }
            return false;

        }

        #endregion

        #region SHIELD
        public readonly HashSet<string> EnemyShieldHashSet = new HashSet<string>
            {
                "Power Word: Shield",
                "Ice Barrier",
                "Divine Aegis",
                "Hand of Freedom", 
                "Hand of Protection",
                "Illuminated Healing"
            };

        public bool AnyShieldTarget(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsPlayer && !target.IsPet)
            {
                return false;
            }

            if (target.ActiveAuras.Any(a => EnemyShieldHashSet.Contains(a.Value.Name)))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region ROOT_SNARE

        public readonly HashSet<int> RootorSnareHashSet = new HashSet<int>
            {
                96294, //Chains of Ice (Chilblains)
                116706, //Disable
                64695, //Earthgrab (Earthgrab Totem)
                339, //Entangling Roots
                113770, //Entangling Roots (Force of Nature - Balance Treants)
                19975, //Entangling Roots (Nature's Grasp)
                113275, //Entangling Roots (Symbiosis)
                113275, //Entangling Roots (Symbiosis)
                19185, //Entrapment
                33395, //Freeze
                63685, //Freeze (Frozen Power)
                39965, //Frost Grenade
                122, //Frost Nova
                110693, //Frost Nova (Mage)
                55536, //Frostweave Net
                87194, //Glyph of Mind Blast
                111340, //Ice Ward
                45334, //Immobilized (Wild Charge - Bear)
                90327, //Lock Jaw (Dog)
                102359, //Mass Entanglement
                128405, //Narrow Escape
                13099, //Net-o-Matic
                115197, //Partial Paralysis
                50245, //Pin (Crab)
                91807, //Shambling Rush (Dark Transformation)
                123407, //Spinning Fire Blossom
                107566, //Staggering Shout
                54706, //Venom Web Spray (Silithid)
                114404, //Void Tendril's Grasp
                4167, //Web (Spider)
                50433, //Ankle Crack (Crocolisk)
                110300, //Burden of Guilt
                35101, //Concussive Barrage
                5116, //Concussive Shot
                120, //Cone of Cold
                3409, //Crippling Poison
                18223, //Curse of Exhaustion
                45524, //Chains of Ice
                50435, //Chilblains
                121288, //Chilled (Frost Armor)
                1604, //Dazed
                63529, //Dazed - Avenger's Shield
                50259, //Dazed (Wild Charge - Cat)
                26679, //Deadly Throw
                119696, //Debilitation
                116095, //Disable
                123727, //Dizzying Haze
                3600, //Earthbind (Earthbind Totem)
                77478, //Earthquake (Glyph of Unstable Earth)
                123586, //Flying Serpent Kick
                113092, //Frost Bomb
                54644, //Frost Breath (Chimaera)
                8056, //Frost Shock
                116, //Frostbolt
                8034, //Frostbrand Attack
                44614, //Frostfire Bolt
                61394, //Frozen Wake (Glyph of Freezing Trap)
                1715, //Hamstring
                13810, //Ice Trap
                58180, //Infected Wounds
                118585, //Leer of the Ox
                15407, //Mind Flay
                12323, //Piercing Howl
                115000, //Remorseless Winter
                20170, //Seal of Justice
                47960, //Shadowflame
                31589, //Slow
                129923, //Sluggish (Glyph of Hindering Strikes)
                61391, //Typhoon
                51490, //Thunderstorm
                127797, //Ursol's Vortex
                137637, //Warbringer
            };

        public bool AnyRootorSnareAtTarget(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsPlayer && !target.IsPet)
            {
                return false;
            }            
            return target.ActiveAuras.Any(
                a =>
                a.Value.ApplyAuraType == WoWApplyAuraType.ModRoot ||
                a.Value.ApplyAuraType == WoWApplyAuraType.ModDecreaseSpeed ||
                RootorSnareHashSet.Contains(a.Value.SpellId));
        }

        #endregion
    }
}
