using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;
using KeyEventForm.Modules.SubtitleManager.FeliratokInfo.Models;

namespace KeyEventForm.Modules.SubtitleManager.FeliratokInfo
{
    public static class FeliratokInfoDownloader
    {
        public static string lang = "Magyar";

        public static bool GetFeliratokInfoHtml(SubtitleModel subtitleModel, string url, string folderPath, string filename)
        {
            using (WebClient client = new WebClient())
            {
                string route = $"/?search={subtitleModel.ShowName}&soriSorszam=&nyelv={lang}&sorozatnev=&sid=&complexsearch=true&knyelv=0&evad={subtitleModel.SeasonNumber}&epizod{subtitleModel.EpisodeNumber}=2&cimke=0&minoseg=0&rlsr=0&tab=all";
                var data = client.DownloadString(new Uri(url+route));

                var subtitleList = GetSubtitles(data);

                bool subtitleFound =
                    FindAdherentSubtitleAndDownload(subtitleList, subtitleModel, url, folderPath, filename);


                return subtitleFound;
            }
        }

        public static List<HtmlNode> GetSubtitles(string data)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(data);

            //string xPath = "/html/body/div/table/tbody/tr[2]/td[2]/table/tbody";
            string xPath = "//tr[@id=\'vilagit\']";
            string xPath2 = "/html[1]/body[1]/table[1]/tr[2]/td[2]/table[1]/tr";

            List<HtmlNode> listOfSubtitles = new List<HtmlNode>();
            foreach (var tr in htmlDocument.DocumentNode.SelectNodes(xPath2))
            {
                listOfSubtitles.Add(tr);
            }
            return listOfSubtitles;
        }

        public static bool FindAdherentSubtitleAndDownload(List<HtmlNode> subtitlesHtmlList, SubtitleModel subtitleModel, string url, string folderPath, string filename)
        {
            
            return FindForExactMatch(subtitlesHtmlList, subtitleModel, url, folderPath, filename);

        }

        public static bool FindForExactMatch(List<HtmlNode> subtitlesHtmlList, SubtitleModel subtitleModel, string url, string folderPath, string filename)
        {
            //Ebben évadpakk nélkül, és S01-02 nélkül keresünk, pontos egyezésre azaz Megtaláljuk (1x1,1080p-RLSR)
            

            string magyarXPath = "//div[@class=\'magyar\']";
            string originalXPath = "//div[@class=\'eredeti\']";
            string downloadXPath = "//td[@align=\'center\']/a[@href]";
            HtmlDocument htmlDocument = new HtmlDocument();

            foreach (var subtitleHtml in subtitlesHtmlList)
            {
                htmlDocument.LoadHtml(subtitleHtml.InnerHtml);

                var magyarNode = htmlDocument.DocumentNode.SelectSingleNode(magyarXPath);
                var originalNode = htmlDocument.DocumentNode.SelectSingleNode(originalXPath);
                var EpisodeFromFeliratokInfo = Helpers.Helper.GetEpisodeFromFeliratokInfo(magyarNode.InnerText);
                var SeasonFromFeliratokInfo = Helpers.Helper.GetSeasonFromFeliratokInfo(magyarNode.InnerText);

                if (EpisodeFromFeliratokInfo.Length == 0 || SeasonFromFeliratokInfo.Length == 0)
                {
                    EpisodeFromFeliratokInfo = Helpers.Helper.GetEpisodeFromFeliratokInfo(originalNode.InnerText);
                    SeasonFromFeliratokInfo = Helpers.Helper.GetSeasonFromFeliratokInfo(originalNode.InnerText);
                }

                if (subtitleModel.SeasonNumber == Int32.Parse(SeasonFromFeliratokInfo) && subtitleModel.EpisodeNumber == Int32.Parse(EpisodeFromFeliratokInfo)
                    && originalNode.InnerText.Contains(subtitleModel.Releaser) && originalNode.InnerText.Contains(subtitleModel.Quality))
                {

                    //var subtitleDescriptionSplittedByComma = Regex.Split(originalNode.InnerText, ",");

                    using (var client = new WebClient())
                    {


                        var downloadNode = htmlDocument.DocumentNode.SelectSingleNode(downloadXPath).Attributes["href"].Value;
                        client.DownloadFile(url + downloadNode, $"{folderPath}\\{filename.Remove(filename.Length-4,4)}.srt");
                        return true;
                    }
                }

            }

            return false;
        }

        public static string Trim(this string s, string trimmer)
        {

            if (String.IsNullOrEmpty(s) || String.IsNullOrEmpty(trimmer) || !s.EndsWith(trimmer, StringComparison.OrdinalIgnoreCase))
                return s;
            else
                return s.Substring(0, s.Length - trimmer.Length);
        }

    }
}
