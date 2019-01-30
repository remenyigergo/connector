using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Series.Parsers.TMDB.Models.TmdbShowModels
{
    public class Network
    {
        public string Name;

        [JsonProperty("origin_country")]
        public string OriginCountry;
    }
}
