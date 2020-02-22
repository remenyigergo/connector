using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Series.Parsers.IMDB.Models;
using Standard.Contracts.Models.Series;
using Standard.Core.Configuration;
using Standard.Core.NetworkManager;

namespace Series.Parsers.IMDB
{
    internal class IMDBParser : IParser
    {
        private readonly IServiceConfiguration _configuration;

        public IMDBParser(IServiceConfiguration config)
        {
            _configuration = config;
        }

        //http://www.omdbapi.com/?t=dexter?&apikey=4eb286e7

        public async Task<InternalSeries> ImportSeries(string title)
        {
            var IMDBSeries = await new WebClientManager().Get<IMDBSeries>($"{_configuration.Endpoints.Imdb}/?t={title}?&apikey=4eb286e7");


            return new Standard.Contracts.Models.Series.InternalSeries
            {
                TvMazeId = IMDBSeries.ImdbId,
                Runtime = IMDBSeries.Runtime,
                Description = IMDBSeries.Plot,
                Rating = IMDBSeries.ImdbRating,
                Title = IMDBSeries.Title,
                Year = IMDBSeries.Year,
                Categories = CategoryStringToList(IMDBSeries.Genre),
                TotalSeasons = IMDBSeries.TotalSeasons
            };
        }

        public List<string> CategoryStringToList(string category)
        {
            return category.Split(',').ToList();
        }
    }
}