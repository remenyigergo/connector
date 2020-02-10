using System.Collections.Generic;
using Newtonsoft.Json;
using Series.Parsers.TMDB.Models.TmdbShowModels;

namespace Series.Parsers.TMDB.Models
{
    public class TmdbShow
    {
        [JsonProperty("created_by")] public List<Creator> CreatedBy;

        [JsonProperty("episode_run_time")] public List<string> EpisodeRunTime;

        [JsonProperty("first_air_date")] public string FirstAirDate;

        public List<Genre> Genres;
        public string Id;

        [JsonProperty("last_episode_to_air")] public TmdbEpisodeSimple LastEpisodeToAir;

        public string Name;

        public List<Network> Networks;

        [JsonProperty("original_language")] public string OriginalLanguage;

        public string Overview;

        public string Popularity;

        [JsonProperty("production_companies")] public List<ProductionCompany> ProductionCompanies;

        public List<TmdbSeasonSimple> Seasons;

        public string Status;

        public string Type;

        [JsonProperty("vote_average")] public double? VoteAverage;

        [JsonProperty("vote_count")] public int VoteCount;
    }
}