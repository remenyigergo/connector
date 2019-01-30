using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Standard.Contracts.Models.Books;
using Book.Service;
using Book.DataManagement.MongoDB.Models;
using Book.Service.Models.Request;
using Standard.Contracts.Requests;
using Standard.Contracts.Requests.Book;

namespace Book.Service.Controllers
{
    [Route("[controller]/")]
    public class BookController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // INSERTS

        [HttpPost("insert")]
        public async Task InsertBook([FromBody]InternalBook book)
        {
            var bookOverride = new InternalBook()
            {
                BookId = 4,
                Title="ninscneve",
                Writer="en",
                Genre= Book.DataManagement.OverallModels.Genres.StarWars,
                Pages=343,
                PublicationYear=2015,
                OverallRating=0,
                Sample="Ez a tortenete:semmi"
            };

            await new BookService().InsertBook(bookOverride);
        }

        [HttpPost("insert/ongoing")]
        public async Task InsertOnGoingBook([FromBody]InternalOnGoingModel model)
        {
            await new BookService().InsertOnGoingBook(model);
        }

        [HttpPost("insert/finished")]
        public async Task InsertFinishedBook([FromBody]InternalBookManagerModel model)
        {
            await new BookService().InsertFinishedBook(model.UserId,model.Book);
        }

        [HttpPost("insert/favorite")]
        public async Task InsertFavorite([FromBody]InternalBookManagerModel favoriteModel)
        {
            await new BookService().SetFavorite(favoriteModel.UserId, favoriteModel.Book);
        }

        [HttpPost("insert/queue")]
        public async Task InsertQueue([FromBody]InternalBookManagerModel queueModel)
        {
            await new BookService().PutInQueue(queueModel.UserId, queueModel.Book);
        }


        // DELETES

        [HttpDelete("delete/ongoing")]
        public async Task DeleteOnGoingBook([FromBody]InternalOnGoingModel model)
        {
            await new BookService().DeleteOnGoingBook(model);
        }

        [HttpDelete("delete/finished")]
        public async Task DeleteFinishedBook([FromBody]InternalBookManagerModel model)
        {
            await new BookService().DeleteFinishedBook(model.UserId, model.Book);
        }

        [HttpDelete("delete/favorite")]
        public async Task DeleteFavoriteBook([FromBody]InternalBookManagerModel model)
        {
            await new BookService().DeleteFavoriteBook(model.UserId, model.Book);
        }

        [HttpDelete("delete/queue")]
        public async Task DeleteQueueBook([FromBody]InternalBookManagerModel model)
        {
            await new BookService().DeleteQueueBook(model.UserId, model.Book);
        }


        // GETS

        [HttpPost("get/title")]
        public async Task<InternalBook> GetBookByTitle([FromBody]string title)
        {
            return await new BookService().GetBookByTitle(title);
        }

        [HttpPost("get/id")]
        public async Task<InternalBook> GetBookById([FromBody]int id)
        {
            return await new BookService().GetBookById(id);
        }

        [HttpPost("update/userid={userid}/bookid={bookid}")]
        public async Task<bool> UpdateReadPageNumber([FromBody]UpdateLog updateLog, int userid, int bookid)
        {
            return await new BookService().UpdateReadPageNumber(updateLog, userid, bookid);
        }


        [HttpPost("isfavorite")]
        public async Task<bool> IsItFavoriteAlready([FromBody]InternalIsBookFavoriteModel isFavoriteModel)
        {
            return await new BookService().IsItFavoriteAlready(isFavoriteModel.UserId, isFavoriteModel.BookId);
        }


        [HttpPost("get/recommendations")]
        public async Task<List<InternalBook>> GetRecommendations([FromBody] int userid)
        {
            return await new BookService().GetRecommendations(userid);
        }

        [HttpPost("get/exist/book")]
        public async Task<bool> IsBookExist([FromBody] int bookid)
        {
            return await new BookService().IsBookExist(bookid);
        }

    }
}
