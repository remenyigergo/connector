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

            var tvMazeInternalSeries = await new TvMazeParser().ImportSeriesFromTvMaze(title);
            //var tvMazeSeries = new InternalSeries();
            //tvMazeSeries = null;

            if (tvMazeInternalSeries != null)
            {
                var tmdbInternalSeries = await new TmdbParser().ImportTmdbSeries(title);
                //FELTÖLTÖM AZ ÜRES PROPERTYKET
                //TODO: GENRE/CATEGORY DUPLICATE TÖRLÉSE
                tvMazeInternalSeries.Created_by = tmdbInternalSeries.Created_by;
                tvMazeInternalSeries.Episode_run_time = tmdbInternalSeries.Episode_run_time;
                tvMazeInternalSeries.First_air_date = tmdbInternalSeries.First_air_date;
                tvMazeInternalSeries.Created_by = tmdbInternalSeries.Created_by;
                tvMazeInternalSeries.Genres = tmdbInternalSeries.Genres;
                tvMazeInternalSeries.LastEpisodeSimpleToAir = tmdbInternalSeries.LastEpisodeSimpleToAir;
                tvMazeInternalSeries.Created_by = tmdbInternalSeries.Created_by;
                tvMazeInternalSeries.Networks = tmdbInternalSeries.Networks;
                tvMazeInternalSeries.Original_language = tmdbInternalSeries.Original_language;
                tvMazeInternalSeries.Popularity = tmdbInternalSeries.Popularity;
                tvMazeInternalSeries.Production_companies = tmdbInternalSeries.Production_companies;
                tvMazeInternalSeries.Rating = tmdbInternalSeries.Rating;
                tvMazeInternalSeries.Vote_count = tmdbInternalSeries.Vote_count;
                tvMazeInternalSeries.Status = tmdbInternalSeries.Status;
                tvMazeInternalSeries.Year = tmdbInternalSeries.Year;
                tvMazeInternalSeries.Type = tmdbInternalSeries.Type;
                tvMazeInternalSeries.Original_language = tmdbInternalSeries.Original_language;

                await _repo.AddInternalSeries(tvMazeInternalSeries);
            }
            else
            {
                var traktSeries = await new TmdbParser().ImportTmdbSeries(title);

                //var internalSeries = await new IMDBParser().ImportSeries(title); EZ MÁR NEM FOG KELLENI, IMDB(OMDB) KUKA
                await _repo.AddIMDBSeries(traktSeries);
                //TODO: MONGO ADD TRAKT SERIES
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
                throw new InternalException((int) CoreCodes.AlreadyImported, "The series has been already imported.");
            }
        }

        public async Task CheckSeriesUpdate(InternalSeries internalSeries)
        {
            var isUpToDate = await _repo.IsUpToDate(internalSeries.Title, internalSeries.LastUpdated);

            if (isUpToDate)
            {
                throw new InternalException((int) CoreCodes.UpToDate, "The series is up to date.");
            }
        }
    }
}