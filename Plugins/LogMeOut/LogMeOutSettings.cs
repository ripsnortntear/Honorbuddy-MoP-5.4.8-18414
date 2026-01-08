using System.IO;
using Styx;
using Styx.Helpers;

namespace LogMeOut
{
    public class LogMeOutSettings : Settings
    {
        public static readonly LogMeOutSettings Instance = new LogMeOutSettings();

        public LogMeOutSettings()
            : base(Path.Combine(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), string.Format(@"Settings/LogMeOut/LogMeOut-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }

        #region Variables du tab Triggers
        //Bags full
        [Setting, DefaultValue(false)]
        public bool AlertOnBagsFull { get; set; }

        //Time elapsed
        [Setting, DefaultValue(false)]
        public bool AlertOnTimeElapsed { get; set; }
        [Setting, DefaultValue(1)]
        public int HoursElapsed { get; set; }
        [Setting, DefaultValue(0)]
        public int MinutesElapsed { get; set; }

        //Deaths
        [Setting, DefaultValue(false)]
        public bool AlertOnDeaths { get; set; }
        [Setting, DefaultValue(10)]
        public int NbDeaths { get; set; }

        //Stucks
        [Setting, DefaultValue(false)]
        public bool AlertOnStucks { get; set; }
        [Setting, DefaultValue(10)]
        public int NbStucks { get; set; }

        //Achievement
        [Setting, DefaultValue(false)]
        public bool AlertOnAchievement { get; set; }
        [Setting, DefaultValue(1)]
        public int AchievementId { get; set; }

        //Teleportation
        [Setting, DefaultValue(false)]
        public bool AlertOnTeleportation { get; set; }
        [Setting, DefaultValue(100)]
        public int TeleportYards { get; set; }

        //Rest in combat
        [Setting, DefaultValue(false)]
        public bool AlertOnMinutesInCombat { get; set; }
        [Setting, DefaultValue(5)]
        public int NbMinutesInCombat { get; set; }

        //Mobs killed
        [Setting, DefaultValue(false)]
        public bool AlertOnMobsKilled { get; set; }
        [Setting, DefaultValue(200)]
        public int NbMobsKilled { get; set; }

        //Looted Item
        [Setting, DefaultValue(false)]
        public bool AlertOnLootedItem { get; set; }
        [Setting, DefaultValue(1)]
        public int LootedItemTimes { get; set; }
        [Setting, DefaultValue(1)]
        public int LootedItemId { get; set; }

        //Cannot loot mobs
        [Setting, DefaultValue(false)]
        public bool AlertOnCannotLoot { get; set; }

        //Durability check
        [Setting, DefaultValue(false)]
        public bool AlertOnDurability { get; set; }
        [Setting, DefaultValue(20)]
        public int DurabilityPercent { get; set; }

        //Whispers Received
        [Setting, DefaultValue(false)]
        public bool AlertOnWhispersReceived { get; set; }
        [Setting, DefaultValue(3)]
        public int NbWhispersReceived { get; set; }

        /// <summary>
        /// Whisper got from a GM.
        /// </summary>
        [Setting, DefaultValue(false)]
        public bool AlertOnWhisperGm { get; set; }

        //Points
        [Setting, DefaultValue(false)]
        public bool AlertOnPoints { get; set; }
        [Setting, DefaultValue(4000)]
        public int NbPoints { get; set; }
        [Setting, DefaultValue(0)]
        public int TypePoints { get; set; }

        //Level Reached
        [Setting, DefaultValue(false)]
        public bool AlertOnLevelReached { get; set; }
        [Setting, DefaultValue(85)]
        public int NbLevel { get; set; }

        //Got Aura
        [Setting, DefaultValue(false)]
        public bool AlertOnGotAura { get; set; }
        [Setting, DefaultValue("Deserter")]
        public string GotAuraName { get; set; }

        //Traning Skill
        [Setting, DefaultValue(false)]
        public bool AlertOnTrainingSkill { get; set; }
        [Setting]
        public SkillLine TrainingSkillLine { get; set; }
        [Setting, DefaultValue(1)]
        public int TrainingSkillLevel { get; set; }

        //Inactivity Timeout
        [Setting, DefaultValue(false)]
        public bool AlertOnInactivity { get; set; }
        [Setting, DefaultValue(20)]
        public int InactivityTimeout { get; set; }

        //Player follows
        [Setting, DefaultValue(false)]
        public bool AlertOnPlayerFollows { get; set; }
        [Setting, DefaultValue(10)]
        public int MinutesPlayerFollows { get; set; }

        //Player targets
        [Setting, DefaultValue(false)]
        public bool AlertOnPlayerTargets { get; set; }
        [Setting, DefaultValue(5)]
        public int MinutesPlayerTargets { get; set; }

        //Beep when fire
        [Setting, DefaultValue(false)]
        public bool BeepWhenFire { get; set; }
        #endregion

        #region Variables du tab Action Before
        //Action before
        [Setting, DefaultValue(0)]
        public int ActionBefore { get; set; }

        //Spell Name
        [Setting, DefaultValue("Enter the spell name (in english) here")]
        public string SpellName { get; set; }

        //Lua waiting time
        [Setting, DefaultValue(1)]
        public int LuaWaitingTime { get; set; }

        //Lua string
        [Setting, DefaultValue("Enter the lua string to run here")]
        public string LuaString { get; set; }
        #endregion

        #region Variables du tab Action After
        //Action after
        [Setting, DefaultValue(0)]
        public int ActionAfter { get; set; }

        //Batch Command
        [Setting, DefaultValue("")]
        public string BatchCommand { get; set; }

        //Batch Ligne
        [Setting, DefaultValue("")]
        public string BatchArgument { get; set; }

        //Kill reloggers
        [Setting, DefaultValue(1)]
        public int ItemId { get; set; }

        //Kill reloggers
        [Setting, DefaultValue(false)]
        public bool KillReloggers { get; set; }

        //Clear cache folder
        [Setting, DefaultValue(false)]
        public bool ClearCacheFolder { get; set; }

        //Clear assemblies folder
        [Setting, DefaultValue(false)]
        public bool ClearAssembliesFolder { get; set; }
        #endregion

        #region Variables du tab Logging
        //Couleur du texte de log
        [Setting, DefaultValue("Orange")]
        public string ColorLogs { get; set; }

        //Ecrit le temps restant
        [Setting, DefaultValue(true)]
        public bool LoggingTime { get; set; }

        //Every
        [Setting, DefaultValue(30)]
        public int LoggingTimeEvery { get; set; }

        /// <summary>
        /// Indique si Honorbuddy doit émettre un bip lorsque le bot est stuck
        /// </summary>
        [Setting, DefaultValue(false)]
        public bool LoggingBeepWhenStuck { get; set; }
        #endregion

        #region Variables du tab Exceptions
        //Exception sur les BGs
        [Setting, DefaultValue(true)]
        public bool ExceptionBg { get; set; }
        //Exception sur les instances
        [Setting, DefaultValue(true)]
        public bool ExceptionInstance { get; set; }
        //Exception sur les mailboxes
        [Setting, DefaultValue(false)]
        public bool ExceptionMailbox { get; set; }
        /// <summary>
        /// Ne déconnecte pas le bot lorsque le joueur a le debuff Déserteur
        /// </summary>
        public bool ExceptionDeserterDebuff { get; set; }

        //Exception sur le compteur de mort en champs de bataille
        [Setting, DefaultValue(false)]
        public bool ExceptionCountDeathsBg { get; set; }
        /// <summary>
        /// Indique s'il ne faut pas tenir compte des joueurs du groupe/raid lors de l'évaluation des déclencheurs.
        /// </summary>
        [Setting, DefaultValue(false)]
        public bool ExceptionMembersPartyRaid { get; set; }
        //Exception pour que le joueur ne soit jamais déconnecté
        [Setting, DefaultValue(false)]
        public bool ExceptionDontLogOut { get; set; }
        //Exception qui détermine si le joueur doit rejoindre son corps lors d'une déconnexion
        [Setting, DefaultValue(false)]
        public bool ExceptionDontRunToCorpse { get; set; }
        #endregion

        #region Autres variables
        //Contient la raison de la dernière déconnexion du bot
        [Setting, DefaultValue("")]
        public string ReasonLastLogOut { get; set; }
        #endregion
    }
}