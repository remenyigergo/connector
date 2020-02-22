using System.Collections.Generic;
using Series.Service.Models;

namespace Series.DataManagement.MongoDB.SeriesFunctionModels
{
    public class StartedAndSeenEpisodesDao
    {
        public List<EpisodeSeenDao> seenEpisodes;
        public List<EpisodeStartedDao> startedEpisodes;
    }
}