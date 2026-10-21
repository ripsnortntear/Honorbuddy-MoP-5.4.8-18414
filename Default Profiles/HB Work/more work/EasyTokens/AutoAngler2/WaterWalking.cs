using System;
using System.Diagnostics;
using System.Linq;
using Styx;
using Styx.CommonBot.Routines;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace HighVoltz
{
    public class WaterWalking
    {
        private static readonly Stopwatch _recastSW = new Stopwatch();

        private static string[] _waterWalkingAbilities = {"Levitate", "Water Walking", "Path of Frost"};

        public static bool CanCast
        {
            get
            {
                return AutoAngler.Instance.MySettings.UseWaterWalking &&
                       (SpellManager.HasSpell("Levitate") || // priest levitate
                        SpellManager.HasSpell("Water Walking") || // shaman water walking
                        SpellManager.HasSpell("Path of Frost") || // Dk Path of frost
                        Utils.IsItemInBag(8827)); //isItemInBag(8827);
            }
        }


        public static bool IsActive
        {
            get
            {
                // DKs have 2 Path of Frost auras. only one can be stored in WoWAuras at any time. 

                return StyxWoW.Me.Auras.Values.Any(a => (StyxWoW.Me.HasAura("Levitate") || StyxWoW.Me.HasAura("Water Walking")) && a.TimeLeft >= new TimeSpan(0, 0, 20)) ||
                       StyxWoW.Me.HasAura("Path of Frost");
            }
        }

        public static bool Cast()
        {
            bool casted = false;
            if (!IsActive)
            {
                if (_recastSW.IsRunning && _recastSW.ElapsedMilliseconds < 5000)
                    return false;
                _recastSW.Reset();
                _recastSW.Start();
                int waterwalkingSpellID = 0;
                switch (StyxWoW.Me.Class)
                {
                    case WoWClass.Priest:
                        waterwalkingSpellID = 1706;
                        break;
                    case WoWClass.Shaman:
                        waterwalkingSpellID = 546;
                        break;
                    case WoWClass.DeathKnight:
                        waterwalkingSpellID = 3714;
                        break;
                }
                if (SpellManager.CanCast(waterwalkingSpellID))
                {
                    SpellManager.Cast(waterwalkingSpellID);
                    casted = true;
                }
                WoWItem waterPot = Utils.GetIteminBag(8827);
                if (waterPot != null && waterPot.Use())
                {
                    casted = true;
                }
            }
            if (StyxWoW.Me.IsSwimming)
            {
                using (StyxWoW.Memory.AcquireFrame())
                {
                    KeyboardManager.AntiAfk();
                    WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend);
                }
            }
            return casted;
        }
    }
}