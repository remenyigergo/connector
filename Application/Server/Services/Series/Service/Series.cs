using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Series.DataManagement.Converters;
using Series.DataManagement.MongoDB.Models.Series;
using Series.DataManagement.MongoDB.Repositories;
using Series.Parsers.TMDB;
using Series.Parsers.TvMaze;
using Series.Service.Models;
using Standard.Contracts.Enum;
using Standard.Contracts.Exceptions;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Contracts.Requests;
using Standard.Core.NetworkManager;
using Microsoft.Extensions.DependencyInjection;
using Standard.Core.Dependency;
namespace Series.Service
{
    public class Series : ISeries
    {
        //private readonly ISeriesRepository _repo = ServiceDependency.Current.GetService<SeriesRepository>();
        private readonly ISeriesRepository _seriesRepository;

        public Series(ISeriesRepository seriesRepository)
        {
            _seriesRepository = seriesRepository;
        }

        public ISeriesRepository SeriesRepository { get; }

        public async Task ImportSeries(string title)
        {
            var isItImported = await IsSeriesImported(title);

            if (isItImported) return;

            var tvMazeInternalSeries = await new TvMazeParser().ImportSeriesFromTvMaze(title);
            if (tvMazeInternalSeries != null)
            {
                var tmdbInternalSeries = await new TmdbParser().ImportTmdbSeries(title);

                if (tmdbInternalSeries != null)
                    tvMazeInternalSeries.Merge(tmdbInternalSeries);

                tvMazeInternalSeries.Seasons = tvMazeInternalSeries.Seasons.OrderBy(x => x.SeasonNumber).ToList();

                await _seriesRepository.AddInternalSeries(tvMazeInternalSeries);
            }
            else
            {
                var tmdbSeries = await new TmdbParser().ImportTmdbSeries(title);

                if (tmdbSeries == null) return;

                await _seriesRepository.AddInternalSeries(tmdbSeries);
            }
        }

        public async Task<bool> MarkAsSeen(int userid, string tvmazeid, string tmdbid, int season, int episode,
            string showname)
        {
            var series = await GetSeriesByTitle(showname);
            var isitSeen = await _seriesRepository.IsItSeen(userid, series[0].TvMazeId, series[0].TmdbId, season, episode);
            if (!isitSeen)
            {
                await _seriesRepository.MarkAsSeen(userid, series[0].TvMazeId, series[0].TmdbId, season, episode);
                await _seriesRepository.DeleteStartedEpisode(series[0].TvMazeId, series[0].TmdbId, season, episode);
                return true;
            }
            return false;
        }


        public async Task AddSeriesToUser(int userid, int seriesid)
        {
            var isAdded = await _seriesRepository.IsSeriesAddedToUser(userid, seriesid);
            if (!isAdded)
                await _seriesRepository.AddSeriesToUser(userid, seriesid);
            else
                throw new InternalException(602, "Series already added to the users list.");
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
                episodeStartedModel.TvMazeId = int.Parse(series.Result[0].TvMazeId);
                episodeStartedModel.TmdbId = int.Parse(series.Result[0].TmdbId);
            }


            await _seriesRepository.MarkEpisodeStarted(episodeStartedModel);
        }

        public async Task UpdateSeries(string title)
        {
            var tvMazeSeries = await new TvMazeParser().ImportSeriesFromTvMaze(title);
            await CheckSeriesUpdate(tvMazeSeries);

            if (!await _seriesRepository.Update(tvMazeSeries))
                throw new InternalException(607, "Error. Series couldn't be updated.");
        }

        public async Task<bool> IsSeriesImported(string title)
        {
            var isImported = await _seriesRepository.IsSeriesImported(title);
            if (isImported)
                throw new InternalException((int) CoreCodes.AlreadyImported, "The series has been already imported.");
            return isImported;
        }

        public async Task CheckSeriesUpdate(InternalSeries internalSeries)
        {
            var isUpToDate = await _seriesRepository.IsUpToDate(internalSeries.Title, internalSeries.LastUpdated);

            if (isUpToDate)
                throw new InternalException((int) CoreCodes.UpToDate, "The series is up to date.");
        }

