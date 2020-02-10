using Standard.Contracts.Models.Movie.ExtendClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Contracts.Models.Movie
{
    //összegyűjti az összes attributumut az összes oldalról
    public class InternalMovie
    {
        public InternalMovie()
        {
        }

        public string Title;
        public bool Adult;
        public string BackdropPath;
        public InternalCollection BelongsToCollection;
        public long? Budget;
        public List<InternalGenre> Genres;
        public string Homepage;
        public int? TmdbId;
        public string ImdbId;
        public string OriginalLanguage;
        public string OriginalTitle;
        public string Overview;
        public double? Popularity;
        public string PosterPath;
        public List<InternalProductionCompany> ProductionCompanies;
        public List<string> ProductionCountries;
        public string ReleaseDate;
        public long? Revenue;
        public int? Runtime;
        public List<string> SpokenLanguages;
        public string Status;
        public string Tagline;
        public double? VoteAverage;
        public int? VoteCount;

        //OMDB
        public int Year;
        public string Rated;
        public string Director;
        public List<string> Writer;
        public List<string> Actors;
        public List<string> Awards;
        public List<InternalRating> Ratings;
        public string Metascore;
        public double ImdbRating;
        public string ImdbVotes;
        public string Type;
        public string DVD;
        public string Production;
        public string Website;
    }
}
