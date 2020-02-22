using DesktopClient.Modules.Helpers;
using DesktopClient.Modules.Helpers.Series;
using DesktopClient.Modules.SubtitleManager;

namespace DesktopClient.Modules.Model
{
    public class Media
    {
        public Media(string fileName, string path)
        {
            Path = path;
            FileName = fileName;
            ShowName = SeriesHelper.GetTitle(fileName);
            EpisodeNumber = SeriesHelper.GetEpisodeNumber(fileName);
            SeasonNumber = SeriesHelper.GetSeasonNumber(fileName);
            Releaser = SubtitleHelper.GetReleaser(fileName);
            Quality = SubtitleHelper.GetQuality(fileName);
        }

        public string Path { get; set; }
        public string FileName { get; set; }
        public string ShowName { get; set; }
        public int EpisodeNumber{ get; set; }
        public int SeasonNumber { get; set; }
        public string Releaser { get; set; }
        public string Quality { get; set; }

        public bool IsItASeries => SeriesHelper.DoesItContainSeasonAndEpisodeS01E01(FileName);
    }
}
