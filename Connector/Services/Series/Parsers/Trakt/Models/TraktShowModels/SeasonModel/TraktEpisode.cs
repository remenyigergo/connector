using System;
using System.Collections.Generic;
using System.Text;

namespace Series.Parsers.Trakt.Models.TraktShowModels.SeasonModel
{
    public class TraktEpisode
    {
        public string Air_date;
        public int Episode_number;
        public string Name;
        public string Overview;
        public string Season_number;
        public string Show_id;
        public string Vote_average;
        public int Vote_count;
        public List<Crew> Crew;
        public List<Guest> Guest_stars;
    }
}
