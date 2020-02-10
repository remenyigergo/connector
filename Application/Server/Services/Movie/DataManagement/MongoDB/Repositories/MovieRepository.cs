using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Movie.DataManagement.MongoDB.Models;
using Standard.Contracts.Requests.Movie;
using Standard.Core.DataManager.MongoDB;
using Standard.Core.DataManager.MongoDB.DbModels;
using Standard.Core.DataManager.MongoDB.Extensions;

namespace Movie.DataManagement.MongoDB.Repositories
{
    public class MovieRepository : BaseMongoDbDataManager, IMovieRepository
    {
        public MovieRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
        }

        public IMongoCollection<MongoMovie> Movies => Database.GetCollection<MongoMovie>("Movies");

        public IMongoCollection<StartedMovie> StartedMovies => Database.GetCollection<StartedMovie>("StartedMovies");
        public IMongoCollection<SeenMovie> SeenMovies => Database.GetCollection<SeenMovie>("SeenMovies");
        public IMongoCollection<Rate> Rates => Database.GetCollection<Rate>("Rates");

        public async Task Import(MongoMovie mongoMovie)
        {
            await Movies.InsertOneAsync(mongoMovie);
        }

        public async Task<bool> CheckIfMovieExistInMongo(MongoMovie mongoMovie)
        {
            var res = await Movies.FindAsynchronous(movie => movie.Title == mongoMovie.Title);
            if (res.Count > 0)
                return true;

            return false;
        }

        public async Task<bool> IsMovieExistInMongoDb(string title)
        {
            var exist = await Movies.FindAsynchronous(x => x.Title.ToLower() == title.ToLower());
            return exist.Count > 0;
        }

        public async Task<bool> IsMovieStarted(int userid, MongoMovie mongoMovie)
        {
            var isItStarted =
                await StartedMovies.CountDocumentsAsync(
                    movie => movie.ImdbId == mongoMovie.ImdbId || movie.TmdbId == mongoMovie.TmdbId);

            return isItStarted > 0;
        }

        public async Task<MongoMovie> GetMovieByTitle(string title)
        {
            var movies = await Movies.FindAsynchronous(movie => movie.Title == title);
            if (movies.Count != 0)
                return movies[0];
            return null;
        }

        public async Task<bool> UpdateStartedMovie(InternalStartedMovieUpdateRequest model)
        {
            var updateDef = Builders<StartedMovie>.Update
                .Set(o => o.HoursElapsed, model.HoursElapsed)
                .Set(o => o.MinutesElapsed, model.MinutesElapsed)
                .Set(o => o.SecondsElapsed, model.SecondsElapsed)
                .Set(o => o.WatchedPercentage, model.WatchedPercentage)
                .Set(o => o.Date, model.Date);

            var s = await StartedMovies.UpdateOneAsync(
                started => started.ImdbId == model.ImdbId ||
                           started.TmdbId == model.TmdbId && started.UserId == model.UserId, updateDef);

            return s.ModifiedCount == 1;
        }

        public async Task<bool> MarkAsSeenMovie(MongoMovie mongoMovie, int userid)
        {
            await SeenMovies.InsertOneAsync(new SeenMovie
            {
                UserId = userid,
                ImdbId = mongoMovie.ImdbId,
                TmdbId = mongoMovie.TmdbId,
                Date = DateTime.Now
            });

            return true;
        }

        public async Task<bool> MarkAsSeenMovie(string title, int userid)
        {
            var movie = await GetMovieByTitle(title);

            var seenMovieModel = new SeenMovie
            {
                ImdbId = movie.ImdbId,
                TmdbId = movie.TmdbId,
                Date = DateTime.Now,
                UserId = userid
            };

            await SeenMovies.InsertOneAsync(seenMovieModel);

            return true;
        }

        public async Task<bool> MarkAsStartedMovie(StartedMovie movie)
        {
            await StartedMovies.InsertOneAsync(new StartedMovie
            {
                Date = movie.Date,
                HoursElapsed = movie.HoursElapsed,
                ImdbId = movie.ImdbId,
                MinutesElapsed = movie.MinutesElapsed,
                SecondsElapsed = movie.SecondsElapsed,
                TmdbId = movie.TmdbId,
                UserId = movie.UserId,
                WatchedPercentage = movie.WatchedPercentage
            });

            return true;
        }

