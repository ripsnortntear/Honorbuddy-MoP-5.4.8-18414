//
// FREE TO USE!
// BUT DONT FORGET THE CREDITS!
// Credits by BarryDurex
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.WoWInternals;
using System.Windows.Forms;
using System.IO;
using Styx.Common;

namespace PokehbuddyLogicCreator
{
    public partial class PetBattle
    {
        #region convertToIcon
        /// <summary>
        /// convert a Image to Icon
        /// </summary>
        /// <param name="image">Image file</param>
        /// <param name="color">Make Transparent</param>
        /// <returns>the Image as Icon</returns>
        public static System.Drawing.Icon convertToIcon(System.Drawing.Image image, System.Drawing.Color color)
        {
            System.Drawing.Bitmap _bitmap = (System.Drawing.Bitmap)image;
            if (color != System.Drawing.Color.Empty)
                _bitmap.MakeTransparent(color);
            System.IntPtr _intPtr = _bitmap.GetHicon();
            System.Drawing.Icon _icon = System.Drawing.Icon.FromHandle(_intPtr);
            return _icon;
        }
        #endregion

        public static bool IsInPetBattle
        {
            get
            {
                string State = Lua.GetReturnVal<string>("return C_PetBattles.IsInBattle()", 0);
                bool _state = false;
                if (State != null && State == "1")
                { _state = true; }
                return _state;
            }
        }

        /// <summary>
        /// return (petID, AbilityID1, AbilityID2, AbilityID3, locked?)
        /// </summary>
        public static List<string> GetPetLoadOutInfo(int slotID)
        { return Lua.GetReturnValues("return C_PetJournal.GetPetLoadOutInfo(" + slotID.ToString() + ");"); }

        public static string PetBattleState
        { get { return Lua.GetReturnVal<string>("return C_PetBattles.GetBattleState()", 0); } }

        public static bool IsWildBattle
        {
            get
            {
                string State = Lua.GetReturnVal<string>("return C_PetBattles.IsWildBattle()", 0);
                bool _state = false;
                if (State != null && State == "1")
                { _state = true; }
                return _state;
            }
        }

        public static bool CanActivePetSwapOut
        {
            get
            {
                string State = Lua.GetReturnVal<string>("return C_PetBattles.CanActivePetSwapOut()", 0);
                bool _state = false;
                if (State != null && State == "1")
                { _state = true; }
                return _state;
            }
        }

        public static bool ShouldShowPetSelect
        {
            get
            { 
                string State = Lua.GetReturnVal<string>("return C_PetBattles.ShouldShowPetSelect()", 0);
                bool _state = false;
                if (State != null && State == "1")
                { _state = true; }
                return _state;
            }
        }

        public static bool IsTrapAvailable
        {
            get
            {
                string State = Lua.GetReturnVal<string>("return C_PetBattles.IsTrapAvailable()", 0);
                bool _state = false;
                if (State != null && State == "1")
                { _state = true; }
                return _state;
            }
        }

        public static bool IsWaitingToFight
        { get { if (Lua.GetReturnValues("return C_PetBattles.IsWaitingOnOpponent()")[0] == "0") { return false; } return true; } }

        public static void UseTrap()
        { Lua.DoString("C_PetBattles.UseTrap()"); }
        
        /// <summary>
        /// only available in a fight
        /// </summary>
        public static int GetActivePetSlod_Me
        { get { string dummy = ReadActiveSlot(); return Lua.GetReturnVal<int>("return C_PetBattles.GetActivePet(LE_BATTLE_PET_ALLY)", 0); } }
        /// <summary>
        /// only available in a fight
        /// </summary>
        public static int GetActivePetSlod_Enemy
        { get { return Lua.GetReturnVal<int>("return C_PetBattles.GetActivePet(LE_BATTLE_PET_ENEMY)", 0); } }

        public static void ChangePetBySlotID(int slotID)
        { Lua.DoString("C_PetBattles.ChangePet(" + slotID.ToString() + ");"); }

        /// <summary>
        /// only available in a fight
        /// </summary>
        public static int GetHealthBySlotID_Me(int slotID)
        { return Lua.GetReturnVal<int>("return C_PetBattles.GetHealth(LE_BATTLE_PET_ALLY, " + slotID + ")", 0); }
        /// <summary>
        /// only available in a fight
        /// </summary>
        public static int GetHealthBySlotID_Enemy(int slotID)
        { return Lua.GetReturnVal<int>("return C_PetBattles.GetHealth(LE_BATTLE_PET_ENEMY, " + slotID + ")", 0); }

