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
using KeyEventForm.Modules.ApplicationManager.MPC;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using Timer = System.Timers.Timer;
using Standard.Contracts.Models.Series;
using Series.Service.Models;
using Standard.Core.NetworkManager;
using DesktopClient.Modules.Helpers.Series;
using DesktopClient.Modules.Helpers.Book;

namespace DesktopClient.Modules.MPCManager
{
    class MPC : IMPCManager
    {
        private static Stopwatch stopWatch = new Stopwatch();
        private static bool mediaJustStarted = false;
        private const string mpcVariablesSiteUrl = @"http://localhost:13579/variables.html";
        private const string mpcPlayerSiteUrL = @"http://localhost:13579/controls.html";
        private const string apiEndpoint = "http://localhost:5001";
        private string tempShowName = string.Empty;

        public async Task MPCManager()
        {
            // TODO get my id
            int userId = 1;

            Thread.Sleep(1000);

            //await Task.Run(async () =>
            // {
            while (true)
            {
                try
                {
                    var runningMedia = IsMediaRunning();

                    var path = SubtitleFetcher.GetFolderPathFromMPCweb();
                    var fileName = SubtitleFetcher.GetFilenameFromMPCweb();

                    var showName = SeriesHelper.GetTitle(fileName);
                    var episodeNumber = SeriesHelper.GetEpisodeNumber(fileName);
                    var seasonNumber = SeriesHelper.GetSeasonNumber(fileName);
                    var releaser = SeriesHelper.GetReleaser(fileName);
                    var quality = SeriesHelper.GetQuality(fileName);

                    if (showName != null)
                    {
                        tempShowName = showName; //leálláskor elvesztettük a nevét, mert újrakértem itt, de már nem volt meg    
                    }


                    Thread.Sleep(1000); //mert gyorsabban olvasta ki a nevét az MPC-nek, mint ahogy elindult volna

                    if (!mediaJustStarted && runningMedia != null && !runningMedia.MainWindowTitle.StartsWith("Media Player Classic"))
                    {

                        var isNewSeries = await SeriesHelper.IsTheShowExist(showName);
                        // TODO enum
                        if (isNewSeries != 1) //nincs a mongoban
                        {
                            await SeriesHelper.ImportRequest(showName);
                        }


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
                    else if (mediaJustStarted && IsMediaRunning() == null)
                    {
                        stopWatch.Stop();  //timer stop
                        var duration = stopWatch.ElapsedMilliseconds / 1000;
                        mediaJustStarted = false;

                        await RecommendBook(userId);
                    }
                    else if (IsMediaRunning() != null && runningMedia != null && !runningMedia.MainWindowTitle.StartsWith("Media Player Classic"))
                    {
                        // TODO elso if részbe tenni (bekapcsoláskor nézni egyből)
                        //Előző rész látott?
                        var previousEpisodes = await SeriesHelper.PreviousEpisodesSeen(new Standard.Contracts.Requests.Series.InternalPreviousEpisodeSeenRequest()
                        {
                            title = showName,
                            episodeNum = episodeNumber,
                            seasonNum = seasonNumber,
                            userid = userId
                        });

                        //Ha vannak nem látott részek/ kihagyott részek.
                        if (previousEpisodes != null)
                        {

                        }

                        //Az adott pozíció elmentése
                        await Task.Run(async () =>
                        {
                            await SavePosition(showName, seasonNumber, episodeNumber);
                        });
                    }

                    Thread.Sleep(1000);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {

                }
                catch (Exception e) { }
                // });
            }

        }

        public async Task RecommendBook(int userId)
        {
            //Könyv ajánlása, amint végignézel egy sorozatot.
            if (await new BookHelper().IsBookModuleActivated(userId))
            {
                var book = await new BookHelper().BookRecommendRequest(tempShowName);

                //check if the user has the book module activated TODO
                if (book != null)
                {
                    MessageBox.Show("Recommended book(s) by this media's theme : \n" + book.Title + "\nPages: " + book.Pages + "\n\nWould You like to add this book to your reading list?", "Recommended book for you.", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                }

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
            }
        }


        public Process IsMediaRunning()
        {
            return new ProcessManager.ProcessManager().FindProcessByName("mpc-hc");
        }

        public async Task<bool> RecognizeMedia(Process playerProcess)
        {
            var name = SeriesHelper.GetTitle(playerProcess.MainWindowTitle);
            var showExistInMongo = await SeriesHelper.Parse(name);

            switch (showExistInMongo)
            {
                case -1:
                    return false; //EKKOR NINCS ILYEN SOROZAT

                case 1:
                    var s = await SeriesHelper.GetShow(name);

                    var imr = new InternalMarkRequest()
                    {
                        ShowName = name,
                        SeasonNumber = SeriesHelper.GetSeasonNumber(playerProcess.MainWindowTitle),
                        EpisodeNumber = SeriesHelper.GetEpisodeNumber(playerProcess.MainWindowTitle)
                    };
                    await SeriesHelper.MarkRequest(imr);
                    break;

                case 2:
                case 3:
                    await SeriesHelper.ImportRequest(name);
                    break;

                case -2:
                    return false; //EKKOR REQUEST HIBA VOLT

                default: return false;
            }
            return false;
        }

    }


}

