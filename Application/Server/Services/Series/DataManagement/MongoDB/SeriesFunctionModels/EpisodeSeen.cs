using System;

namespace Series.DataManagement.MongoDB.SeriesFunctionModels
{
    public class EpisodeSeen
    {
        public int UserId { get; set; }
        public string TvMazeId { get; set; }
        public string TmdbId { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }

        public DateTime Date { get; set; }
    }
}
