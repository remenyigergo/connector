using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Series.Service.Models
{
    public class EpisodeStartedModel
    {
        public int Userid;
        public int Seriesid;
        public int TimeElapsed;
        public int SeasonNumber;
        public int EpisodeNumber;
        public string Date;
        public string WatchedPercentage;
    }
}
