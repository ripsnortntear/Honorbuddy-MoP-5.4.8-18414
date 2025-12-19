using System;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.CommonBot;
using VitalicRotation.Helpers;

namespace VitalicRotation.Helpers
{
    /// <summary>
    /// Helper pour la vérification cohérente de l'état de furtivité
    /// Basé sur la logique originale de Vitalic
    /// </summary>
    internal static class StealthHelper
    {
        /// <summary>
        /// Vérifie si le joueur est actuellement sous l'effet de la furtivité
        /// Logique exacte de Vitalic: Stealth || Subterfuge || Shadow Dance
        /// </summary>
        public static bool IsInStealth(WoWUnit me)
        {
            if (me == null || !me.IsValid) return false;
            
            bool openerState = false; 
            try 
            { 
                openerState = me.HasAura("Stealth") || me.HasAura("Subterfuge") || me.HasAura("Shadow Dance"); 
            } 
            catch 
            { 
            }
            return openerState;
        }
    }
}