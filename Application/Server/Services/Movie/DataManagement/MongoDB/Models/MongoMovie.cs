using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Movie.DataManagement.Parsers.OMDB.Models.OmdbMovieExtendModels;
using Movie.DataManagement.Parsers.TMDB.Models.TmdbMovieExtendModels;
using Newtonsoft.Json;

namespace Movie.DataManagement.MongoDB.Models
{
    public class MongoMovie
    {
        [BsonId]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public bool Adult { get; set; }
        public string BackdropPath { get; set; }
        public Collection BelongsToCollection { get; set; }
        public long? Budget { get; set; }
        public List<Genre> Genres { get; set; }
        public string Homepage { get; set; }
        public int? TmdbId { get; set; }
        public string ImdbId { get; set; }
        public string OriginalLanguage { get; set; }
        public string OriginalTitle { get; set; }
        public string Overview { get; set; }
        public double? Popularity { get; set; }
        public string PosterPath { get; set; }
        public List<ProductionCompany> ProductionCompanies { get; set; }
        public List<ProductionCountry> ProductionCountries { get; set; }
        public string ReleaseDate { get; set; }
        public long? Revenue { get; set; }
        public int? Runtime { get; set; }
        public List<Language> SpokenLanguages { get; set; }
        public string Status { get; set; }
        public string Tagline { get; set; }
        public double? VoteAverage { get; set; }
        public int? VoteCount { get; set; }

        //OMDB

        public int Year { get; set; }
        public string Rated { get; set; }
        public string Director { get; set; }
        public List<string> Writer { get; set; }
        public List<string> Actors { get; set; }
        public List<string> Awards { get; set; }
        public List<Rating> Ratings { get; set; }
        public int Metascore { get; set; }
        public double ImdbRating { get; set; }
        public string ImdbVotes { get; set; }
        public string Type { get; set; }
        public string DVD { get; set; }
        public string Production { get; set; }
        public string Website { get; set; }
    }
}
