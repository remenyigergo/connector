using System.Collections.Generic;

namespace Standard.Contracts.Models.Series
{
    public class InternalStartedAndSeenEpisodes
    {
        public List<InternalEpisodeSeen> seenEpisodeList;
        public List<InternalEpisodeStartedModel> startedEpisodeList;
    }
}