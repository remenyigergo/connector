using System.Collections.Generic;
using Series.Service.Models;

namespace Standard.Contracts.Models.Series.ExtendClasses
{
    public class InternalStartedAndSeenEpisodes
    {
        public List<InternalEpisodeSeen> seenEpisodeList;
        public List<InternalEpisodeStartedModel> startedEpisodeList;
    }
}