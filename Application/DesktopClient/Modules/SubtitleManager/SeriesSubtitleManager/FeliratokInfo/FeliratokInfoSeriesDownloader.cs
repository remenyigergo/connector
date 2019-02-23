using DesktopClient.Modules.Helpers.Series;
using DesktopClient.Modules.SeriesSubtitleManager.FeliratokInfo.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DesktopClient.Modules.SubtitleManager.SeriesSubtitleManager.FeliratokInfo
{
    public static class FeliratokInfoSeriesDownloader
    {
        public static string lang = "Magyar";
        public static string subFolderName = "ConSubs";


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
            //S01-02 nélkül keresünk

            if (Directory.Exists(folderPath + "\\" + subFolderName))
            {
                MoveSubtitleUp(folderPath, subFolderName, filename, subtitleModel.SeasonNumber, subtitleModel.EpisodeNumber);
                return true;
            }

            CheckIfSingleSubAlreadyDownloaded(folderPath, filename);

            string magyarXPath = "//div[@class=\'magyar\']";
            string originalXPath = "//div[@class=\'eredeti\']";
            string downloadXPath = "//td[@align=\'center\']/a[@href]";
            HtmlDocument htmlDocument = new HtmlDocument();

            foreach (var subtitleHtml in subtitlesHtmlList)
            {
                htmlDocument.LoadHtml(subtitleHtml.InnerHtml);

                var magyarNode = htmlDocument.DocumentNode.SelectSingleNode(magyarXPath);
                var originalNode = htmlDocument.DocumentNode.SelectSingleNode(originalXPath);

                string EpisodeFromFeliratokInfo = String.Empty;
                string SeasonFromFeliratokInfo = String.Empty;

                //A felsőben csak egy kötőjel, megnézi a SOROZATxEPIZÓD stílust, és a leírásos (Season X) stílust is
                if (magyarNode.InnerText == "-")
                {
                    EpisodeFromFeliratokInfo = SeriesHelper.GetEpisodeFromFeliratokInfo1x2(originalNode.InnerText);
                    SeasonFromFeliratokInfo = SeriesHelper.GetSeasonFromFeliratokInfo1x2(originalNode.InnerText);

                    if (EpisodeFromFeliratokInfo.Length != 0 && SeasonFromFeliratokInfo.Length != 0 && CheckMatching(subtitleModel, SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo, originalNode))
                    {
                        return Download(htmlDocument, downloadXPath, url, folderPath, filename, subtitleModel, true);
                    }
                    else if (SeasonFromFeliratokInfo.Length == 0)
                    {
                        EpisodeFromFeliratokInfo = subtitleModel.EpisodeNumber.ToString();  //beállítom, hogy a check ne dobjon hibát
                        SeasonFromFeliratokInfo =
                            SeriesHelper.GetSeasonFromFeliratokInfoThird(originalNode.InnerText).ToString();
                        if (SeasonFromFeliratokInfo.Length != 0 && CheckMatching(subtitleModel, SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo, originalNode))
                        {
                            return Download(htmlDocument, downloadXPath, url, folderPath, filename, subtitleModel, false);
                        }
                    }
                }

                //Évadpakk
                //Ekkor van sima neve(felsőben), ezért ráapplikálom a (8. évad) stílust a felsőre,
                //az alsóra meg a (Season 8) félét ha nincs a felsőben találat

                SeasonFromFeliratokInfo = SeriesHelper.GetSeasonFromFeliratokInfoEvad(magyarNode.InnerText).ToString();

                if (Int32.Parse(SeasonFromFeliratokInfo) != -1)
                {
                    EpisodeFromFeliratokInfo =
                        subtitleModel.EpisodeNumber.ToString(); //beállítom, hogy a check ne dobjon hibát
                    var seasonOnSite = SeriesHelper.GetSeasonFromFeliratokInfoThird(originalNode.InnerText);
                    if (seasonOnSite == subtitleModel.SeasonNumber && CheckMatching(subtitleModel,
                            SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo, originalNode))
                    {
                        return Download(htmlDocument, downloadXPath, url, folderPath, filename, subtitleModel, false);
                    }
                }
                else
                {
                    SeasonFromFeliratokInfo = SeriesHelper.GetSeasonFromFeliratokInfoThird(originalNode.InnerText).ToString();

                    if (Int32.Parse(SeasonFromFeliratokInfo) != -1)
                    {
                        EpisodeFromFeliratokInfo =
                            subtitleModel.EpisodeNumber.ToString(); //beállítom, hogy a check ne dobjon hibát
                        if (CheckMatching(subtitleModel, SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo, originalNode))
                        {
                            return Download(htmlDocument, downloadXPath, url, folderPath, filename, subtitleModel, false);
                        }
                    }

                }

                //PONTOS EGYEZÉS
                if (EpisodeFromFeliratokInfo.Length == 0 || (SeasonFromFeliratokInfo.Length == 0 || SeasonFromFeliratokInfo.Length == -1))
                {
                    EpisodeFromFeliratokInfo = SeriesHelper.GetEpisodeFromFeliratokInfo1x2(originalNode.InnerText);
                    SeasonFromFeliratokInfo = SeriesHelper.GetSeasonFromFeliratokInfo1x2(originalNode.InnerText);

                    if (SeasonFromFeliratokInfo.Length == 0)
                    {
                        EpisodeFromFeliratokInfo = subtitleModel.EpisodeNumber.ToString();
                        SeasonFromFeliratokInfo = SeriesHelper.GetSeasonFromFeliratokInfoThird(originalNode.InnerText).ToString();
                    }

                    if (CheckMatching(subtitleModel, SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo, originalNode))
                    {
                        return Download(htmlDocument, downloadXPath, url, folderPath, filename, subtitleModel, true);
                    }
                }
            }

            return false;
        }



        public static bool GetFeliratokInfoHtml(SubtitleModel subtitleModel, string url, string folderPath, string filename)
        {
            using (WebClient client = new WebClient())
            {
                bool subtitleFound = false;
                string data = null;
                List<HtmlNode> subtitleList = null;
                int page = 1;


                do
                {
                    var route =
                        $"/?search={subtitleModel.ShowName}&soriSorszam=&nyelv={lang}&sorozatnev=&sid=&complexsearch=true&knyelv=0&evad={subtitleModel.SeasonNumber}&epizod={subtitleModel.EpisodeNumber}&cimke=0&minoseg=0&rlsr=0&tab=all&page={page}";
                    data = client.DownloadString(new Uri(url + route));

                    subtitleList = SubtitleFetcher.GetSubtitles(data);
                    subtitleFound = FeliratokInfoSeriesDownloader.FindAdherentSubtitleAndDownload(subtitleList, subtitleModel, url, folderPath, filename);

                    page++;
                } while (!subtitleFound && subtitleList != null);

                return subtitleFound;
            }
        }

        public static bool IsThereSubtitles(string folderPath, string showName, int episodeNum, int seasonNum)
        {
            showName = SubtitleFetcher.TrimFileName(showName).ToLower();
            var fileArray = Directory.GetFiles(folderPath, "*.srt");
            foreach (var file1 in fileArray)
            {
                var file = SubtitleFetcher.TrimFileName(file1).ToLower();
                var downloadedSubSeason = SeriesHelper.GetSeasonNumber(file);
                var downloadedSubEpisode = SeriesHelper.GetEpisodeNumber(file);
                var downloadedSubName = SeriesHelper.GetTitle(file);
                if (downloadedSubSeason == seasonNum && downloadedSubEpisode == episodeNum && downloadedSubName == showName)
                {
                    return true;
                }
            }

            return false;
        }


        public static bool Download(HtmlDocument htmlDocument, string downloadXPath, string endpoint, string folderPath, string filename, SubtitleModel subtitleModel, bool IsItSrt)
        {
            using (var client = new WebClient())
            {
                var downloadNode = htmlDocument.DocumentNode.SelectSingleNode(downloadXPath).Attributes["href"]
                    .Value;

                switch (IsItSrt)
                {
                    case true:
                        client.DownloadFile(endpoint + downloadNode,
                            $"{folderPath}\\{filename.Remove(filename.Length - 4, 4)}.srt");
                        return true;
                    case false:
                        string trimmedFolderPath = SubtitleFetcher.TrimFileName(folderPath);
                        string source = $"{folderPath}\\{trimmedFolderPath}.rar";

                        client.DownloadFile(endpoint + downloadNode, source);



                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.FileName = "\"C:\\Program Files\\WinRAR\\winrar.exe\"";
                        p.StartInfo.Arguments = String.Format(@"x -s ""{0}"" *.* ""{1}\"" ", source, folderPath + "\\" + FeliratokInfoSeriesDownloader.subFolderName);
                        p.Start();
                        p.WaitForExit();
                        if (File.Exists(source))
                        {
                            File.Delete(source);
                        }

                        FeliratokInfoSeriesDownloader.MoveSubtitleUp(folderPath, FeliratokInfoSeriesDownloader.subFolderName, filename, subtitleModel.SeasonNumber, subtitleModel.EpisodeNumber);

                        return true;
                    default:
                        return false;
                }

            }
        }

        public static void MoveSubtitleUp(string folderPath, string subFolderName, string filename, int season, int episode)
        {
            if (Directory.Exists(folderPath + "\\" + subFolderName))
            {

                var directories = Directory.GetDirectories(folderPath + "\\" + subFolderName);
                DirectoryInfo d;
                FileInfo[] Subtitles;
                if (directories.Length != 0)
                {
                    d = new DirectoryInfo(directories[0]);
                    Subtitles = d.GetFiles("*.srt");

                    foreach (FileInfo subtitle in Subtitles)
                    {
                        int subtitleSeason = SeriesHelper.GetSeasonNumber(subtitle.ToString());
                        int subtitleEpisode = SeriesHelper.GetEpisodeNumber(subtitle.ToString());
                        if (subtitleSeason == season && episode == subtitleEpisode)
                        {
                            var subname = SubtitleFetcher.TrimFileName(filename);
                            File.Move(subtitle.FullName, folderPath + "\\" + filename + ".srt");
                        }
                    }
                }
                else
                {
                    d = new DirectoryInfo(folderPath + "\\" + subFolderName);
                    Subtitles = d.GetFiles("*.srt");

                    foreach (FileInfo subtitle in Subtitles)
                    {
                        int subtitleSeason = SeriesHelper.GetSeasonNumber(subtitle.ToString());
                        int subtitleEpisode = SeriesHelper.GetEpisodeNumber(subtitle.ToString());
                        if (subtitleSeason == season && episode == subtitleEpisode)
                        {
                            var subname = SubtitleFetcher.TrimFileName(filename);
                            File.Move(subtitle.FullName, folderPath + "\\" + filename + ".srt");
                        }
                    }
                }


            }
        }

        public static bool CheckIfSingleSubAlreadyDownloaded(string folderPath, string filename)
        {
            string path = folderPath + "\\" + filename + ".srt";
            return File.Exists(path);
        }

        public static bool CheckMatching(SubtitleModel subtitleModel, string SeasonFromFeliratokInfo, string EpisodeFromFeliratokInfo, HtmlNode originalNode)
        {
            var name = subtitleModel.ShowName.ToLower();
            Regex trimEnd = new Regex($"({name})(.*?)(\\(.*\\))\\s*(-)\\s.*");

            var nameRegexed = trimEnd.Matches(originalNode.InnerText.ToLower());

            var dash = String.Empty;
            var namefromgroup = String.Empty;
            try
            {
                dash = nameRegexed[0].Groups[4].Value;
                namefromgroup = nameRegexed[0].Groups[1].Value;
            }
            catch (ArgumentOutOfRangeException aoore) { }


            if (subtitleModel.SeasonNumber == Int32.Parse(SeasonFromFeliratokInfo) && subtitleModel.EpisodeNumber ==
                Int32.Parse(EpisodeFromFeliratokInfo)
                && originalNode.InnerText.ToLower().Contains(subtitleModel.Releaser.ToLower()) &&
                originalNode.InnerText.ToLower().Contains(subtitleModel.Quality.ToLower()))
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
