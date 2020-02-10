using System.Collections.Generic;
using Movie.DataManagement.Parsers.TMDB.Models.TmdbMovieExtendModels;
using Newtonsoft.Json;

namespace Movie.DataManagement.Parsers.TMDB.Models
{
    public class TmdbMovie
    {
        [JsonProperty("adult")] public bool Adult;
        [JsonProperty("backdrop_path")] public string BackdropPath;
        [JsonProperty("belongs_to_collection")] public Collection BelongsToCollection;
        [JsonProperty("budget")] public long? Budget;
        [JsonProperty("genres")] public List<Genre> Genres;
        [JsonProperty("homepage")] public string Homepage;
        [JsonProperty("imdb_id")] public string ImdbId;
        [JsonProperty("original_language")] public string OriginalLanguage;
        [JsonProperty("original_title")] public string OriginalTitle;
        [JsonProperty("overview")] public string Overview;
        [JsonProperty("popularity")] public double? Popularity;
        [JsonProperty("poster_path")] public string PosterPath;
        [JsonProperty("production_companies")] public List<ProductionCompany> ProductionCompanies;
        [JsonProperty("production_countries")] public List<ProductionCountry> ProductionCountries;
        [JsonProperty("release_date")] public string ReleaseDate;
        [JsonProperty("revenue")] public long? Revenue;
        [JsonProperty("runtime")] public int? Runtime;
        [JsonProperty("spoken_languages")] public List<Language> SpokenLanguages;
        [JsonProperty("status")] public string Status;
        [JsonProperty("tagline")] public string Tagline;
        [JsonProperty("title")] public string Title;
        [JsonProperty("id")] public int? TmdbId;
        [JsonProperty("vote_average")] public double? VoteAverage;
        [JsonProperty("vote_count")] public int? VoteCount;
    }
}