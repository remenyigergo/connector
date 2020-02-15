using System;

namespace Standard.Contracts.Models.Series
{
    public class InternalEpisodeSeen
    {
        public int UserId { get; set; }
        public string TvMazeId { get; set; }
        public string TmdbId { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public DateTime Date { get; set; }
    }
}