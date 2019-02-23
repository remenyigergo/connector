using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DesktopClient.Modules.SubtitleManager.MovieSubtitleManager.FeliratokInfo.Models;
using HtmlAgilityPack;

namespace DesktopClient.Modules.SubtitleManager.MovieSubtitleManager.FeliratokInfo
{
    public class FeliratokInfoMovieDownloader
    {
        //  ?search=Bohemian+Rhapsody&soriSorszam=&nyelv=Magyar&sorozatnev=&sid=&complexsearch=true&knyelv=0&evad=&epizod1=&cimke=0&minoseg=0&rlsr=0&tab=all

        private const string _endpoint = "https://www.feliratok.info";
        private const string _lang = "Magyar";

        public List<FoundSubtitle> FindSubtitle(SubtitleModel subtitleModel, string path, string filename)
        {
            using (WebClient client = new WebClient())
            {
                bool subtitleFound = false;
                string data = null;
                List<HtmlNode> subtitleList = null;
                int page = 1;

                List<FoundSubtitle> bestSubtitlesFromAllPages = new List<FoundSubtitle>();

                do
                {
                    var url = $"{_endpoint}/?search={subtitleModel.Title}&nyelv={_lang}&page={page}";
                    data = client.DownloadString(new Uri(url));

                    subtitleList = SubtitleFetcher.GetSubtitles(data);

                    if (subtitleList != null)
                    {
                        var subtitles = FindTheBestOnes(subtitleList, subtitleModel);
                        foreach (var foundSubtitle in subtitles)
                        {
                            //hozzáadom a listához amiket megtaláltunk. azért van erre szükség mert több oldal is lehet a feliratokinfo feliratok listjáa
                            bestSubtitlesFromAllPages.Add(foundSubtitle);
                        }
                    }


                    page++;
                } while (subtitleList != null);

                return bestSubtitlesFromAllPages;
            }
        }

        public List<FoundSubtitle> FindTheBestOnes(List<HtmlNode> subtitleHtmlList, SubtitleModel subtitleModel)
        {
            if (subtitleModel != null && subtitleHtmlList.Count != 0)
            {
                string magyarXPath = "//div[@class=\'magyar\']";
                string originalXPath = "//div[@class=\'eredeti\']";
                string downloadXPath = "//td[@align=\'center\']/a[@href]";
                HtmlDocument htmlDocument = new HtmlDocument();
                List<FoundSubtitle> foundSubtitles = new List<FoundSubtitle>();

                foreach (var subtitleHtml in subtitleHtmlList)
                {
                    htmlDocument.LoadHtml(subtitleHtml.InnerHtml);

                    var magyarNode = htmlDocument.DocumentNode.SelectSingleNode(magyarXPath);
                    var originalNode = htmlDocument.DocumentNode.SelectSingleNode(originalXPath);

                    if ((magyarNode.InnerText.Contains(subtitleModel.Title) || originalNode.InnerText.Contains(subtitleModel.Title)) &&
                        (magyarNode.InnerText.Contains(subtitleModel.Quality) || originalNode.InnerText.Contains(subtitleModel.Quality)) &&
                        (magyarNode.InnerText.Contains(subtitleModel.Releaser) || originalNode.InnerText.Contains(subtitleModel.Releaser)))
                    {
                        var downloadNode = htmlDocument.DocumentNode.SelectSingleNode(downloadXPath).Attributes["href"]
                            .Value;

                        foundSubtitles.Add(new FoundSubtitle(magyarNode.InnerText, originalNode.InnerText, downloadNode, htmlDocument));
                    }

                }

                return foundSubtitles;
            }

            return null;
        }


        public static bool Download(string downloadNode, string folderPath, string filename)
        {
            using (var client = new WebClient())
            {
                 client.DownloadFile(_endpoint + downloadNode,$"{folderPath}\\{filename.Remove(filename.Length - 4, 4)}.srt");
                return true;
            }
        }
    }
}
