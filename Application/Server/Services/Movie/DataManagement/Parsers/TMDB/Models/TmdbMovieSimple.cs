using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Movie.DataManagement.Parsers.TMDB.Models
{
    public class TmdbMovieSimple
    {
        [JsonProperty("vote_count")]
        public int VoteCount;
        [JsonProperty("id")]
        public int TmdbId;
        [JsonProperty("vote_average")]
        public double VoteAverage;
        [JsonProperty("title")]
        public string Title;
        [JsonProperty("popularity")]
        public double Popularity;
        [JsonProperty("poster_path")]
        public string PosterPath;
        [JsonProperty("original_language")]
        public string OriginalLanguage;
        [JsonProperty("original_title")]
        public string OriginalTitle;
        [JsonProperty("genre_ids")]
        public List<int> GenreIds;
        [JsonProperty("backdrop_path")]
        public string BackdropPath;
        [JsonProperty("adult")]
        public bool Adult;
        [JsonProperty("overview")]
        public string Overview;
        [JsonProperty("release_date")]
        public string ReleaseDate;
    }
}
