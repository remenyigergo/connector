using Movie.DataManagement.Parsers.OMDB.Models.OmdbMovieExtendModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.DataManagement.Parsers.OMDB.Models
{
    public class OmdbMovie
    {
        [JsonProperty("Title")]
        public string Title;
        [JsonProperty("Year")]
        public string Year;
        [JsonProperty("Rated")]
        public string Rated;
        public string Released;
        public string Runtime;
        public string Genre;
        public string Director;
        public string Writer;
        public string Actors;
        [JsonProperty("Plot")]
        public string Overview;
        [JsonProperty("Language")]
        public string Languages;
        public string Country;
        public string Awards;
        public string Poster;
        public List<Rating> Ratings;
        public string Metascore;
        [JsonProperty("imdbRating")]
        public string ImdbRating;
        [JsonProperty("imdbVotes")]
        public string ImdbVotes;
        [JsonProperty("imdbID")]
        public string ImdbId;
        public string Type;
        [JsonProperty("DVD")]
        public string Dvd;
        public string Production;
        public string Website;
    }
}
