using Movie.DataManagement.Converter;
using Movie.DataManagement.MongoDB.Repositories;
using Standard.Contracts.Models.Movie;
using Standard.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Movie.DataManagement.Parsers.TMDB;
using Standard.Contracts.Exceptions;
using Movie.DataManagement.Parsers.OMDB;
using Movie.DataManagement.MongoDB.Models;
using Movie.DataManagement.Parsers.OMDB.Models;
using Movie.DataManagement.Parsers.TMDB.Models;
using Standard.Contracts.Requests;
using Standard.Contracts.Enum;
using System.Text;
using System;
using System.Linq;
using System.Globalization;
using Standard.Contracts.Requests.Movie;

namespace Movie.Service
{
    public class MovieService
    {
        private readonly IMovieRepository _repo = ServiceDependency.Current.GetService<IMovieRepository>();

        public MovieService()
        {
        }


        public async Task Import(InternalMovie movie)
        {
            var internalMovie = await new TmdbParser().GetMovieFromTmdb(movie.Title);
            var movieFromOmdb = await new OmdbParser().GetMovieFromOmdb(movie.Title);

            //Merge all the movies together

            if (movieFromOmdb != null && internalMovie != null && internalMovie.ImdbId == movieFromOmdb.ImdbId)
            {
                //a movieFromTmdb lesz mindenre használva
                //Ha nem tudjuk mergelni akkor csak ő lesz beimportálva, különben mind2 bennelesz
                internalMovie = Merge(internalMovie, movieFromOmdb);
            }


            var mongoMovie = new InternalToMongo().Movie(internalMovie);

            if (await _repo.CheckIfMovieExistInMongo(mongoMovie))
            {
                throw new InternalException(602, "Movie already exist in DB.");
            }

            await _repo.Import(mongoMovie);
        }

        private InternalMovie Merge(InternalMovie tmdbMovie, InternalMovie omdbMovie)
        {
            //budget
            long? budget = 0;
            if (tmdbMovie.Budget > 0)
            {
                budget = tmdbMovie.Budget;
            }
            else
            {
                budget = omdbMovie.Budget;
            }


            //popularity
            double? popularity = 0;
            if (tmdbMovie.Popularity > 0)
            {
                popularity = tmdbMovie.Popularity;
            }
            else
            {
                popularity = omdbMovie.Popularity;
            }

            //revenue
            long? revenue = 0;
            if (tmdbMovie.Revenue > 0)
            {
                revenue = tmdbMovie.Revenue;
            }
            else
            {
                revenue = omdbMovie.Revenue;
            }

            //revenue
            int? runtime = 0;
            if (tmdbMovie.Runtime > 0)
            {
                runtime = tmdbMovie.Runtime;
            }
            else
            {
                runtime = omdbMovie.Runtime;
            }

            //vote average
            double? voteavg = 0;
            if (tmdbMovie.VoteAverage > 0)
            {
                voteavg = tmdbMovie.VoteAverage;
            }
            else
            {
                voteavg = omdbMovie.VoteAverage;
            }

            //votecount
            int? voteCount = 0;
            if (tmdbMovie.VoteCount > 0)
            {
                voteCount = tmdbMovie.VoteCount;
            }
            else
            {
                voteCount = omdbMovie.VoteCount;
            }

            return new InternalMovie()
            {
                //TMDB RÉSZ
                Title = tmdbMovie.Title ?? omdbMovie.Title,
                Adult = tmdbMovie.Adult ? omdbMovie.Adult : tmdbMovie.Adult,
                BackdropPath = tmdbMovie.BackdropPath ?? omdbMovie.BackdropPath,
                BelongsToCollection = tmdbMovie.BelongsToCollection ?? omdbMovie.BelongsToCollection,
                Budget = budget,
                Genres = tmdbMovie.Genres ?? omdbMovie.Genres,
                Homepage = tmdbMovie.Homepage ?? omdbMovie.Homepage,
                TmdbId = tmdbMovie.TmdbId,
                ImdbId = tmdbMovie.ImdbId ?? omdbMovie.ImdbId,
                OriginalLanguage = tmdbMovie.OriginalLanguage ?? omdbMovie.OriginalLanguage,
                OriginalTitle = tmdbMovie.OriginalTitle ?? omdbMovie.OriginalTitle,
                Overview = tmdbMovie.Overview ?? omdbMovie.Overview,
                Popularity = popularity,
                PosterPath = tmdbMovie.PosterPath ?? omdbMovie.PosterPath,
                ProductionCompanies = tmdbMovie.ProductionCompanies ?? omdbMovie.ProductionCompanies,
                ProductionCountries = tmdbMovie.ProductionCountries ?? omdbMovie.ProductionCountries,
                ReleaseDate = tmdbMovie.ReleaseDate ?? omdbMovie.ReleaseDate,
                Revenue = revenue,
                Runtime = runtime,
                SpokenLanguages = tmdbMovie.SpokenLanguages ?? omdbMovie.SpokenLanguages,
                Status = tmdbMovie.Status ?? omdbMovie.Status,
                Tagline = tmdbMovie.Tagline ?? omdbMovie.Tagline,
                VoteAverage = voteavg,
                VoteCount = voteCount,

                //OMDB RÉSZ
                Year = omdbMovie.Year,
                Rated = omdbMovie.Rated,
                Director = omdbMovie.Director,
                Writer = omdbMovie.Writer,
                Actors = omdbMovie.Actors,
                Awards = omdbMovie.Awards,
                Ratings = omdbMovie.Ratings,
                Metascore = omdbMovie.Metascore,
                ImdbRating = omdbMovie.ImdbRating,
                ImdbVotes = omdbMovie.ImdbVotes,
                Type = omdbMovie.Type,
                DVD = omdbMovie.DVD,
                Production = omdbMovie.Production,
                Website = omdbMovie.Website
            };
        }

