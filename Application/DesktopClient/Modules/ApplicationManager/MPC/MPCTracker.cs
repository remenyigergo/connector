using Standard.Contracts.Models.Books;
using Standard.Core.NetworkManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopClient.Modules.Helpers.Series;
using DesktopClient.Modules.Helpers.Movie;

namespace KeyEventForm.Modules.ApplicationManager.MPC
{
    class MPCTracker
    {
        private readonly string _endpoint = "http://localhost:5002";

        public async Task Run(string title)
        {
            await RecommendBookFromDb(title);
        }


        public async Task RecommendBookFromDb(string title)
        {

            var regexedTitle = String.Empty;

            if (IsItASeries(title))
            {
                regexedTitle = SeriesHelper.GetTitle(title);
            }
            else
            {
                regexedTitle = MovieHelper.GetTitle(title);
            }
            
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
