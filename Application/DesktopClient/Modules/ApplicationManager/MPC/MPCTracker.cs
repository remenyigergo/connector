using System.Threading.Tasks;
using DesktopClient.Modules.Helpers.Movie;
using DesktopClient.Modules.Helpers.Series;

namespace KeyEventForm.Modules.ApplicationManager.MPC
{
    internal class MPCTracker
    {
        private readonly string _endpoint = "http://localhost:5002";

        public async Task Run(string title)
        {
            await RecommendBookFromDb(title);
        }


        public async Task RecommendBookFromDb(string title)
        {
            var regexedTitle = string.Empty;

            if (IsItASeries(title))
                regexedTitle = SeriesHelper.GetTitle(title);
            else
                regexedTitle = MovieHelper.GetTitle(title);

            //var books = await new WebClientManager().RecommendBooksByString(_endpoint + "/book/get/recommendations/" + regexedTitle);

            //MessageBox.Show("Recommended book by this:"+books[0].Title);
        }

        public async Task RecommendBookFromSites(string title)
        {
        }

        public bool IsItASeries(string title)
        {
            return SeriesHelper.DoesItContainSeasonAndEpisode(title);
        }
    }
}