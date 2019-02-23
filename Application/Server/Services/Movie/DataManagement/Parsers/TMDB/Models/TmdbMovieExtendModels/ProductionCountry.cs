using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.DataManagement.Parsers.TMDB.Models.TmdbMovieExtendModels
{
    public class ProductionCountry
    {
        [JsonProperty("iso_3166_1")]
        public string IsoNum;
        [JsonProperty("name")]
        public string Name;

    }
}
