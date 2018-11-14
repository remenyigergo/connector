using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Standard.Contracts.Requests;
using DesktopClient.MPCManager;
using DesktopClient.Modules.Helpers;

namespace DesktopClient.Modules.MPCManager
{
    class MPC : IMPCManager
    {
        public Process IsMediaRunning()
        {
            return new ProcessManager().FindProcessByName("mpc-hc");
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

