using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using DesktopClient.Modules.ApplicationManager.ProcessEqualityComparer;

namespace DesktopClient.Modules.ProcessManager
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
            var processes = Process.GetProcesses();

            foreach (var process in processes)
                try
                {
                    if (process.MainModule.FileVersionInfo.FileDescription != null && process.MainModule.FileVersionInfo
                            .FileDescription.ToLower().Contains(processName.ToLower()))
                        return process;
                }
                catch (Win32Exception ex)
                {
                }
            //return processes.FirstOrDefault(process => process.MainModule != null && process.MainModule.FileVersionInfo.FileDescription.ToLower().Contains(processName.ToLower()));

            return null;
        }

        public HashSet<Process> FindApplicationThatRunBy(string username)
        {
            var processList = new HashSet<Process>(new ProcessEqualityComparer());
            var processes = GetProcesses();

            foreach (var process in processes)
                try
                {
                    var owner = GetProcessOwner(process.MainModule.ModuleName);
                    if (owner == username)
                        processList.Add(process);
                }
                catch (Win32Exception Win32Exception)
                {
                }
                catch (InvalidOperationException IOE)
                {
                }

            if (processList.Count != 0)
                return processList;

            return null;
        }


        public string GetProcessOwner(string processName)
        {
            var query = "Select * from Win32_Process Where Name = \"" + processName + "\"";
            var searcher = new ManagementObjectSearcher(query);
            var processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = {string.Empty, string.Empty};
                var returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    var owner = argList[1] + "\\" + argList[0];
                    return owner;
                }
            }

            return "NO OWNER";
        }
    }
}