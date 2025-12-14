using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.CommonBot;

namespace VitalicRotation.Helpers
{
    public static class SpellBook
    {
        public const int Redirect = 73981;
        // --- Rogue (général / Sub / Combat) ---
        public const int Premeditation = 14183;   // Subtlety talent: Premeditation
        public const int BladeFlurry = 13877;   // Combat toggle: Blade Flurry
        // === Builders ===
        public const int Mutilate = 1329;
        public const int Backstab = 53;
        public const int Hemorrhage = 16511;
        public const int Ambush = 8676;
        public const int SinisterStrike = 1752;
        public const int RevealingStrike = 84617;
        public const int FanOfKnives = 51723;
        public const int Dispatch = 111240;

        // === Finishers ===
        public const int Eviscerate = 2098;
        public const int Rupture = 1943;
        public const int Envenom = 32645;
        public const int SliceAndDice = 5171;
        public const int DeadlyThrow = 26679;
        public const int CrimsonTempest = 121411;
        public const int Recuperate = 73651;

        // === Stealth / CC ===
        public const int CheapShot = 1833;
        public const int Garrote = 703;
        public const int Sap = 6770;
        public const int PickPocket = 921;
        public const int Kick = 1766;
        public const int KidneyShot = 408;
        public const int Blind = 2094;
        public const int Gouge = 1776;
        public const int Dismantle = 51722;
        public const int TricksOfTheTrade = 57934;
        public const int TricksOfTheTradeBuffAlias = 59628; // alias buff id (E19)
        public const int Shiv = 5938;

        // === Defensives ===
        public const int CloakOfShadows = 31224;
        public const int Evasion = 5277;
        public const int Vanish = 1856;
        public const int Preparation = 14185;
        public const int CombatReadiness = 74001;
        public const int SmokeBomb = 76577;
        public const int Feint = 1966;

        // === Offensives ===
        public const int AdrenalineRush = 13750;
        public const int KillingSpree = 51690;
        public const int ShadowDance = 51713;
        public const int Vendetta = 79140;
        public const int ShadowBlades = 121471;
        public const int ColdBlood = 14177;
        public const int SynapseSprings = 96228; // Engineering "spell"; on utilise surtout le slot 10
        public const int Trinket1 = 0; // Placeholder (slot 13)
        public const int Trinket2 = 0; // Placeholder (slot 14)

        // === Raciaux (MoP) ===
        public const int Berserking = 26297;            // Troll
        public const int BloodFury = 20572;             // Orc (AP version)
        public const int ArcaneTorrent = 25046;         // Blood Elf (energy + 8y silence)
        public const int WarStomp = 20549;              // Tauren (AoE stun 8y self)
        public const int EveryManForHimself = 59752;    // Human (break)
        public const int WillOfTheForsaken = 7744;      // Undead (break Fear/Charm/Sleep)

        // === Movement ===
        public const int Sprint = 2983;
        public const int Shadowstep = 36554;
        public const int BurstOfSpeed = 108212;
        public const int Stealth = 1784;
        public const int ShadowWalk = 114842;

        // === Poisons ===
        public const int DeadlyPoison = 2823;
        public const int WoundPoison = 8679;
        public const int CripplingPoison = 3408;
        public const int MindNumbingPoison = 5761;
        public const int LeechingPoison = 108211;
        public const int ParalyticPoison = 108215; // Poison talent off-hand

        // === Ranged fillers (MoP) ===
        public const int ShurikenToss = 114014; // présent dans l'original
        public const int Throw = 121733; // fallback si pas Shuriken Toss

        public const int MarkedForDeath = 137619;
        public const int ShroudOfConcealment = 114018;

        // === Talents / Buffs ===
        public const int Anticipation = 114015; // Anticipation (MoP) aura stacks

        // Helper: returns true if the player has the Shuriken Toss talent in MoP
        public static bool HasShurikenTossTalent
        {
            get
            {
                try { return SpellManager.HasSpell(ShurikenToss); } catch { return false; }
            }
        }

        // === CanCast (2 surcharges) ===
        public static bool CanCast(int spellId)
        {
            var spell = WoWSpell.FromId(spellId);
            if (spell == null)
            {
                if (Settings.VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Diag][CanCast] spellId={0} missing", spellId);
                return false;
            }
            bool ok = SpellManager.CanCast(spell.Name);
            if (!ok && Settings.VitalicSettings.Instance.DiagnosticMode)
            {
                LogFailReason(spell, null);
            }
            return ok;
        }

