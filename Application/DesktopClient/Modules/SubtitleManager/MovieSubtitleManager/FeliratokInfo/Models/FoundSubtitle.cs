using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Modules.SubtitleManager.MovieSubtitleManager.FeliratokInfo.Models
{
    public class FoundSubtitle
    {
        public string MagyarNode;
        public string OriginalNode;
        public string DownloadNode;
        public HtmlDocument htmlDocument;

        public FoundSubtitle(string _magyar, string _original, string node, HtmlDocument htmlDoc)
        {
            MagyarNode = _magyar;
            OriginalNode = _original;
            DownloadNode = node;
            htmlDocument = htmlDoc;
        }

        public FoundSubtitle() { }
    }
}
