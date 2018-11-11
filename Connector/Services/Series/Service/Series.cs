using System.Linq;
using System.Threading.Tasks;
using Contracts.Enum;
using Contracts.Exceptions;
using Contracts.Models.Series;
using Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Series.DataManagement.MongoDB.Models.Series;
using Series.DataManagement.MongoDB.Repositories;
using Series.Parsers;
using Series.Parsers.TMDB;
using Series.Parsers.TvMaze;
using Series.Service.Models;

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

        public async Task MarkAsSeen(int userid, int seriesid, int season, int episode)
        {
            await _repo.MarkAsSeen(userid, seriesid, season, episode);
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

        public async Task MarkEpisodeStarted(EpisodeStartedModel episodeStartedModel)
        {
            //var isStarted = await _repo.IsSeriesStarted(episodeStartedModel);
            var isStarted = false;
            if (!isStarted)
            {
                await _repo.MarkEpisodeStarted(episodeStartedModel);
            }
            else
            {
                throw new InternalException(604, "Series is already started by the user.");
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
    }
}