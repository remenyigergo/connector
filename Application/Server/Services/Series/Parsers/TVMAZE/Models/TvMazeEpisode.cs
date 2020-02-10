using Newtonsoft.Json;

namespace Series.Parsers.TvMaze.Models
{
    internal class TvMazeEpisode
    {
        [JsonProperty("id")]
        public string ShowId { get; set; }

        public string Name { get; set; }
        public string Season { get; set; }
        public string Number { get; set; }
        public string Airdate { get; set; }
        public string Runtime { get; set; }

        public string Summary { get; set; }
        //public double? Rating { get; set; }

        // TODO
    }
}