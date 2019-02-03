using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.DataManagement.Helpers;
using Book.DataManagement.MongoDB.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Standard.Contracts.Models.Books;
using Standard.Core.DataManager.MongoDB;
using Standard.Core.DataManager.MongoDB.DbModels;
using Standard.Core.DataManager.MongoDB.Extensions;
using Book = Book.DataManagement.MongoDB.Models.Book;
using MongoDB.Driver.Linq;
using System.Globalization;
using Book.DataManagement.OverallModels;
using MongoDB.Bson.Serialization;

//
//  NINCS LOGIKA   
//

namespace Book.DataManagement.MongoDB.Repositories
{
    public class BookRepository : BaseMongoDbDataManager, IBookRepository
    {
        private IMongoCollection<Models.Book> Books => Database.GetCollection<Models.Book>("Books");

        private IMongoCollection<OnGoingBook> OnGoingBooks => Database.GetCollection<OnGoingBook>("OnGoingBooks");

        private IMongoCollection<BookManagerModel> FavoriteBooks =>
            Database.GetCollection<BookManagerModel>("FavoriteBooks");

        private IMongoCollection<BookManagerModel> FinishedBooks =>
            Database.GetCollection<BookManagerModel>("FinishedBooks");

        private IMongoCollection<BookManagerModel> Queue => Database.GetCollection<BookManagerModel>("QueueBooks");

        public BookRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
        }

        //MINDENHOVA KELL HIBAKEZELÉS KB

        public async Task<Models.Book> GetBookByTitle(string title)  //enum hiba
        {
            var book = await Books.FindAsynchronous(book1 => book1.Title == title);
            return book[0];
        }

        public async Task<Models.Book> GetBookById(int id)  //enum hiba
        {
            var book = await Books.FindAsynchronous(book1 => book1.BookId == id);
            return book[0];
        }

        public async Task InsertBook(Models.Book book)
        {
            if (!await IsBookExist(book.BookId))
            {
                await Books.InsertOneAsync(book);
            }

            //TODO exception kezelés
        }

        public async Task<bool> UpdateReadPageNumber(UpdateLog updateLog, int userid, int bookid )
        {
            var timenow = DateTime.Now;
            var currentStateOfBook = await GetOnGoingBookByIds(userid, bookid);


            var updateDef = Builders<OnGoingBook>.Update
                .Set(book => book.LastUpdate.PageNumber, currentStateOfBook.LastUpdate.PageNumber + updateLog.PageNumber)
                .Set(book => book.LastUpdate.HoursRead, currentStateOfBook.LastUpdate.HoursRead + updateLog.HoursRead)
                .Set(book => book.LastUpdate.MinutesRead, currentStateOfBook.LastUpdate.MinutesRead + updateLog.MinutesRead)
                .Set(book => book.LastUpdate.UpDateTime, currentStateOfBook.LastUpdate.UpDateTime)
                .Push("Updates", new UpdateLog()
                {
                    UpDateTime = updateLog.UpDateTime,
                    PageNumber = updateLog.PageNumber,
                    MinutesRead = updateLog.MinutesRead,
                    HoursRead = updateLog.HoursRead
                });

            var s = await OnGoingBooks.UpdateOneAsync(
                book => book.BookId == bookid && book.UserId == userid, updateDef);

            return s.ModifiedCount > 0;
        }

        public async Task<OnGoingBook> GetOnGoingBookByIds(int userid, int bookid)
        {
            var book = await OnGoingBooks.FindAsynchronous(model => model.BookId == bookid && model.UserId == userid);

            if (book.Count != 0)
                return book[0];

            return null;
        }

        public async Task SetFavorite(int userId, Models.Book bookParameter)
        {
            await FavoriteBooks.InsertOneAsync(new BookManagerModel()
            {
                Book = bookParameter,
                UserId = userId
            });
        }

        public async Task<bool> IsItFavoriteAlready(int userId, int bookId)
        {
            var isItFavorite =
                await FavoriteBooks.FindAsynchronous(book => book.Book.BookId == bookId && book.UserId == userId);

            return isItFavorite.Count == 1;
        }

        public async Task PutInQueue(int userId, Models.Book bookParameter)
        {
            await Queue.InsertOneAsync(new BookManagerModel()
            {
                Book = bookParameter,
                UserId = userId
            });
        }

        public async Task InsertOnGoingBook(OnGoingBook onGoing)
        {
            await OnGoingBooks.InsertOneAsync(onGoing);
        }

        public async Task InsertFinishedBook(int userId, Models.Book bookParameter)
        {
            await FinishedBooks.InsertOneAsync(new BookManagerModel()
            {
                UserId = userId,
                Book = bookParameter
            });
        }


        public async Task DeleteOnGoingBook(OnGoingBook onGoing)
        {
            await OnGoingBooks.DeleteOneAsync(book => book.UserId == onGoing.UserId && book.BookId == onGoing.BookId);
        }

        public async Task DeleteFinishedBook(int userid, Models.Book finishedBook)
        {
            await FinishedBooks.DeleteOneAsync(model => model.UserId == userid &&
                                                        model.Book.BookId == finishedBook.BookId);
        }

