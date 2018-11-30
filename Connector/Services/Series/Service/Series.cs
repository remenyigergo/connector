using System.Linq;
using System.Threading.Tasks;
using Standard.Contracts.Enum;
using Standard.Contracts.Exceptions;
using Standard.Contracts.Models.Series;
using Standard.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;
using Series.DataManagement.MongoDB.Models.Series;
using Series.DataManagement.MongoDB.Repositories;
using Series.Parsers;
using Series.Parsers.TMDB;
using Series.Parsers.TvMaze;
using Series.Service.Models;
using System.Collections.Generic;
using System;
using Standard.Contracts.Models.Series.ExtendClasses;

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

            //await _repo.AddInternalSeries(tvMazeInternalSeries);

            if (tvMazeInternalSeries != null)
            {
                var tmdbInternalSeries = await new TmdbParser().ImportTmdbSeries(title);
                tvMazeInternalSeries.Merge(tmdbInternalSeries);
                tvMazeInternalSeries.Seasons = tvMazeInternalSeries.Seasons.OrderBy(x => x.SeasonNumber).ToList();
                await _repo.AddInternalSeries(tvMazeInternalSeries);
            }
            else
            {
                var tmdbSeries = await new TmdbParser().ImportTmdbSeries(title);
                await _repo.AddInternalSeries(tmdbSeries);
            }
        }

        public async Task<bool> MarkAsSeen(int userid, string tvmazeid, string tmdbid, int season, int episode, string showname)
        {
            var series = await GetSeriesByTitle(showname);
            var isitSeen = await _repo.IsItSeen(userid, series[0].TvMazeId, series[0].TmdbId, season, episode);
            if (!isitSeen)
            {
                await _repo.MarkAsSeen(userid, series[0].TvMazeId, series[0].TmdbId, season, episode);
                await _repo.DeleteStartedEpisode(series[0].TvMazeId, series[0].TmdbId, season, episode);
                return true;
            }
            return false;
        }



        public async Task AddSeriesToUser(int userid, int seriesid)
        {
            var isAdded = await _repo.IsSeriesAddedToUser(userid, seriesid);
            if (!isAdded)
            {
                await _repo.AddSeriesToUser(userid, seriesid);
            }
            else
            {
                throw new InternalException(602, "Series already added to the users list.");
            }

        }

        public async Task MarkEpisodeStarted(InternalEpisodeStartedModel episodeStartedModel, string showName)
        {
            //var isStarted = await _repo.IsSeriesStarted(episodeStartedModel);
            //var isStarted = false;
            //if (!isStarted)
            //{
            //    await _repo.MarkEpisodeStarted(episodeStartedModel);
            //}
            //else
            //{
            //    throw new InternalException(604, "Series is already started by the user.");
            //}
            var series = GetSeriesByTitle(showName);
            if (series.Result.Count != 0)
            {
                episodeStartedModel.TvMazeId = Int32.Parse(series.Result[0].TvMazeId);
                episodeStartedModel.TmdbId = Int32.Parse(series.Result[0].TmdbId);
            }


            await _repo.MarkEpisodeStarted(episodeStartedModel);

        }

        public async Task UpdateSeries(string title)
        {
            var tvMazeSeries = await new TvMazeParser().ImportSeriesFromTvMaze(title);
            await CheckSeriesUpdate(tvMazeSeries);
            await _repo.Update(tvMazeSeries);
        }

        public async Task IsSeriesImported(string title)
        {
            var isImported = await _repo.IsSeriesImported(title);
            if (isImported)
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

        public async Task<bool> GetShow(EpisodeStarted episodeStarted, string title)
        {
            return await _repo.GetShow(episodeStarted, title);
        }
        public async Task<InternalSeries> GetSeries(string title)
        {
            return await _repo.GetSeries(title);
        }

        public async Task<bool> IsShowExistInMongoDb(string title)
        {
            return await _repo.IsShowExistInMongoDb(title);
        }

        public async Task<bool> IsShowExistInTvMaze(string title)
        {
            return await new TvMazeParser().IsShowExistInTvMaze(title);
        }

        public async Task<bool> IsShowExistInTmdb(string title)
        {
            return await new TmdbParser().IsShowExistInTmdb(title);
        }

        public async Task<List<MongoSeries>> GetSeriesByTitle(string title)
        {
            return await _repo.GetSeriesByTitle(title);
        }

        public async Task<bool> IsEpisodeStarted(InternalEpisodeStartedModel internalEpisode)
        {
            var isItStarted = await _repo.IsEpisodeStarted(internalEpisode);
            return isItStarted;
        }

        public async Task<bool> UpdateStartedEpisode(InternalEpisodeStartedModel internalEpisode, string showName)
        {
            if (internalEpisode.WatchedPercentage < 98)
            {
                var showExist = await IsShowExistInMongoDb(showName);

                if (showExist)
                {
                    //check if episode is started 
                    var show = GetSeriesByTitle(showName);
                    internalEpisode.TmdbId = Int32.Parse(show.Result[0].TmdbId);
                    internalEpisode.TvMazeId = Int32.Parse(show.Result[0].TvMazeId);
                    var isEpisodeStarted = await IsEpisodeStarted(internalEpisode);

                    if (isEpisodeStarted)
                    {
                        return await _repo.UpdateStartedEpisode(internalEpisode, showName);
                    }

                    //hozzáadjuk mint markepisode started
                    await MarkEpisodeStarted(internalEpisode, showName);
                    return true;
                }
                else
                {
                    //import sorozat
                    await ImportSeries(showName);
                    await MarkEpisodeStarted(internalEpisode, showName);
                    return true;
                }
            }
            else
            {
                if (internalEpisode.TvMazeId != 0 && internalEpisode.TmdbId != 0)
                {
                    await MarkAsSeen(1, internalEpisode.TvMazeId.ToString(), internalEpisode.TmdbId.ToString(), internalEpisode.SeasonNumber, internalEpisode.EpisodeNumber, showName);
                }
                return false;
            }

        }


        public async Task RateEpisode(int userid, int? tvmazeid, int? tmdbid, int episode, int season, int rate)
        {
            await _repo.RateEpisode(userid, tvmazeid, tmdbid, episode, season, rate);
        }

        public async Task<List<InternalStartedAndSeenEpisodes>> GetLastDays(int days)
        {
            await _repo.GetLastDaysEpisodes(days);
            return null;
        }
    }
}