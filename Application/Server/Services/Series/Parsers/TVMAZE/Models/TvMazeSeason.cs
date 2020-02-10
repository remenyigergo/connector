using Newtonsoft.Json;

namespace Series.Parsers.TvMaze.Models
{
    public class TvMazeSeason
    {
        [JsonProperty("episodeOrder")] public int? EpisodesCount;
        public int Id;

        [JsonProperty("number")] public int SeasonNumber;

        public string Summary;
    }
}