using DesktopClient.Modules.Helpers.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DesktopClient.Modules.Helpers
{
    public class SubtitleHelper
    {
        public static Regex regexPattern = new Regex(MovieHelper.movieTitlePattern, RegexOptions.IgnoreCase);
        public static string Quality = "(1080|720)[pi]";
        public static Regex QualityPattern = new Regex(Quality, RegexOptions.IgnoreCase);

        public static string GetQuality(string text)
        {
            var regexResult = QualityPattern.Matches(text);
            if (regexResult.Count != 0)
            {
                return regexResult[0].Groups[0].Value;
            }
            return "";
        }

        public static string GetReleaser(string text)
        {
            var regexResult = regexPattern.Matches(text);
            if (regexResult.Count != 0)
            {
                return regexResult[0].Groups[5].Value;
            }
            return "";

        }
    }
}
