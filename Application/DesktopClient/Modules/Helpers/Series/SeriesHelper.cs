using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DesktopClient.Modules.MPCManager.Model;
using HtmlAgilityPack;
using Series.Service.Models;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Requests;
using Standard.Contracts.Requests.Series;
using Standard.Core.NetworkManager;

namespace DesktopClient.Modules.Helpers.Series
{
    public static class SeriesHelper
    {
        //public static string Pattern = @"^((.+?)[. _-]+)?s(\d+)[. _-]*e(\d+)(([. _-]*e|-)((?!(1080|720)[pi])\d+))*[. _-]*((.+?)((?<![. _-])-([^-]+))?)?$";
        public static string Pattern = @"^((.+?)[. _-]+)?s(\d+)[. _-]*e(\d+)(([. _-]*e|-)((?!(1080|720)[pi])\d+))*[. _-]*((.+?)((?<![. _-])-([^-]+))?)\.(mkv|avi|mp4|srt)?$";
        public static string Quality = "(1080|720)[pi]";
        public static string FeliratokInfoSeasonXEpisode = "-(\\s?)(([0-9]{1,2})x([0-9]{1,2}))";
        public static string S01E01Pattern = "s(\\d+)[. _-]*e(\\d+)";

        public static Regex regexPattern = new Regex(Pattern, RegexOptions.IgnoreCase);
        public static Regex QualityPattern = new Regex(Quality, RegexOptions.IgnoreCase);
        public static Regex SeasonXEpisodePattern = new Regex(FeliratokInfoSeasonXEpisode, RegexOptions.IgnoreCase);
        public static Regex S01E01PatternRegex = new Regex(S01E01Pattern, RegexOptions.IgnoreCase);

        public static async Task<int> Parse(string title)
        {
            var isItExist = await IsTheShowExist(title);
            return isItExist;
        }

        public static string GetTitle(string text)
        {
            var regexResult = regexPattern.Matches(text);

            if (regexResult.Count > 0)
            {
                return regexResult[0].Groups[2].Value.Replace('.', ' ');
            }

            return null;
        }

        public static bool DoesItContainHun(string text)
        {
            var regexResult = new Regex(@"[._-](hun|HUN)[._-]").Matches(text);
            if (regexResult.Count != 0)
            {
                return true;
            }
            return false;
        }

        public static string GetEpisodeFromFeliratokInfo1x2(string text)
        {
            var regexResult = SeasonXEpisodePattern.Matches(text);
            if (regexResult.Count != 0)
            {
                return regexResult[0].Groups[4].Value;
            }
            return "";
        }

        public static string GetSeasonFromFeliratokInfo1x2(string text)
        {
            var regexResult = SeasonXEpisodePattern.Matches(text);
            if (regexResult.Count != 0)
            {
                return regexResult[0].Groups[3].Value;
            }
            return "";
        }

        public static int GetSeasonFromFeliratokInfoEvad(string text)
        {
            MatchCollection regexResult = null;
            regexResult = new Regex(@"\(([0-9]{1,2})(\. Ă©vad\))").Matches(text);
            if (regexResult.Count != 0)
            {
                return Int32.Parse(regexResult[0].Groups[1].Value);
            }
            return -1;
        }

        public static int GetSeasonFromFeliratokInfoThird(string text)
        {
            MatchCollection regexResult = null;
            regexResult = new Regex(@"\((Season )([0-9]{1,2})\)").Matches(text);
            if (regexResult.Count != 0)
            {
                return Int32.Parse(regexResult[0].Groups[2].Value);
            }
            return -1;
        }



        public static int GetSeasonNumber(string text)
        {
            var regexResult = regexPattern.Matches(text);
            if (regexResult.Count != 0)
            {
                return Int32.Parse(regexResult[0].Groups[3].Value);
            }
            return -1;

        }
        public static int GetEpisodeNumber(string text)
        {
            var regexResult = regexPattern.Matches(text);
            if (regexResult.Count != 0)
            {
                return Int32.Parse(regexResult[0].Groups[4].Value);
            }
            return -1;
        }

