using System;
using System.Collections.Generic;
using Styx.Helpers;

namespace BattlePetSwapper
{
    public class Pet
    {
        private static Dictionary<string, Pet> PetCache = new Dictionary<string, Pet>();

        private static HashSet<int> BlacklistForEquip = new HashSet<int> { 92395, 49586, 72042, 54539, 101986 }; // http://www.wowhead.com/spell=92395
        public int Level = 0;
        public string PetId = "0x0";
        public int SpeciesID = 0;
        public int CreatureID = 0;
        private int _health = 0;

        IPluginLogger _logger;
        IPetLua _petLua;

        public Pet(IPluginLogger logger, IPetLua petLua)
        {
            _logger = logger;
            _petLua = petLua;
        }

        public int Health
        {
            get
            {
                PopulatePetStatsIfRequried();
                return _health;
            }
        }
        private int _maxHealth = 0;

        public int MaxHealth
        {
            get
            {
                PopulatePetStatsIfRequried();
                return _maxHealth;
            }
        }
        private int _attack = 0;

        public int Attack
        {
            get
            {
                PopulatePetStatsIfRequried();
                return _attack;
            }
        }
        private int _speed = 0;

        public int Speed
        {
            get
            {

                PopulatePetStatsIfRequried();
                return _speed;
            }
        }
        public bool IsWild = false;
        public bool CanBattle = false;
        private int _rarity = 0;	// 3 - green, 4 - rare

        public bool IsEquipable
        {
            get
            {
                return !BlacklistForEquip.Contains(CreatureID); // unequipable
            }
        }

        public bool IsRare
        {
            get
            {
                return Rarity >= 4;
            }
        }

        public int Rarity
        {
            get
            {

                if (PetCache.ContainsKey(this.PetId)) { return PetCache[this.PetId]._rarity; }

                PopulatePetStatsIfRequried();
                return _rarity;
            }
        }
        public bool PROTECTEDfromreleasing = true;
        public string Name = "";

        public int HealthPercentage
        {
            get
            {
                return (int)((double)Health / (double)MaxHealth * 100);
            }
        }
        public override string ToString()
        {
            return Name + " [" + Level + "] " + (HealthPercentage < 100 ? "Health " + HealthPercentage.ToString() + "%" : string.Empty);
        }

        private bool _statsAreLoaded = false;

        public void SetStatsInvalid()
        {
            _statsAreLoaded = false;
        }

        private void PopulatePetStatsIfRequried()
        {
            if (_statsAreLoaded) { return; }
            try
            {
                //_logger.Write("Reading stats for pet: " + this.Name);
                List<String> stats = _petLua.GetPetStats(PetId);

                _health = stats[0].ToInt32();
                _maxHealth = stats[1].ToInt32();
                _attack = stats[2].ToInt32();
                _speed = stats[3].ToInt32();
                _rarity = stats[4].ToInt32();
                _statsAreLoaded = true;

                if (!PetCache.ContainsKey(this.PetId)) { PetCache.Add(this.PetId, this); }
            }
            catch (Exception e)
            {
                _logger.Write(e.Message);
            }
        }

        public bool IsSummonable()
        {
            return _petLua.IsSummonable(PetId);
        }
    }
}