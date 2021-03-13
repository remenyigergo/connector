using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movie.DataManagement.Parsers.OMDB.Converter;
using Movie.DataManagement.Parsers.OMDB.Models;
using Movie.DataManagement.Parsers.TMDB.Models;
using Standard.Contracts.Exceptions;
using Standard.Contracts.Models.Movie;
using Standard.Core.NetworkManager;

namespace Movie.DataManagement.Parsers.OMDB
{
    public class OmdbParser
    {
        //példahívás IMDB id-vel
        //http://www.omdbapi.com/?i=tt3896198&apikey=4eb286e7
        //
        //példahívás Title-el
        //http://www.omdbapi.com/?t=blade+runner
        //
        //Ez egy ?s (search)-el végezve, de szinte semmi infot nemdob vissza
        //http://www.omdbapi.com/?s=Star%20wars%20empire%20strikes%20back&apikey=4eb286e7

        public const string _endpoint = @"http://www.omdbapi.com";

        private const string _apikey = "4eb286e7";

        private readonly OmdbConverter converter = new OmdbConverter();

        public async Task<InternalMovie> GetMovieFromOmdb(string title)
        {
            try
            {
                var omdbMovie = await new WebClientManager().Get<OmdbMovie>($"{_endpoint}/?t={title}&apikey={_apikey}");

                //var internalMovie = new Converter.Converter().ConvertMongoToInternalMovie(tmdbMovie);

                if (omdbMovie.Title != null)
                {
                    //Kötelező letrimmelések, hogy beférjenek a megadott típusba a cuccok
                    var runtime = int.Parse(omdbMovie.Runtime.Substring(0, omdbMovie.Runtime.Length - 3));
                    var splitGenres = converter.SplitGenre(omdbMovie.Genre);
                    var actors = converter.SplitString(omdbMovie.Actors);
                    var awards = converter.SplitString(omdbMovie.Awards);
                    var languages = converter.SplitString(omdbMovie.Languages);
                    var country = converter.SplitString(omdbMovie.Country);
                    var ratings = converter.ConvertOmdbToInternalRatings(omdbMovie.Ratings);
                    var writers = converter.SplitString(omdbMovie.Writer);

                    return new InternalMovie
                    {
                        ImdbId = omdbMovie.ImdbId,
                        Title = omdbMovie.Title,
                        Year = int.Parse(omdbMovie.Year),
                        Rated = omdbMovie.Rated,
                        ReleaseDate = omdbMovie.Released,
                        Actors = actors,
                        Runtime = runtime,
                        Genres = splitGenres,
                        Director = omdbMovie.Director,
                        Writer = writers,
                        Overview = omdbMovie.Overview,
                        SpokenLanguages = languages,
                        ProductionCountries = country,
                        Awards = awards,
                        PosterPath = omdbMovie.Poster,
                        Ratings = ratings,
                        Metascore = omdbMovie.Metascore,
                        ImdbRating = double.Parse(omdbMovie.ImdbRating),
                        ImdbVotes = omdbMovie.ImdbVotes,
                        Type = omdbMovie.Type,
                        DVD = omdbMovie.Dvd,
                        Production = omdbMovie.Production,
                        Website = omdbMovie.Website
                    };
                }

                return null;
            }
            catch
            {
                throw new InternalException(650, "Movie not found on TMDB.");
            }
        }

        public async Task<bool> IsMovieExistInOmdb(string title)
        {
            var boolean =
                await new WebClientManager().Get<List<TmdbMovieSimple>>($"{_endpoint}/?t={title}&apikey={_apikey}");
            if (boolean.Count > 0)
                foreach (var movie in boolean)
                {
                    var seriesName = RemoveAccent(movie.Title.ToLower());

                    if (seriesName == title.ToLower())
                        return true;
                }

            return false;
        }

        public string RemoveAccent(string text)
        {
            var decomposed = text.Normalize(NormalizationForm.FormD);

            var filtered = decomposed
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();
            return new string(filtered);
        }
    }
}