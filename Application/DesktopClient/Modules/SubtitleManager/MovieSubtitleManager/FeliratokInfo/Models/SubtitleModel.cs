using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Modules.SubtitleManager.MovieSubtitleManager.FeliratokInfo.Models
{
    public class SubtitleModel
    {
        public string Title;
        public string Quality;
        public string Releaser;

        public SubtitleModel()
        {
        }

        public SubtitleModel(string title, string q, string r)
        {
            Title = title;
            Quality = q;
            Releaser = r;
        }

        
    }
}
