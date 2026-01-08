using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Styx;
using Styx.WoWInternals.WoWObjects;

namespace LogMeOut.Classes
{
    /// <summary>
    /// Provided by ZenLulz @ thebuddyforum.com
    /// </summary>
    public static class TrainingSkills
    {
        #region Properties and private vars
        /// <summary>
        /// Player pointer.
        /// </summary>
        private static readonly LocalPlayer Me = StyxWoW.Me;
        
        /// <summary>
        /// List of all the professions in World of Warcraft.
        /// </summary>
        public static ReadOnlyCollection<SkillLine> SkillsList
        {
            get
            {
                return new List<SkillLine>
                           {
                               SkillLine.Alchemy,
                               SkillLine.Archaeology,
                               SkillLine.Blacksmithing,
                               SkillLine.Cooking,
                               SkillLine.Enchanting,
                               SkillLine.Engineering,
                               SkillLine.FirstAid,
                               SkillLine.Fishing,
                               SkillLine.Herbalism,
                               SkillLine.Inscription,
                               SkillLine.Jewelcrafting,
                               SkillLine.Leatherworking,
                               SkillLine.Mining,
                               SkillLine.Skinning,
                               SkillLine.Tailoring
                           }.AsReadOnly();
            }
        }

        /// <summary>
        /// List of the professions known by the player.
        /// </summary>
        public static ReadOnlyCollection<SkillLine> SkillsLearnt
        {
            get
            {
                // May happens the call of GetSkill bugs. Try 3 times.
                try
                {
                    return SkillsList.Where(skill => Me.GetSkill(skill).CurrentValue != 0).ToList().AsReadOnly();
                }
                catch
                {
                    try
                    {
                        return SkillsList.Where(skill => Me.GetSkill(skill).CurrentValue != 0).ToList().AsReadOnly();
                    }
                    catch
                    {
                        try
                        {
                            return SkillsList.Where(skill => Me.GetSkill(skill).CurrentValue != 0).ToList().AsReadOnly();
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// State if the player reached a level in a profession.
        /// </summary>
        /// <param name="skill">Concerned profession.</param>
        /// <param name="level">Reached level.</param>
        /// <returns>True if the level is reached.</returns>
        public static bool HasReached(SkillLine skill, int level)
        {
            return Me.GetSkill(skill).CurrentValue >= level;
        }

        /// <summary>
        /// State if the player learnt a profession.
        /// </summary>
        /// <param name="skill">Concerned profession.</param>
        /// <returns>True if the player has learnt the profession.</returns>
        public static bool HasLearnt(SkillLine skill)
        {
            return Me.GetSkill(skill).CurrentValue != 0;
        }

        /// <summary>
        /// State the current level in a profession of the player.
        /// </summary>
        /// <param name="skill">Concerned profession.</param>
        /// <returns>Return the level in the profession. Zero if the player didn't learn the profeesion.</returns>
        public static int CurrentLevel(SkillLine skill)
        {
            return Me.GetSkill(skill).CurrentValue;
        }

        /// <summary>
        /// State the maximum level in a profession of the player.
        /// </summary>
        /// <param name="skill">Concerned profession.</param>
        /// <returns>Return the maximum level in the profession. Zero if the player didn't learn the profeesion.</returns>
        public static int MaxLevel(SkillLine skill)
        {
            return Me.GetSkill(skill).MaxValue;
        }

        /// <summary>
        /// State if the maximum level in a profession is reached.
        /// </summary>
        /// <param name="skill">Concerned profession.</param>
        /// <returns>True if the maximum level is reached.</returns>
        public static bool IsMaxLevel(SkillLine skill)
        {
            return HasLearnt(skill) && CurrentLevel(skill) == MaxLevel(skill);
        }
        #endregion
    }
}
