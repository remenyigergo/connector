using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Contracts.Requests;
using Core.NetworkManager;
using Series.Parsers.TMDB.Models.TmdbShowModels;
using Series.Parsers.TvMaze.Models;
using Series.Service.Models;
using Series.Service.Response;

namespace DesktopClient.Modules.Helper
{
    public class Helper
    {
        public enum Properties
        {
            H264 = 1,
            X264 = 2,
            TrueHd = 3,
        }

        private List<string> properties = new List<string> { "h264", "x264", "web", "truehd", "7.1", "5.1", "WebRip", "Hun", "Eng", "hdtv", "bluray", "web-dl", "DD5.1" };
        private List<string> RegexProperties = new List<string>() { "[0-9]{3,4}p", "s[0-9]{1,3}e[0-9]{1,3}", "s[0-9]{1,3}", "e[0-9]{1,3}" };

        
        public string DeleteVersionName(string title)
        {
            for (int i = title.Length-1; i > title.Length/2; i--)
            {
                if (!char.IsLetter(title[i]) && title[i] != '.')
                {
                    if (title[i] == '-')
                    {
                        return title.Substring(0, i);
                    }
                    return title;
                }
            }
            return title;
        }

        public string[] Cleaning(string title)
        {
            //return Regex.Split(title, properties[0] + "|" + properties[1] + "|" + properties[2] + "|" + RegexProperties[0] + "|" + RegexProperties[1] + "|" + "\\.");
            return Regex.Split(title, "\\." + "|" + " " + properties[0] + "|" + properties[1] + "|" + properties[2] + "|" + RegexProperties[0] + "|" + RegexProperties[1] 
                + "|" + properties[3] + "|" + properties[4] + "|" + properties[5] + "|" + properties[6] + "|" + properties[7] + "|" + properties[8] + "|" + properties[9] 
                + "|" + properties[10] + "|" + properties[11] + "|" + properties[12] + "|" + RegexProperties[2] + "|" + RegexProperties[3], RegexOptions.IgnoreCase);
        }

        public async Task<int> TryNames(string[] title)
        {
            string titleOsszeadott = "";
            var isitvalid = await IsItValidShow("Parks and recreation");
            //foreach (var title1 in title)
            //{
            //    titleOsszeadott += title1 + " ";
            //    var isitvalid = await IsItValidShow(title[0] + " " + title[1] + " " + title[2]);

            //    if (isitvalid != null)
            //    {
            //        return 101;
            //    }
            //}

            return -1;
        }

        public async Task<bool> IsTheShowAddedToMongo()
        {
            
        }

        public async Task<string> IsItValidShow(string title)
        {
            var requestbody = new InternalImportRequest() {Title = title};
            var showExist = await new WebClientManager().PostCheckShowExist<string>($"http://localhost:5001/series/exist", requestbody);
            return showExist;

            //return await new WebClientManager().Post<string>($"http://localhost:5001/series/getshows", new PostRequestModel() { ShowTitle = title});
        }
    }
}
