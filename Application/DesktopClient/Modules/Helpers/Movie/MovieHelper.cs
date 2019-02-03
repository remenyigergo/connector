using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DesktopClient.Modules.Helpers.Series
{
    public class MovieHelper
    {
        public static string movieTitlePattern = "^((?P<Name>.*[^ (_.])[ (_.]+((?P<Year>\\d{4}\\.)))";

        public static Regex moviePattern = new Regex(movieTitlePattern, RegexOptions.IgnoreCase);

        public static string GetTitle(string text)
        {
            var regexResult = moviePattern.Matches(text);

            if (regexResult.Count > 0)
            {
                return regexResult[0].Groups[0].Value.Replace('.', ' ');
            }

            return null;
        }
    }
}
