using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Windows.Media;

namespace LogMeOut.Classes
{
    /// <summary>
    /// Classe gérant les unités et joueurs autour du bot.
    /// </summary>
    class PeopleAroundMe
    {
        /// <summary>
        /// Indique lorsque la dernière erreur a eu lieu.
        /// </summary>
        private DateTime _lastErrorTime;
        /// <summary>
        /// Contient toutes les unités et joueurs aux alentours et les stockent pendant un certain temps.
        /// </summary>
        private readonly List<WoWObject> _listUnitsAndPlayers;
        /// <summary>
        /// Contient le moment d'ajout des unités et joueurs dans la liste "listUnitsAndPeople".
        /// </summary>
        private readonly List<DateTime> _listAddingTime;
        /// <summary>
        /// Contient le moment lorsque des unités et joueurs cible le bot.
        /// </summary>
        private readonly List<DateTime> _listTargetingTime;

        /// <summary>
        /// Constructeur initialisant les variables.
        /// </summary>
        public PeopleAroundMe()
        {
            //Instanciation des listes
            _listUnitsAndPlayers = new List<WoWObject>();
            _listAddingTime = new List<DateTime>();
            _listTargetingTime = new List<DateTime>();
        }

        /// <summary>
        /// Met à jour la capture des unités et joueurs aux alentours.
        /// </summary>
        public void Update()
        {
            try
            {
                //Ajout les nouveaux joueurs et unités
                Dump();
                //Supprime les joueurs et unités inexistants
                Clean();
                //Rafraichit le ciblage des unités et joueurs
                Refresh();
            }
            catch (Exception ex)
            {
                // Evite de spam la console en défisissant un interval de 3 secondes entre l'affichage des erreurs
                if ((DateTime.Now - _lastErrorTime).Seconds > 3)
                {
                    // Sauvegarde l'heure actuelle
                    _lastErrorTime = DateTime.Now;

                    // Affiche l'erreur
                    LogMeOut.WriteLogError(
                        "An error occurred while dumping the units around the bot. Please restart HonorBuddy if this error is displayed multiple times. Details: {0}",
                        ex);
                }
            }
        }

        #region Méthodes de récupération d'informations
        /// <summary>
        /// Récupère tous les joueurs et unités aux alentours du bot.
        /// </summary>
        /// <returns>Retourne un tableau de WoWObject.</returns>
        public WoWObject[] GetAllUnitsAndPlayers()
        {
            return _listUnitsAndPlayers.Where(obj => !(obj is LocalPlayer)) .ToArray();
        }

        /// <summary>
        /// Récupère tous les joueurs et unités ciblant le bot aux alentours du bot.
        /// </summary>
        /// <returns>Retourne un tableau de WoWObject.</returns>
        public WoWObject[] GetAllTargetingUnitsAndPlayers()
        {
            return _listUnitsAndPlayers.Where(obj => !(obj is LocalPlayer) &&
                                                    (obj is WoWUnit && obj.ToUnit().IsTargetingMeOrPet) ||
                                                    (obj is WoWPlayer && obj.ToPlayer().IsTargetingMeOrPet)
                                                    ).ToArray();
        }

        /// <summary>
        /// Récupère tous les joueurs et unités non alliés ciblant le bot aux alentours du bot.
        /// </summary>
        /// <returns>Retourne un tableau de WoWObject.</returns>
        public WoWObject[] GetAllTargetingNonFriendlyUnitAndPlayers()
        {
            return GetAllTargetingUnitsAndPlayers().Where(obj => (obj is WoWUnit && !obj.ToUnit().IsFriendly) || (obj is WoWPlayer && !obj.ToPlayer().IsFriendly)).ToArray();
        }

        /// <summary>
        /// Récupère tous les joueurs ciblant le bot aux alentours du bot.
        /// </summary>
        /// <returns>Retourne un tableau de WoWObject.</returns>
        public WoWObject[] GetAllTargetingPlayers()
        {
            return GetAllTargetingUnitsAndPlayers().Where(obj => obj is WoWPlayer).ToArray();
        }

