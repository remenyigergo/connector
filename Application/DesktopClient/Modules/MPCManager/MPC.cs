using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopClient.Modules.Helpers;
using DesktopClient.Modules.Helpers.Book;
using DesktopClient.Modules.Helpers.Movie;
using DesktopClient.Modules.Helpers.Series;
using DesktopClient.Modules.Model;
using DesktopClient.Modules.MPCManager.Model;
using DesktopClient.Modules.SeriesSubtitleManager.FeliratokInfo.Models;
using DesktopClient.Modules.SubtitleManager;
using DesktopClient.Modules.SubtitleManager.MovieSubtitleManager.FeliratokInfo;
using DesktopClient.Modules.SubtitleManager.SeriesSubtitleManager.FeliratokInfo;
using Serilog;
using Standard.Contracts.Enum;
using Standard.Contracts.Requests;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;


namespace DesktopClient.Modules.MPCManager
{
    internal class MPC : IMPCManager
    {
        private const string mpcVariablesSiteUrl = @"http://localhost:13579/variables.html";
        private const string mpcPlayerSiteUrL = @"http://localhost:13579/controls.html";
        private const string apiEndpoint = "http://localhost:5001";
        private static readonly Stopwatch stopWatch = new Stopwatch();
        private static bool mediaJustStarted;
        private Times elapsedTimeInMedia;
        private string fileName;
        private string tempShowName;
        private Process playerProcess;
        private string videoPlayer;
        private ILogger _log;


        public MPC()
        {
            mediaJustStarted = false;
            elapsedTimeInMedia = null;
            fileName = string.Empty;
            tempShowName = null;
            playerProcess = null;
            videoPlayer = "mpc-hc";
        }

