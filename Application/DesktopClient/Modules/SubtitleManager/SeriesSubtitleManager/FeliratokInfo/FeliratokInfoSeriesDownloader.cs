using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using DesktopClient.Modules.Helpers.Series;
using DesktopClient.Modules.SeriesSubtitleManager.FeliratokInfo.Models;
using HtmlAgilityPack;

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
                return subtitleFound;
            subtitleFound = FindSeasonPack(subtitlesHtmlList, subtitleModel, url, folderPath, filename);

            return false;
        }

        public static bool FindForExactMatch(List<HtmlNode> subtitlesHtmlList, SubtitleModel subtitleModel, string url,
            string folderPath, string filename)
        {
            //S01-02 nélkül keresünk

            if (Directory.Exists(folderPath + "\\" + subFolderName))
            {
                MoveSubtitleUp(folderPath, subFolderName, filename, subtitleModel.SeasonNumber,
                    subtitleModel.EpisodeNumber);
                return true;
            }

            CheckIfSingleSubAlreadyDownloaded(folderPath, filename);

            var magyarXPath = "//div[@class=\'magyar\']";
            var originalXPath = "//div[@class=\'eredeti\']";
            var downloadXPath = "//td[@align=\'center\']/a[@href]";
            var htmlDocument = new HtmlDocument();

            foreach (var subtitleHtml in subtitlesHtmlList)
            {
                htmlDocument.LoadHtml(subtitleHtml.InnerHtml);

                var magyarNode = htmlDocument.DocumentNode.SelectSingleNode(magyarXPath);
                var originalNode = htmlDocument.DocumentNode.SelectSingleNode(originalXPath);

                var EpisodeFromFeliratokInfo = string.Empty;
                var SeasonFromFeliratokInfo = string.Empty;

                //A felsőben csak egy kötőjel, megnézi a SOROZATxEPIZÓD stílust, és a leírásos (Season X) stílust is
                if (magyarNode.InnerText == "-")
                {
                    EpisodeFromFeliratokInfo = SeriesHelper.GetEpisodeFromFeliratokInfo1x2(originalNode.InnerText);
                    SeasonFromFeliratokInfo = SeriesHelper.GetSeasonFromFeliratokInfo1x2(originalNode.InnerText);

                    if (EpisodeFromFeliratokInfo.Length != 0 && SeasonFromFeliratokInfo.Length != 0 &&
                        CheckMatching(subtitleModel, SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo, originalNode))
                        return Download(htmlDocument, downloadXPath, url, folderPath, filename, subtitleModel, true);
                    if (SeasonFromFeliratokInfo.Length == 0)
                    {
                        EpisodeFromFeliratokInfo =
                            subtitleModel.EpisodeNumber.ToString(); //beállítom, hogy a check ne dobjon hibát
                        SeasonFromFeliratokInfo =
                            SeriesHelper.GetSeasonFromFeliratokInfoThird(originalNode.InnerText).ToString();
                        if (SeasonFromFeliratokInfo.Length != 0 && CheckMatching(subtitleModel, SeasonFromFeliratokInfo,
                                EpisodeFromFeliratokInfo, originalNode))
                            return Download(htmlDocument, downloadXPath, url, folderPath, filename, subtitleModel,
                                false);
                    }
                }

                //Évadpakk
                //Ekkor van sima neve(felsőben), ezért ráapplikálom a (8. évad) stílust a felsőre,
                //az alsóra meg a (Season 8) félét ha nincs a felsőben találat

                SeasonFromFeliratokInfo = SeriesHelper.GetSeasonFromFeliratokInfoEvad(magyarNode.InnerText).ToString();

                if (int.Parse(SeasonFromFeliratokInfo) != -1)
                {
                    EpisodeFromFeliratokInfo =
                        subtitleModel.EpisodeNumber.ToString(); //beállítom, hogy a check ne dobjon hibát
                    var seasonOnSite = SeriesHelper.GetSeasonFromFeliratokInfoThird(originalNode.InnerText);
                    if (seasonOnSite == subtitleModel.SeasonNumber && CheckMatching(subtitleModel,
                            SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo, originalNode))
                        return Download(htmlDocument, downloadXPath, url, folderPath, filename, subtitleModel, false);
                }
                else
                {
                    SeasonFromFeliratokInfo = SeriesHelper.GetSeasonFromFeliratokInfoThird(originalNode.InnerText)
                        .ToString();

                    if (int.Parse(SeasonFromFeliratokInfo) != -1)
                    {
                        EpisodeFromFeliratokInfo =
                            subtitleModel.EpisodeNumber.ToString(); //beállítom, hogy a check ne dobjon hibát
                        if (CheckMatching(subtitleModel, SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo,
                            originalNode))
                            return Download(htmlDocument, downloadXPath, url, folderPath, filename, subtitleModel,
                                false);
                    }
                }

                //PONTOS EGYEZÉS
                if (EpisodeFromFeliratokInfo.Length == 0 || SeasonFromFeliratokInfo.Length == 0 ||
                    SeasonFromFeliratokInfo.Length == -1)
                {
                    EpisodeFromFeliratokInfo = SeriesHelper.GetEpisodeFromFeliratokInfo1x2(originalNode.InnerText);
                    SeasonFromFeliratokInfo = SeriesHelper.GetSeasonFromFeliratokInfo1x2(originalNode.InnerText);

                    if (SeasonFromFeliratokInfo.Length == 0)
                    {
                        EpisodeFromFeliratokInfo = subtitleModel.EpisodeNumber.ToString();
                        SeasonFromFeliratokInfo = SeriesHelper.GetSeasonFromFeliratokInfoThird(originalNode.InnerText)
                            .ToString();
                    }

                    if (CheckMatching(subtitleModel, SeasonFromFeliratokInfo, EpisodeFromFeliratokInfo, originalNode))
                        return Download(htmlDocument, downloadXPath, url, folderPath, filename, subtitleModel, true);
                }
            }

            return false;
        }


        public static bool GetFeliratokInfoHtml(SubtitleModel subtitleModel, string url, string folderPath,
            string filename)
        {
            using (var client = new WebClient())
            {
                var subtitleFound = false;
                string data = null;
                List<HtmlNode> subtitleList = null;
                var page = 1;


                do
                {
                    var route =
                        $"/?search={subtitleModel.ShowName}&soriSorszam=&nyelv={lang}&sorozatnev=&sid=&complexsearch=true&knyelv=0&evad={subtitleModel.SeasonNumber}&epizod={subtitleModel.EpisodeNumber}&cimke=0&minoseg=0&rlsr=0&tab=all&page={page}";
                    data = client.DownloadString(new Uri(url + route));

                    subtitleList = SubtitleFetcher.GetSubtitles(data);
                    subtitleFound =
                        FindAdherentSubtitleAndDownload(subtitleList, subtitleModel, url, folderPath, filename);

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
                if (downloadedSubSeason == seasonNum && downloadedSubEpisode == episodeNum &&
                    downloadedSubName == showName)
                    return true;
            }

            return false;
        }


        public static bool Download(HtmlDocument htmlDocument, string downloadXPath, string endpoint, string folderPath,
            string filename, SubtitleModel subtitleModel, bool IsItSrt)
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
                        var trimmedFolderPath = SubtitleFetcher.TrimFileName(folderPath);
                        var source = $"{folderPath}\\{trimmedFolderPath}.rar";

                        client.DownloadFile(endpoint + downloadNode, source);


                        var p = new Process();
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.FileName = "\"C:\\Program Files\\WinRAR\\winrar.exe\"";
                        p.StartInfo.Arguments = string.Format(@"x -s ""{0}"" *.* ""{1}\"" ", source,
                            folderPath + "\\" + subFolderName);
                        p.Start();
                        p.WaitForExit();
                        if (File.Exists(source))
                            File.Delete(source);

                        MoveSubtitleUp(folderPath, subFolderName, filename, subtitleModel.SeasonNumber,
                            subtitleModel.EpisodeNumber);

                        return true;
                    default:
                        return false;
                }
            }
        }

        public static void MoveSubtitleUp(string folderPath, string subFolderName, string filename, int season,
            int episode)
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

                    foreach (var subtitle in Subtitles)
                    {
                        var subtitleSeason = SeriesHelper.GetSeasonNumber(subtitle.ToString());
                        var subtitleEpisode = SeriesHelper.GetEpisodeNumber(subtitle.ToString());
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

                    foreach (var subtitle in Subtitles)
                    {
                        var subtitleSeason = SeriesHelper.GetSeasonNumber(subtitle.ToString());
                        var subtitleEpisode = SeriesHelper.GetEpisodeNumber(subtitle.ToString());
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
            var path = folderPath + "\\" + filename + ".srt";
            return File.Exists(path);
        }

        public static bool CheckMatching(SubtitleModel subtitleModel, string SeasonFromFeliratokInfo,
            string EpisodeFromFeliratokInfo, HtmlNode originalNode)
        {
            var name = subtitleModel.ShowName.ToLower();
            var trimEnd = new Regex($"({name})(.*?)(\\(.*\\))\\s*(-)\\s.*");

            var nameRegexed = trimEnd.Matches(originalNode.InnerText.ToLower());

            var dash = string.Empty;
            var namefromgroup = string.Empty;
            try
            {
                dash = nameRegexed[0].Groups[4].Value;
                namefromgroup = nameRegexed[0].Groups[1].Value;
            }
            catch (ArgumentOutOfRangeException aoore)
            {
            }


            if (subtitleModel.SeasonNumber == int.Parse(SeasonFromFeliratokInfo) && subtitleModel.EpisodeNumber ==
                int.Parse(EpisodeFromFeliratokInfo)
                && originalNode.InnerText.ToLower().Contains(subtitleModel.Releaser.ToLower()) &&
                originalNode.InnerText.ToLower().Contains(subtitleModel.Quality.ToLower()))
                return true;
            return false;
        }

        public static bool FindSeasonPack(List<HtmlNode> subtitlesHtmlList, SubtitleModel subtitleModel, string url,
            string folderPath, string filename)
        {
            return false;
        }

        public static string Trim(this string s, string trimmer)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(trimmer) ||
                !s.EndsWith(trimmer, StringComparison.OrdinalIgnoreCase))
                return s;
            return s.Substring(0, s.Length - trimmer.Length);
        }
    }
}