        public async Task<bool> GetShow(EpisodeStarted episodeStarted, string title)
        {
            return await _seriesRepository.GetShow(episodeStarted, title);
        }

        public async Task<InternalSeries> GetSeries(string title)
        {
            return await _seriesRepository.GetSeries(title);
        }

        public async Task<bool> IsMediaExistInMongoDb(string title)
        {
            var result = await _seriesRepository.IsMediaExistInMongoDb(title);
            if (!result)
                throw new InternalException(605, "Not any show exists with this title. SeriesRepository exception.");
            return result;
        }

        public async Task<bool> IsMediaExistInTvMaze(string title)
        {
            return await new TvMazeParser().IsMediaExistInTvMaze(title);
        }

        public async Task<bool> IsMediaExistInTmdb(string title)
        {
            return await new TmdbParser().IsMediaExistInTmdb(title);
        }

        public async Task<List<MongoSeries>> GetSeriesByTitle(string title)
        {
            return await _seriesRepository.GetSeriesByTitle(title);
        }

        public async Task<bool> IsEpisodeStarted(InternalEpisodeStartedModel internalEpisode)
        {
            var isItStarted = await _seriesRepository.IsEpisodeStarted(internalEpisode);
            return isItStarted;
        }

        public async Task<bool> UpdateStartedEpisode(InternalEpisodeStartedModel internalEpisode, string showName)
        {
            if (internalEpisode.WatchedPercentage < 98)
            {
                var showExist = await IsMediaExistInMongoDb(showName);

                if (showExist)
                {
                    //check if episode is started 
                    var show = GetSeriesByTitle(showName);
                    internalEpisode.TmdbId = int.Parse(show.Result[0].TmdbId);
                    internalEpisode.TvMazeId = int.Parse(show.Result[0].TvMazeId);
                    var isEpisodeStarted = await IsEpisodeStarted(internalEpisode);

                    if (isEpisodeStarted)
                        return await _seriesRepository.UpdateStartedEpisode(internalEpisode);

                    //hozzáadjuk mint markepisode started
                    await MarkEpisodeStarted(internalEpisode, showName);
                    return true;
                }
                //import sorozat
                await ImportSeries(showName);
                await MarkEpisodeStarted(internalEpisode, showName);
                return true;
            }
            if (internalEpisode.TvMazeId != 0 && internalEpisode.TmdbId != 0)
                await MarkAsSeen(1, internalEpisode.TvMazeId.ToString(), internalEpisode.TmdbId.ToString(),
                    internalEpisode.SeasonNumber, internalEpisode.EpisodeNumber, showName);
            return false;
        }


        public async Task RateEpisode(int userid, int? tvmazeid, int? tmdbid, int episode, int season, int rate)
        {
            var modified = await _seriesRepository.RateEpisode(userid, tvmazeid, tmdbid, episode, season, rate);
            if (!modified)
                throw new InternalException(611, "Episode rating failed.");
        }

        public async Task<InternalStartedAndSeenEpisodes> GetLastDays(int days, int userid)
        {
            var results = await _seriesRepository.GetLastDaysEpisodes(days, userid);
            if (results == null || results.seenEpisodes.Count == 0 && results.startedEpisodes.Count == 0)
                throw new InternalException(616, "No episodes were found in DB. Repository error.");

            var res = new Converter().ConvertMongoStartedAndSeenEpisodesToInternal(results);

            return res;
        }

        public async Task<int> IsShowExist(InternalImportRequest request)
        {
            if (request != null)
            {
                if (await IsMediaExistInMongoDb(request.Title))
                    return (int) MediaExistIn.MONGO;
                request.Title = RemoveAccent(request.Title);
                var tvmazexist = await IsMediaExistInTvMaze(request.Title);
                var tmdbexist = await IsMediaExistInTmdb(request.Title);
                if (tvmazexist)
                {
                    if (tmdbexist)
                        return (int) MediaExistIn.TMDB;
                    return (int) MediaExistIn.TVMAZE;
                }
                return (int) MediaExistIn.NONE;
            }
            return (int) MediaExistIn.REQUESTERROR;
        }

