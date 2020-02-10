using System.Collections.Generic;
using Newtonsoft.Json;

namespace Series.Parsers.TMDB.Models.TmdbShowModels.SeasonModel
{
    public class TmdbEpisode
    {
        [JsonProperty("air_date")] public string AirDate;

        public List<Crew> Crew;

        [JsonProperty("episode_number")] public int EpisodeNumber;

        [JsonProperty("guest_stars")] public List<Guest> GuestStars;
        public string Name;
        public string Overview;

        [JsonProperty("season_number")] public int SeasonNumber;

        [JsonProperty("show_id")] public string ShowId;

        [JsonProperty("vote_average")] public double? VoteAverage;

        [JsonProperty("vote_count")] public int VoteCount;
    }
}