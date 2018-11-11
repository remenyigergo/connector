using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Contracts.Requests;
using DesktopClient.MPCManager;
using DesktopClient.Modules.Helpers;

namespace DesktopClient.Modules.MPCManager
{
    class MPC : IMPCManager
    {
        public async Task<bool> IsMediaRunning()
        {
            //TODO Ne csak 64bites MPC-t találjunk meg 
            var process = new ProcessManager().FindProcessByName("mpc-hc64");

            if (process != null)
            {
                await RecognizeMedia(process);
            }

            return false;
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