        public static bool CanCast(int spellId, WoWUnit unit)
        {
            var spell = WoWSpell.FromId(spellId);
            if (spell == null || unit == null || !unit.IsValid)
            {
                if (Settings.VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Diag][CanCast] spellId={0} invalid target/null", spellId);
                return false;
            }
            bool ok = SpellManager.CanCast(spell.Name, unit);
            if (!ok && Settings.VitalicSettings.Instance.DiagnosticMode)
            {
                LogFailReason(spell, unit);
            }
            return ok;
        }

        private static void LogFailReason(WoWSpell spell, WoWUnit unit)
        {
            try
            {
                // Enhanced diagnostics for expected logs validation
                var me = StyxWoW.Me;
                bool inRange = true; 
                try { if (unit != null) inRange = unit.Distance <= 5.0; } catch { }
                bool enoughEnergy = true; 
                try { if (spell.PowerCost > 0) enoughEnergy = me.CurrentEnergy >= spell.PowerCost; } catch { }
                bool gcd = false; 
                try { gcd = SpellManager.GlobalCooldown; } catch { }
                bool facing = true;
                try { if (unit != null) facing = me.IsFacing(unit); } catch { }
                bool los = true;
                try { if (unit != null) los = unit.InLineOfSpellSight; } catch { }
                bool onCooldown = false;
                try { onCooldown = spell.Cooldown; } catch { }
                
                string tgt = unit != null ? unit.Name : "(self)";
                Logger.Write("[Diag][CanCastFail] {0} tgt={1} inRange={2} energyOk={3} gcd={4} facing={5} los={6} cd={7} dist={8:0.1}", 
                    spell.Name, tgt, inRange, enoughEnergy, gcd, facing, los, onCooldown, unit != null ? unit.Distance : 0);
            }
            catch { }
        }

        // === Cast (2 surcharges) — retournent bool (succès) ===
        public static bool Cast(int spellId)
        {
            var spell = WoWSpell.FromId(spellId);
            if (spell == null) return false;
            if (!SpellManager.CanCast(spell.Name)) return false;
            return SpellManager.Cast(spell.Name);
        }

        public static bool Cast(int spellId, WoWUnit unit)
        {
            var spell = WoWSpell.FromId(spellId);
            if (spell == null || unit == null || !unit.IsValid) return false;
            if (!SpellManager.CanCast(spell.Name, unit)) return false;
            return SpellManager.Cast(spell.Name, unit);
        }

        public static string GetSpellName(int spellId)
        {
            var s = WoWSpell.FromId(spellId);
            return s != null ? s.Name : "Unknown Spell (" + spellId + ")";
        }

        // === Poisons (Lua) ===
        // NOTE: on vérifie la présence d'un enchant temporaire (pas le nom exact)
        // Poisons (MoP : buffs joueur, pas enchants d'arme)
        public static bool HasMainHandPoison(string poisonName)
        {
            if (string.IsNullOrEmpty(poisonName)) return false;
            string safe = poisonName.Replace("'", "\\'");
            return Lua.GetReturnVal<bool>("return UnitBuff('player', '" + safe + "') ~= nil", 0);
        }

        public static bool HasOffHandPoison(string poisonName)
        {
            if (string.IsNullOrEmpty(poisonName)) return false;
            string safe = poisonName.Replace("'", "\\'");
            return Lua.GetReturnVal<bool>("return UnitBuff('player', '" + safe + "') ~= nil", 0);
        }


        // === Raccourcis type Vitalic ===
        public static class Spells
        {

            public const int Redirect = SpellBook.Redirect;

            public const int Mutilate = SpellBook.Mutilate;
            public const int Backstab = SpellBook.Backstab;
            public const int Hemorrhage = SpellBook.Hemorrhage;
            public const int Ambush = SpellBook.Ambush;
            public const int SinisterStrike = SpellBook.SinisterStrike;
            public const int RevealingStrike = SpellBook.RevealingStrike;
            public const int FanOfKnives = SpellBook.FanOfKnives;
            public const int Dispatch = SpellBook.Dispatch;

            public const int Eviscerate = SpellBook.Eviscerate;
            public const int Rupture = SpellBook.Rupture;
            public const int Envenom = SpellBook.Envenom;
            public const int SliceAndDice = SpellBook.SliceAndDice;
            public const int DeadlyThrow = SpellBook.DeadlyThrow;
            public const int CrimsonTempest = SpellBook.CrimsonTempest;
            public const int Recuperate = SpellBook.Recuperate;

            public const int CheapShot = SpellBook.CheapShot;
            public const int Garrote = SpellBook.Garrote;
            public const int Sap = SpellBook.Sap;
            public const int PickPocket = SpellBook.PickPocket;

