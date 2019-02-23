using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.DataManagement.Parsers.TMDB.Models.TmdbMovieExtendModels
{
    public class Collection
    {
        [JsonProperty("id")]
        public int? Id;
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("poster_path")]
        public string PosterPath;
        [JsonProperty("backdrop_path")]
        public string BackdropPath;
    }
}
