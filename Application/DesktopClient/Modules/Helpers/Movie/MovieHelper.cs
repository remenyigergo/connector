using Standard.Contracts.Requests;
using Standard.Core.NetworkManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DesktopClient.Modules.Helpers.Series;
using HtmlAgilityPack;
using Series.Service.Models;
using Standard.Contracts.Requests.Movie;

namespace DesktopClient.Modules.Helpers.Movie
{
    public class MovieHelper
    {
        //([ .\w']+?)(\W\d{4}\W?.*) filmnév
        //^([ .\w']+?)(\W\d{4}\W?.*)()-(?<release_group>[^-]+)$  az előző a releaserrel kiegészítve
        // ^(?<showName>[ .\w']+?)(?<aSallang_a_releasig>\W(?<year>\d{4})\W(\d{1}D)?.*)(?<release_language>Hun|hun|HuN)??-(?<release_group>[^-]+)$
        //^([ .\w']+?)(\W(?<year>\d{4})\W(\d{1}D)?.*)(Hun|hun|HuN)??-([^-]+)$ leegyszerűsítve az előző

        //public static string movieTitlePattern = "^((?P<Name>.*[^ (_.])[ (_.]+((?P<Year>\\d{4}\\.)))";
        public static string movieTitlePattern = @"([ .\w']+?)(\W(?<year>\d{4})\W(\d{1}D)?.*)(Hun|hun|HuN)??-([^-]+)";
        

        public static Regex moviePattern = new Regex(movieTitlePattern, RegexOptions.IgnoreCase);

        public static string GetTitle(string text)
        {
            var regexResult = moviePattern.Matches(text);

            if (regexResult.Count > 0)
            {
                return regexResult[0].Groups[1].Value.Replace('.', ' ');
            }

            return null;
        }



        public static string TrimDownloadFolders(string folderPath)
        {
            var trimmed = folderPath.Split('\\');

            return trimmed[trimmed.Length-1];
        }

        public static async Task SavePosition(string showName, string mpcVariablesSiteUrl)
        {
            string positionPath = "(/html/body/p)[9]";
            string durationPath = "(/html/body/p)[11]";

            using (WebClient client = new WebClient())
            {
                string htmlString = client.DownloadString(mpcVariablesSiteUrl);
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlString);

                HtmlNode position = htmlDocument.DocumentNode.SelectSingleNode(positionPath);
                HtmlNode duration = htmlDocument.DocumentNode.SelectSingleNode(durationPath);

                var pos = Regex.Split(position.InnerText, ":");
                var dur = Regex.Split(duration.InnerText, ":");

                double seenSeconds = Int32.Parse(pos[0]) * 60 * 60 + Int32.Parse(pos[1]) * 60 + Int32.Parse(pos[2]);
                double totalSeconds = Int32.Parse(dur[0]) * 60 * 60 + Int32.Parse(dur[1]) * 60 + Int32.Parse(dur[2]);
                double percentage = (100 / totalSeconds) * seenSeconds;

                InternalStartedMovieUpdateRequest startedMovie = new InternalStartedMovieUpdateRequest()
                {
                    Date = DateTime.Now,
                    HoursElapsed = Int32.Parse(pos[0]),
                    MinutesElapsed = Int32.Parse(pos[1]),
                    SecondsElapsed = Int32.Parse(pos[2]),
                    WatchedPercentage = percentage,
                    ImdbId = "",
                    Title = showName,
                    TmdbId = null,
                    UserId = 1  //TODO
                };

                //ez megoldja azokat ami alább kivan kommentelve
                await UpdateStartedSeries(startedMovie);

                //if (percentage <= 98)
                //{
                //    await UpdateStartedSeries(startedMovie);
                //}
                //else
                //{
                //    await SeriesHelper.MarkRequest(new InternalMarkRequest()
                //    {
                //        //IDE HA LESZ FELHASZNÁLÓ KELL A USERID
                //        //átküldés után lekell kérni a showt névszerint
                //        UserId = 1,
                //        TvMazeId = "",
                //        TmdbId = "",
                //        ShowName = showName,
                //        EpisodeNumber = episodeNum,
                //        SeasonNumber = seasonNum
                //    });
                //}
            }
        }

        public static async Task<bool> UpdateStartedSeries(InternalStartedMovieUpdateRequest internalEpisode)
        {

            var isUpdated = await new WebClientManager().Post<bool>($"http://localhost:5003/movie/update", internalEpisode);
            return isUpdated;
        }

        public static async Task<int> IsTheMovieExist(string title)
        {
            var requestbody = new InternalImportRequest() { Title = title };
            var showExist = await new WebClientManager().Exist<int>($"http://localhost:5003/movie/exist", requestbody);
            return showExist;
        }
    }
}
