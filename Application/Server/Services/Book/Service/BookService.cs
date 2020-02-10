using System.Collections.Generic;
using System.Threading.Tasks;
using Book.DataManagement.Helpers;
using Book.DataManagement.MongoDB.Models;
using Book.DataManagement.MongoDB.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Standard.Contracts.Exceptions;
using Standard.Contracts.Models.Books;
using Standard.Contracts.Requests.Book;
using Standard.Core.Dependency;

namespace Book.Service
{
    public class BookService
    {
        private readonly IBookRepository _repo = ServiceDependency.Current.GetService<IBookRepository>();

        public async Task InsertBook(InternalBook internalBook)
        {
            var book = new Converter().ConvertInternalToMongoBook(internalBook);
            await _repo.InsertBook(book);
        }

        public async Task<InternalBook> GetBookByTitle(string title)
        {
            var MongoBook = await _repo.GetBookByTitle(title);
            var InternalBook = new Converter().ConvertMongoToInternalBook(MongoBook);
            return InternalBook;
        }

        public async Task<InternalBook> GetBookById(int id)
        {
            var MongoBook = await _repo.GetBookById(id);
            var InternalBook = new Converter().ConvertMongoToInternalBook(MongoBook);
            return InternalBook;
        }

        public async Task<bool> UpdateReadPageNumber(UpdateLog updateLog, int userid, int bookid)
        {
            return await _repo.UpdateReadPageNumber(updateLog, userid, bookid);
        }

        public async Task SetFavorite(int userid, InternalBook book)
        {
            var mongoBook = new Converter().ConvertInternalToMongoBook(book);
            await _repo.SetFavorite(userid, mongoBook);
        }

        public async Task<bool> IsItFavoriteAlready(int userid, int bookid)
        {
            return await _repo.IsItFavoriteAlready(userid, bookid);
        }

        public async Task PutInQueue(int userId, InternalBook book)
        {
            await _repo.PutInQueue(userId, new Converter().ConvertInternalToMongoBook(book));
        }

        public async Task InsertOnGoingBook(InternalOnGoingModel book)
        {
            await _repo.InsertOnGoingBook(new Converter().ConvertInternalToMongoOnGoingBook(book));
        }

        public async Task InsertFinishedBook(int userid, InternalBook book)
        {
            //ha elkezdett törölni kell a felhasználótól
            await _repo.InsertFinishedBook(userid, new Converter().ConvertInternalToMongoBook(book));
        }


        public async Task DeleteOnGoingBook(InternalOnGoingModel model)
        {
            await _repo.DeleteOnGoingBook(new Converter().ConvertInternalToMongoOnGoingBook(model));
        }

        public async Task DeleteFavoriteBook(int userid, InternalBook model)
        {
            await _repo.DeleteFavoriteBook(userid, new Converter().ConvertInternalToMongoBook(model));
        }

        public async Task DeleteFinishedBook(int userid, InternalBook model)
        {
            await _repo.DeleteFinishedBook(userid, new Converter().ConvertInternalToMongoBook(model));
        }

        public async Task DeleteQueueBook(int userid, InternalBook model)
        {
            await _repo.DeleteQueueBook(userid, new Converter().ConvertInternalToMongoBook(model));
        }

        public async Task<List<InternalBook>> GetRecommendationsByUserId(int userid)
        {
            var result = await _repo.GetRecommendationsByUserId(userid);
            if (result.Count == 0 || result == null)
                throw new InternalException(615, "Recommendation failed in repository.");
            return result;
        }

        public async Task<List<InternalBook>> GetRecommendationsByString(string supposedTitle)
        {
            var returnedMongoBooks = await _repo.GetRecommendationsByString(supposedTitle);
            if (returnedMongoBooks != null)
            {
                var convertedInternalBooks = new List<InternalBook>();

                foreach (var returnedMongoBook in returnedMongoBooks)
                    convertedInternalBooks.Add(new Converter().ConvertMongoToInternalBook(returnedMongoBook));

                return convertedInternalBooks;
            }

            return null;
        }

        public async Task<bool> IsBookExist(int bookid)
        {
            return await _repo.IsBookExist(bookid);
        }
    }
}