using System.Collections.Generic;
using Newtonsoft.Json;

namespace Movie.DataManagement.Parsers.TMDB.Models
{
    public class TmdbMovieSimple
    {
        [JsonProperty("adult")] public bool Adult;
        [JsonProperty("backdrop_path")] public string BackdropPath;
        [JsonProperty("genre_ids")] public List<int> GenreIds;
        [JsonProperty("original_language")] public string OriginalLanguage;
        [JsonProperty("original_title")] public string OriginalTitle;
        [JsonProperty("overview")] public string Overview;
        [JsonProperty("popularity")] public double Popularity;
        [JsonProperty("poster_path")] public string PosterPath;
        [JsonProperty("release_date")] public string ReleaseDate;
        [JsonProperty("title")] public string Title;
        [JsonProperty("id")] public int TmdbId;
        [JsonProperty("vote_average")] public double VoteAverage;
        [JsonProperty("vote_count")] public int VoteCount;
    }
}