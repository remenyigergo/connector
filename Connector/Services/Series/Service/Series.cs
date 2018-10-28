using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Enum;
using Contracts.Exceptions;
using Contracts.Models.Series;
using Core.DataManager.Mongo.IRepository;
using Core.DataManager.Mongo.Repository;
using Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Series.Parsers;
using Series.Parsers.IMDB;
using Series.Parsers.Trakt;
using Series.Parsers.TvMaze;
using Series.Parsers.TvMaze.Models;

namespace Series.Service
{
    public class Series
    {
        private readonly IParser _seriesParser;
        private readonly ISeriesRepository _repo = ServiceDependency.Current.GetService<ISeriesRepository>();

        public Series()
        {
        }

        public Series(IParser seriesParser)
        {
            _seriesParser = seriesParser;
        }
        
        public async Task ImportSeries(string title)
        {
            await IsSeriesImported(title);

            //var tvMazeSeries = await new TvMazeParser().ImportSeriesFromTvMaze(title);
            var tvMazeSeries = new InternalSeries();
            tvMazeSeries = null;

            if (tvMazeSeries != null)
            {
                await _repo.AddTvMazeSeries(tvMazeSeries);
            }
            else
            {
                var traktSeries = new TraktParser().ImportTraktSeries(title);

                var internalSeries = await new IMDBParser().ImportSeries(title);
                await _repo.AddIMDBSeries(internalSeries);
            }
        }

        public async Task UpdateSeries(string title)
        {
            
            var tvMazeSeries = await new TvMazeParser().ImportSeriesFromTvMaze(title);
            await CheckSeriesUpdate(tvMazeSeries);
            await _repo.Update(tvMazeSeries);

        }

        public async Task IsSeriesImported(string title)
        {
            var isAdded = await _repo.IsSeriesAdded(title);
            if (isAdded)
            {
                throw new InternalException((int)CoreCodes.AlreadyImported, "The series has been already imported.");
            }
        }

        public async Task CheckSeriesUpdate(InternalSeries internalSeries)
        {
            var isUpToDate = await _repo.IsUpToDate(internalSeries.Title, internalSeries.LastUpdated);

            if (isUpToDate)
            {
                throw new InternalException((int)CoreCodes.UpToDate, "The series is up to date.");
            }
        }

    }
}