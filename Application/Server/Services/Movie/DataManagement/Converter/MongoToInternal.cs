using System.Collections.Generic;
using Movie.DataManagement.MongoDB.Models;
using Movie.DataManagement.Parsers.TMDB.Models.TmdbMovieExtendModels;
using Standard.Contracts.Models.Movie;
using Standard.Contracts.Models.Movie.ExtendClasses;

namespace Movie.DataManagement.Converter
{
    internal class MongoToInternal
    {
        //MONGO TO INTERNAL
        public InternalMovie Movie(MongoMovie mongoMovie)
        {
            return new InternalMovie
            {
                Title = mongoMovie.Title,
                Adult = mongoMovie.Adult,
                BackdropPath = mongoMovie.BackdropPath,
                BelongsToCollection = Collection(mongoMovie.BelongsToCollection),
                Budget = mongoMovie.Budget,
                Genres = Genre(mongoMovie.Genres),
                Homepage = mongoMovie.Homepage,
                TmdbId = mongoMovie.TmdbId,
                ImdbId = mongoMovie.ImdbId,
                OriginalLanguage = mongoMovie.OriginalLanguage,
                OriginalTitle = mongoMovie.OriginalTitle,
                Overview = mongoMovie.Overview,
                Popularity = mongoMovie.Popularity,
                PosterPath = mongoMovie.PosterPath,
                ProductionCompanies = ProdCompanies(mongoMovie.ProductionCompanies),
                ProductionCountries = ProdCountries(mongoMovie.ProductionCountries),
                ReleaseDate = mongoMovie.ReleaseDate,
                Revenue = mongoMovie.Revenue,
                Runtime = mongoMovie.Runtime,
                SpokenLanguages = SpokenLanguages(mongoMovie.SpokenLanguages),
                Status = mongoMovie.Status,
                Tagline = mongoMovie.Tagline,
                VoteAverage = mongoMovie.VoteAverage,
                VoteCount = mongoMovie.VoteCount
            };
        }

        public InternalCollection Collection(Collection mongoCollection)
        {
            if (mongoCollection != null)
                return new InternalCollection
                {
                    BackdropPath = mongoCollection.BackdropPath,
                    Id = mongoCollection.Id,
                    Name = mongoCollection.Name,
                    PosterPath = mongoCollection.PosterPath
                };
            return null;
        }

        public List<InternalGenre> Genre(List<Genre> mongoGenres)
        {
            var genres = new List<InternalGenre>();
            foreach (var internalGenre in mongoGenres)
                genres.Add(new InternalGenre
                {
                    Id = internalGenre.Id,
                    Name = internalGenre.Name
                });

            return genres;
        }

        public List<InternalProductionCompany> ProdCompanies(List<ProductionCompany> mongoProductionCompanies)
        {
            var companies = new List<InternalProductionCompany>();
            foreach (var internalProdComp in mongoProductionCompanies)
                companies.Add(new InternalProductionCompany
                {
                    Id = internalProdComp.Id,
                    Name = internalProdComp.Name,
                    LogoPath = internalProdComp.LogoPath,
                    OriginCountry = internalProdComp.OriginCountry
                });

            return companies;
        }

        public List<string> ProdCountries(List<ProductionCountry> mongoProductionCountries)
        {
            var countries = new List<string>();
            foreach (var internalProdCountry in mongoProductionCountries)
                //countries.Add(new InternalProductionCountry()
                //{
                //    //IsoNum = internalProdCountry.IsoNum,
                //    Name = internalProdCountry.Name
                //});

                countries.Add(internalProdCountry.Name);

            return countries;
        }

        public List<string> SpokenLanguages(List<Language> mongoLanguages)
        {
            var languages = new List<string>();
            foreach (var internalLang in mongoLanguages)
                //languages.Add(new InternalLanguage()
                //{
                //    //IsoNum = internalLang.IsoNum,
                //    Name = internalLang.Name
                //});

                languages.Add(internalLang.Name);

            return languages;
        }
    }
}