        public Process FindProcessByName()
        {
            return new ProcessManager.ProcessManager().FindProcessByName(videoPlayer);
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

                    var imr = new InternalMarkRequest
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

        public async Task MPCManager()
        {
            // TODO get my id
            var userId = 1;

            //Thread.Sleep(1000);

            //await Task.Run(async () =>
            // {
            while (true)
                try
                {
                    playerProcess = FindProcessByName();
                    var path = GetFolderPathFromMPCweb();
                    var file = GetFilenameFromMPCweb();

                    if (!string.IsNullOrEmpty(file))
                        fileName = file;

                    var media = new Media(fileName, path);

                    
                    bool isThereMediaChange = IsThereMediaChange(media);
                    if (isThereMediaChange)
                    {
                        await SavePosition(media);
                        //await RecommendBook(userId);
                    }

                    if (media.ShowName != null)
                        tempShowName = media.ShowName; //leálláskor elvesztettük a nevét, mert újrakértem itt, de már nem volt meg
                    

                    Thread.Sleep(1000); //mert gyorsabban olvasta ki a nevét az MPC-nek, mint ahogy elindult volna

                    if (IsMediaStarted())
                    {
                        stopWatch.Start();
                        mediaJustStarted = true;

                        if (media.IsItASeries)
                            await ManageSeries(media.ShowName, path, media.EpisodeNumber, media.SeasonNumber, media.Releaser, media.Quality, fileName,
                                playerProcess, userId);
                        else
                            await ManageMovie(path, fileName, userId);

                        var elapsedTime = GetTimes();
                        if (elapsedTime.SeenSeconds != 0)
                            elapsedTimeInMedia = elapsedTime;
                    }
                    

                    Thread.Sleep(200);
                }
                catch (Win32Exception ex)
                {
                    _log.Error(ex.Message);
                }
                catch (Exception e)
                {
                    _log.Error(e.Message);
                }
            // });
        }

        public async Task ManageSeries(string showName, string path, int episodeNumber, int seasonNumber,
            string releaser, string quality, string fileName, Process runningMedia, int userId)
        {
            var isNewSeries = await SeriesHelper.IsTheShowExist(showName);

            if (isNewSeries != (int)MediaExistIn.MONGO) //nincs a mongoban
                await SeriesHelper.ImportRequest(showName);

            if (!FeliratokInfoSeriesDownloader.IsThereSubtitles(path, showName, episodeNumber, seasonNumber))
            {
                var feliratModel = new SubtitleModel
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
                    Process.Start(path + "\\" + fileName);
                    Thread.Sleep(500);
                }
            }

            // TODO elso if részbe tenni (bekapcsoláskor nézni egyből)
            //Előző rész látott?
            var previousEpisodes = await SeriesHelper.PreviousEpisodesSeen(
                new Standard.Contracts.Requests.Series.InternalPreviousEpisodeSeenRequest
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

            tempShowName = movieTitle;

            if (!SeenMovie)
            {
                var StartedMovie = await MovieHelper.IsItStarted(userid, movieTitle);
                if (!StartedMovie)
                {
                    var isNewMovie = await MovieHelper.IsTheMovieExist(movieTitle);
                    if (isNewMovie != (int)MediaExistIn.MONGO) //nincs a mongoban
                        await MovieHelper.ImportRequest(movieTitle);
                }
                else
                {
                    var messageboxResult = MessageBox.Show("You already started this movie before.", "Started movie",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                var messageboxResult = MessageBox.Show("You already seen this movie. Would you like to watch it again?",
                    "Seen movie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }


            //Felirat letöltés

            var releaser = SubtitleHelper.GetReleaser(mediaFolderName);
            var quality = SubtitleHelper.GetQuality(mediaFolderName);

            var subModel =
                new SubtitleManager.MovieSubtitleManager.FeliratokInfo.Models.SubtitleModel(movieTitle, releaser,
                    quality);
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
                    MessageBox.Show(
                        "Recommended book(s) by this media's theme : \n" + book.Title + "\nPages: " + book.Pages +
                        "\n\nWould You like to add this book to your reading list?", "Recommended book for you.",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            }
        }

        public Times GetTimes()
        {
            var positionPath = "(/html/body/p)[9]";
            var durationpath = "(/html/body/p)[11]";

            using (var client = new WebClient())
            {
                var htmlString = client.DownloadString(mpcVariablesSiteUrl);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlString);

                var position = htmlDocument.DocumentNode.SelectSingleNode(positionPath);
                var duration = htmlDocument.DocumentNode.SelectSingleNode(durationpath);

                var pos = Regex.Split(position.InnerText, ":");
                var dur = Regex.Split(duration.InnerText, ":");

                double seenSeconds = int.Parse(pos[0]) * 60 * 60 + int.Parse(pos[1]) * 60 + int.Parse(pos[2]);
                double totalSeconds = int.Parse(dur[0]) * 60 * 60 + int.Parse(dur[1]) * 60 + int.Parse(dur[2]);
                var percentage = 100 / totalSeconds * seenSeconds;

                return new Times
                {
                    Duration = Convert.ToInt32(totalSeconds),
                    Position = Convert.ToInt32(seenSeconds),
                    SeenSeconds = int.Parse(pos[2]),
                    SeenHours = int.Parse(pos[0]),
                    SeenMinutes = int.Parse(pos[1])
                };
            }
        }

        private bool IsMediaStarted()
        {
            return playerProcess != null && !playerProcess.MainWindowTitle.StartsWith("Media Player Classic");
        }

        private bool IsThereMediaChange(Media media)
        {
            if (tempShowName == null)
            {
                return false;
            }

            return tempShowName != media.ShowName;
        }

        private async Task SavePosition(Media media)
        {
            stopWatch.Stop();
            var duration = stopWatch.ElapsedMilliseconds / 1000;
            mediaJustStarted = false;

            if (media.IsItASeries)
                await Task.Run(async () =>
                {
                    await SeriesHelper.SavePosition(media.ShowName, media.SeasonNumber, media.EpisodeNumber,
                        (int)stopWatch.ElapsedMilliseconds / 1000, elapsedTimeInMedia);
                });
            else
                await Task.Run(async () =>
                {
                    await MovieHelper.SavePosition(tempShowName,
                        (int)stopWatch.ElapsedMilliseconds / 1000, elapsedTimeInMedia);
                });
        }

        public string GetFolderPathFromMPCweb()
        {
            var path = string.Empty;

            try
            {
                using (var client = new WebClient())
                {
                    var htmlString = client.DownloadString(mpcVariablesSiteUrl);
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlString);
                    var xPath = "(/html/body/p)[5]";
                    var node = htmlDocument.DocumentNode.SelectSingleNode(xPath);
                    path = node.InnerHtml.Replace(@"\\", @"\");
                }
            }
            catch (WebException WebEx)
            {
                _log.Error(WebEx.Message);
            }
            return path;
        }

        public string GetFilenameFromMPCweb()
        {
            var filename = string.Empty;

            try
            {
                using (var client = new WebClient())
                {
                    var htmlString = client.DownloadString(mpcVariablesSiteUrl);
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlString);
                    var xPath = "(/html/body/p)[1]";
                    var node = htmlDocument.DocumentNode.SelectSingleNode(xPath);
                    filename = node.InnerHtml.Replace(@"\\", @"\");
                }
            }
            catch (WebException WebEx)
            {
            }
            return filename;
        }
    }
}