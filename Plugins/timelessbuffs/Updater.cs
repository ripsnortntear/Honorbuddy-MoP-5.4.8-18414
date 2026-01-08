#region

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Styx.Common;

using System.Collections.Generic;
using System.Windows.Forms;

#endregion

namespace TimelessBuffs
{
    internal class Updater
    {
        private const string SvnUrl = "http://timelessbuffs.googlecode.com/svn/trunk/";

        private static readonly Regex LinkPattern = new Regex(@"<li><a href="".+"">(?<ln>.+(?:..))</a></li>",
            RegexOptions.CultureInvariant);

        public static string NewString;

        public static void CheckForUpdate(int currentRevision)
        {
            try
            {
                int oldrevision = currentRevision;
                int onlineRevision = GetOnlineRevision();

                Logging.Write("Checking online for new revision of TimelessBuffs");
                if (oldrevision < onlineRevision)
                {
                    //string changeLog = GetChangeLog(onlineRevision);
                    MessageBox.Show("Revision " + onlineRevision.ToString() + " is available for download, you are currently using rev " + currentRevision.ToString() + ". This will now download in the background, you will be informed when its complete.");

                    Logging.Write("Revision {0} is available for download, you are currently using rev {1}.",
                        onlineRevision, currentRevision);
                    Logging.Write("This will now download in the background, you will be informed when its complete.");

                    DownloadFilesFromSvn(new WebClient(), SvnUrl);

                    Logging.Write(" ");
                    Logging.Write("Download of revision " + onlineRevision +
                                  " is complete. You must close and restart HB for the changes to be applied.");
                    MessageBox.Show("Download of revision " + onlineRevision +
                                  " is complete. You must close and restart HB for the changes to be applied.");

                    UpdateSettingsTimelessBuffs.myPrefs.Revision = onlineRevision;
                    UpdateSettingsTimelessBuffs.myPrefs.Save();

                }
                else
                {
                    Logging.Write("No updates have been found. Revision " + oldrevision + " is the latest build.");
                }
            }
            catch
            {
            }
        }

        private static int GetOnlineRevision()
        {
            var client = new WebClient();
            var html = client.DownloadString(SvnUrl);
            var pattern = new Regex(@" - Revision (?<rev>\d+):", RegexOptions.CultureInvariant);
            var match = pattern.Match(html);
            if (match.Success && match.Groups["rev"].Success) return int.Parse(match.Groups["rev"].Value);
            throw new Exception("Unable to retreive revision! The sky is falling!");
        }

        private static void DownloadFilesFromSvn(WebClient client, string url)
        {
            string basePath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                                           @"Plugins\timelessbuffs\");
            string html = client.DownloadString(url);
            MatchCollection results = LinkPattern.Matches(html);

            IEnumerable<Match> matches = from match in results.OfType<Match>()
                                         where match.Success && match.Groups["ln"].Success
                                         select match;
            foreach (Match match in matches)
            {
                string file = RemoveXmlEscapes(match.Groups["ln"].Value);
                string newUrl = url + file;
                if (newUrl[newUrl.Length - 1] == '/') // it's a directory...
                {
                    DownloadFilesFromSvn(client, newUrl);
                }
                else // its a file.
                {
                    string filePath, dirPath;
                    if (url.Length > SvnUrl.Length)
                    {
                        string relativePath = url.Substring(SvnUrl.Length);
                        dirPath = Path.Combine(basePath, relativePath);
                        filePath = Path.Combine(dirPath, file);
                    }
                    else
                    {
                        dirPath = Environment.CurrentDirectory;
                        filePath = Path.Combine(basePath, file);
                    }
                    Logging.Write("Downloading {0}", filePath);
                    if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                    client.DownloadFile(newUrl, filePath);
                }
            }

        }
        private static string RemoveXmlEscapes(string xml)
        {
            return
                xml.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace(
                    "&apos;", "'");
        }
    }
}