        /// <summary>
        /// only available in a fight
        /// </summary>
        public static int GetMaxHealthBySlotID_Me(int slotID)
        { return Lua.GetReturnVal<int>("return C_PetBattles.GetMaxHealth(LE_BATTLE_PET_ALLY, " + slotID + ")", 0); }
        /// <summary>
        /// only available in a fight
        /// </summary>
        public static int GetMaxHealthBySlotID_Enemy(int slotID)
        { return Lua.GetReturnVal<int>("return C_PetBattles.GetMaxHealth(LE_BATTLE_PET_ENEMY, " + slotID + ")", 0); }

        /// <summary>
        /// only available in a fight
        /// </summary>
        public static double GetHealthPercent_Me(int slotID)
        { return ((100 / (double)GetMaxHealthBySlotID_Me(slotID)) * (double)GetHealthBySlotID_Me(slotID)); }
        /// <summary>
        /// only available in a fight
        /// </summary>
        public static double GetHealthPercent_Enemy(int slotID)
        { return ((100 / (double)GetMaxHealthBySlotID_Me(slotID)) * (double)GetHealthBySlotID_Enemy(slotID)); } 

        public static int GetLevelBySlotID_Me(int slotID)
        { return Convert.ToInt32(GetPetInfoByPetID(Convert.ToInt32(GetPetLoadOutInfo(slotID)[0]))[2]); } 
        /// <summary>
        /// only available in a fight
        /// </summary>
        public static int GetLevelBySlotID_Enemy(int slotID)
        { return Lua.GetReturnVal<int>("return C_PetBattles.GetLevel(LE_BATTLE_PET_ENEMY, " + slotID + ");", 0); }

        public static void UseAbilityByKeyNum(int KeyNum)
        { Lua.DoString("C_PetBattles.UseAbility(" + KeyNum.ToString() + ");"); Styx.Helpers.KeyboardManager.AntiAfk(); }

        /// <summary>
        /// return (secondInfo, SelectedAction) - PetChange[3/(Pet1=2,Pet2=3,Pet3=4)] - Action[2/(Abili1=1,Abili2=2,Abili3=3)]
        /// </summary>
        public static List<string> GetSelectedAction
        { get { return Lua.GetReturnValues("return C_PetBattles.GetSelectedAction();"); } }

        /// <summary>
        /// return (id, name, icon, maxCD, desc, turns, petType, noStrongWeakHints)
        /// </summary>
        public static List<string> GetAbilityInfosByID(int AbilityID)
        { return Lua.GetReturnValues("return _G.C_PetBattles.GetAbilityInfoByID(" + AbilityID.ToString() + ");"); }
        /// <summary>
        /// return (id, name, icon, maxCD, desc, turns, petType, noStrongWeakHints)
        /// </summary>
        public static List<string> GetAbilityInfosByID(string AbilityID)
        { return Lua.GetReturnValues("return _G.C_PetBattles.GetAbilityInfoByID(" + AbilityID + ");"); }

        /// <summary>
        /// only available in a fight
        /// return (id, name, icon, maxCD, desc, turns, petType, noStrongWeakHints)
        /// </summary>
        public static List<string> GetAbilityInfo_Enemy(int PetSlod, int AbilitySlot)
        { return Lua.GetReturnValues("return C_PetBattles.GetAbilityInfo(LE_BATTLE_PET_ENEMY, " + PetSlod + ", " + AbilitySlot + ");"); }

        /// <summary>
        /// only available in a fight
        /// return (id, name, icon, maxCD, desc, turns, petType, noStrongWeakHints)
        /// </summary>
        public static List<string> GetAbilityInfo_Me(int PetSlod, int AbilitySlot)
        { return Lua.GetReturnValues("return C_PetBattles.GetAbilityInfo(LE_BATTLE_PET_ALLY, " + PetSlod + ", " + AbilitySlot + ");"); }

        public static int GetNumPetsTotal //TODO (PetJournal.isWild)
        { get { return Convert.ToInt32(Lua.GetReturnValues("return C_PetJournal.GetNumPets(false);")[0]); } }
        public static int GetNumPetsOwned
        { get { return Convert.ToInt32(Lua.GetReturnValues("return C_PetJournal.GetNumPets(false);")[1]); } }

