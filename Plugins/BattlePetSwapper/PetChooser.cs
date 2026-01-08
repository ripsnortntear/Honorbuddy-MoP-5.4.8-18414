using System.Collections.Generic;

namespace BattlePetSwapper
{
    public class PetChooser : IPetChooser
    {
        private IPluginLogger _logger;
        private IPluginProperties _pluginProperties;
        private IPetLua _petLua;

        public PetChooser(IPluginLogger logger, IPluginProperties pluginProperties, IPetLua petLua)
        {
            _logger = logger;
            _petLua = petLua;
            _pluginProperties = pluginProperties;
        }

        private HashSet<string> _blacklistForSelection = new HashSet<string>();
        private List<Pet> _selectedpets = new List<Pet>();

        public List<Pet> SelectPetsForLevel(List<Pet> ownedPetsList,List<Pet> favourites, int Level)
        {
            _selectedpets = new List<Pet>();

            if (Level == 0) Level = 1;

            if (_pluginProperties.Mode == eMode.Ringer)
            {
                //Ringer mode
                _logger.Write("Choosing pets to level and 'Ringer' pet.");

                FillSelectedPets(ownedPetsList, _pluginProperties.MinLevel, _pluginProperties.MaxLevel,false);
                FillSelectedPets(ownedPetsList, _pluginProperties.MinLevel, _pluginProperties.MaxLevel, false);

                for (int petLevel = _pluginProperties.MaxLevel; petLevel > 0; petLevel--)
                {
                    FillSelectedPets(favourites != null ? favourites : ownedPetsList, petLevel, petLevel,true);
                    if (_selectedpets.Count >= 3) { return _selectedpets; }
                }
            }
            else if (_pluginProperties.Mode == eMode.Ringerx2)
            {
                //Ringer mode
                _logger.Write("Choosing pet to level and 'Ringer x2' pets.");

                FillSelectedPets(ownedPetsList, _pluginProperties.MinLevel, _pluginProperties.MaxLevel, false);

                for (int petLevel = _pluginProperties.MaxLevel; petLevel > 0; petLevel--)
                {
                    FillSelectedPets(favourites != null ? favourites : ownedPetsList, petLevel, petLevel, true);
                    FillSelectedPets(favourites != null ? favourites : ownedPetsList, petLevel, petLevel, true);
                    if (_selectedpets.Count >= 3) { return _selectedpets; }
                }
            }
            else if (_pluginProperties.Mode == eMode.Capture)
            {
                //fill any gaps if no pets are int the range
                for (int petLevel = Level + 1; petLevel <= _pluginProperties.MaxLevel; petLevel++) // look for pets in range: Level to maxlevel
                {
                    FillSelectedPets(ownedPetsList, petLevel, petLevel,false);
                    if (_selectedpets.Count >= 3) { return _selectedpets; }
                }
            }
            else
            {
                //Relative Mode
                _logger.Write("Choosing pets for level: " + Level);

                FillSelectedPets(ownedPetsList, Level - 2, Level + 1, false); // 1st pet - -2 to +1 levels relative to zone
                FillSelectedPets(ownedPetsList, Level - 2, Level + 1, false); // 2nd pet - -2 to +1 levels relative to zone
                FillSelectedPets(ownedPetsList, Level + 1, Level + 25, false);// 3rd pet - +1 to +25 levels relative to zone

                //fill any gaps if no pets are int the range
                for (int petLevel = Level; petLevel <= _pluginProperties.MaxLevel; petLevel++) // look for pets in range: Level to maxlevel
                {
                    FillSelectedPets(ownedPetsList, petLevel, petLevel, false);
                    if (_selectedpets.Count >= 3) { return _selectedpets; }
                }

                for (int petLevel = Level; petLevel > _pluginProperties.MinLevel; petLevel--) // look for pets in range: Level to minlevel
                {
                    FillSelectedPets(ownedPetsList, petLevel, petLevel, false);
                    if (_selectedpets.Count >= 3) { return _selectedpets; }
                }
            }

            return _selectedpets;
        }

        private void FillSelectedPets(List<Pet> petsList, int minPetLevel, int maxPetLevel, bool isRingerSelection)
        {
            Pet selectedPet = SelectedPet(petsList, minPetLevel, maxPetLevel, isRingerSelection);
            if (selectedPet != null)
            {
                //_logger.Write("Selected for slot " + _selectedpets.Count + ": " + selectedPet.Name + " level:" + selectedPet.Level + " Health % " + selectedPet.HealthPercentage);
                _selectedpets.Add(selectedPet);
            }
        }

        private Pet SelectedPet(List<Pet> petsList, int minPetLevel, int maxPetLevel,bool isRingerSelection)
        {
            //validate parameters
            if (_selectedpets.Count >= 3) { return null; }
            if (minPetLevel < _pluginProperties.MinLevel) { minPetLevel = _pluginProperties.MinLevel; }
            if (minPetLevel > _pluginProperties.MaxLevel) { minPetLevel = _pluginProperties.MaxLevel; }
            if (maxPetLevel < _pluginProperties.MinLevel) { maxPetLevel = _pluginProperties.MinLevel; }
            if (maxPetLevel > _pluginProperties.MaxLevel) { maxPetLevel = _pluginProperties.MaxLevel; }

            //find the lowest available pet which matches the criteria
            for (int level = minPetLevel; level <= maxPetLevel; level++)
            {
                foreach (Pet availablePet in petsList)
                {
                    if (_selectedpets.Contains(availablePet)) { continue; } //already selected
                    if (!availablePet.IsEquipable) { continue; } // not equippable

                    if (availablePet.IsWild && !_pluginProperties.UseWildPets && (availablePet.Level < 25 || !isRingerSelection)) { continue; } // is wild
                    if (!availablePet.CanBattle) { continue; } // can't battle
                    if (_blacklistForSelection.Contains(availablePet.PetId)) { continue; } // black listed

                    if (availablePet.Level == level)
                    {
                        if (availablePet.HealthPercentage < _pluginProperties.MinPetHealth && !isRingerSelection) { continue; } // health too low
                        if (availablePet.HealthPercentage < _pluginProperties.MinRingerPetHealth && isRingerSelection) { continue; } // health too low
                        if (_pluginProperties.OnlyBluePets && !availablePet.IsRare && (availablePet.Level < 25 || !isRingerSelection)) { continue; } // not rare
                        if (availablePet.IsSummonable())
                        {
                            return availablePet;
                        }
                        else
                        {
                            _logger.Write("Can't Summon pet " + availablePet.ToString() + "... blacklisting it.");
                            _blacklistForSelection.Add(availablePet.PetId);
                        }
                    }
                }
            }
            return null;
        }
    }
}