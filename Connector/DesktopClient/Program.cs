using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DesktopClient.Modules.MPCManager;
using DesktopClient.MPCManager;

namespace DesktopClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Thread.Sleep(5000);
            try
            {
                while (true)
                {
                    new MPC().IsMediaRunning();
                    Thread.Sleep(1000000);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
