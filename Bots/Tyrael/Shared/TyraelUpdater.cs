// Thanks to Highvoltz for the autoupdater!

using Styx.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Tyrael.Shared
{
    internal static class TyraelUpdater
    {
        private static readonly string TyraelSvnUrl = TyraelSettings.Instance.SvnUrl == TyraelUtilities.SvnUrl.Release
            ? "https://subversion.assembla.com/svn/team-random/trunk/release/plugins/Tyrael/"
            : "https://subversion.assembla.com/svn/team-random/trunk/dev-pub/plugins/Tyrael/";
        private static readonly Regex LinkPattern = new Regex(@"<li><a href="".+"">(?<ln>.+(?:..))</a></li>", RegexOptions.CultureInvariant);

        internal static void CheckForUpdate()
        {
            CheckForUpdate(Utilities.AssemblyDirectory + "\\Bots\\Tyrael\\", true);
        }

        internal static void CheckForUpdate(string path, bool checkallow)
        {
            try
            {
                TyraelUtilities.PrintLog("\r\n------------------------------------------");
                TyraelUtilities.PrintLog("Tyrael is up to date (Revision {0}).", Tyrael.Revision);
                TyraelUtilities.PrintLog("------------------------------------------");
            }
            catch (Exception ex)
            {
                TyraelUtilities.DiagnosticLog("CheckForUpdate Error: {0}.", ex);
            }
        }

        private static int GetRevision()
        {
            return 564;
        }

        private static void DownloadFilesFromSvn(WebClient client, string url, string path)
        {
            var html = client.DownloadString(url);
            MatchCollection results = LinkPattern.Matches(html);

            IEnumerable<Match> matches = from match in results.OfType<Match>()
                                         where match.Success && match.Groups["ln"].Success
                                         select match;
            foreach (Match match in matches)
            {
                var file = RemoveXmlEscapes(match.Groups["ln"].Value);
                var newUrl = url + file;
                if (newUrl[newUrl.Length - 1] == '/')
                {
                    DownloadFilesFromSvn(client, newUrl, path);
                }
                else
                {
                    string filePath, dirPath;
                    if (url.Length > TyraelSvnUrl.Length)
                    {
                        var relativePath = url.Substring(TyraelSvnUrl.Length);
                        dirPath = Path.Combine(path, relativePath);
                        filePath = Path.Combine(dirPath, file);
                    }
                    else
                    {
                        dirPath = Environment.CurrentDirectory;
                        filePath = Path.Combine(path, file);
                    }

                    TyraelUtilities.DiagnosticLog("Downloading {0}.", file);

                    try
                    {
                        if (!Directory.Exists(dirPath))
                            Directory.CreateDirectory(dirPath);
                        client.DownloadFile(newUrl, filePath);
                        TyraelUtilities.DiagnosticLog("Download {0} done.", file);
                    }
                    catch (Exception ex)
                    {
                        TyraelUtilities.DiagnosticLog("DownloadFilesFromSvn Error: {0}.", ex);
                    }
                }
            }
        }

        private static string RemoveXmlEscapes(string xml)
        {
            return xml.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'");
        }
    }
}