using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.DataManagement.MongoDB.Models;
using Standard.Contracts.Requests.Movie;

namespace Movie.DataManagement.MongoDB.Repositories
{
    public interface IMovieRepository
    {
        Task Import(MongoMovie movie);

        Task<bool> CheckIfMovieExistInMongo(MongoMovie movie);
        Task<bool> IsMovieExistInMongoDb(string title);
        Task<bool> IsMovieStarted(int userid, MongoMovie mongoMovie);
        Task<MongoMovie> GetMovieByTitle(string title);
        Task<bool> UpdateStartedMovie(InternalStartedMovieUpdateRequest model);
        Task<bool> MarkAsSeenMovie(MongoMovie movie, int userid);
        Task<bool> MarkAsSeenMovie(string title, int userid);
        Task<bool> MarkAsStartedMovie(StartedMovie movie);
        Task<bool> RemoveStartedMovie(StartedMovie movie);
        Task<bool> IsMovieSeen(MongoMovie movie, int userid);
        Task<bool> RateMovie(int? tmdbid, int? imdbid, int userid, int rating);
        Task<List<StartedMovie>> StartedMoviesByUser(int userid);
        Task<List<SeenMovie>> SeenMoviesByUser(int userid);

        Task<List<MongoMovie>> GetMovies();
        Task<List<MongoMovie>> GetLastDaysSeen(int days, int userid);
        Task<List<MongoMovie>> GetLastDaysStarted(int days, int userid);
    }
}