        /// <summary>
        /// return (speciesID, customName, level, xp, maxXp, displayID, name, icon, petType, creatureID, sourceText, description, isWild, canBattle, tradable)
        /// </summary>
        public static List<string> GetPetInfoByPetID(int PetID)
        { return Lua.GetReturnValues("return C_PetJournal.GetPetInfoByPetID(" + PetID.ToString() + ")"); }
        /// <summary>
        /// return (speciesID, customName, level, xp, maxXp, displayID, name, icon, petType, creatureID, sourceText, description, isWild, canBattle, tradable)
        /// </summary>
        public static List<string> GetPetInfoByPetID(string PetID)
        { return Lua.GetReturnValues("return C_PetJournal.GetPetInfoByPetID(" + PetID + ")"); }

        /// <summary>
        /// return (speciesID, customName, level, xp, maxXp, displayID,_ , name, icon, petType, creatureID, sourceText, description, isWild, canBattle, tradable)
        /// </summary>
        public static List<string> GetPetInfoBySlotID(int SlotID)
        { return Lua.GetReturnValues("local petID = C_PetJournal.GetPetLoadOutInfo(" + SlotID.ToString() + "); return C_PetJournal.GetPetInfoByPetID(petID)"); }

        /// <summary>
        /// return (name, petIcon, petType, creatureID, sourceText, ?, isWild, canBattle)
        /// </summary>
        public static List<string> GetPetInfoBySpeciesID(int SpeciesID)
        { return Lua.GetReturnValues("return GetPetInfoBySpeciesID(" + SpeciesID.ToString() + ");"); }

        /// <summary>
        /// only available in a fight
        /// </summary>
        public static int GetPetSpeciesIDBySlotID_Me(int slodID)
        { return Convert.ToInt32(Lua.GetReturnValues("return C_PetBattles.GetPetSpeciesID(LE_BATTLE_PET_ALLY, " + slodID.ToString() + ");")[0]); }
        /// <summary>
        /// only available in a fight
        /// </summary>
        public static int GetPetSpeciesIDBySlotID_Enemy(int slodID)
        { return Convert.ToInt32(Lua.GetReturnValues("return C_PetBattles.GetPetSpeciesID(LE_BATTLE_PET_ENEMY, " + slodID.ToString() + ");")[0]); }

        /// <summary>
        /// return (petID, speciesID, isOwned, nil?, level, nil?, name, icon, int?, creatureID, fundOrte, desc,  nil?)
        /// </summary>
        public static List<string> GetPetInfoByIndex(int index) //TODO (PetJournal.isWild)
        { return Lua.GetReturnValues("return C_PetJournal.GetPetInfoByIndex(" + index + ", false);"); }

        public static int GetPetIndexBySlotID(int SlotID)
        {
            return Convert.ToInt32(Lua.GetReturnValues("local petID = C_PetJournal.GetPetLoadOutInfo("+SlotID+") for i = 1,C_PetJournal.GetNumPets(false) do if (petID == C_PetJournal.GetPetInfoByIndex(i, false)) then return i end end return 0")[0]);
            //for (int i = 1; i <= GetNumPetsTotal; i++)
            //{
            //    if (GetPetInfoByIndex(i)[0] == PetID.ToString())
            //    {
            //        return i;
            //    }
            //}
            //return 0;
        }

        public static bool isOwned
        {
            get
            {
                //Dictionary<int>
                //var queuedBgs = XX.Count(s => s[0] == "1" && GetPetInfoByIndex(XX.IndexOf(s))[0] == GetActivePetID_Enemy.ToString());

                //for (int i = 1; i <= GetNumPetsTotal; i++)
                //{
                //    if (GetPetInfoByIndex(i)[0] == GetActivePetID_Enemy.ToString())
                //    {
                //        Styx.Common.Logging.Write("infoByPetID: {0}", GetPetInfoByPetID(GetActivePetID_Enemy).ToString());
                //        if (GetPetInfoByIndex(i)[2] == "1")
                //        { Styx.Common.Logging.Write("IsOwned = true"); return true; }
                //        break;
                //    }
                //}

                bool x = Lua.GetReturnVal<bool>("local ret = 0 local spID = C_PetBattles.GetPetSpeciesID(LE_BATTLE_PET_ENEMY, C_PetBattles.GetActivePet(LE_BATTLE_PET_ENEMY))    for i=1,C_PetJournal.GetNumPets(false) do     local petID, speciesID, isOwned, customName, level, favorite, isRevoked, name, icon, petType  = C_PetJournal.GetPetInfoByIndex(i, false)  if (speciesID == spID) then ret = isOwned end end return ret", 0);
                return x;
                //return false;
            }
        }

