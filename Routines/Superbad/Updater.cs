#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Styx.Common;

#endregion

namespace Superbad
{
    internal class Updater
    {
        private const string SvnUrl = "http://superbad.googlecode.com/svn/trunk/";
        private const string ChangeLogUrl = "http://code.google.com/p/superbad/source/detail?r=";

        private static readonly Regex LinkPattern = new Regex(@"<li><a href="".+"">(?<ln>.+(?:..))</a></li>",
            RegexOptions.CultureInvariant);

        public static string ChangeLog;
        public static string NewString;

        private static readonly Regex ChangelogPattern =
            new Regex(
                "<h4 style=\"margin-top:0\">Log message</h4>\r?\n?<pre class=\"wrap\" style=\"margin-left:1em\">(?<log>.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?)</pre>",
                RegexOptions.CultureInvariant);

        public static int CcRevision
        {
            get
            {
                int revision = 0;

                try
                {
                    string path = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                        @"Routines\Superbad\MiscStuff.xml");

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
                    @"Routines\Superbad\MiscStuff.xml");

                var reader = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(reader);

                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("MiscInformation");
                nodeList[0].FirstChild.ChildNodes[0].InnerText = value.ToString(CultureInfo.InvariantCulture);

                var writer = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                xmlDocument.Save(writer);
            }
        }

        public static void DeleteAll()
        {
            Logging.Write("Preparing update process.");
            Logging.Write("Delete old files in Superbad folder.");
            string basePath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                @"Routines\Superbad\");
            var downloadedMessageInfo = new DirectoryInfo(basePath);

            foreach (FileInfo file in downloadedMessageInfo.GetFiles().Where(file => file.Name != "MiscStuff.xml"))
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
            {
                dir.Delete(true);
            }
            Logging.Write("Delete old files in Config folder.");
            basePath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                @"Routines\Config\");
            downloadedMessageInfo = new DirectoryInfo(basePath);

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
            {
                dir.Delete(true);
            }
            Logging.Write("Cleanup completed. You need to reconfigure Superbad!");
        }

        public static void CheckForUpdate()
        {
            try
            {
                int revision = CcRevision;
                int onlineRevision = GetOnlineRevision();

                Logging.Write("Checking online for new revision of Superbad");
                if (revision < onlineRevision)
                {
                    //string changeLog = GetChangeLog(onlineRevision);
                    Logging.Write("Revision {0} is available for download, you are currently using rev {1}.",
                        onlineRevision, revision);
                    Logging.Write("This will now download in the background, you will be informed when its complete.");

                    DownloadFilesFromSvn(new WebClient(), SvnUrl);
                    Logging.Write(" ");
                    Logging.Write("Download of revision " + onlineRevision +
                                  " is complete. You must close and restart HB for the changes to be applied.");
                    Logging.Write(" ");
                    CcRevision = onlineRevision;
                    ChangeLog = GetChangeLog(onlineRevision);
                    NewString = ChangeLog.Replace("//", Environment.NewLine);
                    if (SuperbadSettings.Instance.Changelog)
                        new ChangelogForm().ShowDialog();
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
            string html = client.DownloadString(SvnUrl);
            var pattern = new Regex(@" - Revision (?<rev>\d+):", RegexOptions.CultureInvariant);
            Match match = pattern.Match(html);
            if (match.Success && match.Groups["rev"].Success) return int.Parse(match.Groups["rev"].Value);
            throw new Exception("Unable to retreive revision! The sky is falling!");
        }

        private static void DownloadFilesFromSvn(WebClient client, string url)
        {
            string basePath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                @"Routines\Superbad\");
            string html = client.DownloadString(url);
            MatchCollection results = LinkPattern.Matches(html);
            IEnumerable<Match> matches = from match in results.OfType<Match>()
                where match.Success && match.Groups["ln"].Success
                select match;
            DeleteAll();
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

        private static string GetChangeLog(int revision)
        {
            var client = new WebClient();
            string html = client.DownloadString(ChangeLogUrl + revision);
            Match match = ChangelogPattern.Match(html);
            if (match.Success && match.Groups["log"].Success) return RemoveXmlEscapes(match.Groups["log"].Value);
            return null;
        }
    }
}