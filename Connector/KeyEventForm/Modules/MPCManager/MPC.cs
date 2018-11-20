using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using KeyEventForm.Modules.Helpers;
using KeyEventForm.Modules.SubtitleManager;
using KeyEventForm.Modules.SubtitleManager.FeliratokInfo.Models;
using Standard.Contracts.Requests;
using Timer = System.Timers.Timer;

namespace KeyEventForm.Modules.MPCManager
{
    class MPC : IMPCManager
    {

        public static Stopwatch stopWatch = new Stopwatch();
        public static bool mediaJustStarted = false;


        public void MPCManager()
        {
            Thread.Sleep(1000);
            try
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        var runningMedia = IsMediaRunning();
                        Thread.Sleep(1000); //mert gyorsabban olvasta ki a nevét az MPC-nek, mint ahogy elindult volna
                        if (!mediaJustStarted && runningMedia != null && !runningMedia.MainWindowTitle.StartsWith("Media Player Classic"))
                        {
                            var path = SubtitleFetcher.GetFolderPathFromMPCweb();
                            var fileName = runningMedia.MainWindowTitle;

                            var showName = Helper.GetTitle(fileName);
                            var episodeNumber = Helper.GetEpisodeNumber(fileName);
                            var seasonNumber = Helper.GetSeasonNumber(fileName);
                            var releaser = Helper.GetReleaser(fileName);
                            var quality = Helper.GetQuality(fileName);


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
                        Thread.Sleep(1000);
                    }
                });
            }
            catch (Exception ex)
            {

            }
        }

        public Process IsMediaRunning()
        {
            return new ProcessManager.ProcessManager().FindProcessByName("mpc-hc");
        }

        public async Task Timering(string title)
        {
            Timer t = new Timer(1000);

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
                    var imr = new InternalMarkRequest()
                    {
                        SeriesId = await Helper.GetShow(name),
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

