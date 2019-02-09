using Standard.Contracts.Models.Books;
using Standard.Core.NetworkManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Modules.Helpers.Book
{
    class BookHelper
    {

        public async Task<InternalBook> BookRecommendRequest(string showName)
        {
            var books = await new WebClientManager().RecommendBooksByString($"http://localhost:5002/book/get/recommendations/string", showName);
            return books[0];
        }

        public async Task<bool> IsBookModuleActivated(int userid)
        {
            return await new WebClientManager().IsBookModuleActivated($"http://localhost:5000/modules/bookModule/activated",userid);
        }
    }
}
