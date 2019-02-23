using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Modules.MPCManager
{
    public interface IMPCManager
    {
        Process IsMediaRunning();

        Task<bool> RecognizeMedia(Process process);

        //Task SavePosition(string showName, int seasonNum, int episodeNum, string mpcUrl);

    }
}
