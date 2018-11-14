using System.IO;
using System.Net;
using HtmlAgilityPack;
using KeyEventForm.Modules.Helpers;
using KeyEventForm.Modules.MPCManager;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using KeyEventForm.Modules.SubtitleManager.FeliratokInfo;
using System.Threading.Tasks;
using KeyEventForm.Modules.SubtitleManager.FeliratokInfo.Models;

namespace KeyEventForm.Modules.SubtitleManager
{
    public static class SubtitleFetcher
    {
        private const string downloadPath = "D:\\uTorrent";
        private const string mpcVariablesSiteUrl = @"http://localhost:13579/variables.html";
        private const string feliratokInfoEndpoint = @"https://www.feliratok.info";

        public static string GetFolderPathByMPCtitle()
        {
            foreach (var d in System.IO.Directory.GetDirectories(downloadPath))
            {
                var dirName = new DirectoryInfo(d).Name;
                var dirNameCleaned = Helper.GetTitle(dirName);

                var showName = Helper.GetTitle(new MPC().IsMediaRunning().MainWindowTitle);



                if (dirNameCleaned != null && dirNameCleaned.ToLower() == showName)  //TODO sorozat elnevezések ami nem (Title.SeasonEpisode.Coding-Releaser) alakuak nemjók ugye
                {
                    //var isThereSubtitle = IsThereSubtitles(dirName, dirNameCleaned);
                    return "egyezik";
                }
            }

            return "";
        }

        public static string GetFolderPathFromMPCweb()
        {
            string path = string.Empty;


            using (WebClient client = new WebClient())
            {
                string htmlString = client.DownloadString(mpcVariablesSiteUrl);
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlString);
                string xPath = "(/html/body/p)[5]";
                HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode(xPath);
                path = node.InnerHtml.Replace(@"\\", @"\");
            }

            return path;

        }

        public static bool IsThereSubtitles(string folderPath, string showName, int episodeNum, int seasonNum)
        {
            showName = TrimFileName(showName).ToLower();
            string[] fileArray = Directory.GetFiles(folderPath);
            foreach (var file1 in fileArray)
            {
                var file = TrimFileName(file1).ToLower();
                if (file.EndsWith(".srt"))
                {
                    if (file.Contains(showName) && (file.Contains("S" + seasonNum) || file.Contains("s" + seasonNum) || file.Contains("S0" + seasonNum) || file.Contains("s0" + seasonNum)) &&
                        (file.Contains("E" + episodeNum) || file.Contains("e" + episodeNum) || file.Contains("E0" + episodeNum) || file.Contains("e0" + episodeNum))) //TODO ÉS SZEZONT ÉS EPIZÓDOT
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool DownloadSubtitle(SubtitleModel subtitleModel, string path, string filename)
        {
            return FeliratokInfoDownloader.GetFeliratokInfoHtml(subtitleModel, feliratokInfoEndpoint, path, filename) != false;
        }

        public static string TrimFileName(string path)
        {
            for (int i = path.Length-1; i > 0; i--)
            {
                if (path[i] == '\\')
                {
                    int length = path.Length;
                    var sub = path.Substring(i+1, length - (i+1));
                    return sub;
                }
            }
            return path;
        }

    }
}
