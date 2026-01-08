using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Helpers;

namespace BattlePetSwapper
{
    public class PetJournal : IPetJournal
    {
        private IPluginLogger _logger;
        private IPluginProperties _pluginProperties;
        private IPetLua _petLua;

        public PetJournal(IPluginLogger logger, IPluginProperties pluginProperties, IPetLua petLua)
        {
            _logger = logger;
            _petLua = petLua;
            _pluginProperties = pluginProperties;
        }

        List<Pet> _ownedPetsList = new List<Pet>();
        List<Pet> _favouritePetsList = new List<Pet>();

        public void Clear()
        {
            _ownedPetsList = new List<Pet>();
        }

        public List<Pet> FavouritePetsList
        {
            get { return _favouritePetsList; }
        }

        public List<Pet> OwnedPetsList
        {
            get { return _ownedPetsList; }
        }

        public bool IsLoaded { get { return _ownedPetsList.Count > 0; } }

        public void PopulatePetJournal()
        {
            _logger.Write("Populating pet journal...");

            try
            {
                bool useOnlyFavourites = _pluginProperties.UseFavouritePetsOnly;
                _petLua.SetFilterAllCollectedPets();
                if (useOnlyFavourites) { _petLua.SetFavouritesFlag(); }
                _ownedPetsList = LoadFromJournal();
                _logger.Write((useOnlyFavourites ? "Favourite" : "Owned") + " pets journal count: " + _ownedPetsList.Count);

                _favouritePetsList = null;
                if (!useOnlyFavourites && _pluginProperties.UseFavouriteRingers)
                {
                    _petLua.SetFavouritesFlag();
                    _favouritePetsList = LoadFromJournal();
                    _logger.Write("Favourite ringer pets journal count: " + _favouritePetsList.Count);
                }

                _ownedPetsList.Sort(delegate(Pet p1, Pet p2) { return p1.Name.CompareTo(p2.Name); });

                if (_pluginProperties.Mode != eMode.Capture)
                {
                    WritePetsByLevel();
                }
            }
            catch
            {
                _logger.Write("Journal init query fail!!! ");
                try
                {
                    int PetCount = _petLua.GetNumPets();
                    int PetsOwned = _petLua.GetNumPetsOwned();
                    _logger.Write("Query too large?? " + PetsOwned + "," + PetCount);
                }
                catch
                {
                    _logger.Write("simple C_PetJournal.GetNumPets function failed. Try in WoW: \n/run local numPets, numOwned = C_PetJournal.GetNumPets(false); print('Journal: Pet count:' .. tostring(numOwned) .. ' total:' .. tostring(numPets));");
                }
            }
        }

        private void WritePetsByLevel()
        {
            int[] levelCount = new int[26];
            for (int i = 0; i < levelCount.Length; i++) { levelCount[i] = 0; }

            foreach (Pet pet in _ownedPetsList)
            {
                if (pet.Level < 26 && pet.CanBattle && pet.IsEquipable && (_pluginProperties.UseWildPets || !pet.IsWild) && (!_pluginProperties.OnlyBluePets || pet.IsRare))
                {
                    levelCount[pet.Level]++;
                }
            }

            StringBuilder levelText = new StringBuilder("Pets by level: ");
            for (int i = _pluginProperties.MinLevel; i <= _pluginProperties.MaxLevel; i++)
            {
                if (levelCount[i] > 0) { levelText.Append("#" + i.ToString() + "=" + levelCount[i].ToString() + " "); }
            }
            _logger.Write(levelText.ToString());
        }

        private List<Pet> LoadFromJournal()
        {
            List<Pet> ownedPets = new List<Pet>();
            int partsize = 10;
            int PetsOwned = _petLua.GetNumPets();

            if (PetsOwned == 0) { _logger.Write("0 pets in journal."); return ownedPets; }

            int remaining = (PetsOwned - 1) % partsize;
            int maxportions = ((PetsOwned - 1) / partsize) + 1;

            string[] AllCollectedPetFullData;
            string additionlreporttext = "";
            for (uint k = 0; k < maxportions; k++)
            {
                List<string> List1 = null;
                for (uint t = 0; t < 1; t++)
                {
                    int currentportionsize = ((k == maxportions - 1) ? remaining : partsize - 1);
                    List1 = _petLua.GetPetInfoByIndex(partsize, k, currentportionsize);

                    if (List1 == null)
                    {
                        continue;
                    }
                    break;
                }
                if (List1 == null)
                    continue;
                AllCollectedPetFullData = List1.ToArray();

                for (int i = 0; i < AllCollectedPetFullData.Count(); i++)
                {
                    if (AllCollectedPetFullData[i] == "----------")
                    {
                        Pet pd = new Pet(_logger, _petLua);
                        pd.PROTECTEDfromreleasing = true;
                        for (int j = 1; j < 25; j++)
                        {
                            if (i + j >= AllCollectedPetFullData.Count()) break;

                            if (AllCollectedPetFullData[i + j] == "----------")
                                break;
                            if (j == 1)
                            {
                                pd.PetId = AllCollectedPetFullData[i + j];
                            }
                            if (j == 2)
                                pd.SpeciesID = AllCollectedPetFullData[i + j].ToInt32();
                            if (j == 5) pd.Level = AllCollectedPetFullData[i + j].ToInt32();
                            if (j == 8) pd.Name = AllCollectedPetFullData[i + j];
                            if (j == 11) pd.CreatureID = AllCollectedPetFullData[i + j].ToInt32();
                            if (j == 12) if ((AllCollectedPetFullData[i + j].IndexOf("Áèòâû ïèòîìöåâ") >= 0) || (AllCollectedPetFullData[i + j].IndexOf("battles") >= 0)) pd.PROTECTEDfromreleasing = false;
                            if (j == 12) if (AllCollectedPetFullData[i + j].IndexOf("UI-GOLDICON") >= 0) pd.PROTECTEDfromreleasing = true;
                            if (j == 12) if (AllCollectedPetFullData[i + j].IndexOf("UI-SILVERICON") >= 0) pd.PROTECTEDfromreleasing = true;
                            if (j == 12) if (AllCollectedPetFullData[i + j].IndexOf("MONEYFRAME") >= 0) pd.PROTECTEDfromreleasing = true;
                            if (j == 12) if ((AllCollectedPetFullData[i + j].IndexOf("Ïðîôåññèÿ") >= 0) || (AllCollectedPetFullData[i + j].IndexOf("rofess") >= 0)) pd.PROTECTEDfromreleasing = true;
                            if (j == 12) if ((AllCollectedPetFullData[i + j].IndexOf("Äîñòèæåíèå") >= 0) || (AllCollectedPetFullData[i + j].IndexOf("chiev") >= 0)) pd.PROTECTEDfromreleasing = true;
                            if (j == 12) if ((AllCollectedPetFullData[i + j].IndexOf("ÿéöî") >= 0) || (AllCollectedPetFullData[i + j].IndexOf("egg") >= 0)) pd.PROTECTEDfromreleasing = true;
                            if (j == 14) pd.IsWild = AllCollectedPetFullData[i + j].ToBoolean();
                            if (j == 15) pd.CanBattle = AllCollectedPetFullData[i + j].ToBoolean();
                        }
                        ownedPets.Add(pd);
                    }
                };
            }
            return ownedPets;
        }
        public List<string> DistinctPetNames
        {
            get
            {
                List<string> result = new List<string>();
                foreach (Pet pet in OwnedPetsList)
                {
                    if (!result.Contains(pet.Name)) { result.Add(pet.Name); }
                }
                return result;
            }
        }
    }
}