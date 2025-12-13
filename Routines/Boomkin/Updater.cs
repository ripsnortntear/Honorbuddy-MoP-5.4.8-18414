#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Styx.Common;

#endregion

namespace Boomkin
{
    internal class Updater
    {
        private const string SvnURL = "https://riouxsvn.com/svn/boomkin/trunk/";

        private static readonly Regex LinkPattern = new Regex(@"<li><a href="".+"">(?<ln>.+(?:..))</a></li>",
                                                              RegexOptions.CultureInvariant);

        public static int CCRevision
        {
            get
            {
                int revision = 0;

                try
                {
                    string path = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                                               @"Routines\Boomkin\Revision.xml");

                    var reader = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(reader);
                    XmlNodeList nodeList = xmlDocument.GetElementsByTagName("MiscInformation");
                    revision = Convert.ToInt16(nodeList[0].FirstChild.ChildNodes[0].InnerText);
                }

                catch
                {
                }

                return revision;
            }

            set
            {
                string path = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                                           @"Routines\Boomkin\Revision.xml");

                var reader = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(reader);

                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("MiscInformation");
                nodeList[0].FirstChild.ChildNodes[0].InnerText = value.ToString();

                var writer = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                xmlDocument.Save(writer);
            }
        }

        public static void CheckForUpdate()
        {
            try
            {
                int revision = CCRevision;
                int onlineRevision = GetOnlineRevision();

                Logging.Write("Checking online for new revision of Boomkin");
                if (revision < onlineRevision)
                {
                    //string changeLog = GetChangeLog(onlineRevision);
                    Logging.Write(
                        string.Format("Revision {0} is available for download, you are currently using rev {1}.",
                                      onlineRevision, revision));
                    Logging.Write("This will now download in the background, you will be informed when its complete.");

                    DownloadFilesFromSvn(new WebClient(), SvnURL);
                    Logging.Write(" ");
                    Logging.Write("Download of revision " + onlineRevision +
                                  " is complete. You must close and restart HB for the changes to be applied.");
                    Logging.Write(" ");
                    //Logging.Log(string.Format("=== Change log for revision {0} ===", onlineRevision), Logging.Colour(UpdaterMessageColour));
                    //Logging.Log(changeLog, Logging.Colour(UpdaterMessageColour));
                    //Logging.Log(" ");

                    CCRevision = onlineRevision;
                }
                else
                {
                    Logging.Write("No updates have been found. Revision " + revision + " is the latest build.");
                }
            }
            catch
            {
            }
        }

        private static int GetOnlineRevision()
        {
            var client = new WebClient();
            string html = client.DownloadString(SvnURL);
            var pattern = new Regex(@" - Revision (?<rev>\d+):", RegexOptions.CultureInvariant);
            Match match = pattern.Match(html);
            if (match.Success && match.Groups["rev"].Success) return int.Parse(match.Groups["rev"].Value);
            throw new Exception("Unable to retreive revision! The sky is falling!");
        }

        private static void DownloadFilesFromSvn(WebClient client, string url)
        {
            string basePath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                                           @"Routines\Boomkin\");
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
                    if (url.Length > SvnURL.Length)
                    {
                        string relativePath = url.Substring(SvnURL.Length);
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