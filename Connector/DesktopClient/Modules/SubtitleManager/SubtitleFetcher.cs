using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using DesktopClient.Modules.Helpers;
using DesktopClient.Modules.MPCManager;

namespace DesktopClient.Modules.SubtitleManager
{
    class SubtitleFetcher
    {
        private const string downloadPath = "D:\\uTorrent";

        public string GetFolderPath()
        {
            foreach (var d in System.IO.Directory.GetDirectories(downloadPath))
            {
                var dirName = new DirectoryInfo(d).Name;
                var dirNameCleaned = Helper.GetTitle(dirName);

                var showName = Helper.GetTitle(new MPC().IsMediaRunning().MainWindowTitle);

                

                if (dirNameCleaned != null && dirNameCleaned.ToLower() == showName)  //TODO sorozat elnevezések ami nem (Title.SeasonEpisode.Coding-Releaser) alakuak nemjók ugye
                {
                    var isThereSubtitle = IsThereSubtitles(dirName, dirNameCleaned);
                    return "egyezik";
                }
            }

            return "";
        }

        public bool IsThereSubtitles(string folderName, string showName)
        {
            string[] fileArray = Directory.GetFiles(downloadPath+"\\"+folderName);
            foreach (var file in fileArray)
            {
               if (file.EndsWith(".srt"))
                {
                    if (file.Contains(showName)) //TODO ÉS SZEZONT ÉS EPIZÓDOT
                    {
                        return true;
                    }
                }
            }

            DownloadSubtitle();

            return false;
        }

        public void DownloadSubtitle()
        {
            
        }
    }
}
