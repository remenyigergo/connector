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
using Standard.Contracts.Enum;
using DesktopClient.Modules.SubtitleManager.SeriesSubtitleManager.FeliratokInfo;
using DesktopClient.Modules.SubtitleManager.MovieSubtitleManager.FeliratokInfo;
using DesktopClient.Modules.Helpers.Movie;
using DesktopClient.Modules.SeriesSubtitleManager.FeliratokInfo.Models;
using DesktopClient.Modules.MPCManager.Model;
using Standard.Contracts.Requests.Movie;

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
        private Times elapsedTimeInMedia;
        private string fileName = string.Empty;

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

                    var file = SubtitleFetcher.GetFilenameFromMPCweb();
                    if (file != "")
                    {
                        fileName = file;
                    }

                    var showName = SeriesHelper.GetTitle(fileName);
                    var episodeNumber = SeriesHelper.GetEpisodeNumber(fileName);
                    var seasonNumber = SeriesHelper.GetSeasonNumber(fileName);
                    var releaser = SubtitleHelper.GetReleaser(fileName);
                    var quality = SubtitleHelper.GetQuality(fileName);


                    if (showName != null)
                    {
                        tempShowName =
                            showName; //leálláskor elvesztettük a nevét, mert újrakértem itt, de már nem volt meg    
                    }


                    Thread.Sleep(1000); //mert gyorsabban olvasta ki a nevét az MPC-nek, mint ahogy elindult volna

                    //bool isItASeries = false;
                    //Eldöntjük sorozat vagy film
                    var isItASeries = SeriesHelper.DoesItContainSeasonAndEpisodeS01E01(fileName);



                    if (!mediaJustStarted && runningMedia != null &&
                        !runningMedia.MainWindowTitle.StartsWith("Media Player Classic"))
                    {
                        stopWatch.Start(); //timer indul
                        mediaJustStarted = true;

                        if (isItASeries)
                        {
                            await ManageSeries(showName, path, episodeNumber, seasonNumber, releaser, quality, fileName,
                                runningMedia, userId);
                        }
                        else
                        {
                            await ManageMovie(path, fileName, userId);
                        }

                    }
                    else if (mediaJustStarted && IsMediaRunning() == null)
                    {
                        stopWatch.Stop(); //timer stop
                        var duration = stopWatch.ElapsedMilliseconds / 1000;
                        mediaJustStarted = false;

                        if (isItASeries)
                        {
                            //Az adott pozíció elmentése sorozat esetén
                            await Task.Run(async () =>
                            {
                                await SeriesHelper.SavePosition(showName, seasonNumber, episodeNumber, (int)stopWatch.ElapsedMilliseconds / 1000, elapsedTimeInMedia);
                            });
                        }
                        else
                        {
                            //Az adott pozíció elmentése film esetén
                            await Task.Run(async () =>
                            {
                                var mediaFolderName = MovieHelper.TrimDownloadFolders(path);
                                var movieTitle = MovieHelper.GetTitle(mediaFolderName);
                                await MovieHelper.SavePosition(movieTitle, (int)stopWatch.ElapsedMilliseconds / 1000, elapsedTimeInMedia);
                            });
                        }

                        await RecommendBook(userId);
                    }
                    else if (IsMediaRunning() != null && runningMedia != null &&
                             !runningMedia.MainWindowTitle.StartsWith("Media Player Classic"))
                    {
                        //    if (isItASeries)
                        //    {
                        //        //Az adott pozíció elmentése sorozat esetén
                        //        await Task.Run(async () =>
                        //        {
                        //            await SeriesHelper.SavePosition(showName, seasonNumber, episodeNumber,
                        //                mpcVariablesSiteUrl);
                        //        });
                        //    }
                        //    else
                        //    {
                        //        //Az adott pozíció elmentése film esetén
                        //        await Task.Run(async () =>
                        //        {
                        //            var mediaFolderName = MovieHelper.TrimDownloadFolders(path);
                        //            var movieTitle = MovieHelper.GetTitle(mediaFolderName);
                        //            await MovieHelper.SavePosition(movieTitle, mpcVariablesSiteUrl);
                        //        });
                        //    }

                        var elapsedTime = GetTimes();
                        if (elapsedTime.SeenSeconds != 0)
                        {
                            elapsedTimeInMedia = elapsedTime;
                        }

                    }

                    Thread.Sleep(1000);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                }
                catch (Exception e)
                {
                }
                // });
            }
        }

        public async Task ManageSeries(string showName, string path, int episodeNumber, int seasonNumber,
            string releaser, string quality, string fileName, Process runningMedia, int userId)
        {
            var isNewSeries = await SeriesHelper.IsTheShowExist(showName);

            if (isNewSeries != (int)MediaExistIn.MONGO) //nincs a mongoban
            {
                await SeriesHelper.ImportRequest(showName);
            }

            if (!FeliratokInfoSeriesDownloader.IsThereSubtitles(path, showName, episodeNumber, seasonNumber))
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

            // TODO elso if részbe tenni (bekapcsoláskor nézni egyből)
            //Előző rész látott?
            var previousEpisodes = await SeriesHelper.PreviousEpisodesSeen(
                new Standard.Contracts.Requests.Series.InternalPreviousEpisodeSeenRequest()
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


        }

        public async Task ManageMovie(string folderPath, string fileName, int userid)
        {
            var mediaFolderName = MovieHelper.TrimDownloadFolders(folderPath);
            var movieTitle = MovieHelper.GetTitle(mediaFolderName);
            var SeenMovie = await MovieHelper.IsItSeen(userid, movieTitle);


            if (!SeenMovie)
            {
                var StartedMovie = await MovieHelper.IsItStarted(userid, movieTitle);
                if (!StartedMovie)
                {
                    var isNewMovie = await MovieHelper.IsTheMovieExist(movieTitle);
                    if (isNewMovie != (int) MediaExistIn.MONGO) //nincs a mongoban
                    {
                        await SeriesHelper.ImportRequest(folderPath);
                    }
                }
                else
                {
                    DialogResult messageboxResult = MessageBox.Show("You already started this movie before.", "Started movie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                DialogResult messageboxResult = MessageBox.Show("You already seen this movie. Would you like to watch it again?", "Seen movie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }



            //Felirat letöltés

            var releaser = SubtitleHelper.GetReleaser(mediaFolderName);
            var quality = SubtitleHelper.GetQuality(mediaFolderName);

            var subModel = new Modules.SubtitleManager.MovieSubtitleManager.FeliratokInfo.Models.SubtitleModel(movieTitle, releaser, quality);
            var s = new FeliratokInfoMovieDownloader().FindSubtitle(subModel, folderPath, fileName);

            //TODO Most a legelsőt fogom letölteni, de később az asztali alkalmazásban fellehet sorolni amiket talált matching-re.
            FeliratokInfoMovieDownloader.Download(s[0].DownloadNode, folderPath, fileName);

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
                    MessageBox.Show(
                        "Recommended book(s) by this media's theme : \n" + book.Title + "\nPages: " + book.Pages +
                        "\n\nWould You like to add this book to your reading list?", "Recommended book for you.",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);
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
            var MediaExistInMongo = await SeriesHelper.Parse(name);

            switch (MediaExistInMongo)
            {
                case -1:
                    return false; //EKKOR NINCS ILYEN SOROZAT

                case 1:
                    //var s = await SeriesHelper.GetShow(name);

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

        public Times GetTimes()
        {
            string positionPath = "(/html/body/p)[9]";
            string durationpath = "(/html/body/p)[11]";

            using (WebClient client = new WebClient())
            {
                string htmlString = client.DownloadString(mpcVariablesSiteUrl);
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlString);

                HtmlNode position = htmlDocument.DocumentNode.SelectSingleNode(positionPath);
                HtmlNode duration = htmlDocument.DocumentNode.SelectSingleNode(durationpath);

                var pos = Regex.Split(position.InnerText, ":");
                var dur = Regex.Split(duration.InnerText, ":");

                double seenSeconds = Int32.Parse(pos[0]) * 60 * 60 + Int32.Parse(pos[1]) * 60 + Int32.Parse(pos[2]);
                double totalSeconds = Int32.Parse(dur[0]) * 60 * 60 + Int32.Parse(dur[1]) * 60 + Int32.Parse(dur[2]);
                double percentage = (100 / totalSeconds) * seenSeconds;

                return new Times()
                {
                    Duration = Convert.ToInt32(totalSeconds),
                    Position = Convert.ToInt32(seenSeconds),
                    SeenSeconds = Int32.Parse(pos[2]),
                    SeenHours = Int32.Parse(pos[0]),
                    SeenMinutes = Int32.Parse(pos[1])
                };
            }
        }

    }
}