using System;
using System.Threading;
using DesktopClient.MPCManager;

namespace DesktopClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(5000);
            try
            {
                while (true)
                {
                    new MPC().IsMediaRunning();
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                
            }

        }
    }
}