            public const int Kick = SpellBook.Kick;
            public const int KidneyShot = SpellBook.KidneyShot;
            public const int Blind = SpellBook.Blind;
            public const int Gouge = SpellBook.Gouge;
            public const int Dismantle = SpellBook.Dismantle;
            public const int TricksOfTheTrade = SpellBook.TricksOfTheTrade;
            public const int TricksOfTheTradeBuffAlias = SpellBook.TricksOfTheTradeBuffAlias;
            public const int Shiv = SpellBook.Shiv;

            public const int CloakOfShadows = SpellBook.CloakOfShadows;
            public const int Evasion = SpellBook.Evasion;
            public const int Vanish = SpellBook.Vanish;
            public const int Preparation = SpellBook.Preparation;
            public const int CombatReadiness = SpellBook.CombatReadiness;
            public const int SmokeBomb = SpellBook.SmokeBomb;
            public const int Feint = SpellBook.Feint;

            public const int AdrenalineRush = SpellBook.AdrenalineRush;
            public const int KillingSpree = SpellBook.KillingSpree;
            public const int ShadowDance = SpellBook.ShadowDance;
            public const int Vendetta = SpellBook.Vendetta;
            public const int ShadowBlades = SpellBook.ShadowBlades;
            public const int ColdBlood = SpellBook.ColdBlood;

            public const int Sprint = SpellBook.Sprint;
            public const int Shadowstep = SpellBook.Shadowstep;
            public const int BurstOfSpeed = SpellBook.BurstOfSpeed;
            public const int Stealth = SpellBook.Stealth;
            public const int ShadowWalk = SpellBook.ShadowWalk;

            public const int DeadlyPoison = SpellBook.DeadlyPoison;
            public const int WoundPoison = SpellBook.WoundPoison;
            public const int CripplingPoison = SpellBook.CripplingPoison;
            public const int MindNumbingPoison = SpellBook.MindNumbingPoison;
            public const int LeechingPoison = SpellBook.LeechingPoison;
            public const int ParalyticPoison = SpellBook.ParalyticPoison;

            // Ranged fillers
            public const int ShurikenToss = SpellBook.ShurikenToss;
            public const int Throw = SpellBook.Throw;

            // Racials
            public const int ArcaneTorrent = SpellBook.ArcaneTorrent;
            public const int WarStomp = SpellBook.WarStomp;
        }

        // === Trinkets / On-Use Items ===
        public static bool CanUseTrinket1()
        {
            return Lua.GetReturnVal<bool>("local s,c,_=GetInventoryItemCooldown('player',13); return (s==0 and c==0)", 0);
        }

        public static bool CanUseTrinket2()
        {
            return Lua.GetReturnVal<bool>("local s,c,_=GetInventoryItemCooldown('player',14); return (s==0 and c==0)", 0);
        }

        public static void UseTrinket1()
        {
            Lua.DoString("UseInventoryItem(13)");
        }

        public static void UseTrinket2()
        {
            Lua.DoString("UseInventoryItem(14)");
        }

        // === Engineering: Synapse Springs (slot 10 = gants) ===
        public static bool CanUseSynapseSprings()
        {
            return Lua.GetReturnVal<bool>("local s,c,_=GetInventoryItemCooldown('player',10); return (s==0 and c==0)", 0);
        }

        public static void UseSynapseSprings()
        {
            Lua.DoString("UseInventoryItem(10)");
        }
        /// <summary>
        /// Portée mêlée simple (par défaut 5.0y). On vérifie validité, LoS en amont selon l'appelant.
        /// </summary>
        public static bool InMeleeRange(WoWUnit unit, double range = 3.5)
        {
            if (unit == null || !unit.IsValid || unit.IsDead) return false;
            return unit.Distance <= range;
        }

        /// <summary>
        /// P2.2 - Récupère le temps de recharge d'un sort en secondes
        /// Retourne 0 si le sort est disponible, -1 si erreur
        /// </summary>
        public static double GetSpellCooldown(int spellId)
        {
            try
            {
                var spell = WoWSpell.FromId(spellId);
                if (spell == null) return -1.0;
                
                string luaScript = string.Format(
                    "local start, duration = GetSpellCooldown('{0}'); " +
                    "if start == 0 then return 0; end " +
                    "local remaining = start + duration - GetTime(); " +
                    "return math.max(0, remaining);", 
                    spell.Name.Replace("'", "\\'"));
                    
                return Lua.GetReturnVal<double>(luaScript, 0);
            }
            catch { return -1.0; }
        }
    }
}
