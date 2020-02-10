namespace Series.Service.Requests
{
    public class EpisodeRateRequest
    {
        public int EpisodeNumber;
        public int Rate;
        public int SeasonNumber;
        public int? TmdbId;
        public int? TvMazeId;
        public int UserId;
    }
}