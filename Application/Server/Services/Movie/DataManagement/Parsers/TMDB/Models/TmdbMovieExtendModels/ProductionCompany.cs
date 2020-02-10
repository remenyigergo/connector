using Newtonsoft.Json;

namespace Movie.DataManagement.Parsers.TMDB.Models.TmdbMovieExtendModels
{
    public class ProductionCompany
    {
        [JsonProperty("id")] public int Id;
        [JsonProperty("logo_path")] public string LogoPath;
        [JsonProperty("name")] public string Name;
        [JsonProperty("origin_country")] public string OriginCountry;
    }
}