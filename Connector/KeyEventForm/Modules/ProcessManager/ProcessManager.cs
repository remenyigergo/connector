using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace KeyEventForm.Modules.ProcessManager
{
    public class ProcessManager
    {
        private Process[] GetProcesses()
        {
            return Process.GetProcesses();
        }

        public Process FindProcessByName(string processName)
        {
            //var p = Process.GetProcessesByName(processName).First(process => process.MainModule.FileName.Contains(processName));
            var s = Process.GetProcesses().FirstOrDefault(process => process.ProcessName.Contains(processName));
            return s;

        }

    }
}