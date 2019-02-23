using Movie.DataManagement.Parsers.TMDB.Models;
using Standard.Contracts.Exceptions;
using Standard.Contracts.Models.Movie;
using Standard.Core.NetworkManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movie.DataManagement.Converter;

namespace Movie.DataManagement.Parsers.TMDB
{
    public class TmdbParser
    {
        //private const string _endpoint = @"https://api.themoviedb.org/3/movie/{movie_id}?api_key=<<api_key>>&language=en-US";

        // title : "Star Wars New Hope"   SIMPLE
        //https://api.themoviedb.org/3/search/movie?api_key=e9443688992dbb4fa3940ed77a0a8e1d&language=en-US&query=Star%20Wars%20New%20Hope&page=1&include_adult=false

        // ID alapú film
        //https://api.themoviedb.org/3/movie/11?api_key=e9443688992dbb4fa3940ed77a0a8e1d&language=en-US

        private const string _endpoint = "https://api.themoviedb.org";
        private const string _lang = "en-US";
        private const string _key = "e9443688992dbb4fa3940ed77a0a8e1d";

        

        public async Task<InternalMovie> GetMovieFromTmdb(string title)
        {
            try
            {
                var tmdbMovieSimple =
                    await new WebClientManager().Get<TmdbQueryMovies>(
                        $"{_endpoint}/3/search/movie?api_key={_key}&language={_lang}&query={title}&page=1&include_adult=false");

                var tmdbMovieForID = tmdbMovieSimple.Results[0];

                var tmdbMovie =
                    await new WebClientManager().Get<TmdbMovie>(
                        $"{_endpoint}/3/movie/{tmdbMovieForID.TmdbId}?api_key={_key}&language={_lang}");

                //var internalMovie = new Converter.Converter().ConvertMongoToInternalMovie(tmdbMovie);

                return new InternalMovie()
                {
                    Title = tmdbMovie.Title,
                    TmdbId = tmdbMovie.TmdbId,
                    Adult = tmdbMovie.Adult,
                    BackdropPath = tmdbMovie.BackdropPath,
                    BelongsToCollection = new MongoToInternal().Collection(tmdbMovie.BelongsToCollection),
                    Budget = tmdbMovie.Budget,
                    Genres = new MongoToInternal().Genre(tmdbMovie.Genres),
                    Homepage = tmdbMovie.Homepage,
                    ImdbId = tmdbMovie.ImdbId,
                    OriginalLanguage = tmdbMovie.OriginalLanguage,
                    OriginalTitle = tmdbMovie.OriginalTitle,
                    Overview = tmdbMovie.Overview,
                    Popularity = tmdbMovie.Popularity,
                    PosterPath = tmdbMovie.PosterPath,
                    ProductionCompanies = new MongoToInternal().ProdCompanies(tmdbMovie.ProductionCompanies),
                    ProductionCountries = new MongoToInternal().ProdCountries(tmdbMovie.ProductionCountries),
                    ReleaseDate = tmdbMovie.ReleaseDate,
                    Revenue = tmdbMovie.Revenue,
                    Runtime = tmdbMovie.Runtime,
                    SpokenLanguages = new MongoToInternal().SpokenLanguages(tmdbMovie.SpokenLanguages),
                    Status = tmdbMovie.Status,
                    Tagline = tmdbMovie.Tagline,
                    VoteAverage = tmdbMovie.VoteAverage,
                    VoteCount = tmdbMovie.VoteCount
                };
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InternalException(650, "Movie not found on TMDB.");
            }
        }


        public async Task<bool> IsMovieExistInTmdb(string title)
        {
            var boolean = await new WebClientManager().Get<List<TmdbMovieSimple>>($"{_endpoint}/3/search/movie?api_key={_key}&language={_lang}&query={title}&page=1&include_adult=false");
            if (boolean.Count > 0)
            {
                foreach (var movie in boolean)
                {
                    var seriesName = RemoveAccent(movie.Title.ToLower());

                    if (seriesName == title.ToLower())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public string RemoveAccent(string text)
        {
            var decomposed = text.Normalize(NormalizationForm.FormD);

            char[] filtered = decomposed
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();
            return new String(filtered);
        }
    }
}
