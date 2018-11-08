using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DesktopClient
{
    public class ProcessManager
    {
        private Process[] GetProcesses()
        {
            return Process.GetProcesses();
        }

        public Process FindProcessByName(string processName)
        {
            var p = Process.GetProcessesByName(processName).FirstOrDefault(process => process.ProcessName.StartsWith(processName));
            Console.WriteLine(p);
            return p;
        }

    }
}