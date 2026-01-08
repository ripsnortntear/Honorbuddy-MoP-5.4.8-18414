/*----------------------------------------------------
 * LogMeOut! - Developped by ZenLulz (on thebuddyforum.com / binarysharp.com)
 * Supported WoW Version : 5.4.8
 * SVN : https://zenlulzdev.googlecode.com/svn/trunk/HonorBuddy/Plugins/LogMeOut/
 * Note : This is a free plugin, and could not be sold, or repackaged.
 * Version : 1.2.18 (Release)
 ----------------------------------------------------*/

using LogMeOut.Classes;
using Styx.Common;
using System;
using System.Threading;
using System.Diagnostics;
using Styx.CommonBot;
using Styx.CommonBot.Frames;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Styx;
using System.Windows.Media;
using System.Windows.Forms;

namespace LogMeOut
{
    public class LogMeOut : HBPlugin
    {
        //Normal Stuff.
        public override string Name { get { return "LogMeOut!"; } }
        public override string Author { get { return "ZenLulz"; } }
        public override Version Version { get { return Classes.Updater.Version; } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Settings"; } }

        /// <summary>
        /// Représente l'instance de cette classe.
        /// </summary>
        public static LogMeOut Instance;
        /// <summary>
        /// Objet pointant sur le les propriétés du bot.
        /// </summary>
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        /// <summary>
        /// Crée une variable qui définit si le module a déjà été chargé.
        /// </summary>
        private bool _blnInit;
        /// <summary>
        /// Indique si le code de démarrage du plugin est exécuté.
        /// </summary>
        private bool _isStartingCodeExecuted;
        /// <summary>
        /// Indique si le personnage a déjà été dans un champ de bataille ou instance dans la phase de déconnexion.
        /// </summary>
        private bool _blnBattlegroundOrInstance;
        /// <summary>
        /// Indique si le personnage a déjà été mort dans la phase de déconnexion.
        /// </summary>
        private bool _blnIsDead;
        /// <summary>
        /// Indique si le personnage a déjà été en combat dans la phase de déconnexion.
        /// </summary>
        private bool _blnCombat;
        /// <summary>
        /// Indique si le personnage a déjà été en monture volante ou sur un trajet aérien dans la phase de déconnexion.
        /// </summary>
        private bool _blnFlying;
        /// <summary>
        /// Indique si le message du force logout a déjà été affiché.
        /// </summary>
        private bool _blnForceLogOutMessageShown;
        /// <summary>
        /// Indique si une alerte est effectuée à cause d'une action avant déconnexion dans la phase de déconnexion.
        /// </summary>
        private bool _blnAlertActionBefore;
        /// <summary>
        /// Définit lorsque la technique avant la fermeture de Honorbuddy a été utilisée.
        /// </summary>
        private DateTime? _abilityBeforeExitTimestamp;
        /// <summary>
        /// Définit le temps de caste de la technique avant la fermeture de Honorbuddy.
        /// </summary>
        private TimeSpan _abilityBeforeExitCastTime;
        /// <summary>
        /// Indique depuis combien de temps la procédure de déconnexion a été entamée.
        /// </summary>
        private DateTime _dtStartingLogOutProcess = DateTime.MinValue;
        /// <summary>
        /// Indique si le processus de déconnexion est arrivé à terme.
        /// </summary>
        private bool _blnDisconnected;
        /// <summary>
        /// Indique lorsque le timer est lancé.
        /// </summary>
        private DateTime _dtStartingTimer;
        /// <summary>
        /// Indique lorsque le temps écoulé a été indiqué pour la dernière fois dans la console Log.
        /// </summary>
        private DateTime _dtLastLoggingTime;
        /// <summary>
        /// Indique la dernière fois que le bot est resté stuck.
        /// </summary>
        private DateTime _dtLastStuck;
        /// <summary>
        /// Indique le nombre de stucks consécutifs.
        /// </summary>
        private int _intNbStucks;
        /// <summary>
        /// Contient la liste des derniers essais de loot ratés durant les 5 dernières minutes.
        /// </summary>
        /// <remarks>Cette liste est rafraîchie à chaque nouveaux messages dans le log de HB.</remarks>
        private readonly List<DateTime> _listDtCannotLoot = new List<DateTime>();
        /// <summary>
        /// Indique le nombre de chuchotements reçus.
        /// </summary>
        private int _intNbWhispers;
        /// <summary>
        /// Indique le nombre de fois que le bot est mort.
        /// </summary>
        private int _intNbDeaths;
        /// <summary>
        /// Indique si le joueur est vivant.
        /// </summary>
        private bool _blnIsAlive = true;
        /// <summary>
        /// Indique la dernière fois que le bot était en chute libre.
        /// </summary>
        private DateTime _dtLastIsFalling;
        /// <summary>
        /// Indique depuis combien de temps le bot est en combat.
        /// </summary>
        private DateTime _dtInCombat;
        /// <summary>
        /// Indique le moment de la dernière action du joueur.
        /// </summary>
        private DateTime _lastAction;
        /// <summary>
        /// Contient le nom des unités et joueurs ciblant le bot.
        /// </summary>
        private String _strTargetingUnitsAndPlayersNames;
        /// <summary>
        /// Contient les joueurs/unités qui entoure le joueur avec le temps lorsque celui-ci a été ajouté
        /// </summary>
        private Classes.PeopleAroundMe _objPeopleAroundMe;
        /// <summary>
        /// Indique la dernière position connue du joueur.
        /// </summary>
        private WoWPoint _lastPosition;
        /// <summary>
        /// Indique la dernière fois que le joueur a exécuté une action pouvant causer une téléportation.
        /// </summary>
        private DateTime _lastTeleportableAction;
        /// <summary>
        /// Indique le dernier stuck.
        /// </summary>
        private DateTime _lastStuck;
        /// <summary>
        /// Collection d'objets qui videra le contenu d'un dossier lors de leur suppression.
        /// </summary>
        private List<BotFolderRemoval> _folderRemovalList; 

        /// <summary>
        /// Méthode écrivant du texte dans le console Log.
        /// </summary>
        public static void WriteLog(string format, params object[] args)
        {
            Logging.Write((Color)ColorConverter.ConvertFromString(LogMeOutSettings.Instance.ColorLogs), "[LogMeOut!]: " + format, args);
        }

        /// <summary>
        /// Méthode écrivant du texte dans le console Log en tant qu'erreur.
        /// </summary>
        public static void WriteLogError(string format, params object[] args)
        {
            Logging.Write(Colors.Red, "[LogMeOut!]: " + format, args);
        }

        /// <summary>
        /// Méthode écrivant du texte dans le console Debug.
        /// </summary>
        public static void WriteLogDebug(string format, params object[] args)
        {
            Logging.WriteVerbose((Color)ColorConverter.ConvertFromString(LogMeOutSettings.Instance.ColorLogs), "[LogMeOut!]: " + format, args);
        }

        /// <summary>
        /// Méthode s'exécutant lors de l'appui du bouton Settings dans la fenêtre Plugin
        /// </summary>
        public override void OnButtonPress()
        {
            //Ouverture de la fenêtre de configuration
            var winSettings = new Forms.FrmSettings();
            //Affiche la fenêtre
            winSettings.ShowDialog();
        }

        /// <summary>
        /// Réécriture de la méthode Initialize().
        /// </summary>
        public override void Initialize()
        {
            if (_blnInit) return;
            _blnInit = true;

            //Ajout d'un texte qui affiche que le plugin a correctement été chargé
            WriteLog("Loaded (Ver.{0})", Version.ToString());
            //Affiche la raison de la dernière déconnexion
            CheckLastLogOut();
            //Vérifie si il y a une nouvelle version du plugin
            if (Classes.Updater.IsAvailable)
                WriteLog("UPDATE AVAILABLE : A new version of LogMeOut! is available (Ver." + Classes.Updater.GetLatestVersion() + "). To update it, open the settings of this plugin.");
            //Bind les événements lors de l'appui sur le bouton Start/Stop du bot
            BotEvents.OnBotStarted += OnBotStarted;
            BotEvents.OnBotStopped += OnBotStopped;
            //Lie cette instance avec la variable correspondante
            Instance = this;
            //Instancie l'objet gérant les unités et joueurs environnants
            _objPeopleAroundMe = new PeopleAroundMe();
            //Set le temps de démarrage du bot à maintenant (sinon le bot s'arrête s'il est démarré en cours de route)
            _dtStartingTimer = DateTime.Now;
            //Crée une collection vide pour nettoyer certains dossiers de HonorBuddy
            _folderRemovalList = new List<BotFolderRemoval>();
            //Vérifie que le dossier du plugin n'a pas été renommé
            CheckPluginPath();
        }

        public override void Dispose()
        {
            _blnInit = false;
            WriteLog("Disposed");
            //Retire les événements lors de l'appui sur le bouton Start/Stop du bot
            BotEvents.OnBotStarted -= OnBotStarted;
            BotEvents.OnBotStopped -= OnBotStopped;
        }

        public void OnBotStarted(EventArgs args)
        {
            if (!IsEnabled()) return;

            //Crée un pointeur sur l'instance des données sauvegardées
            var settings = LogMeOutSettings.Instance;
            //Charge les derniers paramètres du plugin
            settings.Load();
            //Parse les logs de HB
            Logging.OnLogMessage += Logging_OnLogMessage;
            //Bind les événements lors de la réception de chuchotements
            Chat.Whisper += HandleWhispers;
            //Bind lors de l'acceptation de quêtes (possible téléportation)
            BotEvents.Questing.OnQuestAccepted += QuestingOnOnQuestAccepted;
            //Réinitialise les variables vitales du plugin
            _lastPosition = Me.Location;
            IsLoggingOut = false;
            _dtStartingLogOutProcess = DateTime.MinValue;
            _blnDisconnected = false;
            _blnAlertActionBefore = false;
            _abilityBeforeExitTimestamp = new DateTime?();
            _dtInCombat = DateTime.Now;
            _lastAction = DateTime.Now;
            _lastStuck = DateTime.Now;
            _intNbWhispers = 0;
            _intNbDeaths = 0;
            _intNbStucks = 0;
            //Vide le cache des derniers loot ratés
            _listDtCannotLoot.Clear();
            //Vide le cache contant les unités et joueurs proches
            _objPeopleAroundMe.Clear();
            //Set l'heure actuel pour le début du timer
            _dtStartingTimer = DateTime.Now;
            //Ajout d'un texte
            WriteLog("Started successfully !");
            WriteLog("-------------------------------------------------");
            WriteLog(" The bot will stop on the following triggers :");
            //Affiche les triggers activés
            if (settings.AlertOnBagsFull)
                WriteLog(" + On Bags full");
            if (settings.AlertOnTimeElapsed)
                WriteLog(" + On Time Elapsed (" + settings.HoursElapsed.ToString("00") + "h" + settings.MinutesElapsed.ToString("00") + ")");
            if (settings.AlertOnDeaths)
                WriteLog(" + On number of deaths (" + settings.NbDeaths + " max)");
            if (settings.AlertOnStucks)
                WriteLog(" + On Stucks (" + settings.NbStucks + " max)");
            if (settings.AlertOnMinutesInCombat)
                WriteLog(" + On minutes in combat (" + settings.NbMinutesInCombat + " max)");
            if (settings.AlertOnMobsKilled)
                WriteLog(" + On number of mobs killed (" + settings.NbMobsKilled + " max)");
            if (settings.AlertOnLootedItem)
                WriteLog(" + On looted item (Id: " + settings.LootedItemId + ", times: " + settings.LootedItemTimes + ")");
            if (settings.AlertOnCannotLoot)
                WriteLog(" + On cannot loot mobs");
            if (settings.AlertOnDurability)
                WriteLog(" + On durability check (" + settings.DurabilityPercent + "%)");
            if (settings.AlertOnWhispersReceived)
                WriteLog(" + On whispers received (" + settings.NbWhispersReceived + " max)");
            if (settings.AlertOnPoints)
                WriteLog(" + On " + settings.NbPoints + " " + Arrays.NamesPoints[settings.TypePoints] + " points reached");
            if (settings.AlertOnLevelReached)
                WriteLog(" + On level reached (" + settings.NbLevel + " max)");
            if (settings.AlertOnGotAura)
                WriteLog(" + On got aura (" + settings.GotAuraName + ")");
            if (settings.AlertOnTrainingSkill)
                WriteLog(" + On training skill reached (" + settings.TrainingSkillLevel + " in " + settings.TrainingSkillLine.ToString() + ")");
            if (settings.AlertOnInactivity)
                WriteLog(" + On Inactivity (" + settings.InactivityTimeout + " minutes)");
            if (settings.AlertOnPlayerFollows)
                WriteLog(" + On " + settings.MinutesPlayerFollows + " minutes after a player follows the bot");
            if (settings.AlertOnPlayerTargets)
                WriteLog(" + On " + settings.MinutesPlayerTargets + " minutes after a player targets the bot");
            if (settings.AlertOnWhisperGm)
                WriteLog(" + On get a whisper from a GM");
            if (settings.AlertOnTeleportation)
                WriteLog(" + On teleported further than " + settings.TeleportYards + " yards");
            if (settings.AlertOnAchievement)
                WriteLog(" + On achievement completed (id: " + settings.AchievementId + ")");
            WriteLog("-------------------------------------------------");
            _isStartingCodeExecuted = true;
        }

        /// <summary>
        /// Lors de l'acceptation d'une quête.
        /// </summary>
        private void QuestingOnOnQuestAccepted(Quest quest)
        {
            // L'acception d'une quête peut causer une téléportation
            // Évite que cela ne soit le cas
            _lastTeleportableAction = DateTime.Now;
        }

        void OnBotStopped(EventArgs args)
        {
            //Arrête de parse les logs de HB
            Logging.OnLogMessage -= Logging_OnLogMessage;
            //Unbind les événements de réception de chuchotements
            Chat.Whisper -= HandleWhispers;
            _isStartingCodeExecuted = false;
        }

        /// <summary>
        /// Méthode s'exécutant lors de l'écriture dans la console debug.
        /// </summary>
        public void Logging_OnLogMessage(System.Collections.ObjectModel.ReadOnlyCollection<Logging.LogMessage> messages)
        {
            foreach (Logging.LogMessage logMessage in messages)
            {
                ParseLogMessage(logMessage.Message);
            }

        }

        /// <summary>
        /// Évalue sur un message du log doit déclencher un trigger.
        /// </summary>
        /// <param name="message"></param>
        public void ParseLogMessage(string message)
        {
            //Vérifie si le personnage n'a pas pu looter le mob si le trigger est actif
            if (LogMeOutSettings.Instance.AlertOnCannotLoot)
            {
                //S'il y a un message d'erreur de loot, nous enregistrons l'heure
                if (message.Contains("Reason Tried to loot more than 2 times"))
                {
                    _listDtCannotLoot.Add(DateTime.Now);
                    WriteLog("Cannot loot this mob ({0}/3 in the latest 5 minutes)", _listDtCannotLoot.Count);
                }
                //Efface les erreurs de loot plus vieilles que 5 minutes
                _listDtCannotLoot.RemoveAll(er => er.AddMinutes(5) < DateTime.Now);
                //S'il y a au moins trois entrées dans la liste, c'est que nous avons un problème !
                if (_listDtCannotLoot.Count >= 3)
                {
                    Disconnect("The player cannot loot mobs.");
                }
            }

            //Récupère le nombre de stucks consécutifs si le trigger est actif
            if (LogMeOutSettings.Instance.AlertOnStucks)
            {
                //Si un stuck est détecté
                if (message.Contains("We are stuck!"))
                {
                    // S'il n'y a pas eu de stuck durant les 6 dernières secondes
                    if ((DateTime.Now - _lastStuck).TotalSeconds > 6)
                    {
                        // Enregistre le moment du stuck
                        _lastStuck = DateTime.Now;
                        //Si c'est notre premier stuck (pas eu d'autre il y a 25 secondes)
                        if ((DateTime.Now - _dtLastStuck).TotalSeconds > 25)
                        {
                            _intNbStucks = 1;
                        }
                        //Si ce n'est pas le premier de la série
                        else
                        {
                            _intNbStucks++;
                        }

                        //On enregistre à quel moment il a été effectué
                        _dtLastStuck = DateTime.Now;

                        //On inscrit un message dans la console Log
                        var position = Me.Location;
                        WriteLog("Stuck detected at <X=" + position.X + "; Y=" + position.Y + "; Z=" + position.Z +
                                 ">. Number of consecutive stuck : " +
                                 _intNbStucks + "/" + LogMeOutSettings.Instance.NbStucks);

                        //Emet un bip si l'option a été activée
                        if (LogMeOutSettings.Instance.LoggingBeepWhenStuck)
                        {
                            Beep();
                        }
                    }
                }
            }
            //Vérifie si le personnage n'a pas pu looter le mob si le trigger est actif
            if (LogMeOutSettings.Instance.AlertOnCannotLoot)
            {
                //S'il y a un message d'erreur de loot, nous enregistrons l'heure
                if (message.Contains("Reason Tried to loot more than 2 times"))
                {
                    _listDtCannotLoot.Add(DateTime.Now);
                    WriteLog("Cannot loot this mob ({0}/3 in the latest 5 minutes)", _listDtCannotLoot.Count);
                }
                //Efface les erreurs de loot plus vieilles que 5 minutes
                _listDtCannotLoot.RemoveAll(er => er.AddMinutes(5) < DateTime.Now);
                //S'il y a au moins trois entrées dans la liste, c'est que nous avons un problème !
                if (_listDtCannotLoot.Count >= 3)
                {
                    Disconnect("The player cannot loot mobs.");
                }
            }
        }

        /// <summary>
        /// Méthode s'exécutant lors d'un chuchotement.
        /// </summary>
        public void HandleWhispers(Chat.ChatWhisperEventArgs e)
        {
            //Vérifie que le plugin et que le déclencheur de chuchotement soit activé
            if (IsEnabled() && LogMeOutSettings.Instance.AlertOnWhispersReceived)
            {
                // Vérifie si nous ignorons les chuchotements du groupe/raid
                if (LogMeOutSettings.Instance.ExceptionMembersPartyRaid &&
                    Me.PartyMembers.Union(Me.RaidMembers)
                        .Any(p => p.Name.Equals(e.Author)))
                {
                    return;
                }
                //Incrémente le compteur
                _intNbWhispers++;
                //Indique que nous avons reçu un chuchotement
                WriteLog("Got a whisper from " + e.Author + " (IsGM=" + (e.Status == "GM") + "). " + _intNbWhispers + "/" + LogMeOutSettings.Instance.NbWhispersReceived + " (message: \"" + e.Message + "\")");
                //Emet un bip si l'option a été activée
                if (LogMeOutSettings.Instance.BeepWhenFire)
                    Beep();
                // Vérifie s'il s'agit d'un GM
                if (LogMeOutSettings.Instance.AlertOnWhisperGm && e.Status == "GM")
                    Disconnect("Message got from a Game Master.");
            }
        }

        /// <summary>
        /// Méthode exécutée lors des pulsations du plugin.
        /// </summary>
        public override void Pulse()
        {
            //Si le code de démarrage n'est pas terminé, pas de pulse !
            if (!_isStartingCodeExecuted)
                return;

            //Crée un pointeur sur l'instance des données sauvegardées
            var settings = LogMeOutSettings.Instance;
            //Charge les derniers paramètres du plugin
            settings.Load();

            //Vérifie si le joueur est en phase de déconnexion
            if (IsLoggingOut)
                Disconnect("");

            //Rafraichit les joueurs environnant dans le cache de LogMeOut!
            _objPeopleAroundMe.Update();

            //Détecte l'action du joueur
            RefreshLastAction();

            //Si le joueur ne se déconnecte pas vers les mailbox
            if (settings.ExceptionMailbox && IsMailboxNearby())
                return;

            // Vérifie si le joueur a été téléporté
            if (settings.AlertOnTeleportation)
            {
                // Vérifie une liste d'action pouvant causer une téléportation
                // Détermine si le joueur est à portée d'un objet invoqué
                if (ObjectManager.GetObjectsOfType<WoWGameObject>().Any(o => o.SubType == WoWGameObjectType.SpellCaster && o.CanUseNow()))
                    _lastTeleportableAction = DateTime.Now;
                // Détermine si le joueur est dans une instance/champ de bataille
                if (Me.IsInInstance || Battlegrounds.IsInsideBattleground)
                    _lastTeleportableAction = DateTime.Now;
                // Détermine si le joueur a une fenêtre de discution avec un PNJ
                if (GossipFrame.Instance.IsVisible || QuestFrame.Instance.IsVisible)
                    _lastTeleportableAction = DateTime.Now;
                // Détermine si le joueur est à proximité d'une pierre de rencontre
                if (ObjectManager.GetObjectsOfType<WoWGameObject>().Any(o => o.SubType == WoWGameObjectType.MeetingStone && o.CanUseNow()))
                    _lastTeleportableAction = DateTime.Now;
                // Détermine si le joueur est sur un zeppelin/bateau
                if (ObjectManager.GetObjectsOfType<WoWGameObject>().Any(o => o.SubType == WoWGameObjectType.MapObjectTransport && o.Distance < 50))
                    _lastTeleportableAction = DateTime.Now.AddSeconds(20); // Ajoute un délai pour l'écran de chargement
                // Détermine si le joueur est à proximité d'un PNJ donnant une/des quête(s)
                if (ObjectManager.GetObjectsOfType<WoWUnit>().Any(u => u.IsQuestGiver && u.WithinInteractRange))
                    _lastTeleportableAction = DateTime.Now;
                // Certains PNJ téléportent le joueur et doivent être ajoutés en exception
                if (ObjectManager.GetObjectsOfType<WoWUnit>().Any(u => u.Entry == 48708 && u.Distance < 10)) // The Uncrashable (Eastern Plaguelands)
                    _lastTeleportableAction = DateTime.Now;
                // Liste de situations dans lesquelles le joueur n'est pas téléporté sans raison
                var isAbleToTeleport =
                    Me.IsDead || // Si le joueur est mort (téléportation au cimetière)
                    Me.HealthPercent < 5 ||
                    Me.Auras.Any(a => a.Value.SpellId == 8326) || // Si le joueur possède le buff "Fantôme"
                    Me.ActiveAuras.Any(a => a.Value.SpellId.Equals(2479)) || // Si le joueur a l'aura "Cible sans honneur"
                    (DateTime.Now - _lastTeleportableAction).TotalSeconds <= 5; // Si le joueur est/était près d'un élément invoqué

                // Détermine la distance entre le joueur et sa dernière position connue
                if (Me.Location.Distance(_lastPosition) >= settings.TeleportYards && !isAbleToTeleport)
                {
                    //Emet un bip si l'option a été activée
                    if (settings.BeepWhenFire)
                        Beep();
                    // Déconnecte le bot
                    Disconnect("The player was moved for about " + Me.Location.Distance(_lastPosition) + " yards. Previous position: " + _lastPosition + "; Actual position: " + Me.Location);
                }
                // Rafraîchit la nouvelle position
                _lastPosition = Me.Location;
            }

            //Vérifie si les sacs du joueur sont plein
            if (settings.AlertOnBagsFull)
            {
                //Tente de laisser au moins un slot libre dans les sacs
                if (Me.FreeBagSlots <= 1)
                {
                    //Emet un bip si l'option a été activée
                    if (settings.BeepWhenFire)
                        Beep();
                    //Déconnecte le bot
                    Disconnect("The character's bags are full !");
                }
            }

            //Vérifie le temps passé
            if (settings.AlertOnTimeElapsed)
            {
                //Détermine le temps restant
                var tsRemaining = (_dtStartingTimer.AddHours(settings.HoursElapsed).AddMinutes(settings.MinutesElapsed) - DateTime.Now);

                //Vérifie s'il est temps de se déconnecter
                if (tsRemaining <= TimeSpan.Zero)
                {
                    //Emet un bip si l'option a été activée
                    if (settings.BeepWhenFire)
                        Beep();
                    //Déconnecte le bot
                    Disconnect("Logout time reached");
                }
                //Vérifie si l'option de log est activée
                if (settings.LoggingTime)
                {
                    //S'il est temps d'écrire le temps avant la déconnexion
                    if ((DateTime.Now - _dtLastLoggingTime).TotalMinutes >= settings.LoggingTimeEvery)
                    {
                        //Enregistrement du moment présent
                        _dtLastLoggingTime = DateTime.Now;
                        //Ecriture du log
                        WriteLog("Remaining time before log out : " + tsRemaining.Hours.ToString("00") + "h" + tsRemaining.Minutes.ToString("00") + "m" + tsRemaining.Seconds.ToString("00") + "s");
                    }
                }
            }
            else
            {
                //Sinon l'heure du début du timer est égal à maintenant
                _dtStartingTimer = DateTime.Now;
            }

            //Vérifie le nombre de mort du joueur
            if (settings.AlertOnDeaths)
            {
                //Si l'exception du nombre de mort pendant les BGs est activée
                if (!(Battlegrounds.IsInsideBattleground && settings.ExceptionCountDeathsBg))
                {
                    //Si le plugin n'a pas encore traité la mort du joueur
                    if (!Me.IsAlive)
                    {
                        if (_blnIsAlive)
                        {
                            //Incrémente le nombre de mort
                            _intNbDeaths++;
                            //Affiche comment le joueur est mort
                            WriteLog("Death detected " + _intNbDeaths + "/" + settings.NbDeaths + ". Zone name : " + Me.RealZoneText + " (X=\"" + Me.X + "\" Y=\"" + Me.Y + "\" Z=\"" + Me.Z + "\").");
                            //Affiche qui attaquait le joueur ou si le joueur est mort suite à une chute
                            if (!string.IsNullOrEmpty(_strTargetingUnitsAndPlayersNames))
                                WriteLog("The following unit(s) killed you : " + _strTargetingUnitsAndPlayersNames);
                            else if ((DateTime.Now - _dtLastIsFalling).TotalSeconds <= 1)
                                WriteLog("I died because I fell too far !");
                            //Si le nombre de mort a atteint le quota désigné par le joueur
                            if (_intNbDeaths >= settings.NbDeaths)
                                Disconnect("The character is dead " + _intNbDeaths + " times ! (" + settings.NbDeaths + " max)");
                            //Annonce que le bot est mort
                            _blnIsAlive = false;
                            //Emet un bip si l'option a été activée
                            if (settings.BeepWhenFire)
                                Beep();
                        }
                    }
                    else
                    {
                        //Annonce que le bot est vivant
                        _blnIsAlive = true;
                        //Si le bot est en chute libre, nous enregistrons le moment
                        if (Me.IsFalling)
                            _dtLastIsFalling = DateTime.Now;

                        //Récupère tous les joueurs et unités non alliés qui ciblent le joueur (pour les afficher si le bot meurt par la suite)
                        _strTargetingUnitsAndPlayersNames = "";
                        foreach (WoWObject wowObj in _objPeopleAroundMe.GetAllTargetingNonFriendlyUnitAndPlayers())
                        {
                            //Ajoute le nom de l'unité ou du joueur dans la chaîne de caractères
                            //Si c'est un joueur
                            if (wowObj is WoWPlayer)
                                _strTargetingUnitsAndPlayersNames += "[Player] " + wowObj.ToPlayer().Name + "(" + wowObj.ToPlayer().Level + "); ";
                            //Si c'est un MJ
                            else if (wowObj is WoWPlayer && wowObj.ToPlayer().IsGM)
                                _strTargetingUnitsAndPlayersNames += "[GM] " + wowObj.ToPlayer().Name;
                            //Si c'est un pet
                            else if (wowObj is WoWUnit && wowObj.ToUnit().IsPet)
                            {
                                _strTargetingUnitsAndPlayersNames += "[Pet] " + wowObj.ToUnit().Name;
                                if (wowObj.ToUnit().OwnedByUnit != null)
                                    _strTargetingUnitsAndPlayersNames += "(of " + wowObj.ToUnit().OwnedByUnit.Name + "); ";
                            }
                            //Si c'est un mob élite
                            else if (wowObj is WoWUnit && wowObj.ToUnit().Elite)
                                _strTargetingUnitsAndPlayersNames += "[Elite] " + wowObj.ToUnit().Name + "(" + wowObj.ToUnit().Level + "); ";
                            //Si c'est un mob
                            else
                                _strTargetingUnitsAndPlayersNames += "[Mob] " + wowObj.ToUnit().Name + "(" + wowObj.ToUnit().Level + "); ";
                        }
                    }
                }
            }

            //Vérifie le nombre de stucks
            if (settings.AlertOnStucks)
            {
                //Si le nombre de stuck maximum a été dépassé, nous déconnectons le bot
                if (_intNbStucks >= settings.NbStucks)
                {
                    //Emet un bip si l'option a été activée
                    if (settings.BeepWhenFire)
                        Beep();

                    //Déconnecte le bot
                    Disconnect("The character was stuck " + _intNbStucks + " times !");
                }
            }

            //Vérifie si le temps de combat ne dépasse pas le quota
            if (settings.AlertOnMinutesInCombat)
            {
                //Il faut que le bot soit en combat
                if (Me.Combat)
                {
                    //Vérifie le temps passé en combat
                    if ((DateTime.Now - _dtInCombat).TotalMinutes >= settings.NbMinutesInCombat)
                    {
                        //Emet un bip si l'option a été activée
                        if (settings.BeepWhenFire)
                            Beep();

                        //Déconnecte le bot
                        Disconnect("The fight lasts more than " + settings.NbMinutesInCombat + " minutes.");
                    }
                }
                else
                {
                    //Durée du combat ramenée à maintenant
                    _dtInCombat = DateTime.Now;
                }
            }

            //Vérifie le nombre de monstres tués
            if (settings.AlertOnMobsKilled)
            {
                if (GameStats.MobsKilled >= settings.NbMobsKilled)
                {
                    //Emet un bip si l'option a été activée
                    if (settings.BeepWhenFire)
                        Beep();
                    //Déconnecte le bot
                    Disconnect("The character killed " + GameStats.MobsKilled + " mobs. (" + settings.NbMobsKilled + " max)");
                }
            }

            //Vérifie si le joueur possède un objet un certain nombre de fois
            if (settings.AlertOnLootedItem)
            {
                if (NumberOfItemInBag(settings.LootedItemId) >= settings.LootedItemTimes)
                {
                    if (settings.BeepWhenFire)
                        Beep();
                    Disconnect("The character looted the item n°" + settings.LootedItemId + " at least " + settings.LootedItemTimes + " times");
                }
            }

            if (settings.AlertOnDurability)
            {
                if (Me.DurabilityPercent * 100 < settings.DurabilityPercent)
                {
                    if (settings.BeepWhenFire)
                        Beep();
                    Disconnect("The character is under " + settings.DurabilityPercent + "% of durability");
                }
            }

            //Vérifie le nombre de whispes reçus
            if (settings.AlertOnWhispersReceived)
            {
                if (_intNbWhispers >= settings.NbWhispersReceived)
                    Disconnect("The character received " + _intNbWhispers + " whispes.");
            }

            //Vérifie le nombre de points atteints
            if (settings.AlertOnPoints)
            {
                int intMonnaie = 0;

                switch (settings.TypePoints)
                {
                    //Alerte sur les points de justice
                    case 0:
                        intMonnaie = GetCurrency((int)IdPoints.Justice);
                        break;
                    case 1:
                        intMonnaie = GetCurrency((int)IdPoints.Vaillance);
                        break;
                    case 2:
                        intMonnaie = GetCurrency((int)IdPoints.Honneur);
                        break;
                    case 3:
                        intMonnaie = GetCurrency((int)IdPoints.Conquete);
                        break;
                }
                //Déconnexion si le nombre de points dépasse le seuil limite
                if (intMonnaie >= settings.NbPoints)
                {
                    //Emet un bip si l'option a été activée
                    if (settings.BeepWhenFire)
                        Beep();
                    //Déconnecte le bot
                    Disconnect("The character reached " + intMonnaie + " " + Arrays.NamesPoints[settings.TypePoints] + " points ! (" + settings.NbPoints + " max)");
                }
            }

            //Vérification du niveau du joueur
            if (settings.AlertOnLevelReached)
            {
                if (Me.Level >= settings.NbLevel)
                {
                    //Emet un bip si l'option a été activée
                    if (settings.BeepWhenFire)
                        Beep();
                    //Déconnecte le bot
                    Disconnect("The character reached the level " + settings.NbLevel + " !");
                }
            }

            //Vérifie si le joueur possède une aura spécifique
            if (settings.AlertOnGotAura)
            {
                if (Me.HasAura(settings.GotAuraName))
                {
                    if (settings.BeepWhenFire)
                        Beep();
                    Disconnect("The character got the aura " + settings.GotAuraName);
                }
            }

            //Vérifie si le joueur possède un certain niveau dans une profession
            if (settings.AlertOnTrainingSkill)
            {
                if (Classes.TrainingSkills.HasReached(settings.TrainingSkillLine, settings.TrainingSkillLevel))
                {
                    if (settings.BeepWhenFire)
                        Beep();
                    Disconnect("The character reached " + settings.TrainingSkillLevel + " in " + settings.TrainingSkillLine.ToString());
                }
            }

            //Vérifie si le joueur n'est pas inactif
            if (settings.AlertOnInactivity)
            {
                if ((DateTime.Now - _lastAction).TotalMinutes >= settings.InactivityTimeout)
                {
                    if (settings.BeepWhenFire)
                        Beep();
                    Disconnect("The character is inactive since " + settings.InactivityTimeout + " minutes");
                }
            }

            //Vérifie si des joueurs suivent le joueur
            if (settings.AlertOnPlayerFollows)
            {
                //Obtient les joueurs qui sont autour du bot plus longtemps que le temps spécifié par l'utilisateur
                WoWObject[] followers = _objPeopleAroundMe.GetAllPlayers(new TimeSpan(0, settings.MinutesPlayerFollows, 0), settings.ExceptionMembersPartyRaid);
                if (followers.Length > 0)
                {
                    //Raison de la déconnexion
                    String strRaison = "These players follow the bot during more than " + settings.MinutesPlayerFollows + " minutes : ";
                    foreach (WoWObject wowObj in followers)
                    {
                        strRaison += wowObj.Name + " ";
                    }
                    //Emet un bip si l'option a été activée
                    if (settings.BeepWhenFire)
                        Beep();
                    //Déconnecte le bot
                    Disconnect(strRaison);
                }
            }

            //Vérifie si des joueurs ciblent le joueur
            if (settings.AlertOnPlayerTargets)
            {
                //Obtient les joueurs qui ciblent le bot plus longtemps que le temps spécifié par l'utilisateur
                var targeters = _objPeopleAroundMe.GetAllTargetingPlayers(new TimeSpan(0, settings.MinutesPlayerTargets, 0), settings.ExceptionMembersPartyRaid);
                if (targeters.Length > 0)
                {
                    //Raison de la déconnexion
                    var strRaison = "These players target the bot during more than " + settings.MinutesPlayerTargets + " minutes : ";
                    foreach (var wowObj in targeters)
                    {
                        strRaison += wowObj.Name + " ";
                    }
                    //Emet un bip si l'option a été activée
                    if (settings.BeepWhenFire)
                        Beep();
                    //Déconnecte le bot
                    Disconnect(strRaison);
                }
            }

            // Vérifie si le haut-fait a été accompli
            if (settings.AlertOnAchievement)
            {
                if (IsAchievementCompleted(settings.AchievementId))
                {
                    //Emet un bip si l'option a été activée
                    if (settings.BeepWhenFire)
                        Beep();
                    Disconnect("Achievement #" + settings.AchievementId + " completed");
                }
            }
        }

        /// <summary>
        /// Méthode de déconnexion du joueur.
        /// </summary>
        /// <param name="reason">Raison de la déconnexion</param>
        public void Disconnect(string reason)
        {
            //Crée un pointeur sur l'instance des données sauvegardées
            LogMeOutSettings settings = LogMeOutSettings.Instance;
            //Ecriture de textes dans le log si cela n'a pas déjà été fait auparavant
            if (!IsLoggingOut)
            {
                WriteLog("Starting logout process...");
                WriteLog("Reason : " + reason);

                //Indique que la procédure de déconnexion a été lancé
                IsLoggingOut = true;

                //Si nous ne voulons pas que le joueur soit déconnecté
                if (settings.ExceptionDontLogOut)
                    WriteLog("Don't log out option is activated. The character won't log out. Please stop/start the bot to clear the triggers.");
            }

            //Si nous ne voulons pas que le joueur soit déconnecté
            if (settings.ExceptionDontLogOut)
                return;

            #region "Phase de vérification du personnage"
            //Vérifie si le processus ets déjà arrivé à terme
            if (_blnDisconnected)
            {
                //Termine la séquence de déconnexion
                return;
            }
            //Vérifie que le joueur est connecté
            if (!StyxWoW.IsInGame)
            {
                //Termine la séquence de déconnexion
                return;
            }
            //Vérifie si le joueur se trouve dans un champ de bataille
            if (Battlegrounds.IsInsideBattleground)
            {
                //Si l'exception est activée, nous restons jusqu'à la fin
                if (settings.ExceptionBg)
                {
                    if (!_blnBattlegroundOrInstance)
                        WriteLog("The character is in the battleground " + Battlegrounds.Current.ToString() + ". Waiting the end...");
                    _blnBattlegroundOrInstance = true;
                    //Reset le temps du timeout
                    _dtStartingLogOutProcess = DateTime.MinValue;
                    //Termine la séquence de déconnexion
                    return;
                }

                //Sinon nous quittons
                Battlegrounds.LeaveBattlefield();
                //Attente de l'écran de chargement
                WaitLoadingScreens();
                //Termine la séquence de déconnexion
                return;
            }
            //Vérifie si le joueur se trouve dans une instance
            if (Me.IsInInstance)
            {
                if (settings.ExceptionInstance)
                {
                    if (!_blnBattlegroundOrInstance)
                        WriteLog("The character is in the instance (name: " + Me.RealZoneText + "). Waiting the end...");
                    _blnBattlegroundOrInstance = true;
                    //Reset le temps du timeout
                    _dtStartingLogOutProcess = DateTime.MinValue;
                    //Termine la séquence de déconnexion
                    return;
                }
            }
            //Réinitialise l'avertissement des champs de bataille et instances
            _blnBattlegroundOrInstance = false;
            //Vérifie que le joueur ne soit pas en combat
            if (Me.Combat && !settings.AlertOnMinutesInCombat)
            {
                if (!_blnCombat)
                    WriteLog("Waiting the end of the fight...");
                _blnCombat = true;
                //Reset le temps du timeout
                _dtStartingLogOutProcess = DateTime.MinValue;
                //Termine la séquence de déconnexion
                return;
            }
            //Réinitialise l'avertissement de combat
            _blnCombat = false;

            //Une fois les vérifications obligatoires, on démarre le timer du timeout
            if (_dtStartingLogOutProcess == DateTime.MinValue)
                _dtStartingLogOutProcess = DateTime.Now;
            //Détermine si le bot doit se déconnecter sans exécuter certains contrôles à cause du timeout
            if (!MustLogOut)
            {
                //Vérifie si le joueur n'est pas mort
                if ((Me.IsDead || Me.IsGhost) && !settings.ExceptionDontRunToCorpse)
                {
                    if (!_blnIsDead)
                        WriteLog("The character is dead. Waiting respawn...");
                    _blnIsDead = true;
                    //Termine la séquence de déconnexion
                    return;
                }
                //Réinitialise l'avertissement de mort
                _blnIsDead = false;
                //Vérifie si le joueur boit ou mange
                if (Me.Buffs.ContainsKey("Food"))
                {
                    WriteLog("The character is eating. Waiting...");
                    //Termine la séquence de déconnexion
                    return;
                }
                //Reset la variable de combat car il ne l'est plus
                _blnCombat = false;
                //Si le joueur veut lancer une action avant la déconnexion
                if (settings.ActionBefore != 0 && Me.IsAlive)
                {
                    //Vérifie si le joueur est sur une monture volante ou sur un trajet aérien si nous souhaitons utiliser une objet/sort avant la déconnexion
                    if (Me.IsFlying || Me.IsOnTransport)
                    {
                        if (!_blnFlying)
                            WriteLog("The character is flying. Waiting...");
                        _blnFlying = true;
                        //Termine la séquence de déconnexion
                        return;
                    }
                    //Vérifie que le joueur ne soit pas en train de se déplacer
                    if (Me.IsMoving || Me.IsFalling)
                    {
                        //Termine la séquence de déconnexion
                        return;
                    }
                    //Réinitialise l'avertissement de vol
                    _blnFlying = false;

                    if (Me.IsCasting)
                    {
                        WriteLog("Waiting the end of your actual cast (" + Me.CastingSpell.Name + ")");
                        //Termine la séquence de déconnexion
                        return;
                    }
                    switch (settings.ActionBefore)
                    {
                        //Si nous voulons exécuter une pierre de foyer
                        case 1:
                            //Récupère les éventuelles objets faisant office de pierre de foyer
                            var hearthItems = GetItemsByIdFromBags(HearthItemIDs).ToArray();
                            //Vérifie que nous possédions au moins une pierre de foyer
                            if (!hearthItems.Any())
                            {
                                if (!_blnAlertActionBefore)
                                    WriteLog("No hearth items on the character.");
                                _blnAlertActionBefore = true;
                            }
                            else
                            {
                                // Tente d'obtenir une pierre de foyer sans cooldown
                                var hearth = hearthItems.FirstOrDefault(item => item.Cooldown == 0);

                                // Dans le cas où le caste est terminé
                                if (_abilityBeforeExitTimestamp.HasValue)
                                {
                                    // Patiente 1 seconde le temps que le jeu caste réellement la technique (sinon le reste intervient trop tot)
                                    if (DateTime.UtcNow < _abilityBeforeExitTimestamp.Value + TimeSpan.FromSeconds(1))
                                    {
                                        return;
                                    }

                                    // Vérifie que le temps entre le début du caste et la fin correspond au moins à la longueur de caste de l'objet/sort
                                    if (DateTime.UtcNow < _abilityBeforeExitTimestamp.Value + _abilityBeforeExitCastTime)
                                    {
                                        // La technique n'a pas pu se terminer correctement
                                        _abilityBeforeExitTimestamp = null;
                                        return;
                                    }

                                    // La technique a pu se terminer correctement, on interrompt le reste du code
                                    break;
                                }

                                // Si l'objet/sort est est en recharge
                                if (hearth == null)
                                {
                                    if (!_blnAlertActionBefore)
                                    {
                                        WriteLog("Your hearth item is not ready yet.");
                                    }
                                    _blnAlertActionBefore = true;

                                    //Termine la séquence de déconnexion
                                    return;
                                }

                                // Le bot n'a pas encore utilisé la pierre de foyer
                                if (!_abilityBeforeExitTimestamp.HasValue)
                                {
                                    // Utilise la pierre de foyer ou l'objet équivalent
                                    WriteLog("Using " + hearth.Name + "...");
                                    hearth.Use();

                                    // Inscrit le temps de caste
                                    _abilityBeforeExitCastTime = TimeSpan.FromMilliseconds(hearth.ItemSpells[0].ActualSpell.CastTime);

                                    // Inscrit le timestamp du déclenchement
                                    _abilityBeforeExitTimestamp = DateTime.UtcNow;

                                    return;
                                }
                            }
                            break;
                        //Si nous voulons lancer un sort
                        case 2:
                            //Vérifie que le joueur possède ce sort
                            if (!SpellManager.HasSpell(settings.SpellName))
                            {
                                if (!_blnAlertActionBefore)
                                    WriteLog("The character does not have the spell : " + settings.SpellName);
                                _blnAlertActionBefore = true;
                            }
                            else
                            {
                                // Dans le cas où le caste est terminé
                                if (_abilityBeforeExitTimestamp.HasValue)
                                {
                                    // Patiente 1 seconde le temps que le jeu caste réellement la technique (sinon le reste intervient trop tot)
                                    if (DateTime.UtcNow < _abilityBeforeExitTimestamp.Value + TimeSpan.FromSeconds(1))
                                    {
                                        return;
                                    }

                                    // Vérifie que le temps entre le début du caste et la fin correspond au moins à la longueur de caste de l'objet/sort
                                    if (DateTime.UtcNow < _abilityBeforeExitTimestamp.Value + _abilityBeforeExitCastTime)
                                    {
                                        // La technique n'a pas pu se terminer correctement
                                        _abilityBeforeExitTimestamp = null;
                                        return;
                                    }

                                    // La technique a pu se terminer correctement, on interrompt le reste du code
                                    break;
                                }

                                //Vérifie que le joueur peut lancer ce sort
                                if (!SpellManager.CanCast(settings.SpellName))
                                {
                                    if (!_blnAlertActionBefore)
                                        WriteLog("The character cannot cast the spell : " + settings.SpellName);
                                    _blnAlertActionBefore = true;
                                }
                                else
                                {
                                    //Tente de lancer le sort sur sois-même
                                    if (!_abilityBeforeExitTimestamp.HasValue)
                                    {
                                        // Caste le spell
                                        WriteLog("Casting " + settings.SpellName + "...");
                                        SpellManager.Cast(settings.SpellName, Me.ToUnit());

                                        // Inscrit le temps de caste
                                        _abilityBeforeExitCastTime = TimeSpan.FromMilliseconds(SpellManager.Spells[settings.SpellName].CastTime);

                                        // Inscrit le timestamp du déclenchement
                                        _abilityBeforeExitTimestamp = DateTime.UtcNow;

                                        return;
                                    }
                                }
                            }
                            break;
                        //Si nous voulons utiliser un objet
                        case 3:
                            //Récupère l'objet dans les sacs
                            var specificItems = GetItemsByIdFromBags((new[] { (uint)settings.ItemId })).ToArray();
                            //Vérifie que cet objet existe bien dans notre inventaire
                            if (!specificItems.Any())
                            {
                                if (!_blnAlertActionBefore)
                                    WriteLog("The character does not have the item ID : " + settings.ItemId);
                            }
                            else
                            {
                                var specificItem = specificItems.First();

                                // Dans le cas où le caste est terminé
                                if (_abilityBeforeExitTimestamp.HasValue)
                                {
                                    // Patiente 1 seconde le temps que le jeu caste réellement la technique (sinon le reste intervient trop tot)
                                    if (DateTime.UtcNow < _abilityBeforeExitTimestamp.Value + TimeSpan.FromSeconds(1))
                                    {
                                        return;
                                    }

                                    // Vérifie que le temps entre le début du caste et la fin correspond au moins à la longueur de caste de l'objet/sort
                                    if (DateTime.UtcNow < _abilityBeforeExitTimestamp.Value + _abilityBeforeExitCastTime)
                                    {
                                        // La technique n'a pas pu se terminer correctement
                                        _abilityBeforeExitTimestamp = null;
                                        return;
                                    }

                                    // La technique a pu se terminer correctement, on interrompt le reste du code
                                    break;
                                }

                                //Vérifie si celui-ci possède un cooldown actif
                                if (specificItem.Cooldown != 0)
                                {
                                    if (!_blnAlertActionBefore)
                                        WriteLog("Cannot use \"" + specificItem.Name + "\" because this item is not ready.");
                                    //Termine la séquence de déconnexion
                                    return;
                                }

                                //Tente d'utiliser l'objet proposé
                                if (!specificItem.Usable)
                                {
                                    if (!_blnAlertActionBefore)
                                        WriteLog("The item \"" + specificItem.Name + "\" is not usable.");
                                }
                                else
                                {
                                    // Le bot n'a pas encore utilisé l'objet
                                    if (!_abilityBeforeExitTimestamp.HasValue)
                                    {
                                        // Utilise l'objet
                                        WriteLog("Using " + specificItem.Name + "...");
                                        specificItem.Use();

                                        // Inscrit le temps de caste
                                        _abilityBeforeExitCastTime = TimeSpan.FromMilliseconds(specificItem.ItemSpells[0].ActualSpell.CastTime);

                                        // Inscrit le timestamp du déclenchement
                                        _abilityBeforeExitTimestamp = DateTime.UtcNow;

                                        return;
                                    }
                                }
                            }
                            break;
                        //Si nous voulons exécuter un script Lua
                        case 4:
                            WriteLog("Executing a lua script with a waiting time of {0} second(s)...", settings.LuaWaitingTime);
                            Lua.DoString(settings.LuaString);
                            Thread.Sleep(TimeSpan.FromSeconds(settings.LuaWaitingTime));
                            break;
                    }
                }
            }
            else
            {
                //Si le bot doit forcer la déconnexion, affiche un message la première fois
                if (!_blnForceLogOutMessageShown)
                {
                    WriteLog("Timeout of the logout reached. Force quit now.");
                    _blnForceLogOutMessageShown = true;
                }
            }

            #endregion

            //Inscrit la raison de la déconnexion
            settings.ReasonLastLogOut = reason;
            settings.Save();
            //Processus de déconnexion OK
            _blnDisconnected = true;
            //Déconnexion de World of Warcraft !
            WriteLog("Log out in 20 secondes...");
            Lua.DoString("Logout()");

            #region Action After
            //Effectue l'éventuelle action après la déconnexion
            switch (settings.ActionAfter)
            {
                //Eteint l'ordinateur
                case 1:
                    WriteLog("Shutdown the computer.");
                    Process.Start("shutdown", "/s /t 5 /f");
                    break;
                case 2:
                    WriteLog("Executes \"" + settings.BatchCommand + "\" with the argument(s) \"" + settings.BatchArgument + "\"");
                    Process.Start(settings.BatchCommand, settings.BatchArgument);
                    break;
            }

            //Vérifie si les reloggers doivent être arrêtés
            if (settings.KillReloggers)
            {
                WriteLog("Killing active reloggers...");
                Process.GetProcesses().Where(p => p.ProcessName == "ARelog" || p.ProcessName == "HBRelog").ToList().ForEach(p => p.Kill());
            }

            // Vérifie si le dossier CompiledAssemblies doit être vidé
            if (settings.ClearAssembliesFolder)
            {
                WriteLog("Prepare to clear the CompiledAssemblies folder when HonorBuddy is closing");
                _folderRemovalList.Add(new BotFolderRemoval("CompiledAssemblies"));
            }

            // Vérifie si le dossier Cache doit être vidé
            if (settings.ClearCacheFolder)
            {
                WriteLog("Prepare to clear the Cache folder when HonorBuddy is closing");
                _folderRemovalList.Add(new BotFolderRemoval("Cache"));
            }
            #endregion

            //Arrêt du processus
            WriteLog("Goodbye");
            Lua.DoString("ForceQuit()");
            //Si le processus ne s'est pas arrêté après 7 secondes, nous le tuons
            int intProcessId = StyxWoW.Memory.Process.Id;
            int intTickCount = Environment.TickCount;
            while (Environment.TickCount - intTickCount < 7000 && Process.GetProcessById(intProcessId) != null)
                Thread.Sleep(200);
            if (Process.GetProcessById(intProcessId) != null)
                Process.GetProcessById(intProcessId).Kill();

            //Arrêt du bot
            WriteLog("Stopping HonorBuddy...");
            Application.Exit();
        }

        /// <summary>
        /// IDs des différents objets faisant office de pierre de foyer.
        /// </summary>
        static readonly uint[] HearthItemIDs = 
        {
            (uint)HearthItems.PierreDeFoyer,
            (uint)HearthItems.PortailEtherien,
            (uint)HearthItems.Archeologie
        };

        /// <summary>
        /// Vérifie s'il existe un/des objet(s) faisant référence à un tableau d'IDs.
        /// </summary>
        /// <param name="uintItems">Tableau IDs d'objets à rechercher.</param>
        /// <returns></returns>
        IEnumerable<WoWItem> GetItemsByIdFromBags(IEnumerable<uint> uintItems)
        {
            return from WoWItem Item in Me.BagItems
                   where uintItems.Contains(Item.Entry)
                   select Item;
        }

        /// <summary>
        /// Méthode qui met en pause le bot durant les écrans de chargement.
        /// </summary>
        public void WaitLoadingScreens()
        {
            do
            {
                Thread.Sleep(100);
            } while (!StyxWoW.IsInWorld);
        }

        /// <summary>
        /// Détermine le nombre de monnaie que possède le joueur.
        /// </summary>
        /// <param name="intCurrencyId">ID de la monnaie.</param>
        /// <returns>Nombre de monnaie que possède le joueur.</returns>
        public int GetCurrency(int intCurrencyId)
        {
            return Lua.GetReturnVal<int>("return GetCurrencyInfo(" + intCurrencyId + ")", 1);
        }

        /// <summary>
        /// Détermine si le haut-fait a été accompli.
        /// </summary>
        /// <param name="achievementId">Identifiant du haut-fait.</param>
        public bool IsAchievementCompleted(int achievementId)
        {
            return Lua.GetReturnVal<bool>("return GetAchievementInfo(" + achievementId + ")", 3);
        }

        /// <summary>
        /// Vérifie qu ele plugin soit bien activé.
        /// </summary>
        /// <returns>Retourne l'état d'activation du plugin.</returns>
        public bool IsEnabled()
        {
            return PluginManager.Plugins.First(plugin => plugin.Name == Name).Enabled;
        }

        /// <summary>
        /// Vérifie que le dossier de LogMeOut! existe.
        /// </summary>
        public void CheckPluginPath()
        {
            if (!Directory.Exists(Path.Combine(Styx.Helpers.GlobalSettings.Instance.PluginsPath, Classes.Updater.StrPluginPath)))
                WriteLog("WARNING: The folder of LogMeOut! has been renamed ! Some features won't be able to work. Please, restore the name to \"LogMeOut\".");
        }

        /// <summary>
        /// Emet un bip.
        /// </summary>
        public void Beep()
        {
            //Il faut que le bot ne soit pas en train de se déconnecter pour émettre un bip
            if (!IsLoggingOut)
            {
                var spBeep = new System.Media.SoundPlayer(Path.Combine(Styx.Helpers.GlobalSettings.Instance.PluginsPath, Classes.Updater.StrPluginPath + "Sounds\\beep.wav"));
                spBeep.PlaySync();
                spBeep.PlaySync();
            }

        }

        /// <summary>
        /// Affiche pourquoi le bot s'est déconnecté la dernière fois (si la déconnexion est dû grâce à LogMeOut!).
        /// </summary>
        public void CheckLastLogOut()
        {
            //Vérifie que cette information soit disponible
            if (!string.IsNullOrEmpty(LogMeOutSettings.Instance.ReasonLastLogOut))
            {
                //Affiche la raison de la précédente déconnexion
                WriteLog("Reason of the last log out : {0}", LogMeOutSettings.Instance.ReasonLastLogOut);
                //Efface cette raison
                LogMeOutSettings.Instance.ReasonLastLogOut = "";
                LogMeOutSettings.Instance.Save();
            }
        }

        /// <summary>
        /// Indique s'il y a une boîte aux lettres à proximité du joueur.
        /// </summary>
        /// <returns>True si une boîte aux lettres est présente.</returns>
        public bool IsMailboxNearby()
        {
            return ObjectManager.GetObjectsOfType<WoWGameObject>().Any(obj => obj.SubType == WoWGameObjectType.Mailbox);
        }

        /// <summary>
        /// Retourne le nombre de fois qu'un objet est dans l'inventaire du joueur.
        /// </summary>
        /// <param name="itemId">Id de l'objet.</param>
        /// <returns>Nombre entier correspondant au nombre de fois que l'objet est présent dans l'inventaire.</returns>
        public int NumberOfItemInBag(int itemId)
        {
            return Me.BagItems.Count(item => item.ItemInfo.Id == itemId);
        }

        /// <summary>
        /// Détecte si le joueur effectue une action.
        /// </summary>
        /// <remarks>Pour le trigger Inactivity Timeout.</remarks>
        public void RefreshLastAction()
        {
            // Si le joueur ne souhaite pas être déconnecté avec le debuff Déserteur
            if (LogMeOutSettings.Instance.ExceptionDeserterDebuff && Me.ActiveAuras.Any(a => a.Value.SpellId.Equals(26013)))
            {
                _lastAction = DateTime.Now;
            }

            // Détecte les actions du bot
            if (Me.IsMoving || Me.IsCasting || Me.IsChanneling || Me.IsActuallyInCombat)
            {
                _lastAction = DateTime.Now;
            }
        }

        #region Propriétés
        /// <summary>
        /// Renvoi l'heure de démarrage du bot.
        /// </summary>
        public DateTime StartingBot
        {
            get
            {
                return _dtStartingTimer;
            }
            set
            {
                _dtStartingTimer = value;
            }
        }

        /// <summary>
        /// Renvoi l'état du processus d'arrêt.
        /// </summary>
        public bool IsLoggingOut { get; set; }

        /// <summary>
        /// Indique si le bot DOIT se déconnecter.
        /// </summary>
        /// <remarks>Cela signifie que la procédure de déconnexion a été lancée il y a au moins 3 minutes.</remarks>
        public bool MustLogOut
        {
            get
            {
                if ((DateTime.Now - _dtStartingLogOutProcess).TotalMinutes >= 3)
                    return true;
                return false;
            }
        }
        #endregion
    }
}