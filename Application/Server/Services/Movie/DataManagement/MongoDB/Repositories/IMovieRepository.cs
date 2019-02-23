using Movie.DataManagement.MongoDB.Models;
using Standard.Contracts;
using Standard.Contracts.Requests.Movie;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        Task<bool> MarkAsStartedMovie(StartedMovie movie);
        Task<bool> RemoveStartedMovie(StartedMovie movie);
        Task<bool> IsMovieSeen(MongoMovie movie, int userid);
    }
}
