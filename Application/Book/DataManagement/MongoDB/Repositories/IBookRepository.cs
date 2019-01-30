using System.Collections.Generic;
using System.Threading.Tasks;
using Book.DataManagement.MongoDB.Models;
using Standard.Contracts.Models.Books;

namespace Book.DataManagement.MongoDB.Repositories
{
    public interface IBookRepository
    {
        Task InsertBook(Models.Book book);

        Task<Models.Book> GetBookByTitle(string title);

        Task<Models.Book> GetBookById(int id);

        Task<bool> UpdateReadPageNumber(UpdateLog updateLog, int userid, int bookid);

        Task SetFavorite(int userId, Models.Book book);

        Task<bool> IsItFavoriteAlready(int userId, int bookId);

        Task PutInQueue(int userId, Models.Book book);

        Task InsertOnGoingBook(OnGoingBook onGoingBook);

        Task InsertFinishedBook(int userId, Models.Book book);

        Task DeleteQueueBook(int userId, Models.Book queueBook);

        Task DeleteFavoriteBook(int userId, Models.Book favoriteBook);

        Task DeleteFinishedBook(int userId, Models.Book finishedBook);

        Task DeleteOnGoingBook(OnGoingBook onGoingBook);

        Task<List<InternalBook>> GetRecommendations(int userid);
        Task<bool> IsBookExist(int bookid);
    }
}
