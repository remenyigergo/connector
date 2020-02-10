using System.Threading.Tasks;

namespace DesktopClient.Modules.Helpers.Book
{
    internal class BookHelper
    {
        public async Task<InternalBook> BookRecommendRequest(string showName)
        {
            var books = await new WebClientManager().RecommendBooksByString(
                $"http://localhost:5002/book/get/recommendations/string", showName);
            return books[0];
        }

        public async Task<bool> IsBookModuleActivated(int userid)
        {
            return await new WebClientManager().IsBookModuleActivated(
                $"http://localhost:5000/modules/bookModule/activated", userid);
        }
    }
}