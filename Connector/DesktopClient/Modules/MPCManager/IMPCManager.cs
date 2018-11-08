using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.MPCManager
{
    public interface IMPCManager
    {
        Task<bool> IsMediaRunning();
        Task<bool> RecognizeMedia(Process process);
        //Task<bool> IsItValidShow(string title);  átkerült a  helperbe

    }
}
