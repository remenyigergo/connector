using System;
using System.Collections.Generic;
using System.Text;
using Contracts.Models.Series;
using Core.DataManager.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Series.Parsers.TvMaze.Models
{
    public class TvMazeSeason
    {
        public int Id;

        [JsonProperty("number")]
        public int SeasonNumber;

        [JsonProperty("episodeOrder")]
        public int? EpisodesCount;

        public string Summary;
    }
}