        /// <summary>
        /// Récupère tous les joueurs ciblant le bot aux alentours du bot depuis plus du temps spécifié.
        /// </summary>
        /// <param name="tsPresentSinceAtLeast">Temps de présence minimum dans le cache.</param>
        /// <param name="ignorePartyRaid">Indique si les joueurs appartenant au groupe ou raid doivent être ignorés.</param>
        /// <returns>Retourne un tableau de WoWObject.</returns>
        public WoWObject[] GetAllTargetingPlayers(TimeSpan tsPresentSinceAtLeast, bool ignorePartyRaid = false)
        {
            //Crée une liste qui contiendra les objets en question
            var woWObj = new List<WoWObject>();
            //Pou chaque objets
            for (int intIndex = 0; intIndex < _listUnitsAndPlayers.Count; intIndex++)
            {
                //Vérifie que le cet objet soit bien un joueur et qu'il cible bien notre bot depuis plus du temps spécifié
                if (_listUnitsAndPlayers[intIndex] is WoWPlayer && !(_listUnitsAndPlayers[intIndex] is LocalPlayer) &&
                        _listUnitsAndPlayers[intIndex].ToPlayer().IsTargetingMeOrPet && 
                        (DateTime.Now - _listTargetingTime[intIndex]) >= tsPresentSinceAtLeast &&
                        (!ignorePartyRaid || !_listUnitsAndPlayers[intIndex].ToPlayer().IsInMyPartyOrRaid))
                    woWObj.Add(_listUnitsAndPlayers[intIndex]);
            }
            //Retourne les objets
            return woWObj.ToArray();
        }

        /// <summary>
        /// Récupère tous les joueurs aux alentours du bot.
        /// </summary>
        /// <returns>Retourne un tableau de WoWObject.</returns>
        public WoWObject[] GetAllPlayers()
        {
            return GetAllUnitsAndPlayers().Where(obj => obj is WoWPlayer).ToArray();
        }

        /// <summary>
        /// Récupère tous les joueurs aux alentours du bot qui sont là depuis un certain temps.
        /// </summary>
        /// <param name="tsPresentSinceAtLeast">Temps de présence minimum dans le cache.</param>
        /// <param name="ignorePartyRaid">Indique si les joueurs appartenant au groupe ou raid doivent être ignorés.</param>
        /// <returns>Retourne un tableau de WoWObject.</returns>
        public WoWObject[] GetAllPlayers(TimeSpan tsPresentSinceAtLeast, bool ignorePartyRaid = false)
        {
            //Crée une liste qui contiendra les objets en question
            var woWObj = new List<WoWObject>();
            //Pour chaque objets
            for (int intIndex = 0; intIndex < _listUnitsAndPlayers.Count; intIndex++)
            {
                //Vérifie que le cet objet soit bien un joueur et qu'il est été ajouté depuis plus du temps spécifié
                if (_listUnitsAndPlayers[intIndex] is WoWPlayer && !(_listUnitsAndPlayers[intIndex] is LocalPlayer) && 
                        (DateTime.Now - _listAddingTime[intIndex]) >= tsPresentSinceAtLeast &&
                        (!ignorePartyRaid || !_listUnitsAndPlayers[intIndex].ToPlayer().IsInMyPartyOrRaid))
                    woWObj.Add(_listUnitsAndPlayers[intIndex]);
            }
            //Retourne les objets
            return woWObj.ToArray();
        }

        /// <summary>
        /// Récupère tous les joueurs non alliés aux alentours du bot.
        /// </summary>
        /// <returns>Retourne un tableau de WoWObject.</returns>
        public WoWObject[] GetAllNonFriendlyPlayers()
        {
            return GetAllPlayers().Where(obj => !obj.ToPlayer().IsFriendly).ToArray();
        }

