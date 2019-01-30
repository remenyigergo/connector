using Newtonsoft.Json;

namespace Series.Parsers.TMDB.Models.TmdbShowModels
{
    public class TmdbEpisodeSimple
    {
        [JsonProperty("air_date")]
        public string AirDate;

        [JsonProperty("episode_number")]
        public int EpisodeNumber;

        [JsonProperty("season_number")]
        public int SeasonNumber;

        public string Name;

        public string Overview;

        [JsonProperty("vote_average")]
        public double? VoteAverage;

        [JsonProperty("vote_count")]
        public int VoteCount;
    }
}
