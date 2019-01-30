using System.Collections.Generic;
using Newtonsoft.Json;

namespace Series.Parsers.TMDB.Models.TmdbShowModels.SeasonModel
{
    public class TmdbSeason
    {
        public int Id;

        public string Name;

        public string Overview;

        [JsonProperty("season_number")]
        public int SeasonNumber;

        [JsonProperty("air_date")]
        public string AirDate;
        
        public List<TmdbEpisode> Episodes;        
    }
}
