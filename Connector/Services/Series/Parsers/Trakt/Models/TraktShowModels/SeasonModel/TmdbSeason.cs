using System;
using System.Collections.Generic;
using System.Text;

namespace Series.Parsers.Trakt.Models.TraktShowModels.SeasonModel
{
    public class TmdbSeason
    {
        public int Id;
        public string Name;
        public string Overview;
        public int Season_number;
        public string Air_date;
        public List<TmdbEpisode> Episodes;
        
    }
}
