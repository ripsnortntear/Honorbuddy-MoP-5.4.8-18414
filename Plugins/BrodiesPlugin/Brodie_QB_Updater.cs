using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Styx.Common;

namespace BrodieHQB
{
    class Brodie_QB_Updater
    {
        private String trunkURL = "";
        private String pluginDir = "";
        private String QBDir = "";
        private String subQBDir = "";
        private WebClient client;
        private int newestRev = 0;
        private int currentRev = 0;
        static readonly Regex LinkPattern = new Regex(@"<li><a href="".+"">(?<ln>.+(?:..))</a></li>", RegexOptions.CultureInvariant);

        public int CurrentRev
        {
            get
            {
                return (currentRev == 0) ? GetCurrentRev() : currentRev;
            }
        }
        public Brodie_QB_Updater(string svnLocation, string pluginFolderName)
        {
            client = new WebClient();
            trunkURL = svnLocation;
            pluginDir = Application.StartupPath + "\\Plugins\\BrodiesPlugin\\";
            QBDir = Application.StartupPath + "\\Quest Behaviors\\";
            subQBDir = QBDir + pluginFolderName + "";
        }

        public int GetCurrentRev()
        {
            StreamReader verFile = new StreamReader(pluginDir + "Brodie_QB_Updater.ver");
            string verString = verFile.ReadToEnd();
            verFile.Close();

            try
            {
                int currentRev = int.Parse(verString.Replace("$Revision: ", "").Replace(" $", ""));
                return currentRev;
            }
            catch
            {
                return 0;
            }
        }

        public int GetNewestRev()
        {
            string html = client.DownloadString(trunkURL);

            Match revisionMatch = Regex.Match(html, @"Revision ([A-Za-z0-9\-.]+):");

            if (revisionMatch.Success == true)
            {
                newestRev = int.Parse(revisionMatch.Groups[1].Value);
                return newestRev;
            }
            else
            {
                return 0;
            }
        }

        public void WriteNewRevFile(int newRev)
        {
            Logging.Write("Writing new rev file with " + newRev.ToString());
            StreamWriter file = new StreamWriter(@"" + pluginDir + "Brodie_QB_Updater.ver");
            file.Write("$Revision: " + newRev.ToString() + " $");
            file.Close();
        }

        public bool UpdateAvailable()
        {
            return (GetNewestRev() > GetCurrentRev());
        }

        static string RemoveXmlEscapes(string xml)
        {
            return xml.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'");
        }

        public Boolean Update()
        {
            if (newestRev == 0)
            {
                newestRev = GetNewestRev();
            }
            if (this.DownloadUpdate())
            {
                WriteNewRevFile(newestRev);
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean DownloadUpdate()
        {
            return DownloadUpdate(trunkURL);
        }

        public Boolean DownloadUpdate(string remotePath)
        {
            try
            {
                string html = client.DownloadString(remotePath);
                MatchCollection results = LinkPattern.Matches(html);

                IEnumerable<Match> matches = from match in results.OfType<Match>() where match.Success && match.Groups["ln"].Success select match;

                foreach (Match match in matches)
                {
                    string file = RemoveXmlEscapes(match.Groups["ln"].Value);
                    string newUrl = remotePath + file.TrimStart(' ');

                    if (newUrl.Contains("Parent Directory"))
                        continue;

                    if (newUrl[newUrl.Length - 1] == '/')
                    {
                        // Recursive directory download
                        DownloadUpdate(newUrl);
                    }
                    else
                    {
                        string relativePath = subQBDir + remotePath.Replace(trunkURL, "").Replace('/', '\\');

                        String directoryName = Path.GetDirectoryName(relativePath);
                        DirectoryInfo info = new DirectoryInfo(relativePath);

                        if (!Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                        string fileName = newUrl.Replace(remotePath, "");

                        if (fileName != "Brodie_QB_Updater.ver")
                        {
                            client.DownloadFile(newUrl, directoryName + "\\" + fileName);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logging.Write(LogLevel.Diagnostic, "[Brodies QB Updater]: Exception in DownloadUpload " + ex.Message);
                return false;
            }
        }
    }
}