        public WoWObject[] GetAllNonFriendlyPlayers(TimeSpan tsPresentSinceAtLeast)
        {
            return GetAllPlayers(tsPresentSinceAtLeast).Where(obj => !obj.ToPlayer().IsFriendly).ToArray();
        }
        #endregion

        /// <summary>
        /// Vide le cache entier contenant les unités et joueurs.
        /// </summary>
        public void Clear()
        {
            _listUnitsAndPlayers.Clear();
            _listAddingTime.Clear();
            _listTargetingTime.Clear();
        }

        /// <summary>
        /// Affiche tous les unités et joueurs avec leur heure d'apparition dans la console.
        /// </summary>
        public void ShowAll()
        {
            //Header
            Logging.WriteVerbose(Colors.Orange, "=======Units & Players==" + DateTime.Now.ToLongTimeString() + "========");
            //Affiche les objets
            for (int intIndex = 0; intIndex < _listUnitsAndPlayers.Count; intIndex++)
            {
                Logging.WriteVerbose(Colors.Orange, _listUnitsAndPlayers[intIndex].Name + " (" + 
                              _listUnitsAndPlayers[intIndex].GetType().ToString().Substring(_listUnitsAndPlayers[intIndex].GetType().ToString().LastIndexOf(".")) + 
                              ") " + _listAddingTime[intIndex].ToLongTimeString());
            }
            //Footer
            Logging.WriteVerbose(Colors.Orange, "================================");
        }

        /// <summary>
        /// Ajoute les nouveaux joueurs et unités au cache.
        /// </summary>
        private void Dump()
        {
            //Rafraichit l'ObjetManager de HonorBuddy
            ObjectManager.Update();
            //Récupère les nouveaux joueurs et unités
            foreach (WoWObject WoWObj in ObjectManager.ObjectList.Where(obj => ((obj is WoWUnit && !_listUnitsAndPlayers.Contains(obj.ToUnit())) || (obj is WoWPlayer && !_listUnitsAndPlayers.Contains(obj.ToPlayer())))))
            {
                //Ajoute l'unité ou le joueur à la liste
                _listUnitsAndPlayers.Add(WoWObj);
                //Ajoute son heure d'ajout
                _listAddingTime.Add(DateTime.Now);
                //Ajoute l'heure de ciblage
                _listTargetingTime.Add(DateTime.Now);
            }
        }

        /// <summary>
        /// Rafraichit l'heure de ciblage des unités et joueurs dans le cache.
        /// </summary>
        private void Refresh()
        {
            //Boucle sur chaque unité/joueur
            for (int intIndex = 0; intIndex < _listUnitsAndPlayers.Count; intIndex++)
            {
                //Si l'unité ou le joueur ne cible pas le bot ou si le bot est sur un transport, on rafraichit l'heure
                if (
                    (_listUnitsAndPlayers[intIndex] is WoWUnit && !_listUnitsAndPlayers[intIndex].ToUnit().IsTargetingMeOrPet) ||
                    (_listUnitsAndPlayers[intIndex] is WoWPlayer && !_listUnitsAndPlayers[intIndex].ToPlayer().IsTargetingMeOrPet) ||
                    StyxWoW.Me.IsOnTransport)
                {
                    _listTargetingTime[intIndex] = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Nettoie la liste des unités/joueurs
        /// </summary>
        private void Clean()
        {
            for (int intIndex = _listUnitsAndPlayers.Count - 1; intIndex >= 0 ; intIndex--)
            {
                //Si l'unité ou le joueur n'est plus valide
                if (!_listUnitsAndPlayers[intIndex].IsValid)
                {
                    //Supprime l'occurrence de la liste des unités/joueurs
                    _listUnitsAndPlayers.RemoveAt(intIndex);
                    //Supprime l'occurrence de temps
                    _listAddingTime.RemoveAt(intIndex);
                    //Supprime l'heure de ciblage
                    _listTargetingTime.RemoveAt(intIndex);
                }
            }
        }
    }
}