        public async Task<int> IsMovieExist(InternalImportRequest request)
        {
            if (request != null)
            {
                if (await IsMovieExistInMongoDb(request.Title))
                {
                    return (int)MediaExistIn.MONGO;
                }
                request.Title = RemoveAccent(request.Title);
                var tvmazexist = await IsMovieExistInTmdb(request.Title);
                var tmdbexist = await IsMovieExistInOmdb(request.Title);
                if (tvmazexist)
                {
                    if (tmdbexist)
                    {
                        return (int)MediaExistIn.TMDB;
                    }
                    return (int)MediaExistIn.TVMAZE;
                }
                return (int)MediaExistIn.NONE;
            }
            return (int)MediaExistIn.REQUESTERROR;
        }

        public async Task<bool> IsMovieExistInMongoDb(string title)
        {
            var result = await _repo.IsMovieExistInMongoDb(title);
            //if (result)
            //{
            //    throw new InternalException(600, "Movie already imported.");
            //}
            return result;
        }

        public async Task<bool> IsMovieExistInTmdb(string title)
        {
            return await new TmdbParser().IsMovieExistInTmdb(title);
        }

        public async Task<bool> IsMovieExistInOmdb(string title)
        {
            return await new OmdbParser().IsMovieExistInOmdb(title);
        }


        public string RemoveAccent(string text)
        {
            var decomposed = text.Normalize(NormalizationForm.FormD);

            char[] filtered = decomposed
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();
            return new String(filtered);
        }


        public async Task<bool> UpdateStartedMovie(InternalStartedMovieUpdateRequest requestModel)
        {
            if (requestModel != null)
            {
                var mongoMovie = await _repo.GetMovieByTitle(requestModel.Title);
                if (mongoMovie != null)
                {
                    //Megnézem, hogy láttuk -e már a filmet, mert ha igen akkor figyelmeztetem
                    var isItSeen = await _repo.IsMovieSeen(mongoMovie, requestModel.UserId);
                    if (!isItSeen)
                    {

                        //Létrehozom, hogy tudjuk jelölni mint elkezdett film és törölni is majd onnan.
                        var startedMovie = new StartedMovie()
                        {
                            Date = requestModel.Date,
                            HoursElapsed = requestModel.HoursElapsed,
                            ImdbId = requestModel.ImdbId,
                            MinutesElapsed = requestModel.MinutesElapsed,
                            SecondsElapsed = requestModel.SecondsElapsed,
                            TmdbId = requestModel.TmdbId,
                            UserId = requestModel.UserId,
                            WatchedPercentage = requestModel.WatchedPercentage
                        };


                        if (requestModel.WatchedPercentage < 98)
                        {
                            //ha létezik, updateljük, különben -**hiba**- importáljuk a sorozatot és berakjuk elkezdett filmek közé
                            if (await IsMovieExistInMongoDb(requestModel.Title))
                            {
                                //feltöltjük az üres ID-ket.
                                requestModel.TmdbId = mongoMovie.TmdbId;
                                requestModel.ImdbId = mongoMovie.ImdbId;

                                //check if movie is started 
                                bool isItStarted = await _repo.IsMovieStarted(requestModel.UserId, mongoMovie);
                                if (isItStarted)
                                {
                                    //update
                                    return await _repo.UpdateStartedMovie(requestModel);
                                }

                                return await _repo.MarkAsStartedMovie(startedMovie);
                            }
                            else
                            {
                                await Import(new InternalMovie() {Title = requestModel.Title});
                                return await _repo.UpdateStartedMovie(requestModel);
                                //throw new InternalException(650, "Movie not found in MONGODB.");
                            }
                        }
                        else
                        {
                            //kitöröljük az elkezdett filmek közül
                            await _repo.RemoveStartedMovie(startedMovie);

                            //bejelöljük látottnak
                            return await _repo.MarkAsSeenMovie(mongoMovie, requestModel.UserId);
                        }
                    }
                    else
                    {
                        throw new InternalException(606, "Movie already seen.");
                    }

                }
                else
                {
                    throw new InternalException(650, "Movie not found by title in DB.");
                }
            }

            return false;
        }


    }
}