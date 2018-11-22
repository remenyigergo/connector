using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Series.Service.Models
{
    public class InternalEpisodeStartedModel
    {
        public int Userid;
        public int TvMazeId;
        public int TmdbId;
        public int HoursElapsed;
        public int MinutesElapsed;
        public int SecondsElapsed;
        public int SeasonNumber;
        public int EpisodeNumber;
        public DateTime Date;
        public double WatchedPercentage;
    }
}
