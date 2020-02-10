using HtmlAgilityPack;

namespace DesktopClient.Modules.SubtitleManager.MovieSubtitleManager.FeliratokInfo.Models
{
    public class FoundSubtitle
    {
        public string DownloadNode;
        public HtmlDocument htmlDocument;
        public string MagyarNode;
        public string OriginalNode;

        public FoundSubtitle(string _magyar, string _original, string node, HtmlDocument htmlDoc)
        {
            MagyarNode = _magyar;
            OriginalNode = _original;
            DownloadNode = node;
            htmlDocument = htmlDoc;
        }

        public FoundSubtitle()
        {
        }
    }
}