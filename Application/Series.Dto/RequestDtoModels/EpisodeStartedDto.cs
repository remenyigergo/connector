using System;

namespace Series.Dto.RequestDtoModels
{
    public class EpisodeStartedDto
    {
        public DateTime Date { get; }
        public int EpisodeNumber { get; }
        public int HoursElapsed { get; }
        public int MinutesElapsed { get; }
        public int SeasonNumber { get; }
        public int SecondsElapsed { get; }
        public int TmdbId { get; set; }
        public int TvMazeId { get; set;  }
        public int Userid { get; }
        public double WatchedPercentage { get; }
    }
}
