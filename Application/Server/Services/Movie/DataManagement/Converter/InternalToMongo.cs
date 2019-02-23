using System;
using System.Collections.Generic;
using System.Text;
using Movie.DataManagement.Parsers.OMDB.Models.OmdbMovieExtendModels;
using Movie.DataManagement.Parsers.TMDB.Models.TmdbMovieExtendModels;
using Standard.Contracts.Models.Movie;
using Standard.Contracts.Models.Movie.ExtendClasses;

namespace Movie.DataManagement.Converter
{
    class InternalToMongo
    {

        public MongoDB.Models.MongoMovie Movie(InternalMovie internalMovie)
        {
            return new MongoDB.Models.MongoMovie()
            {
                Title = internalMovie.Title,
                Adult = internalMovie.Adult,
                BackdropPath = internalMovie.BackdropPath,
                BelongsToCollection = this.Collection(internalMovie.BelongsToCollection),
                Budget = internalMovie.Budget,
                Genres = this.Genre(internalMovie.Genres),
                Homepage = internalMovie.Homepage,
                TmdbId = internalMovie.TmdbId,
                ImdbId = internalMovie.ImdbId,
                OriginalLanguage = internalMovie.OriginalLanguage,
                OriginalTitle = internalMovie.OriginalTitle,
                Overview = internalMovie.Overview,
                Popularity = internalMovie.Popularity,
                PosterPath = internalMovie.PosterPath,
                ProductionCompanies = this.ProdCompanies(internalMovie.ProductionCompanies),
                ProductionCountries = this.ProdCountries(internalMovie.ProductionCountries),
                ReleaseDate = internalMovie.ReleaseDate,
                Revenue = internalMovie.Revenue,
                Runtime = internalMovie.Runtime,
                SpokenLanguages = this.SpokenLanguages(internalMovie.SpokenLanguages),
                Status = internalMovie.Status,
                Tagline = internalMovie.Tagline,
                VoteAverage = internalMovie.VoteAverage,
                VoteCount = internalMovie.VoteCount,

                //OMDB RÉSZE ÁTALAKÍTÁS
                Year = internalMovie.Year,
                Rated = internalMovie.Rated,
                Director = internalMovie.Director,
                Writer = internalMovie.Writer,
                Actors = internalMovie.Actors,
                Awards = internalMovie.Awards,
                Ratings = this.Rating(internalMovie.Ratings),
                Metascore = internalMovie.Metascore,
                ImdbRating = internalMovie.ImdbRating,
                ImdbVotes = internalMovie.ImdbVotes,
                Type = internalMovie.Type,
                DVD = internalMovie.DVD,
                Production = internalMovie.Production,
                Website = internalMovie.Website
            };

        }

        public Collection Collection(InternalCollection internalCollection)
        {
            if (internalCollection != null)
            {
                return new Collection()
                {
                    BackdropPath = internalCollection.BackdropPath,
                    Id = internalCollection.Id,
                    Name = internalCollection.Name,
                    PosterPath = internalCollection.PosterPath
                };
            }
            return null;
        }

        public List<Genre> Genre(List<InternalGenre> internalGenres)
        {
            List<Genre> genres = new List<Genre>();
            foreach (var internalGenre in internalGenres)
            {
                genres.Add(new Genre()
                {
                    Id = internalGenre.Id,
                    Name = internalGenre.Name
                });
            }

            return genres;
        }

        public List<ProductionCompany> ProdCompanies(List<InternalProductionCompany> internalProductionCompanies)
        {
            List<ProductionCompany> companies = new List<ProductionCompany>();
            foreach (var internalProdComp in internalProductionCompanies)
            {
                companies.Add(new ProductionCompany()
                {
                    Id = internalProdComp.Id,
                    Name = internalProdComp.Name,
                    LogoPath = internalProdComp.LogoPath,
                    OriginCountry = internalProdComp.OriginCountry
                });
            }

            return companies;
        }

        public List<ProductionCountry> ProdCountries(List<string> internalProductionCountries)
        {
            List<ProductionCountry> countries = new List<ProductionCountry>();
            foreach (var internalProdCountry in internalProductionCountries)
            {
                countries.Add(new ProductionCountry()
                {
                    //IsoNum = internalProdCountry.IsoNum,
                    Name = internalProdCountry
                });
            }

            return countries;
        }

        public List<Language> SpokenLanguages(List<string> internalLanguages)
        {
            List<Language> languages = new List<Language>();
            foreach (var internalLang in internalLanguages)
            {
                languages.Add(new Language()
                {
                    //IsoNum = internalLang.IsoNum,
                    Name = internalLang
                });
            }

            return languages;
        }

        public List<Rating> Rating(List<InternalRating> ratings)
        {
            List<Rating> ratingList = new List<Rating>();
            foreach (var rating in ratings)
            {
                ratingList.Add(new Rating()
                {
                    Source = rating.Source,
                    Value = rating.Value
                });
            }

            return ratingList;
        }
    }
}
