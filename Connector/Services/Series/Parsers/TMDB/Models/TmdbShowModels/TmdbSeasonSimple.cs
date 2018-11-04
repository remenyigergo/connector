using Newtonsoft.Json;

namespace Series.Parsers.TMDB.Models.TmdbShowModels
{
    public class TmdbSeasonSimple
    {
        public string Id;

        [JsonProperty("air_date")]
        public string AirDate;

        [JsonProperty("episode_count")]
        public int EpisodeCount;

        public string Name;

        public string Overview;

        [JsonProperty("season_number")]
        public int SeasonNumber;
    }
}
