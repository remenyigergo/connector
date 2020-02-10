using System.Collections.Generic;
using Movie.DataManagement.Parsers.OMDB.Models.OmdbMovieExtendModels;
using Newtonsoft.Json;

namespace Movie.DataManagement.Parsers.OMDB.Models
{
    public class OmdbMovie
    {
        public string Actors;
        public string Awards;
        public string Country;
        public string Director;
        [JsonProperty("DVD")] public string Dvd;
        public string Genre;
        [JsonProperty("imdbID")] public string ImdbId;
        [JsonProperty("imdbRating")] public string ImdbRating;
        [JsonProperty("imdbVotes")] public string ImdbVotes;
        [JsonProperty("Language")] public string Languages;
        public string Metascore;
        [JsonProperty("Plot")] public string Overview;
        public string Poster;
        public string Production;
        [JsonProperty("Rated")] public string Rated;
        public List<Rating> Ratings;
        public string Released;
        public string Runtime;
        [JsonProperty("Title")] public string Title;
        public string Type;
        public string Website;
        public string Writer;
        [JsonProperty("Year")] public string Year;
    }
}