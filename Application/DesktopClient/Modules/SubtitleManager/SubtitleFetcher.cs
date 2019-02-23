using System.IO;
using System.Net;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Threading.Tasks;
using DesktopClient.Modules.Helpers;
using DesktopClient.Modules.MPCManager;
using DesktopClient.Modules.Helpers.Series;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using DesktopClient.Modules.SubtitleManager.SeriesSubtitleManager.FeliratokInfo;
using DesktopClient.Modules.SeriesSubtitleManager.FeliratokInfo.Models;

namespace DesktopClient.Modules.SubtitleManager
{
    public static class SubtitleFetcher
    {
        private static string lang = "Magyar";
        private const string downloadPath = "D:\\uTorrent";
        private const string mpcVariablesSiteUrl = @"http://localhost:13579/variables.html";
        private const string feliratokInfoEndpoint = @"https://www.feliratok.info";

        public static string GetFolderPathByMPCtitle()
        {
            foreach (var d in Directory.GetDirectories(downloadPath))
            {
                var dirName = new DirectoryInfo(d).Name;
                var dirNameCleaned = SeriesHelper.GetTitle(dirName);

                var showName = SeriesHelper.GetTitle(new MPC().IsMediaRunning().MainWindowTitle);



                if (dirNameCleaned != null && dirNameCleaned.ToLower() == showName)  //TODO sorozat elnevezések ami nem (Title.SeasonEpisode.Coding-Releaser) alakuak nemjók ugye
                {
                    //var isThereSubtitle = IsThereSubtitles(dirName, dirNameCleaned);
                    return "egyezik";
                }
            }

            return "";
        }

        public static string GetFolderPathFromMPCweb()
        {
            string path = String.Empty;

            try
            {
                using (WebClient client = new WebClient())
                {
                    string htmlString = client.DownloadString(mpcVariablesSiteUrl);
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlString);
                    string xPath = "(/html/body/p)[5]";
                    HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode(xPath);
                    path = node.InnerHtml.Replace(@"\\", @"\");
                }
            }
            catch (WebException WebEx)
            {
                
            }
            return path;
        }

        public static string GetFilenameFromMPCweb()
        {
            string filename = String.Empty;

            try
            {
                using (WebClient client = new WebClient())
                {
                    string htmlString = client.DownloadString(mpcVariablesSiteUrl);
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlString);
                    string xPath = "(/html/body/p)[1]";
                    HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode(xPath);
                    filename = node.InnerHtml.Replace(@"\\", @"\");
                }
            }
            catch (WebException WebEx)
            {

            }
            return filename;
        }

        //public static bool IsThereSubtitles(string folderPath, string showName, int episodeNum, int seasonNum)
        //{
        //    showName = TrimFileName(showName).ToLower();
        //    string[] fileArray = Directory.GetFiles(folderPath);
        //    foreach (var file1 in fileArray)
        //    {
        //        var file = TrimFileName(file1).ToLower();
        //        if (file.EndsWith(".srt"))
        //        {
        //            if (file.Contains(showName) && (file.Contains("S" + seasonNum) || file.Contains("s" + seasonNum) || file.Contains("S0" + seasonNum) || file.Contains("s0" + seasonNum)) &&
        //                (file.Contains("E" + episodeNum) || file.Contains("e" + episodeNum) || file.Contains("E0" + episodeNum) || file.Contains("e0" + episodeNum))) //TODO ÉS SZEZONT ÉS EPIZÓDOT
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}


        

        public static bool DownloadSubtitle(SubtitleModel subtitleModel, string path, string filename)
        {
            if (!SeriesHelper.DoesItContainHun(filename))
            {
                // TODO async
                return FeliratokInfoSeriesDownloader.GetFeliratokInfoHtml(subtitleModel, feliratokInfoEndpoint, path, filename) != false;
            }
            return false;
        }


        public static string TrimFileName(string path)
        {
            for (int i = path.Length - 1; i > 0; i--)
            {
                if (path[i] == '\\')
                {
                    int length = path.Length;
                    var sub = path.Substring(i + 1, length - (i + 1));
                    return sub;
                }
            }
            return path;
        }

        public static List<HtmlNode> GetSubtitles(string data)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(data);

            //string xPath = "/html/body/div/table/tbody/tr[2]/td[2]/table/tbody";
            string xPath = "//tr[@id=\'vilagit\']";
            string xPath2 = "/html[1]/body[1]/table[1]/tr[2]/td[2]/table[1]/tr";
            string pagination = "/html/body/div[2]/div";

            List<HtmlNode> listOfSubtitles = new List<HtmlNode>();
            try
            {

                foreach (var tr in htmlDocument.DocumentNode.SelectNodes(xPath2))
                {
                    listOfSubtitles.Add(tr);
                }
                return listOfSubtitles;
            }
            catch (NullReferenceException nullException)
            {
                return null;
            }

        }
    }
}
