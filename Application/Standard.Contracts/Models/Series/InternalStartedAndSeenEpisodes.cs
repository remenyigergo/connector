using Series.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Contracts.Models.Series.ExtendClasses
{
    public class InternalStartedAndSeenEpisodes
    {
        public List<InternalEpisodeStartedModel> startedEpisodeList;
        public List<InternalEpisodeSeen> seenEpisodeList;
    }
}
