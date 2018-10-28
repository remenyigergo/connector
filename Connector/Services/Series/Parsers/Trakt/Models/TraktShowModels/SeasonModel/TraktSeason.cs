using System;
using System.Collections.Generic;
using System.Text;

namespace Series.Parsers.Trakt.Models.TraktShowModels.SeasonModel
{
    public class TraktSeason
    {
        public string Id;
        public string Name;
        public string Overview;
        public string Season_number;
        public string Air_date;
        public List<TraktEpisode> Episodes;
        
    }
}