        public async Task<bool> RemoveStartedMovie(StartedMovie startedMovie)
        {
            var deleteMovie =
                await StartedMovies.DeleteOneAsync(movie => (movie.TmdbId == startedMovie.TmdbId ||
                                                             movie.ImdbId == startedMovie.ImdbId) &&
                                                            movie.UserId == startedMovie.UserId);
            return deleteMovie.DeletedCount > 0;
        }

        public async Task<bool> IsMovieSeen(MongoMovie mongoMovie, int userid)
        {
            var result = await SeenMovies.FindAsynchronous(
                movie => (movie.TmdbId == mongoMovie.TmdbId ||
                          movie.ImdbId == mongoMovie.ImdbId) && movie.UserId == userid);

            return result.Count == 1;
        }

        public async Task<bool> RateMovie(int? tmdbid, int? imdbid, int userid, int rating)
        {
            var updateDef = Builders<Rate>.Update
                .Set(o => o.ImdbId, imdbid)
                .Set(o => o.TmdbId, tmdbid)
                .Set(o => o.UserId, userid)
                .Set(o => o.Rating, rating);

            var s = await Rates.UpdateOneAsync(
                rate => (rate.ImdbId == imdbid || rate.TmdbId == tmdbid) && rate.UserId == userid, updateDef);

            return s.ModifiedCount == 1;
        }

        public async Task<List<StartedMovie>> StartedMoviesByUser(int userid)
        {
            return await StartedMovies.FindAsynchronous(movie => movie.UserId == userid);
        }

        public async Task<List<SeenMovie>> SeenMoviesByUser(int userid)
        {
            return await SeenMovies.FindAsynchronous(movie => movie.UserId == userid);
        }

        public async Task<List<MongoMovie>> GetMovies()
        {
            return await Movies.FindAsynchronous(m => m.Title.Length > 0);
        }

        public async Task<List<MongoMovie>> GetLastDaysSeen(int days, int userid)
        {
            //var seenMovies = await SeenMovies.FindAsynchronous(mov => mov.UserId == userid);

            //DateTime dateDaysBefore = DateTime.Now.AddDays(-days);

            //var daysInSeconds = (int)(DateTime.Now - dateDaysBefore).TotalSeconds;

            //List<SeenMovie> moviesLastDays = new List<SeenMovie>();

            //foreach (var seenMovie in seenMovies)
            //{
            //    var dateDiffInSeconds = (int)(DateTime.Now - seenMovie.Date).TotalSeconds;

            //    if (dateDiffInSeconds < daysInSeconds)
            //    {
            //        moviesLastDays.Add(seenMovie);
            //    }
            //}


            var dateDaysBefore = DateTime.Now.AddDays(-days);
            var moviesLastDays = await SeenMovies.FindAsynchronous(m => m.UserId == userid && m.Date >= dateDaysBefore);

            var mongoMoviesToReturn = new List<MongoMovie>();
            foreach (var moviesLastDay in moviesLastDays)
            {
                var movie = await Movies.FindAsynchronous(
                    m => m.TmdbId == moviesLastDay.TmdbId || m.ImdbId == moviesLastDay.ImdbId);
                mongoMoviesToReturn.Add(movie[0]);
            }


            return mongoMoviesToReturn;
        }


        public async Task<List<MongoMovie>> GetLastDaysStarted(int days, int userid)
        {
            var dateDaysBefore = DateTime.Now.AddDays(-days);
            var moviesLastDaysStarted =
                await StartedMovies.FindAsynchronous(m => m.UserId == userid && m.Date >= dateDaysBefore);

            var mongoMoviesToReturn = new List<MongoMovie>();
            foreach (var moviesLastDay in moviesLastDaysStarted)
            {
                var movie = await Movies.FindAsynchronous(
                    m => m.TmdbId == moviesLastDay.TmdbId || m.ImdbId == moviesLastDay.ImdbId);
                mongoMoviesToReturn.Add(movie[0]);
            }


            return mongoMoviesToReturn;
        }
    }
}