        /// <summary>
        /// only available in a fight
        /// </summary>
        public static string GetNameBySlotID_Enemy(int slotID)
        { return Lua.GetReturnValues("return C_PetBattles.GetName(LE_BATTLE_PET_ENEMY, " + slotID.ToString() + ");")[0]; }
        public static string GetNameBySlotID_Me(int slotID)
        { return GetNameByPetID(Convert.ToInt32(PetBattle.GetPetLoadOutInfo(slotID)[0])); }

        public static string GetNameByPetID(int PetID)
        { if (PetID == 1) return "[none]"; return GetPetInfoByPetID(PetID)[6]; }

        /// <summary>
        /// return (health, maxHealth, attack, speed, rarity)
        /// </summary>
        public static List<string> GetPetStatsByPetID(int PetID)
        { return Lua.GetReturnValues("return C_PetJournal.GetPetStats(" + PetID.ToString() + ");"); }
        /// <summary>
        /// return (health, maxHealth, attack, speed, rarity)
        /// </summary>
        public static List<string> GetPetStatsByPetID(string PetID)
        { return Lua.GetReturnValues("return C_PetJournal.GetPetStats(" + PetID.ToString() + ");"); }
        /// <summary>
        /// return (health, maxHealth, attack, speed, rarity)
        /// </summary>
        public static List<string> GetPetStatsBySlotID(int SlotID)
        { return Lua.GetReturnValues("local petID = C_PetJournal.GetPetLoadOutInfo(" + SlotID.ToString() + "); return C_PetJournal.GetPetStats(petID);"); }

        /// <param name="Attack">int PetType</param>
        /// <param name="Defend">int PetType</param>
        public static double GetAttackModifier(int AttackPetType, int DefendPetType)
        { return Convert.ToDouble(Lua.GetReturnValues("return C_PetBattles.GetAttackModifier(" + AttackPetType.ToString() + ", " + DefendPetType.ToString() + ");")[0]); }
        /// <param name="Attack">int PetType</param>
        /// <param name="Defend">int PetType</param>
        public static double GetAttackModifier(string AttackPetType, string DefendPetType)
        { return Convert.ToDouble(Lua.GetReturnValues("return C_PetBattles.GetAttackModifier(" + AttackPetType + ", " + DefendPetType + ");")[0]); }

        /// <summary>
        /// only available in a fight
        /// </summary>
        public static int GetPetTypeBySlotID_Enemy(int slotID)
        { return Lua.GetReturnVal<int>("return C_PetBattles.GetPetType(LE_BATTLE_PET_ENEMY, " + slotID.ToString() + ");", 0); }
        /// <summary>
        /// only available in a fight
        /// </summary>
        public static int GetPetTypeBySlotID_Me(int slotID)
        { return Lua.GetReturnVal<int>("return C_PetBattles.GetPetType(LE_BATTLE_PET_ALLY, " + slotID.ToString() + ");", 0); }

        /// <summary>
        /// return null, "queued", "proposal"
        /// </summary>
        public static string GetPVPMatchmakingInfo
        { get { return Lua.GetReturnVal<string>("return C_PetBattles.GetPVPMatchmakingInfo();", 0); } }

        public static void StartPVPMatchmaking()
        { Lua.DoString("C_PetBattles.StartPVPMatchmaking();"); }

        public static void StopPVPMatchmaking()
        { Lua.DoString("C_PetBattles.StopPVPMatchmaking();"); } 

        public static void AcceptQueuedPVPMatch()
        { Lua.DoString("C_PetBattles.AcceptQueuedPVPMatch();"); }

        public static void DeclineQueuedPVPMatch()
        { Lua.DoString("C_PetBattles.DeclineQueuedPVPMatch();"); }

        public static int GetBattlePetLevelByTarget
        { get { return Lua.GetReturnVal<int>("return UnitBattlePetLevel('target');", 0); } }

        public static int GetMiniLevel
        { get { return Lua.GetReturnVal<int>("local lvl = 33 for i=1,3 do local petID = C_PetJournal.GetPetLoadOutInfo(i) local _, _, level = C_PetJournal.GetPetInfoByPetID(petID) if level < lvl then lvl=level end end return lvl", 0); } }
        
        public static void ForfeitGame()
        { Lua.DoString("C_PetBattles.ForfeitGame()"); }
            



