﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Standard.Contracts.Requests;
using Standard.Core.NetworkManager;

namespace KeyEventForm.Modules.Helpers
{
    public static class Helper
    {
        public static string Pattern = @"^((.+?)[. _-]+)?s(\d+)[. _-]*e(\d+)(([. _-]*e|-)((?!(1080|720)[pi])\d+))*[. _-]*((.+?)((?<![. _-])-([^-]+))?)?$";
        public static string Quality = "(1080|720)[pi]";
        public static string FeliratokInfoSeasonXEpisode = "-(\\s?)(([0-9]{1,2})x([0-9]{1,2}))";

        public static Regex regexPattern = new Regex(Pattern, RegexOptions.IgnoreCase);
        public static Regex QualityPattern = new Regex(Quality, RegexOptions.IgnoreCase);
        public static Regex SeasonXEpisodePattern = new Regex(FeliratokInfoSeasonXEpisode, RegexOptions.IgnoreCase);

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


        public static string GetEpisodeFromFeliratokInfo(string text)
        {
            var regexResult = SeasonXEpisodePattern.Matches(text);
            return regexResult[0].Groups[4].Value;
        }
        public static string GetSeasonFromFeliratokInfo(string text)
        {
            var regexResult = SeasonXEpisodePattern.Matches(text);
            return regexResult[0].Groups[3].Value;
        }

        public static string GetQuality(string text)
        {
            var regexResult = QualityPattern.Matches(text);
            return regexResult[0].Groups[0].Value;
        }

        public static string GetReleaser(string text)
        {
            var regexResult = regexPattern.Matches(text);
            return regexResult[0].Groups[11].Value;
        }

        public static int GetSeasonNumber(string text)
        {
            var regexResult = regexPattern.Matches(text);
            return Int32.Parse(regexResult[0].Groups[3].Value);
        }
        public static int GetEpisodeNumber(string text)
        {
            var regexResult = regexPattern.Matches(text);
            return Int32.Parse(regexResult[0].Groups[4].Value);
        }

        public static async Task<int> GetShow(string title)
        {
            var requestbody = new InternalImportRequest() { Title = title };
            var showExist = await new WebClientManager().GetShowPost<string>($"http://localhost:5001/series/getseries", requestbody);
            return showExist.Length;
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

        public static async Task<int> MarkRequest(InternalMarkRequest imr)
        {
            //var requestbody = new InternalMarkRequest() { TvMazeId = title };
            //var showExist = await new WebClientManager().Post<bool>($"http://localhost:5001/series/import", requestbody);
            //return showExist;
            return 0;
        }
    }
}
