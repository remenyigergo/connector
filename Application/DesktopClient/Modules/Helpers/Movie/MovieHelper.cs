using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DesktopClient.Modules.MPCManager.Model;
using Standard.Contracts;
using Standard.Contracts.Requests;
using Standard.Contracts.Requests.Movie;
using Standard.Core.NetworkManager;

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
                return regexResult[0].Groups[1].Value.Replace('.', ' ');

            return null;
        }


        public static string TrimDownloadFolders(string folderPath)
        {
            var trimmed = folderPath.Split('\\');

            return trimmed[trimmed.Length - 1];
        }

        public static async Task SavePosition(string showName, int actualSeenSecondsFromMPC, Times elapsedTimesInMedia)
        {
            //string positionPath = "(/html/body/p)[9]";
            //string durationPath = "(/html/body/p)[11]";

            //using (WebClient client = new WebClient())
            //{
            //    string htmlString = client.DownloadString(mpcVariablesSiteUrl);
            //    HtmlDocument htmlDocument = new HtmlDocument();
            //    htmlDocument.LoadHtml(htmlString);

            //    HtmlNode position = htmlDocument.DocumentNode.SelectSingleNode(positionPath);
            //    HtmlNode duration = htmlDocument.DocumentNode.SelectSingleNode(durationPath);

            //    var pos = Regex.Split(position.InnerText, ":");
            //    var dur = Regex.Split(duration.InnerText, ":");

            //double seenSeconds = Int32.Parse(pos[0]) * 60 * 60 + Int32.Parse(pos[1]) * 60 + Int32.Parse(pos[2]);
            //double totalSeconds = Int32.Parse(dur[0]) * 60 * 60 + Int32.Parse(dur[1]) * 60 + Int32.Parse(dur[2]);
            //double percentage = (100 / totalSeconds) * seenSeconds;

            var seenSeconds = 0.0;
            if (elapsedTimesInMedia != null)
            {
                seenSeconds = elapsedTimesInMedia.Position;
            }
            else
            {
                if (Math.Abs(actualSeenSecondsFromMPC - elapsedTimesInMedia.SeenSeconds) < 60)
                    seenSeconds = actualSeenSecondsFromMPC;
            }

            var percentage = 100.0 / elapsedTimesInMedia.Duration * seenSeconds;

            InternalStartedMovieUpdateRequest startedMovie = new InternalStartedMovieUpdateRequest
            {
                Date = DateTime.Now,
                HoursElapsed = elapsedTimesInMedia.SeenHours,
                MinutesElapsed = elapsedTimesInMedia.SeenMinutes,
                SecondsElapsed = elapsedTimesInMedia.SeenSeconds,
                WatchedPercentage = percentage,
                ImdbId = "",
                Title = showName,
                TmdbId = null,
                UserId = 1 //TODO
            };

            //ez megoldja azokat ami alább kivan kommentelve
            var updateResult = await UpdateStartedSeries(startedMovie);

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
            //}
        }

        public static async Task<Result<bool>> UpdateStartedSeries(InternalStartedMovieUpdateRequest internalEpisode)
        {
            var isUpdated =
                await new WebClientManager().Post<Result<bool>>($"http://localhost:5003/movie/update", internalEpisode);
            return isUpdated;
        }

        public static async Task<int> IsTheMovieExist(string title)
        {
            var requestbody = new InternalImportRequest
            {
                Title = title
            };

            var showExist = await new WebClientManager().Exist<int>($"http://localhost:5003/movie/exist", requestbody);
            return showExist;
        }

        public static async Task<bool> IsItSeen(int userid, string title)
        {
            var requestbody = new InternalMovieSeenRequest {UserId = userid, Title = title};
            var showExist =
                await new WebClientManager().SeenMovie<bool>($"http://localhost:5003/movie/seen", requestbody);
            return showExist;
        }

        public static async Task<bool> IsItStarted(int userid, string title)
        {
            var requestbody = new InternalMovieSeenRequest {UserId = userid, Title = title};
            var showExist =
                await new WebClientManager().SeenMovie<bool>($"http://localhost:5003/movie/started", requestbody);
            return showExist;
        }

        public static async Task<int> ImportRequest(string title)
        {
            var requestbody = new InternalImportRequest {Title = title};
            var movieExist =
                await new WebClientManager().Post<bool>($"http://localhost:5003/movie/import", requestbody);
            return movieExist;
        }
    }
}