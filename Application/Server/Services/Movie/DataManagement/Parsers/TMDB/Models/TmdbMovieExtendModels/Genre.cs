using Newtonsoft.Json;

namespace Movie.DataManagement.Parsers.TMDB.Models.TmdbMovieExtendModels
{
    public class Genre
    {
        [JsonProperty("id")] public int Id;
        [JsonProperty("name")] public string Name;
    }
}