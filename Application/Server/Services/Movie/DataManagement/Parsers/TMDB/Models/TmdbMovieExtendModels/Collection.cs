using Newtonsoft.Json;

namespace Movie.DataManagement.Parsers.TMDB.Models.TmdbMovieExtendModels
{
    public class Collection
    {
        [JsonProperty("backdrop_path")] public string BackdropPath;
        [JsonProperty("id")] public int? Id;
        [JsonProperty("name")] public string Name;
        [JsonProperty("poster_path")] public string PosterPath;
    }
}