        public async Task<List<InternalSeries>> RecommendSeriesFromDb(int userid)
        {
            var recommends = await _seriesRepository.RecommendSeries(userid);
            if (recommends.Count == 0)
                throw new InternalException(615, "No recommendations are available");
            return recommends;
        }

        public async Task<List<InternalSeries>> RecommendSeriesFromDbByGenre(List<string> genres, string username,
            int userid)
        {
            List<InternalSeriesGenre> genreList =
                new List<InternalSeriesGenre>();
            foreach (var genre in genres)
                genreList.Add(new InternalSeriesGenre(genre));

            var userId =
                await new WebClientManager().GetUserIdFromUsername("http://localhost:5000/users/get/" + username);
            if (userid == 0)
                throw new InternalException(618, "UserId couldn't be fetched.");

            var result = await _seriesRepository.RecommendSeries(genreList, username, userid);

            if (result == null || result.Count == 0)
                throw new InternalException(615, "Recommend failed.");

            return result;
        }

        public async Task<List<InternalEpisode>> PreviousEpisodeSeen(string showTitle, int seasonNum, int episodeNum,
            int userid)
        {
            //a látott sorozatokat és magát a sorozatot keresm ki ahol egyezik az id
            var model = await _seriesRepository.GetSeriesByStartedEpisode(showTitle, seasonNum, episodeNum, userid);


            var foundSeries = new InternalSeries();
            foreach (var mongoSeries in model.foundSeriesList)
                if (int.Parse(mongoSeries.TvMazeId) == model.startedEpisodesList.TvMazeId ||
                    int.Parse(mongoSeries.TmdbId) == model.startedEpisodesList.TmdbId)
                    foundSeries = new Converter().ConvertMongoToInternalSeries(mongoSeries);

            if (foundSeries != null)
            {
                //id-k tryparsolása külön
                if (foundSeries.TvMazeId == null)
                    foundSeries.TvMazeId = "0";
                if (foundSeries.TmdbId == null)
                    foundSeries.TmdbId = "0";

                var seenEpisodesMongo = await _seriesRepository.PreviousEpisodeSeen(seasonNum, episodeNum,
                    int.Parse(foundSeries.TvMazeId), int.Parse(foundSeries.TmdbId), userid);
                var seenEpisodesInternal = new List<InternalEpisodeSeen>();
                //Convert to Internal
                foreach (var seenEpisode in seenEpisodesMongo)
                    seenEpisodesInternal.Add(new Converter().ConvertMongoToInternalEpisode(seenEpisode));

                //ebbe fogom gyűjteni azokat az epizódszámokat amelyeket nem láttunk
                var notSeenEpisodes = new List<int>();

                //ha nem láttuk az összes előzőleges részt
                if (seenEpisodesInternal.Count != episodeNum - 1)
                {
                    int episodeCounter = foundSeries.Seasons[seasonNum].Episodes.First().EpisodeNumber;
                    //megnézzük melyiket nem láttuk
                    foreach (var seenEpisodeInternal in seenEpisodesInternal)
                    {
                        if (seenEpisodeInternal.EpisodeNumber != episodeCounter)
                        {
                            foundSeries.Seasons[seasonNum].Episodes
                                .RemoveAll(x => x.SeasonNumber == seenEpisodeInternal.SeasonNumber &&
                                                x.EpisodeNumber == seenEpisodeInternal.EpisodeNumber ||
                                                x.EpisodeNumber > episodeNum);
                            notSeenEpisodes.Add(episodeCounter);
                        }
                        episodeCounter++;
                    }

                    //a notSeenEpisodes lista intjei alapján kikeresem azokat a részeket amiket nem láttunk, a returnhoz
                    //var notseen =  await _repo.GetNotSeenEpisodes(seasonNum, notSeenEpisodes, Int32.Parse(foundSeries.TvMazeId), Int32.Parse(foundSeries.TmdbId));
                    return foundSeries.Seasons[seasonNum].Episodes;
                }
            }

            throw new InternalException(615, "No recommendation were done.");
            //return null;
        }


        public string RemoveAccent(string text)
        {
            var decomposed = text.Normalize(NormalizationForm.FormD);

            var filtered = decomposed
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();
            return new string(filtered);
        }
    }
}