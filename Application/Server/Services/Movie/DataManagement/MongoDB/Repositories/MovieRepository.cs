using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Standard.Contracts;
using MongoDB.Driver;
using Movie.DataManagement.MongoDB.Models;
using Standard.Core.DataManager.MongoDB;
using Microsoft.Extensions.Options;
using Standard.Core.DataManager.MongoDB.DbModels;
using Standard.Core.DataManager.MongoDB.Extensions;
using Standard.Contracts.Requests.Movie;

namespace Movie.DataManagement.MongoDB.Repositories
{
    public class MovieRepository : BaseMongoDbDataManager, IMovieRepository
    {
        public IMongoCollection<MongoMovie> Movies => Database.GetCollection<MongoMovie>("Movies");

        public IMongoCollection<StartedMovie> StartedMovies => Database.GetCollection<StartedMovie>("StartedMovies");
        public IMongoCollection<SeenMovie> SeenMovies => Database.GetCollection<SeenMovie>("SeenMovies");

        public MovieRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
        }

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
            var isItStarted = await StartedMovies.CountDocumentsAsync(movie=>movie.ImdbId == mongoMovie.ImdbId || movie.TmdbId == mongoMovie.TmdbId);

            return isItStarted > 0;
        }

        public async Task<MongoMovie> GetMovieByTitle(string title)
        {
            var movies = await Movies.FindAsynchronous(movie=>movie.Title == title);
            if (movies.Count != 0)
            {
                return movies[0];
            }
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

            var s = await StartedMovies.UpdateOneAsync(started => (started.ImdbId == model.ImdbId) || (started.TmdbId == model.TmdbId) && started.UserId == model.UserId, updateDef);

            return s.ModifiedCount == 1;
        }

        public async Task<bool> MarkAsSeenMovie(MongoMovie mongoMovie, int userid)
        {
            return SeenMovies.InsertOneAsync(new SeenMovie()
            {
                UserId = userid,
                ImdbId = mongoMovie.ImdbId,
                TmdbId = mongoMovie.TmdbId,
                Date = DateTime.Now,
            }).IsCompleted;

        }

        public async Task<bool> MarkAsStartedMovie(StartedMovie movie)
        {
            await StartedMovies.InsertOneAsync(new StartedMovie()
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
                await StartedMovies.DeleteOneAsync(movie => movie.TmdbId == startedMovie.TmdbId ||
                                                          Int32.Parse(movie.ImdbId) == Int32.Parse(startedMovie.ImdbId) &&
                                                          movie.UserId == startedMovie.UserId);
            return deleteMovie.DeletedCount > 0;
        }

        public async Task<bool> IsMovieSeen(MongoMovie mongoMovie, int userid)
        {
            var result = await SeenMovies.FindAsynchronous(
                movie => movie.TmdbId == mongoMovie.TmdbId ||
                         movie.ImdbId == mongoMovie.ImdbId && movie.UserId == userid);

            return result.Count == 1;
        }
    }
}