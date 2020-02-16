using System;

namespace Series.Dto.RequestDtoModels
{
    public class EpisodeStartedDto
    {
        public DateTime Date { get; set; }
        public int EpisodeNumber { get; set; }
        public int HoursElapsed { get; set; }
        public int MinutesElapsed { get; set; }
        public int SeasonNumber { get; set; }
        public int SecondsElapsed { get; set; }
        public int TmdbId { get; set; }
        public int TvMazeId { get; set;  }
        public int Userid { get; set; }
        public double WatchedPercentage { get; set; }
    }
}
