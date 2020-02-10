using Newtonsoft.Json;

namespace Series.Parsers.TMDB.Models.TmdbShowModels
{
    public class Network
    {
        public string Name;

        [JsonProperty("origin_country")] public string OriginCountry;
    }
}