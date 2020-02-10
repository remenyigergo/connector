namespace DesktopClient.Modules.SubtitleManager.MovieSubtitleManager.FeliratokInfo.Models
{
    public class SubtitleModel
    {
        public string Quality;
        public string Releaser;
        public string Title;

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