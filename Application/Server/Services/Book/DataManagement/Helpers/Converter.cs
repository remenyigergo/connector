using System.Collections.Generic;
using Book.DataManagement.MongoDB.Models;
using Book.Service.Models.Request;
using Standard.Contracts.Models.Books;
using Standard.Contracts.Requests.Book;

namespace Book.DataManagement.Helpers
{
    public class Converter
    {
        public MongoDB.Models.Book ConvertInternalToMongoBook(InternalBook internalBook)
        {
            //var convertedGenre = (Genres)Enum.Parse(typeof(Genres), genre);

            return new MongoDB.Models.Book
            {
                BookId = internalBook.BookId,
                OverallRating = internalBook.OverallRating,
                Pages = internalBook.Pages,
                PublicationYear = internalBook.PublicationYear,
                Sample = internalBook.Sample,
                Title = internalBook.Title,
                Writer = internalBook.Writer,
                Genre = internalBook.Genre
            };
        }

        public InternalBook ConvertMongoToInternalBook(MongoDB.Models.Book mongoBook)
        {
            //var convertedGenre = (Genres) Enum.Parse(typeof(Genres), genre);

            return new InternalBook
            {
                BookId = mongoBook.BookId,
                OverallRating = mongoBook.OverallRating,
                Pages = mongoBook.Pages,
                PublicationYear = mongoBook.PublicationYear,
                Sample = mongoBook.Sample,
                Title = mongoBook.Title,
                Writer = mongoBook.Writer,
                Genre = mongoBook.Genre
            };
        }

        public BookManagerModel ConvertInternalBookManagerModelToMongoBookManagerModel(
            InternalBookManagerModel internalBook)
        {
            return new BookManagerModel
            {
                UserId = internalBook.UserId,
                Book = ConvertInternalToMongoBook(internalBook.Book)
            };
        }

        public OnGoingBook ConvertInternalToMongoOnGoingBook(InternalOnGoingModel internalOnGoingModel)
        {
            var updates = new List<UpdateLog>();

            foreach (var internalUpdateLog in internalOnGoingModel.Updates)
                updates.Add(InternalToMongoLog(internalUpdateLog));

            return new OnGoingBook
            {
                BookId = internalOnGoingModel.BookId,
                UserId = internalOnGoingModel.UserId,
                LastUpdate = InternalToMongoLog(internalOnGoingModel.LastUpdate),
                Updates = updates
            };
        }

        public InternalOnGoingModel ConvertMongoToInternalOnGoingBook(OnGoingBook mongoOnGoingModel)
        {
            var updates = new List<InternalUpdateLog>();

            foreach (var mongoUpdateLog in mongoOnGoingModel.Updates)
                updates.Add(MongoToInternalLog(mongoUpdateLog));

            return new InternalOnGoingModel
            {
                BookId = mongoOnGoingModel.BookId,
                UserId = mongoOnGoingModel.UserId,
                LastUpdate = MongoToInternalLog(mongoOnGoingModel.LastUpdate),
                Updates = updates
            };
        }

        public InternalUpdateLog MongoToInternalLog(UpdateLog updateMongoLog)
        {
            return new InternalUpdateLog
            {
                UpDateTime = updateMongoLog.UpDateTime,
                MinutesRead = updateMongoLog.MinutesRead,
                PageNumber = updateMongoLog.PageNumber,
                HoursRead = updateMongoLog.HoursRead
            };
        }

        public UpdateLog InternalToMongoLog(InternalUpdateLog updateInternalLog)
        {
            return new UpdateLog
            {
                UpDateTime = updateInternalLog.UpDateTime,
                MinutesRead = updateInternalLog.MinutesRead,
                PageNumber = updateInternalLog.PageNumber,
                HoursRead = updateInternalLog.HoursRead
            };
        }
    }
}