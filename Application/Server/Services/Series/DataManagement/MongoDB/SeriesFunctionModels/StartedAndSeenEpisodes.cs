using System;
using System.Collections.Generic;
using System.Text;

namespace Series.DataManagement.MongoDB.SeriesFunctionModels
{
    public class StartedAndSeenEpisodes
    {
        public List<Service.Models.EpisodeStarted> startedEpisodes;
        public List<EpisodeSeen> seenEpisodes;
    }
}
