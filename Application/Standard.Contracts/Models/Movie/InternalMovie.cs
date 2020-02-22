using System.Collections.Generic;
using Standard.Contracts.Models.Movie.ExtendClasses;

namespace Standard.Contracts.Models.Movie
{
    //összegyűjti az összes attributumut az összes oldalról
    public class InternalMovie
    {
        public List<string> Actors;
        public bool Adult;
        public List<string> Awards;
        public string BackdropPath;
        public InternalCollection BelongsToCollection;
        public long? Budget;
        public string Director;
        public string DVD;
        public List<InternalMovieGenre> Genres;
        public string Homepage;
        public string ImdbId;
        public double ImdbRating;
        public string ImdbVotes;
        public string Metascore;
        public string OriginalLanguage;
        public string OriginalTitle;
        public string Overview;
        public double? Popularity;
        public string PosterPath;
        public string Production;
        public List<InternalProductionCompany> ProductionCompanies;
        public List<string> ProductionCountries;

        public string Rated;
        public List<InternalRating> Ratings;
        public string ReleaseDate;
        public long? Revenue;
        public int? Runtime;
        public List<string> SpokenLanguages;
        public string Status;
        public string Tagline;
        public string Title;
        public int? TmdbId;
        public string Type;
        public double? VoteAverage;
        public int? VoteCount;
        public string Website;
        public List<string> Writer;

        //OMDB
        public int Year;
    }
}