        //for j = 1, (level >= 4 and 3 or level >= 2 and 2 or 1) do
        //abilityID = _G.C_PetBattles.GetAbilityInfo(player, i, j);
        //table.insert(t[i], abilityID);

        // local auraID, instanceID, turnsRemaining, isBuff, casterOwner, casterIndex = C_PetBattles.GetAuraInfo(LE_BATTLE_PET_WEATHER, PET_BATTLE_PAD_INDEX, 1);

        // C_PetBattles.CanActivePetSwapOut()
        // C_PetJournal.IsFindBattleEnabled
        // C_PetJournal.PetIsSummonable(PetJournalPetCard.petID)
        // C_PetJournal.GetSummonedPetID()

        // C_PetJournal.GetPetAbilityList(speciesID, loadoutPlate.abilities, loadoutPlate.abilityLevels);  --Read ability/ability levels into the correct tables
        // C_PetJournal.GetPetAbilityList(speciesID, abilities, {}) 


        // if C_PetBattles.IsInBattle() or C_PetBattles.GetPVPMatchmakingInfo()
        
        // hooksecurefunc(C_PetJournal, "SetPetLoadOutInfo", UpdateCurrentTeam)
        // hooksecurefunc(C_PetJournal, "SetAbility", UpdateCurrentTeam)
        // hooksecurefunc(C_PetJournal,"PickupPet", PickupPetHook)
        // self:RegisterEvent("PET_BATTLE_OPENING_START");
        // self:RegisterEvent("PET_BATTLE_OPENING_DONE");
 
        // self:RegisterEvent("PET_BATTLE_TURN_STARTED");
        // self:RegisterEvent("PET_BATTLE_PET_ROUND_PLAYBACK_COMPLETE");
        // self:RegisterEvent("PET_BATTLE_PET_CHANGED");
        // self:RegisterEvent("PET_BATTLE_XP_CHANGED");
 
        // -- Transitioning out of battle event
        // self:RegisterEvent("PET_BATTLE_OVER");
 
        // -- End of battle event:
        // self:RegisterEvent("PET_BATTLE_CLOSE");
    
    }
}

#region Data (Map[itemid] => speciesid)
//--
//-- Data (Map[itemid] => speciesid)
//--

