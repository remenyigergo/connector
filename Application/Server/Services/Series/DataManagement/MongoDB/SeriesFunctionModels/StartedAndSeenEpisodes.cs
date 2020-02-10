using System.Collections.Generic;
using Series.Service.Models;

namespace Series.DataManagement.MongoDB.SeriesFunctionModels
{
    public class StartedAndSeenEpisodes
    {
        public List<EpisodeSeen> seenEpisodes;
        public List<EpisodeStarted> startedEpisodes;
    }
}