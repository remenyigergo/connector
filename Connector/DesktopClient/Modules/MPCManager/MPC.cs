using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DesktopClient.Modules.Helper;
using DesktopClient.MPCManager;
using Series.Service.Response;

namespace DesktopClient.MPCManager
{
    class MPC : IMPCManager
    {
        public async Task<bool> IsMediaRunning()
        {
            var process = new ProcessManager().FindProcessByName("mpc-hc64");

            if (process != null)
            {
                await RecognizeMedia(process);
            }

            return false;
        }

        

        public async Task<bool> RecognizeMedia(Process playerProcess)
        {
            if (playerProcess.MainModule.FileVersionInfo.FileDescription == "MPC-HC")
            {
                var titleNameWithoutVersion = new Helper().DeleteVersionName(playerProcess.MainWindowTitle);
                var cleaned = new Helper().Cleaning(titleNameWithoutVersion);

                if (cleaned[0] != null)
                {

                    var show = await new Helper().TryNames(cleaned);
                    return show != -1;
                }
                

                return true;
            }

            return false;
        }

    }


}