//Map[4401] = 39
//Map[8485] = 40
//Map[8486] = 41
//Map[8487] = 43
//Map[8488] = 45
//Map[8489] = 46
//Map[8490] = 44
//Map[8491] = 42
//Map[8492] = 50
//Map[8494] = 49
//Map[8495] = 51
//Map[8496] = 47
//Map[8497] = 72
//Map[8498] = 59
//Map[8499] = 58
//Map[8500] = 68
//Map[8501] = 67
//Map[10360] = 75
//Map[10361] = 77
//Map[10392] = 78
//Map[10393] = 55
//Map[10398] = 83
//Map[10709] = 70
//Map[10822] = 56
//Map[11023] = 52
//Map[11026] = 65
//Map[11027] = 64
//Map[11110] = 84
//Map[11474] = 87
//Map[11825] = 85
//Map[11826] = 86
//Map[12264] = 89
//Map[13582] = 94
//Map[13583] = 92
//Map[13584] = 93
//Map[15996] = 95
//Map[16450] = 90
//Map[19054] = 758
//Map[19055] = 757
//Map[19450] = 106
//Map[20371] = 107
//Map[20769] = 114
//Map[21277] = 116
//Map[21301] = 119
//Map[21305] = 120
//Map[21308] = 118
//Map[21309] = 117
//Map[22114] = 121
//Map[22235] = 122
//Map[22780] = 1073
//Map[22781] = 124
//Map[23007] = 126
//Map[23083] = 128
//Map[23713] = 130
//Map[25535] = 131
//Map[27445] = 132
//Map[28738] = 125
//Map[28740] = 127
//Map[29363] = 136
//Map[29364] = 137
//Map[29901] = 138
//Map[29902] = 139
//Map[29903] = 140
//Map[29904] = 141
//Map[29953] = 142
//Map[29956] = 143
//Map[29957] = 144
//Map[29958] = 145
//Map[29960] = 146
//Map[30360] = 111
//Map[31760] = 149
//Map[32233] = 153 
//Map[32498] = 155
//Map[32588] = 156
//Map[32616] = 158
//Map[32617] = 157
//Map[32622] = 159
//Map[33154] = 162
//Map[33816] = 163
//Map[33818] = 164
//Map[33993] = 165
//Map[34425] = 191
//Map[34478] = 167
//Map[34492] = 168 
//Map[34493] = 169
//Map[34518] = 170
//Map[34519] = 171
//Map[34535] = 57
//Map[35349] = 173
//Map[35350] = 174
//Map[35504] = 175
//Map[37297] = 179
//Map[37298] = 180
//Map[38050] = 183
//Map[38628] = 186
//Map[38658] = 187
//Map[39286] = 188
//Map[39656] = 189
//Map[39896] = 194
//Map[39898] = 197
//Map[39899] = 195
//Map[39973] = 190
//Map[41133] = 192
//Map[43698] = 193
//Map[44721] = 196
//Map[44723] = 198
//Map[44794] = 200
//Map[44810] = 201
//Map[44822] = 74
//Map[44965] = 204
//Map[44970] = 205
//Map[44971] = 206
//Map[44973] = 207
//Map[44974] = 209
//Map[44980] = 210
//Map[44982] = 213
//Map[44983] = 211
//Map[44984] = 212
//Map[44998] = 214
//Map[45002] = 215
//Map[45022] = 216
//Map[45180] = 217
//Map[45606] = 218
//Map[45890] = 172
//Map[46325] = 220
//Map[46398] = 224
//Map[46544] = 226
//Map[46545] = 225
//Map[46707] = 166
//Map[46767] = 227
//Map[46802] = 228
//Map[46820] = 229
//Map[46821] = 229
//Map[46892] = 217
//Map[48112] = 232
//Map[48114] = 233
//Map[48116] = 234
//Map[48118] = 235
//Map[48120] = 236
//Map[48122] = 237
//Map[48124] = 238
//Map[48126] = 239
//Map[48527] = 240
//Map[49287] = 241
//Map[49343] = 242
//Map[49646] = 244
//Map[49662] = 245
//Map[49663] = 246
//Map[49664] = 247
//Map[49665] = 248
//Map[49693] = 249
//Map[50446] = 251
//Map[54436] = 254
//Map[56806] = 258
//Map[59597] = 261
//Map[60216] = 262
//Map[60847] = 264
//Map[60955] = 266
//Map[62540] = 268
//Map[63138] = 270
//Map[63355] = 271
//Map[63398] = 272
//Map[64372] = 277
//Map[64403] = 278
//Map[64494] = 279
//Map[64996] = 271
//Map[65361] = 280
//Map[65362] = 281
//Map[65363] = 282
//Map[65364] = 283
//Map[65661] = 259
//Map[65662] = 260
//Map[66067] = 291
//Map[66073] = 289
//Map[66076] = 286
//Map[66080] = 287
//Map[67128] = 285
//Map[67274] = 267
//Map[67275] = 292
//Map[67282] = 293
//Map[67418] = 294
//Map[68385] = 297
//Map[68618] = 296
//Map[68619] = 298
//Map[68833] = 301
//Map[68840] = 302
//Map[68841] = 303
//Map[69239] = 306
//Map[69251] = 307
//Map[69821] = 309
//Map[69824] = 310
//Map[69847] = 311
//Map[69895] = 344
//Map[69896] = 345
//Map[69991] = 167
//Map[69992] = 229
//Map[70099] = 316
//Map[70140] = 317
//Map[70160] = 318
//Map[70908] = 319
//Map[71033] = 320
//Map[71076] = 321
//Map[71624] = 328
//Map[71726] = 329
//Map[72042] = 331
//Map[72045] = 332
//Map[72068] = 311
//Map[72134] = 333
//Map[72153] = 665
//Map[73762] = 336
//Map[73764] = 330
//Map[73765] = 335
//Map[73797] = 337
//Map[73903] = 338
//Map[73905] = 339
//Map[73953] = 340
//Map[74610] = 341
//Map[74611] = 342
//Map[74932] = 253 
//Map[74981] = 343
//Map[75906] = 256
//Map[76062] = 346
//Map[78916] = 347
//Map[79744] = 348
//Map[80008] = 848
//Map[82774] = 845
//Map[82775] = 846
//Map[84105] = 847
//Map[85220] = 650 
//Map[85222] = 1042 
//Map[85447] = 652
//Map[85513] = 802
//Map[85871] = 671
//Map[86563] = 836
//Map[86564] = 834
//Map[87526] = 844
//Map[88148] = 792
//Map[89367] = 850
//Map[89368] = 849
//Map[89587] = 381
//Map[90177] = 903
//Map[90897] = 278
//Map[90898] = 278
//Map[90953] = 242
//Map[98079] = 308
#endregion