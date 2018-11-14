using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace KeyEventForm.Modules.MPCManager
{
    public interface IMPCManager
    {
        Process IsMediaRunning();
        Task<bool> RecognizeMedia(Process process);
        //Task<bool> IsTheShowExist(string title);  átkerült a  helperbe

    }
}
