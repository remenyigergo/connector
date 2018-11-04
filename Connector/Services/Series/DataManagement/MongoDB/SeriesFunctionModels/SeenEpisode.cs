namespace Series.DataManagement.MongoDB.SeriesFunctionModels
{
    public class SeenEpisode
    {
        public int UserId { get; set; }
        public int SeriesId { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
    }
}
