using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KeyEventForm.Modules.ApplicationManager.ProcessEqualityComparer;


namespace DesktopClient.Modules.ApplicationManager
{
    public class ApplicationManager
    {

        private Dictionary<int, Stopwatch> followedProgramsRunning = new Dictionary<int, Stopwatch>();


        public async Task RunApplicationManager()
        {
            while (true)
            {
                //SendToast();
                var myPrograms = GetRunningApplications(); // ez a felhasználó általi programok
                InsertNewPrograms(myPrograms);
                RunMonitoring(myPrograms);
                Thread.Sleep(400);
            }
        }



        public async void RunMonitoring(HashSet<string> processes)
        {
            //TODO Get my ID
            int Id = 1;

            //Gather all the followed programs to monitor
            var followedPrograms = await new RequestManager().GatherFollowedPrograms(Id); //ezzel lehet még akciózni, hogy ha adott prgoram fut akkor
                                                                                          //feltehetőleg nem állítunk a követett programokon, így nemkell meghívni csak sokkal később ezt

            AddToFollowedProgramsRunning(followedPrograms, processes);


            var stoppedApps = new Dictionary<int, int>();
            //Start each applications timer
            foreach (var program in followedProgramsRunning)
            {
                var running = isItRunning(program.Key, processes, followedPrograms);
                if (!program.Value.IsRunning && running) // és FUT?
                {
                    program.Value.Start();
                }
                else if (program.Value.IsRunning && !running)
                {
                    program.Value.Stop();
                    stoppedApps.Add(program.Key, (int)program.Value.ElapsedMilliseconds / 1000);

                    //followedProgramsRunning.Remove(program.Key);
                }
            }

            //Updateli azokat az appokat a DB-ben amelyek megállnak. 
            if (stoppedApps.Count != 0)
            {
                await new RequestManager().UpdateRequest(stoppedApps, Id);
            }
        }

        /// <summary>
        /// Ha van olyan futó program ami nincs a serveren, 
        /// </summary>
        /// <param name="myPrograms"></param>
        public async void InsertNewPrograms(HashSet<string> myPrograms)
        {
            var serverPrograms = await new RequestManager().GetAllPrograms();
            var differences = CheckDifferences(serverPrograms, myPrograms.ToList());

            if (differences.Count > 0)
                new RequestManager().InsertProgramRequest(differences);
            

        }        

        //private void ShowToast(string msg)
        //{
        //    // Construct the visuals of the toast
        //    ToastContent toastContent = new ToastContent()
        //    {
        //        // Arguments when the user taps body of toast
        //        Launch = "action=ok",

        //        Visual = new ToastVisual()
        //        {
        //            BindingGeneric = new ToastBindingGeneric()
        //            {
        //                Children =
        //                {
        //                    new AdaptiveText()
        //                    {
        //                        Text = msg
        //                    }
        //                }
        //            }
        //        }
        //    };

        //    var doc = new XmlDocument();
        //    doc.LoadXml(toastContent.GetContent());

        //    // And create the toast notification
        //    var toast = new ToastNotification(doc);

        //    // And then show it
        //    DesktopNotificationManagerCompat.CreateToastNotifier().Show(toast);
        //}
    

    /// <summary>
    /// Megmondja, hogy a követett programok közül van-e olyan ami fut a gépen.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="processes"></param>
    /// <param name="followedPrograms"></param>
    /// <returns></returns>
    public bool isItRunning(int Id, HashSet<string> processes, Dictionary<string, int> followedPrograms)
        {
            var programName = followedPrograms.FirstOrDefault(x => x.Value == Id).Key;

            return processes.Contains(programName);
        }

        /// <summary>
        /// Azoknak a programoknak a listájához ad hozzá új programot, amelyeket követve vannak és futnak.
        /// </summary>
        /// <param name="followedPrograms"></param>
        /// <param name="processes"></param>
        public void AddToFollowedProgramsRunning(Dictionary<string, int> followedPrograms, HashSet<string> processes)
        {
            bool notInList = true;
            if (followedPrograms != null)
            {
                foreach (var followed in followedPrograms)
                {
                    foreach (var process in processes)
                    {
                        try
                        {
                            if (followedProgramsRunning.Count != 0)
                            {
                                notInList = CheckIfProgramNotInUpdatedList(followedPrograms, followedProgramsRunning, process);
                            }

                            if (followed.Key == process && notInList)
                            {
                                Stopwatch timer = new Stopwatch();
                                followedProgramsRunning.Add(followed.Value, timer);
                            }
                        }
                        catch (Win32Exception ex)
                        {
                        }
                    }
                }
            }

        }


        /// <summary>
        /// Megnézi, hogy a process benne van-e a "followedProgramsRunning" listában
        /// </summary>
        /// <param name="followedPrograms"></param>
        /// <param name="programsToUpdate"></param>
        /// <param name="processName"></param>
        /// <returns></returns>
        public bool CheckIfProgramNotInUpdatedList(Dictionary<string, int> followedPrograms, Dictionary<int, Stopwatch> programsToUpdate, string processName)
        {
            int? id = null;
            foreach (var followedProgram in followedPrograms)
            {
                if (processName == followedProgram.Key)
                {
                    id = followedProgram.Value;
                    break;
                }
            }

            foreach (var programtoUpdate in this.followedProgramsRunning)
            {
                if (programtoUpdate.Key == id)
                    return false;
            }

            return true;
        }





        /// <summary>
        /// Saját rendszeren futó alkalmazások összegyűjtése
        /// </summary>
        /// <returns></returns>
        public HashSet<string> GetRunningApplications()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var allApplication = new ProcessManager.ProcessManager().FindApplicationThatRunBy(userName);

            HashSet<string> processes = new HashSet<string>();
            foreach (var app in allApplication)
            {
                try
                {
                    processes.Add(app.MainModule.ModuleName);
                }
                catch (System.InvalidOperationException IOE)
                {
                }
                catch (Win32Exception WinException)
                {
                }
            }

            if (processes.Count != 0)
            {
                return processes;
            }

            return null;
        }




        /// <summary>
        /// A serveren lévő programok és a paraméterben kapott programok közötti különbségeket adja vissza.
        /// </summary>
        /// <param name="serverPrograms"></param>
        /// <param name="myPrograms"></param>
        /// <returns></returns>
        public List<string> CheckDifferences(List<string> serverPrograms, List<string> myPrograms)
        {
            bool programFound = false;
            List<string> programsToAdd = new List<string>();

            foreach (var myProgram in myPrograms)
            {
                foreach (var serverProgram in serverPrograms)
                {
                    if (serverProgram == myProgram && !programFound)
                    {
                        programFound = true;
                        break;
                    }
                }

                if (!programFound)
                {
                    programsToAdd.Add(myProgram);
                }
                else
                {
                    programFound = false;
                }
            }

            return programsToAdd;
        }

        public List<string> ProcessToList(Process[] processes)
        {
            List<string> processList = new List<string>();
            foreach (var process in processes)
            {
                processList.Add(process.MainModule.ModuleName);
            }

            return processList;
        }



    }
}