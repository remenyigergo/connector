using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Series.Service.Requests
{
    public class EpisodeRateRequest
    {
        public int UserId;
        public int? TvMazeId;
        public int? TmdbId;
        public int SeasonNumber;
        public int EpisodeNumber;
        public int Rate;
    }
}
