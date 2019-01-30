using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DesktopClient.Modules.Helpers;
using DesktopClient.Modules.SubtitleManager;
using DesktopClient.Modules.SubtitleManager.FeliratokInfo.Models;
using Standard.Contracts.Requests;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using Timer = System.Timers.Timer;
using Standard.Contracts.Models.Series;
using Series.Service.Models;
using Standard.Core.NetworkManager;

namespace DesktopClient.Modules.MPCManager
{
    class MPC : IMPCManager
    {
        private static Stopwatch stopWatch = new Stopwatch();
        private static bool mediaJustStarted = false;
        private const string mpcVariablesSiteUrl = @"http://localhost:13579/variables.html";
        private const string mpcPlayerSiteUrL = @"http://localhost:13579/controls.html";
        private const string apiEndpoint = "localhost:5001";

        public async Task MPCManager()
        {
            Thread.Sleep(1000);
            try
            {
                await Task.Run(async () =>
                 {
                     while (true)
                     {
                         var runningMedia = IsMediaRunning();

                         var path = SubtitleFetcher.GetFolderPathFromMPCweb();
                         var fileName = SubtitleFetcher.GetFilenameFromMPCweb();

                         var showName = Helper.GetTitle(fileName);
                         var episodeNumber = Helper.GetEpisodeNumber(fileName);
                         var seasonNumber = Helper.GetSeasonNumber(fileName);
                         var releaser = Helper.GetReleaser(fileName);
                         var quality = Helper.GetQuality(fileName);

                         Thread.Sleep(1000); //mert gyorsabban olvasta ki a nevét az MPC-nek, mint ahogy elindult volna

                         if (!mediaJustStarted && runningMedia != null && !runningMedia.MainWindowTitle.StartsWith("Media Player Classic"))
                         {
                             if (!SubtitleFetcher.IsThereSubtitles(path, showName, episodeNumber, seasonNumber))
                             {
                                 var feliratModel = new SubtitleModel()
                                 {
                                     ShowName = SubtitleFetcher.TrimFileName(showName),
                                     SeasonNumber = seasonNumber,
                                     EpisodeNumber = episodeNumber,
                                     Releaser = releaser,
                                     Quality = quality
                                 };

                                 if (SubtitleFetcher.DownloadSubtitle(feliratModel, path, fileName))
                                 {
                                     runningMedia.Kill();
                                     System.Diagnostics.Process.Start(path + "\\" + fileName);
                                     Thread.Sleep(500);
                                 }
                             }

                             stopWatch.Start(); //timer indul
                             mediaJustStarted = true;
                         }
                         else if (mediaJustStarted && new MPC().IsMediaRunning() == null)
                         {
                             stopWatch.Start();  //timer stop
                             var duration = stopWatch.ElapsedMilliseconds / 1000;
                             mediaJustStarted = false;
                         }
                         else if (IsMediaRunning() != null && !runningMedia.MainWindowTitle.StartsWith("Media Player Classic"))
                         {
                             await Task.Run(async () =>
                             {
                                 await SavePosition(showName, seasonNumber, episodeNumber);
                             });
                         }

                         Thread.Sleep(1000);
                     }
                 });
            }
            catch (Exception ex)
            {

            }
        }

        public async Task SavePosition(string showName, int seasonNum, int episodeNum)
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

                InternalEpisodeStartedModel episode = new InternalEpisodeStartedModel()
                {
                    Date = DateTime.Now,
                    EpisodeNumber = episodeNum,
                    SeasonNumber = seasonNum,
                    HoursElapsed = Int32.Parse(pos[0]),
                    MinutesElapsed = Int32.Parse(pos[1]),
                    SecondsElapsed = Int32.Parse(pos[2]),
                    WatchedPercentage = percentage
                    
                    //még 3 mező nincs feltöltve: userid, tmdbid, tvmazeid 
                };

                if (percentage <= 98)
                {
                    await Helper.UpdateStartedSeries(episode, showName);
                }
                else
                {
                    await Helper.MarkRequest(new InternalMarkRequest()
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
            }
        }


        public Process IsMediaRunning()
        {
            return new ProcessManager.ProcessManager().FindProcessByName("mpc-hc");
        }

        public async Task<bool> RecognizeMedia(Process playerProcess)
        {
            var name = Helper.GetTitle(playerProcess.MainWindowTitle);
            var showExistInMongo = await Helper.Parse(name);

            switch (showExistInMongo)
            {
                case -1:
                    return false; //EKKOR NINCS ILYEN SOROZAT

                case 1:
                    var s = await Helper.GetShow(name);

                    var imr = new InternalMarkRequest()
                    {
                        ShowName = name,
                        SeasonNumber = Helper.GetSeasonNumber(playerProcess.MainWindowTitle),
                        EpisodeNumber = Helper.GetEpisodeNumber(playerProcess.MainWindowTitle)
                    };
                    await Helper.MarkRequest(imr);
                    break;

                case 2:
                case 3:
                    await Helper.ImportRequest(name);
                    break;

                case -2:
                    return false; //EKKOR REQUEST HIBA VOLT

                default: return false;
            }
            return false;
        }

    }


}

