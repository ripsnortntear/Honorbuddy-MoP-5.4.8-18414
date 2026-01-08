using System.Collections.Generic;

namespace BattlePetSwapper
{
    public class PetLoader : IPetLoader
    {
        IPluginLogger _logger;
        IPetLua _petLua;

        public PetLoader(IPluginLogger logger, IPetLua petLua)
        {
            _logger = logger;
            _petLua = petLua;
        }

        public void Load(List<Pet> selectedpets)
        {
            int slot = 1;
            foreach (Pet selectedpet in selectedpets)
            {
                _petLua.LoadPet(slot, selectedpet.PetId);
                _logger.Write(string.Format("Filling Pet Slot {0} with {1}", slot, selectedpet.ToString()));
                slot++;
            }
        }
    }
}