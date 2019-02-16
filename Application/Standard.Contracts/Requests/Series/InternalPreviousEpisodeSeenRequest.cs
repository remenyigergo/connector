using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Contracts.Requests.Series
{
    public class InternalPreviousEpisodeSeenRequest
    {
        public string title;
        public int seasonNum;
        public int episodeNum;
        public int userid;
    }
}
