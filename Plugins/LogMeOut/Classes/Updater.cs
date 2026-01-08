using System;
using System.Net;
using System.Xml.Linq;
using System.IO;
using Styx.Helpers;

namespace LogMeOut.Classes
{
    /// <summary>
    /// Gère les mise à jour du plugin.
    /// </summary>
    class Updater
    {
        /// <summary>
        /// Url du serveur SubVersion.
        /// </summary>
        static public string StrSvn { get { return "https://zenlulzdev.googlecode.com/svn/trunk/HonorBuddy/Plugins/LogMeOut/"; } }
        static public string StrWebSite { get { return "http://www.thebuddyforum.com/honorbuddy-forum/plugins/uncataloged/70582-plugin-logmeout-world-warcraft-disconnecter.html"; } }
        /// <summary>
        /// Indique l'emplacement du dossier du plugin.
        /// </summary>
        static public string StrPluginPath { get { return "LogMeOut\\"; } }
        /// <summary>
        /// Indique la version du plugin.
        /// </summary>
        static public Version Version { get { return new Version(1, 2, 18); } }
        /// <summary>
        /// Indique si une nouvelle mise à jour est disponible.
        /// </summary>
        static public bool IsAvailable { get { return !string.IsNullOrEmpty(GetLatestVersion()); } }


        /// <summary>
        /// Vérifie si le plugin est dans sa dernière version.
        /// </summary>
        /// <returns>Retourne une chaîne de caractères avec le numéro de version de la dernière version.</returns>
        static public string GetLatestVersion()
        {
            try
            {
                //Détermine la dernière version du plugin
                var xConfig = XDocument.Load(StrSvn + "Config/Update.xml");
                var strLatestVersion = xConfig.Element("update").Element("version").Value;
                //Vérifie si la version actuelle est différente
                return Version.ToString() != strLatestVersion ? strLatestVersion : string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Installe la dernière version du plugin.
        /// </summary>
        /// <remarks>
        /// Ce code a été inspiré de l'addon ProfileChanger.
        /// </remarks>
        /// <returns>Si la mise à jour est déjà installé ou s'est installée correctement, retourne true.</returns>
        static public bool InstallLatestVersion()
        {
            //Vérifie qu'il y ait une mise à jour
            if (IsAvailable)
            {
                //Instancie un browser
                var client = new WebClient();
                //Charge le fichier de mise à jour
                XDocument xConfig = XDocument.Load(StrSvn + "Config/Update.xml");
                //Il faut que le dossier du plugin existe
                try
                {
                    if (Directory.Exists(Path.Combine(GlobalSettings.Instance.PluginsPath, StrPluginPath)))
                    {
                        //Met à jour les fichiers sources
                        foreach (var node in xConfig.Element("update").Element("content").Elements("file"))
                        {
                            //Vérifie si le fichier en question se trouve dans un dossier
                            if (node.Value.Contains("\\"))
                            {
                                //Vérifie si le dossier existe, dans le cas contraire, le crée
                                if (!Directory.Exists(Path.Combine(GlobalSettings.Instance.PluginsPath, StrPluginPath) + node.Value.Substring(0, node.Value.IndexOf("\\"))))
                                    Directory.CreateDirectory(Path.Combine(GlobalSettings.Instance.PluginsPath, StrPluginPath) + node.Value.Substring(0, node.Value.IndexOf("\\")));
                            }
                            //Télécharge le fichier
                            client.DownloadFile(StrSvn + node.Value.Replace('\\', '/'), Path.Combine(GlobalSettings.Instance.PluginsPath, StrPluginPath) + node.Value);
                        }
                    }
                    //Affiche un message de réussite
                    System.Windows.Forms.MessageBox.Show("Update completed ! You need to restart HonorBuddy to let changes take effect.", "LogMeOut! Updater",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    //Affiche un message d'erreur
                    System.Windows.Forms.MessageBox.Show("Error during LogMeOut! is updating :\n" + ex.Message , "LogMeOut! Updater",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                //Affiche un message que le plugin est déjà à jour
                System.Windows.Forms.MessageBox.Show("You already have the latest version of this plugin !", "LogMeOut! Updater", 
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            return true;
        }
    }
}
