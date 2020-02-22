using System.Collections.Generic;
using Movie.DataManagement.MongoDB.Models;
using Movie.DataManagement.Parsers.OMDB.Models.OmdbMovieExtendModels;
using Movie.DataManagement.Parsers.TMDB.Models.TmdbMovieExtendModels;
using Standard.Contracts.Models.Movie;
using Standard.Contracts.Models.Movie.ExtendClasses;

namespace Movie.DataManagement.Converter
{
    internal class InternalToMongo
    {
        public MongoMovie Movie(InternalMovie internalMovie)
        {
            var collection = new Collection();
            if (internalMovie.BelongsToCollection != null)
                collection = Collection(internalMovie.BelongsToCollection);

            var genres = new List<Genre>();
            if (internalMovie.Genres != null)
                genres = Genre(internalMovie.Genres);

            var prodComps = new List<ProductionCompany>();
            if (internalMovie.ProductionCompanies != null)
                prodComps = ProdCompanies(internalMovie.ProductionCompanies);

            var prodCountries = new List<ProductionCountry>();
            if (internalMovie.ProductionCountries != null)
                prodCountries = ProdCountries(internalMovie.ProductionCountries);

            var langs = new List<Language>();
            if (internalMovie.SpokenLanguages != null)
                langs = SpokenLanguages(internalMovie.SpokenLanguages);

            var ratings = new List<Rating>();
            if (internalMovie.Ratings != null)
                ratings = Rating(internalMovie.Ratings);

            return new MongoMovie
            {
                Title = internalMovie.Title ?? "",
                Adult = internalMovie.Adult,
                BackdropPath = internalMovie.BackdropPath ?? "",
                BelongsToCollection = collection,
                Budget = internalMovie.Budget ?? 0,
                Genres = genres,
                Homepage = internalMovie.Homepage ?? "",
                TmdbId = internalMovie.TmdbId,
                ImdbId = internalMovie.ImdbId,
                OriginalLanguage = internalMovie.OriginalLanguage ?? "",
                OriginalTitle = internalMovie.OriginalTitle ?? "",
                Overview = internalMovie.Overview ?? "",
                Popularity = internalMovie.Popularity,
                PosterPath = internalMovie.PosterPath ?? "",
                ProductionCompanies = prodComps,
                ProductionCountries = prodCountries,
                ReleaseDate = internalMovie.ReleaseDate ?? "",
                Revenue = internalMovie.Revenue,
                Runtime = internalMovie.Runtime,
                SpokenLanguages = langs,
                Status = internalMovie.Status ?? "",
                Tagline = internalMovie.Tagline ?? "",
                VoteAverage = internalMovie.VoteAverage,
                VoteCount = internalMovie.VoteCount,

                //OMDB RÉSZE ÁTALAKÍTÁS
                Year = internalMovie.Year,
                Rated = internalMovie.Rated ?? "",
                Director = internalMovie.Director ?? "",
                Writer = internalMovie.Writer ?? new List<string>(),
                Actors = internalMovie.Actors ?? new List<string>(),
                Awards = internalMovie.Awards ?? new List<string>(),
                Ratings = ratings,
                Metascore = internalMovie.Metascore ?? "",
                ImdbRating = internalMovie.ImdbRating,
                ImdbVotes = internalMovie.ImdbVotes ?? "",
                Type = internalMovie.Type ?? "",
                DVD = internalMovie.DVD ?? "",
                Production = internalMovie.Production ?? "",
                Website = internalMovie.Website ?? ""
            };
        }

        public Collection Collection(InternalCollection internalCollection)
        {
            if (internalCollection != null)
                return new Collection
                {
                    BackdropPath = internalCollection.BackdropPath,
                    Id = internalCollection.Id,
                    Name = internalCollection.Name,
                    PosterPath = internalCollection.PosterPath
                };
            return null;
        }

        public List<Genre> Genre(List<InternalMovieGenre> internalGenres)
        {
            var genres = new List<Genre>();
            foreach (var internalGenre in internalGenres)
                genres.Add(new Genre
                {
                    Id = internalGenre.Id,
                    Name = internalGenre.Name
                });

            return genres;
        }

        public List<ProductionCompany> ProdCompanies(List<InternalProductionCompany> internalProductionCompanies)
        {
            var companies = new List<ProductionCompany>();
            foreach (var internalProdComp in internalProductionCompanies)
                companies.Add(new ProductionCompany
                {
                    Id = internalProdComp.Id,
                    Name = internalProdComp.Name,
                    LogoPath = internalProdComp.LogoPath,
                    OriginCountry = internalProdComp.OriginCountry
                });

            return companies;
        }

        public List<ProductionCountry> ProdCountries(List<string> internalProductionCountries)
        {
            var countries = new List<ProductionCountry>();
            foreach (var internalProdCountry in internalProductionCountries)
                countries.Add(new ProductionCountry
                {
                    //IsoNum = internalProdCountry.IsoNum,
                    Name = internalProdCountry
                });

            return countries;
        }

        public List<Language> SpokenLanguages(List<string> internalLanguages)
        {
            var languages = new List<Language>();
            foreach (var internalLang in internalLanguages)
                languages.Add(new Language
                {
                    //IsoNum = internalLang.IsoNum,
                    Name = internalLang
                });

            return languages;
        }

        public List<Rating> Rating(List<InternalRating> ratings)
        {
            var ratingList = new List<Rating>();
            foreach (var rating in ratings)
                ratingList.Add(new Rating
                {
                    Source = rating.Source,
                    Value = rating.Value
                });

            return ratingList;
        }
    }
}