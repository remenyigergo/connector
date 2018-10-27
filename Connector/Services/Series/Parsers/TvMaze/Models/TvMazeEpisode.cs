using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Series.Parsers.TvMaze.Models
{
    class TvMazeEpisode
    {
        [JsonProperty("id")]
        public string ShowId { get; set; }
        public string Name { get; set; }
        public string Season { get; set; }
        public string Number { get; set; }
        public string Airdate { get; set; }
        public string Runtime { get; set; }
        public string Summary { get; set; }

        // TODO
        //public double? Rating { get; set; }
    }
}
