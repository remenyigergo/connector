using System;

namespace Standard.Contracts.Models.Series
{
    public class InternalEpisodeStartedModel
    {
        public DateTime Date;
        public int EpisodeNumber;
        public int HoursElapsed;
        public int MinutesElapsed;
        public int SeasonNumber;
        public int SecondsElapsed;
        public int TmdbId;
        public int TvMazeId;
        public int Userid;
        public double WatchedPercentage;
    }
}