using System.Collections.Generic;

namespace Series.Parsers.TMDB.Models.TmdbShowModels.SeasonModel
{
    public class TmdbEpisode
    {
        public string Air_date;
        public int Episode_number;
        public string Name;
        public string Overview;
        public int Season_number;
        public string Show_id;
        public double? Vote_average;
        public int Vote_count;
        public List<Crew> Crew;
        public List<Guest> Guest_stars;
    }
}
