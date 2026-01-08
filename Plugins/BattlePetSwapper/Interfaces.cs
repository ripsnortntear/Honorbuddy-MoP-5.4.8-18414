using System.Collections.Generic;
using BattlePetSwapper;

namespace BattlePetSwapper
{
    public interface IPluginLogger
    {
        void Write(string message);
    }

    public interface IPetLua
    {
        int GetLevelBySlotID_Enemy(int slotID);
        int GetNumPets();
        int GetNumPetsOwned();
        System.Collections.Generic.List<string> GetPetInfoByIndex(int partsize, uint k, int currentportionsize);
        System.Collections.Generic.List<string> GetPetStats(string PetID);
        int GetTargetLevel();
        bool IsInBattle();
        bool IsSummonable(string PetID);
        void LoadPet(int slot, string petID);
        void SetFilterAllCollectedPets();
        void SetFavouritesFlag();

        void ResurrectPets();
    }

    public interface IPluginProperties
    {
        int MaxLevel { get; set; }
        int MinLevel { get; set; }
        int MinPetHealth { get; set; }
        eMode Mode { get; set; }
        bool OnlyBluePets { get; set; }
        bool UseWildPets { get; set; }
        bool UseFavouritePetsOnly { get; set; }
        bool UseFavouriteRingers { get; set; }
        int MinRingerPetHealth { get; set; }
    }

    public interface IPluginSettings
    {
        void ConvertSettingsToProperties();
        void ConvertsPropertiesToSettings();
        void Save();
    }

    public interface IPetChooser
    {
        List<Pet> SelectPetsForLevel(List<Pet> ownedPetsList, List<Pet> favourites, int Level);
    }

    public interface IPetJournal
    {
        bool IsLoaded { get; }
        void PopulatePetJournal();
        void Clear();
        List<Pet> OwnedPetsList { get; }
        List<Pet> FavouritePetsList { get; }
        List<string> DistinctPetNames { get; }
    }

    public interface IPetLoader
    {
        void Load(List<Pet> selectedpets);
    }
}