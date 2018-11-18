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

        public static bool GetFeliratokInfoHtml(SubtitleModel subtitleModel, string url, string folderPath,
            string filename)
        {
            using (WebClient client = new WebClient())
            {
                string route =
                    $"/?search={subtitleModel.ShowName}&soriSorszam=&nyelv={lang}&sorozatnev=&sid=&complexsearch=true&knyelv=0&evad={subtitleModel.SeasonNumber}&epizod{subtitleModel.EpisodeNumber}=2&cimke=0&minoseg=0&rlsr=0&tab=all";
                var data = client.DownloadString(new Uri(url + route));

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

        public static bool FindAdherentSubtitleAndDownload(List<HtmlNode> subtitlesHtmlList,
            SubtitleModel subtitleModel, string url, string folderPath, string filename)
        {
            var subtitleFound = FindForExactMatch(subtitlesHtmlList, subtitleModel, url, folderPath, filename);

            if (subtitleFound)
            {
                return subtitleFound;
            }
            else
            {
                subtitleFound = FindSeasonPack(subtitlesHtmlList, subtitleModel, url, folderPath, filename);
            }

            return false;
        }

        public static bool FindForExactMatch(List<HtmlNode> subtitlesHtmlList, SubtitleModel subtitleModel, string url,
            string folderPath, string filename)
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

                string EpisodeFromFeliratokInfo = string.Empty;
                string SeasonFromFeliratokInfo = string.Empty;

                //A felsőben csak egy kötőjel, megnézi a SOROZATxEPIZÓD stílust, és a leírásos (Season X) stílust is
                if (magyarNode.InnerText == "-")
                {
                    EpisodeFromFeliratokInfo = Helpers.Helper.GetEpisodeFromFeliratokInfo1x2(originalNode.InnerText);
                    SeasonFromFeliratokInfo = Helpers.Helper.GetSeasonFromFeliratokInfo1x2(originalNode.InnerText);

                    if (CheckMatching(subtitleModel, SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo, originalNode))
                    {
                        return Download(htmlDocument, downloadXPath, url, folderPath, filename);
                    }
                    else if (SeasonFromFeliratokInfo.Length == 0)
                    {
                        SeasonFromFeliratokInfo =
                            Helpers.Helper.GetSeasonFromFeliratokInfoThird(originalNode.InnerText).ToString();
                        if (CheckMatching(subtitleModel, SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo, originalNode))
                        {
                            return Download(htmlDocument, downloadXPath, url, folderPath, filename);
                        }
                    }
                }

                //Évadpakk
                //Ekkor van sima neve(felsőben), ezért ráapplikálom a (8. évad) stílust a felsőre,
                //az alsóra meg a (Season 8) félét ha nincs a felsőben találat
                //ELSŐ KÖRBEN AZ X-est
                SeasonFromFeliratokInfo = Helpers.Helper.GetSeasonFromFeliratokInfoEvad(magyarNode.InnerText).ToString();

                if (SeasonFromFeliratokInfo.Length == 0)
                {
                    EpisodeFromFeliratokInfo = subtitleModel.EpisodeNumber.ToString();  //beállítom, hogy a check ne dobjon hibát
                    var seasonOnSite = Helpers.Helper.GetSeasonFromFeliratokInfoThird(originalNode.InnerText);
                    if (seasonOnSite == subtitleModel.SeasonNumber && CheckMatching(subtitleModel, SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo, originalNode))
                    {
                        return Download(htmlDocument, downloadXPath, url, folderPath, filename);
                    }
                }

                //PONTOS EGYEZÉS
                if (EpisodeFromFeliratokInfo.Length == 0 || SeasonFromFeliratokInfo.Length == 0)
                {
                    EpisodeFromFeliratokInfo = Helpers.Helper.GetEpisodeFromFeliratokInfo1x2(originalNode.InnerText);
                    SeasonFromFeliratokInfo = Helpers.Helper.GetSeasonFromFeliratokInfo1x2(originalNode.InnerText);

                    if (SeasonFromFeliratokInfo.Length == 0)
                    {
                        EpisodeFromFeliratokInfo = subtitleModel.EpisodeNumber.ToString();
                        SeasonFromFeliratokInfo = Helpers.Helper.GetSeasonFromFeliratokInfoThird(originalNode.InnerText).ToString();
                    }

                    if (CheckMatching(subtitleModel, SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo, originalNode))
                    {
                        return Download(htmlDocument, downloadXPath, url, folderPath, filename);
                    }
                }
            }

            return false;
        }

        public static bool Download(HtmlDocument htmlDocument, string downloadXPath, string url, string folderPath, string filename)
        {


            using (var client = new WebClient())
            {
                var downloadNode = htmlDocument.DocumentNode.SelectSingleNode(downloadXPath).Attributes["href"]
                    .Value;
                client.DownloadFile(url + downloadNode,
                    $"{folderPath}\\{filename.Remove(filename.Length - 4, 4)}.srt");
                return true;
            }

        }

        public static bool CheckMatching(SubtitleModel subtitleModel, string SeasonFromFeliratokInfo, string EpisodeFromFeliratokInfo, HtmlNode originalNode)
        {
            if (subtitleModel.SeasonNumber == Int32.Parse(SeasonFromFeliratokInfo) && subtitleModel.EpisodeNumber ==
                Int32.Parse(EpisodeFromFeliratokInfo)
                && originalNode.InnerText.Contains(subtitleModel.Releaser) &&
                originalNode.InnerText.Contains(subtitleModel.Quality))
            {
                return true;
            }
            return false;
        }

        public static bool FindSeasonPack(List<HtmlNode> subtitlesHtmlList, SubtitleModel subtitleModel, string url,
            string folderPath, string filename)
        {


            return false;
        }

        public static string Trim(this string s, string trimmer)
        {
            if (String.IsNullOrEmpty(s) || String.IsNullOrEmpty(trimmer) ||
                !s.EndsWith(trimmer, StringComparison.OrdinalIgnoreCase))
                return s;
            else
                return s.Substring(0, s.Length - trimmer.Length);
        }
    }
}