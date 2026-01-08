using System;
using System.Collections.Generic;
using Styx.Helpers;
using Styx.WoWInternals;

namespace BattlePetSwapper
{
    public class PetLua : IPetLua
    {
        IPluginLogger _logger;
        public PetLua(IPluginLogger logger)
        {
            _logger = logger;
        }

        public bool IsSummonable(string PetID)
        {
            string lua = string.Format("local Vv=C_PetJournal.PetIsSummonable(\"{0}\");return tostring(Vv);", PetID);
            bool isSummomable = Lua.GetReturnValues(lua)[0].ToBoolean();
            return isSummomable;
        }

        public List<String> GetPetStats(string PetID)
        {
            string lua = "local RetInfo = {}; local b = {};b[0],b[1],b[2],b[3],b[4] = C_PetJournal.GetPetStats(\"" + PetID + "\");" +
                        "for j_=0,4 do table.insert(RetInfo,tostring(b[j_]));end; " +
                        "return unpack(RetInfo)";

            List<String> stats = Lua.GetReturnValues(lua);
            return stats;
        }

        public int GetTargetLevel()
        {
            try
            {
                List<string> cnt = Lua.GetReturnValues("return UnitBattlePetLevel('target')");
                return Convert.ToInt32(cnt[0]);
            }
            catch { }
            return -1;
        }

        public int GetLevelBySlotID_Enemy(int slotID)
        {
            return Lua.GetReturnVal<int>("return C_PetBattles.GetLevel(LE_BATTLE_PET_ENEMY, " + slotID + ");", 0);
        }

        public bool IsInBattle()
        {
            List<string> cnt = Lua.GetReturnValues("dummy,reason=C_PetBattles.IsTrapAvailable() return dummy,reason");
            return cnt[1] != "0";
        }

        public void LoadPet(int slot, string petID)
        {
            string lua = "local petID, ability1, ability2, ability3, locked = C_PetJournal.GetPetLoadOutInfo({0}) ";
            lua += "if locked then C_PetJournal.SetPetLoadOutInfo({0},\"0x0\") end ";
            lua += "if petID ~= \"{1}\" then  C_PetJournal.SetPetLoadOutInfo({0}, \"{1}\") end";
            string slotLua = string.Format(lua, slot, petID);
            Lua.DoString(slotLua);
        }

        public void SetFilterAllCollectedPets()
        {
            Lua.DoString("C_PetJournal.ClearSearchFilter();");
            Lua.DoString("C_PetJournal.AddAllPetSourcesFilter();");
            Lua.DoString("C_PetJournal.AddAllPetTypesFilter();");
            Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false);");
            Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) ;");
            Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, false);");
        }

        public void SetFavouritesFlag()
        {
            Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, true);");
        }

        public int GetNumPetsOwned()
        {
            string lua = "local numPets, numOwned = C_PetJournal.GetNumPets(false); return tostring(numOwned);";
            return Lua.GetReturnValues(lua)[0].ToInt32();
        }

        public int GetNumPets()
        {
            string lua = "local numPets, numOwned = C_PetJournal.GetNumPets(false); return tostring(numPets);";
            return Lua.GetReturnValues(lua)[0].ToInt32();
        }

        public List<string> GetPetInfoByIndex(int partsize, uint k, int currentportionsize)
        {
            string lua = "local RetInfo = {}; local a = {};" +
            "for i_=" + k * partsize + "," + ((k * partsize) + currentportionsize) + " do " +
                "table.insert(RetInfo,'----------'); " +
                "a[0],a[1],a[2],a[3],a[4],a[5],a[6],a[7],a[8],a[9],a[10],a[11],a[12],a[13],a[14],a[15],a[16],a[17] = C_PetJournal.GetPetInfoByIndex(i_ + 1); " +
                "for j_=0,17 do table.insert(RetInfo,tostring(a[j_]));end;" +
            "end;" +
            "return unpack(RetInfo)";

            List<string> List1 = new List<string>();

            try
            {
                List1 = Lua.GetReturnValues(lua);
            }
            catch (Exception e)
            {
                _logger.Write(e.ToString());
            }

            if (List1 == null)
            {
                _logger.Write("---- error reading part of the journal. null list. k:" + k);
            }
            return List1;
        }

        public void ResurrectPets()
        {
            try
            {
                WoWSpell spell = WoWSpell.FromId(125439);
                if (spell != null && spell.CanCast)
                {
                    _logger.Write("Casting 'Revive Battle Pets'...");
                    spell.Cast();
                }
            }
            catch { }
        }
    }
}