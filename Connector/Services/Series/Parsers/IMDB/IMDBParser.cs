﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.NetworkManager;
using Series.Parsers.IMDB.Models;

namespace Series.Parsers.IMDB
{
    class IMDBParser
    {
        private const string _endpoint = "http://www.omdbapi.com";
        //http://www.omdbapi.com/?t=dexter?&apikey=4eb286e7

        public async Task<Contracts.Models.Series.InternalSeries> ImportSeries(string title)
        {
            var IMDBSeries = await new WebClientManager().Get<IMDBSeries>($"{_endpoint}/?t={title}?&apikey=4eb286e7");
            
            
            return new Contracts.Models.Series.InternalSeries()
            {
                SeriesId = IMDBSeries.ImdbId,
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