        public async Task DeleteFavoriteBook(int userId, Models.Book favoriteBook)
        {
            await FavoriteBooks.DeleteOneAsync(model => model.UserId == userId &&
                                                        model.Book.BookId == favoriteBook.BookId);
        }

        public async Task DeleteQueueBook(int userId, Models.Book queueBook)
        {
            await Queue.DeleteOneAsync(model => model.UserId == userId && model.Book.BookId == queueBook.BookId);
        }

        public async Task<List<InternalBook>> GetRecommendationsByUserId(int userid)
        {
            //Helper változó
            List<Models.Book> onGoingBooks = new List<Models.Book>();


            //Folyamatban lévő könyvek összegyűjtése
            var onGoingBooksId = await OnGoingBooks.FindAsynchronous(book => book.UserId == userid);
            foreach (var onGoingBook in onGoingBooksId)
            {
                var book = await GetBookById(onGoingBook.BookId);  //hiba enummal
                onGoingBooks.Add(book);
            }

            //Elolvasott könyvek gyűjtése
            var finishedBooks = await FinishedBooks.FindAsynchronous(model => model.UserId == userid);


            //Top könyvek kiválogatása, a legtöbbet olvasott genre alapján, a két Listából összevetve
            int max = 0;
            var genres = Enum.GetValues(typeof(OverallModels.Genres));

            Dictionary<string, int> bookGenresCount = new Dictionary<string, int>();
            foreach (var genre in genres)
            {
                var countMaxOnGoing = onGoingBooks.Count(model => model.Genre == genre.ToString());
                var countMaxFinished = finishedBooks.Count(model => model.Book.Genre == genre.ToString());

                if (countMaxFinished + countMaxOnGoing != 0)
                {
                    bookGenresCount.Add(genre.ToString(), countMaxFinished + countMaxOnGoing);
                }

            }

            bookGenresCount.OrderByDescending(x => x.Value);

            //,k' könyv ajánlása a top 3 legtöbbet olvasott kategóriából véletlenszerűen. -> k>=3
            List<string> stringListGenres = new List<string>();
            foreach (var genre in bookGenresCount)
            {
                stringListGenres.Add(genre.Key);
            }

            //ajánláshoz megnézem tudunk-e 'k könyvet ajánlani'
            var allBooks = await Books.CountDocumentsAsync(x => x.BookId >= 0);
            var k = 3;
            while (allBooks - (onGoingBooksId.Count + finishedBooks.Count) < k && k > 0)
            {
                k--;
            }

            return await GetBooksForRecommendation(k, stringListGenres, stringListGenres.Count, onGoingBooksId);

        }

        public async Task<List<InternalBook>> GetBooksForRecommendation(int k, List<string> genres, int topX, List<OnGoingBook> onGoingBooks)
        {
            List<InternalBook> recommendedBooks = new List<InternalBook>();
            var booksWithSpecificGenre = await Books.FindAsynchronous(x => x.Genre == genres[0]);
            var copyBooksWithGenre = new List<Models.Book>(booksWithSpecificGenre);

            //törlöm az ongoing bookokat az osszesnek a listajabol, hogy konnyebb legyen ajanlani
            foreach (var onGoingBook in onGoingBooks)
            {
                foreach (var book in booksWithSpecificGenre)
                {
                    if (onGoingBook.BookId == book.BookId)
                    {
                        copyBooksWithGenre.Remove(book);
                    }
                }
            }

            Random rnd = new Random();



            if (recommendedBooks.Count != 1)
            {
                var usedIndexes = new List<int>();
                for (int i = 0; i < copyBooksWithGenre.Count; i++)
                {
                    var randomIndex = rnd.Next(0, copyBooksWithGenre.Count);
                    while (usedIndexes.Contains(randomIndex))
                    {
                        randomIndex = rnd.Next(0, copyBooksWithGenre.Count);
                    }
                    
                    recommendedBooks.Add(new Converter().ConvertMongoToInternalBook(copyBooksWithGenre[randomIndex]));
                    usedIndexes.Add(randomIndex);
                }
            }
            else
            {
                recommendedBooks.Add(new Converter().ConvertMongoToInternalBook(copyBooksWithGenre[0]));
                return recommendedBooks;
            }
            


            return null;
        }

        public async Task<List<Models.Book>> GetRecommendationsByString(string supposedTitle)
        {
            var booksWithThisName =
                await Books.FindAsynchronous(book => book.Title.ToLower().Contains(supposedTitle.ToLower()));

            if (booksWithThisName != null)
                return booksWithThisName;

            return null;
        }

        public async Task<bool> IsBookInOnGoing(int bookid)
        {
            var count = await OnGoingBooks.CountDocumentsAsync(book => book.BookId == bookid);
            return count == 1;
        }

        public async Task<bool> IsBookFinished(int bookid)
        {
            var count = await FinishedBooks.CountDocumentsAsync(book => book.Book.BookId == bookid);
            return count == 1;
        }

        public async Task<bool> IsBookExist(int bookid)
        {
            var isitExist = await Books.FindAsynchronous(book => book.BookId == bookid);

            return isitExist.Count == 1;
        }
    }
}