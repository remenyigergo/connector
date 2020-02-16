using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Series.Parsers.TMDB;
using Series.Parsers.TvMaze;
using Standard.Contracts.Enum;
using Standard.Contracts.Exceptions;
using Standard.Core.NetworkManager;
using Series.Dto.RequestDtoModels;
using Series.DataManagement.MongoDB.Repositories;
using Series.Dto.RequestDtoModels.SeriesDto;
using Standard.Core.DataMapping;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Series.Dto.RequestDtoModels.SeriesDtos.EpisodeDtos;

namespace Series.Service
{
    public class Series : ISeries
    {
        private readonly ISeriesRepository _seriesRepository;
        private readonly IDataMapper<EpisodeStartedDto, InternalEpisodeStartedModel> _episodeStartedMapper;
        private readonly IDataMapper<SeriesDto, InternalSeries> _seriesMapper;

        public Series(ISeriesRepository seriesRepository, 
            IDataMapper<EpisodeStartedDto, InternalEpisodeStartedModel> episodeStartedMapper,
            IDataMapper<SeriesDto, InternalSeries> seriesMapper)
        {
            _seriesRepository = seriesRepository;
            _episodeStartedMapper = episodeStartedMapper;
            _seriesMapper = seriesMapper;
        }

        
        public async Task ImportSeries(string title)
        {
            var isItImported = await IsSeriesImported(title);

            if (isItImported) return;

            var tvMazeInternalSeries = await new TvMazeParser().ImportSeries(title);
            if (tvMazeInternalSeries != null)
            {
                var tmdbInternalSeries = await new TmdbParser().ImportSeries(title);

                if (tmdbInternalSeries != null)
                    tvMazeInternalSeries.Merge(tmdbInternalSeries);

                tvMazeInternalSeries.Seasons = tvMazeInternalSeries.Seasons.OrderBy(x => x.SeasonNumber).ToList();

                await _seriesRepository.AddInternalSeries(tvMazeInternalSeries);
            }
            else
            {
                var tmdbSeries = await new TmdbParser().ImportSeries(title);

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

        public async Task MarkEpisodeStarted(InternalEpisodeStartedModel internalEpisodeStartedModel, string showName)
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
                internalEpisodeStartedModel.TvMazeId = int.Parse(series.Result[0].TvMazeId);
                internalEpisodeStartedModel.TmdbId = int.Parse(series.Result[0].TmdbId);
            }

            await _seriesRepository.MarkEpisodeStarted(internalEpisodeStartedModel);
        }

        public async Task UpdateSeries(string title)
        {
            var tvMazeSeries = await new TvMazeParser().ImportSeries(title);
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

        public async Task<bool> GetShow(EpisodeStartedDto episodeStarted, string title)
        {
            return await _seriesRepository.GetShow(_episodeStartedMapper.Map(episodeStarted), title);
        }

        public async Task<SeriesDto> GetSeries(string title)
        {
            var internalSeries = await _seriesRepository.GetSeries(title);
            return _seriesMapper.Map(internalSeries);
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

        public async Task<List<SeriesDto>> GetSeriesByTitle(string title)
        {
            var internalSeries = await _seriesRepository.GetSeriesByTitle(title);
            return internalSeries.Select(x=> _seriesMapper.Map(x)).ToList();
        }

        public async Task<bool> IsEpisodeStarted(InternalEpisodeStartedModel episodeStarted)
        {
            var isItStarted = await _seriesRepository.IsEpisodeStarted(episodeStarted);
            return isItStarted;
        }

        public async Task<bool> UpdateStartedEpisode(EpisodeStartedDto episodeStarted, string showName)
        {
            var internalEpisode = _episodeStartedMapper.Map(episodeStarted);

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

        public async Task<StartedAndSeenEpisodesDto> GetLastDays(int days, int userid)
        {
            var results = await _seriesRepository.GetLastDaysEpisodes(days, userid);
            if (results == null || results.seenEpisodeList.Count == 0 && results.startedEpisodeList.Count == 0)
                throw new InternalException(616, "No episodes were found in DB. Repository error.");

            //var res = new Converter().ConvertMongoStartedAndSeenEpisodesToInternal(results);

            //TODO map InternalStartedAndSeenEpisodes to Dto
            return null;
        }

        public async Task<int> IsShowExist(ImportSeriesDto series)
        {
            if (series != null)
            {
                if (await IsMediaExistInMongoDb(series.Title))
                    return (int) MediaExistIn.MONGO;
                series.Title = RemoveAccent(series.Title);
                var tvmazexist = await IsMediaExistInTvMaze(series.Title);
                var tmdbexist = await IsMediaExistInTmdb(series.Title);
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

        public async Task<List<SeriesDto>> RecommendSeriesFromDb(int userid)
        {
            var recommends = await _seriesRepository.RecommendSeries(userid);
            if (recommends.Count == 0)
                throw new InternalException(615, "No recommendations are available");
            return recommends.Select(x=>_seriesMapper.Map(x)).ToList();
        }

        public async Task<List<SeriesDto>> RecommendSeriesFromDbByGenre(List<string> genres, string username,
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

            return result.Select(x=>_seriesMapper.Map(x)).ToList();
        }

        public async Task<List<EpisodeDto>> PreviousEpisodeSeen(string showTitle, int seasonNum, int episodeNum,
            int userid)
        {
            //a látott sorozatokat és magát a sorozatot keresem ki ahol egyezik az id
            var model = await _seriesRepository.GetSeriesByStartedEpisode(showTitle, seasonNum, episodeNum, userid);

            var foundSeries = new InternalSeries();
            foreach (var internalSeries in model.foundSeriesList)
                if (int.Parse(internalSeries.TvMazeId) == model.startedEpisodesList.TvMazeId ||
                    int.Parse(internalSeries.TmdbId) == model.startedEpisodesList.TmdbId)
                        foundSeries = internalSeries;

            if (foundSeries != null)
            {
                //id-k tryparsolása külön
                if (foundSeries.TvMazeId == null)
                    foundSeries.TvMazeId = "0";
                if (foundSeries.TmdbId == null)
                    foundSeries.TmdbId = "0";

                var seenEpisodesInternal = await _seriesRepository.PreviousEpisodeSeen(seasonNum, episodeNum,
                    int.Parse(foundSeries.TvMazeId), int.Parse(foundSeries.TmdbId), userid);

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
                    return foundSeries.Seasons[seasonNum].Episodes.Select(x=>new EpisodeDto() {
                        Title = x.Title,
                        AirDate = x.AirDate,
                        Crew = x.Crew.Select(crew=> new EpisodeCrewDto() {
                            Name = crew.Name,
                            Department = crew.Department,
                            Job = crew.Job
                        }).ToList(),
                        Description = x.Description,
                        GuestStars = x.GuestStars.Select(stars => new EpisodeGuestDto() {
                            Name = stars.Name,
                            Character = stars.Character
                        }).ToList(),
                        Length = x.Length,
                        TmdbShowId = x.TmdbShowId
                    }).ToList();
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