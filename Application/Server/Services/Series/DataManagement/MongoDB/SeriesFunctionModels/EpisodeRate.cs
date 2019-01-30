using System;
using System.Collections.Generic;
using System.Text;

namespace Series.DataManagement.MongoDB.SeriesFunctionModels
{
    class EpisodeRate
    {
        public int UserId;
        public int TvMazeId;
        public int TmdbId;
        public int SeasonNumber;
        public int EpisodeNumber;
        public int Rate;
    }
}