        public static bool DoesItContainSeasonAndEpisode(string title)
        {
            var result = SeasonXEpisodePattern.Matches(title);

            if (result.Count != 0)
            {
                return true;
            }

            return false;
        }

        public static bool DoesItContainSeasonAndEpisodeS01E01(string title)
        {
            var result = S01E01PatternRegex.Matches(title);

            if (result.Count != 0)
            {
                return true;
            }

            return false;
        }

        public static async Task<InternalSeries> GetShow(string title)
        {
            var requestbody = new InternalImportRequest() { Title = title };
            var showExist = await new WebClientManager().GetShowPost<string>($"http://localhost:5001/series/getseries", requestbody);
            return showExist;
        }

        public static async Task<int> IsTheShowExist(string title)
        {
            var requestbody = new InternalImportRequest() { Title = title };
            var showExist = await new WebClientManager().Post<int>($"http://localhost:5001/series/exist", requestbody);
            return showExist;
        }

        public static async Task<int> ImportRequest(string title)
        {
            var requestbody = new InternalImportRequest() { Title = title };
            var showExist = await new WebClientManager().Post<bool>($"http://localhost:5001/series/import", requestbody);
            return showExist;
        }


        public static async Task<bool> UpdateStartedSeries(InternalEpisodeStartedModel internalEpisode, string title)
        {

            var isUpdated = await new WebClientManager().Post<bool>($"http://localhost:5001/series/updateStartedEpisode/{title}", internalEpisode);
            return isUpdated;
        }
        public static async Task<bool> MarkRequest(InternalMarkRequest imr)
        {
            var marked = await new WebClientManager().PostMarkAsSeen<bool>($"http://localhost:5001/series/mark", imr);
            return marked;
        }

        public static async Task<List<InternalEpisode>> PreviousEpisodesSeen(InternalPreviousEpisodeSeenRequest model)
        {
            var marked = await new WebClientManager().PreviousEpisodesSeen<List<InternalEpisode>>($"http://localhost:5001/series/check/seen/previous", model);

            return marked;
        }

        public static async Task SavePosition(string showName, int seasonNum, int episodeNum, int actualSeenSecondsFromMPC, Times elapsedTimesInMedia)
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
            double seenSeconds = 0.0;
            if (elapsedTimesInMedia != null)
            {
                seenSeconds = elapsedTimesInMedia.Position;
            }
            else
            {
                if (Math.Abs(actualSeenSecondsFromMPC - elapsedTimesInMedia.SeenSeconds) < 60)
                {
                    seenSeconds = actualSeenSecondsFromMPC;
                }
            }


            //double totalSeconds = Int32.Parse(dur[0]) * 60 * 60 + Int32.Parse(dur[1]) * 60 + Int32.Parse(dur[2]);
            //double percentage = (100 / totalSeconds) * seenSeconds;
            
            double percentage = (100.0 / (double)elapsedTimesInMedia.Duration) * seenSeconds;

            InternalEpisodeStartedModel episode = new InternalEpisodeStartedModel()
            {
                Date = DateTime.Now,
                EpisodeNumber = episodeNum,
                SeasonNumber = seasonNum,
                HoursElapsed = elapsedTimesInMedia.SeenHours,
                MinutesElapsed = elapsedTimesInMedia.SeenMinutes,
                SecondsElapsed = Convert.ToInt32(seenSeconds),
                WatchedPercentage = percentage

                //még 3 mező nincs feltöltve: userid, tmdbid, tvmazeid 
            };

            if (percentage <= 98)
            {
                await SeriesHelper.UpdateStartedSeries(episode, showName);
            }
            else
            {
                await SeriesHelper.MarkRequest(new InternalMarkRequest()
                {
                    //IDE HA LESZ FELHASZNÁLÓ KELL A USERID
                    //átküldés után lekell kérni a showt névszerint
                    UserId = 1,
                    TvMazeId = "",
                    TmdbId = "",
                    ShowName = showName,
                    EpisodeNumber = episodeNum,
                    SeasonNumber = seasonNum
                });
            }
            //}
        }




    }
}
