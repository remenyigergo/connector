using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DesktopClient.Modules.Helpers.Series;
using DesktopClient.Modules.MPCManager;
using DesktopClient.Modules.SeriesSubtitleManager.FeliratokInfo.Models;
using DesktopClient.Modules.SubtitleManager.SeriesSubtitleManager.FeliratokInfo;
using HtmlAgilityPack;

namespace DesktopClient.Modules.SubtitleManager
{
    public static class SubtitleFetcher
    {
        private const string downloadPath = "D:\\uTorrent";
        private const string mpcVariablesSiteUrl = @"http://localhost:13579/variables.html";
        private const string feliratokInfoEndpoint = @"https://www.feliratok.info";
        private static string lang = "Magyar";

        public static string GetFolderPathByMPCtitle()
        {
            foreach (var d in Directory.GetDirectories(downloadPath))
            {
                var dirName = new DirectoryInfo(d).Name;
                var dirNameCleaned = SeriesHelper.GetTitle(dirName);

                var showName = SeriesHelper.GetTitle(new MPC().FindProcessByName().MainWindowTitle);


                if (dirNameCleaned != null && dirNameCleaned.ToLower() == showName
                ) //TODO sorozat elnevezések ami nem (Title.SeasonEpisode.Coding-Releaser) alakuak nemjók ugye
                    return "egyezik";
            }

            return "";
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
                return FeliratokInfoSeriesDownloader.GetFeliratokInfoHtml(subtitleModel, feliratokInfoEndpoint, path,
                    filename);
            return false;
        }


        public static string TrimFileName(string path)
        {
            for (var i = path.Length - 1; i > 0; i--)
                if (path[i] == '\\')
                {
                    var length = path.Length;
                    var sub = path.Substring(i + 1, length - (i + 1));
                    return sub;
                }
            return path;
        }

        public static List<HtmlNode> GetSubtitles(string data)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(data);

            //string xPath = "/html/body/div/table/tbody/tr[2]/td[2]/table/tbody";
            var xPath = "//tr[@id=\'vilagit\']";
            var xPath2 = "/html[1]/body[1]/table[1]/tr[2]/td[2]/table[1]/tr";
            var pagination = "/html/body/div[2]/div";

            var listOfSubtitles = new List<HtmlNode>();
            try
            {
                foreach (var tr in htmlDocument.DocumentNode.SelectNodes(xPath2))
                    listOfSubtitles.Add(tr);
                return listOfSubtitles;
            }
            catch (NullReferenceException nullException)
            {
                return null;
            }
        }
    }
}