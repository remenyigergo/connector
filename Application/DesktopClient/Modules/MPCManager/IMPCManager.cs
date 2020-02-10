﻿using System.Diagnostics;
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