using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NetFusion.Common.Extensions.Collections;
using Series.DataManagement.Converters;
using Series.DataManagement.MongoDB.Models.Series;
using Series.DataManagement.MongoDB.SeriesFunctionModels;
using Series.Parsers.TvMaze;
using Series.Service.Models;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataManager.MongoDB;
using Standard.Core.DataManager.MongoDB.DbModels;
using Standard.Core.DataManager.MongoDB.Extensions;
using Standard.Core.DataMapping;

/*
    IDE SEMMILYEN LOGIKA NEM KERÜL
     */
namespace Series.DataManagement.MongoDB.Repositories
{
    public class SeriesRepository : BaseMongoDbDataManager, ISeriesRepository
    {
        private readonly IDataMapper<InternalSeries, MongoSeries> _seriesMapper;
        private readonly IDataMapper<InternalEpisodeStartedModel, EpisodeStartedDao> _episodeStartedMapper;

        public SeriesRepository(IOptions<MongoDbSettings> settings,
            IDataMapper<InternalSeries, MongoSeries> seriesMapper,
            IDataMapper<InternalEpisodeStartedModel, EpisodeStartedDao> episodeStartedMapper) : base(settings)
        {
            _seriesMapper = seriesMapper;
            _episodeStartedMapper = episodeStartedMapper;
        }

        public IMongoCollection<MongoSeries> Series => 
            Database.GetCollection<MongoSeries>("Series");
        private IMongoCollection<EpisodeSeenDao> SeenEpisodes =>
            Database.GetCollection<EpisodeSeenDao>("SeenEpisodes");
        private IMongoCollection<AddedSeries> AddedSeries =>
            Database.GetCollection<AddedSeries>("AddedSeries");
        private IMongoCollection<EpisodeStartedDao> EpisodeStarted =>
            Database.GetCollection<EpisodeStartedDao>("EpisodeStarted");
        private IMongoCollection<FavoriteSeriesDao> FavoriteSeries =>
            Database.GetCollection<FavoriteSeriesDao>("FavoriteSeries");
        private IMongoCollection<FavoriteEpisode> FavoriteEpisode =>
            Database.GetCollection<FavoriteEpisode>("FavoriteEpisode");
        private IMongoCollection<SeriesComment> SeriesComments =>
            Database.GetCollection<SeriesComment>("SeriesComments");
        private IMongoCollection<EpisodeComment> EpisodeComments =>
            Database.GetCollection<EpisodeComment>("EpisodeComments");
        private IMongoCollection<SeriesRate> SeriesRates =>
            Database.GetCollection<SeriesRate>("SeriesRates");
        private IMongoCollection<EpisodeRate> EpisodeRates =>
            Database.GetCollection<EpisodeRate>("EpisodeRates");

        /// <summary>
        ///     Sorozat letárolása adatbázisba.
        /// </summary>
        /// <param name="internalSeries"></param>
        public async Task AddInternalSeries(InternalSeries internalSeries)
        {
            //TODO obsolete
            //var mongoSeries = new Converter().ConvertInternalToMongoSeries(internalSeries);

            await Series.InsertOneAsync(_seriesMapper.Map(internalSeries));
        }

        /// <summary>
        ///     Sorozat törlése ID alapján
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteSeriesById(int id)
        {
            //List<Series> seriesList = GetAllSeries().Result;
            var seriesExistCheck = Series.Find(x => x.TvMazeId == id.ToString() || x.TmdbId == id.ToString());
            if (seriesExistCheck != null)
                await Series.DeleteOneAsync(Builders<MongoSeries>.Filter.Eq("TvMazeId", id));
        }

        /// <summary>
        ///     Cím alapján sorozat lekérése.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<List<InternalSeries>> GetSeriesByTitle(string title)
        {
            var coll = Series.AsQueryable();

            //var series = coll.Where(b => b.Title == title).ToList();
            var series = await Series.FindAsynchronous(b => b.Title.ToLower() == title.ToLower());

            return series.Select(x=>_seriesMapper.Map(x)).ToList();
        }

        /// <summary>
        ///     Ellenőrzi, hogy egy sorozatot berendeltünk-e már? Megegyező névként keresve
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<bool> IsSeriesImported(string title)
        {
            var s = await Series.CountDocumentsAsync(x => x.Title.ToLower() == title.ToLower());

            return s > 0;
        }

        /// <summary>
        ///     Sorozat cseréje sorozattal, frissítésként használva.
        /// </summary>
        /// <param name="internalSeries"></param>
        /// <returns></returns>
        public async Task<bool> Update(InternalSeries internalSeries)
        {
            //TODO obsolete
            //var mongoSeries = new Converter().ConvertInternalToMongoSeries(internalSeries);

            var result = await Series.ReplaceOneAsync(x => x.Title == internalSeries.Title, _seriesMapper.Map(internalSeries));
            return result.ModifiedCount > 0;
        }

        /// <summary>
        ///     Ellenőrzés, hogy a sorozat a legfrissebb adatokat tartalmazza-e?
        /// </summary>
        /// <param name="title"></param>
        /// <param name="updateCode"></param>
        public async Task<bool> IsUpToDate(string title, string updateCode)
        {
            var series = (await Series.FindAsync(x => x.Title == title)).FirstOrDefault();
            //var a = await Series.Find(x => x.Title == showTitle).Project(x => new MongoSeries() { LastUpdated = x.LastUpdated }).ToListAsync();            
            var a = await Series.FindAndProject(x => x.Title == title, x => x.LastUpdated);

            if (a.FirstOrDefault() == updateCode)
                return true;
            return false;
        }


        /// <summary>
        ///     Látottként jelöl egy epizódot. (teljesen láttuk)
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="seasonNumber"></param>
        /// <param name="episodeNumber"></param>
        public async Task MarkAsSeen(int userid, string tvmazeid, string tmdbid, int seasonNumber, int episodeNumber)
        {
            await SeenEpisodes.InsertOneAsync(new EpisodeSeenDao
            {
                UserId = userid,
                TvMazeId = tvmazeid,
                TmdbId = tmdbid,
                EpisodeNumber = episodeNumber,
                SeasonNumber = seasonNumber
            });
        }

        /// <summary>
        ///     Ellenőrzi, hogy láttunk-e már egy adott epizódot, azaz teljesen végignéztük-e.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="season"></param>
        /// <param name="episode"></param>
        public async Task<bool> IsItSeen(int userid, string tvmazeid, string tmdbid, int season, int episode)
        {
            var s = await SeenEpisodes.CountDocumentsAsync(
                ep => (ep.TmdbId == tmdbid || ep.TvMazeId == tvmazeid) && ep.UserId == userid &&
                      ep.SeasonNumber == season && ep.EpisodeNumber == episode);
            return s > 0;
        }

        /// <summary>
        ///     Sorozatfelvétel a sorozatlistához, megadott felhasználóhoz.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="seriesid"></param>
        public async Task AddSeriesToUser(int userid, int seriesid)
        {
            await AddedSeries.InsertOneAsync(new AddedSeries
            {
                Userid = userid,
                Seriesid = seriesid
            });
        }

        /// <summary>
        ///     Ellenőrzi, hogy egy felhasználó felvette-e már az adott sorozatot.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="seriesid"></param>
        /// <returns></returns>
        public async Task<bool> IsSeriesAddedToUser(int userid, int seriesid)
        {
            var s = await AddedSeries.CountDocumentsAsync(x => x.Userid == userid && x.Seriesid == seriesid);
            return s > 0;
        }

        /// <summary>
        ///     Megjelöl egy epizódot, mint elkezdett epizód.
        /// </summary>
        public async Task MarkEpisodeStarted(InternalEpisodeStartedModel episodeStartedModel)
        {
            //TODO obsolete things
            //bool isNull = episodeStartedModel.GetType().GetProperties()
            //    .All(p => p.GetValue(episodeStartedModel) != null);

            var episodeStartedDao = _episodeStartedMapper.Map(episodeStartedModel);

            await EpisodeStarted.InsertOneAsync(episodeStartedDao);
            
            //await EpisodeStarted.InsertOneAsync(new EpisodeStartedDao
            //{
            //    Date = episodeStartedModel.Date,
            //    EpisodeNumber = episodeStartedModel.EpisodeNumber,
            //    SeasonNumber = episodeStartedModel.SeasonNumber,
            //    HoursElapsed = episodeStartedModel.HoursElapsed,
            //    MinutesElapsed = episodeStartedModel.MinutesElapsed,
            //    SecondsElapsed = episodeStartedModel.SecondsElapsed,
            //    TmdbId = episodeStartedModel.TmdbId,
            //    TvMazeId = episodeStartedModel.TvMazeId,
            //    Userid = episodeStartedModel.Userid,
            //    WatchedPercentage = episodeStartedModel.WatchedPercentage
            //});

            //            if (!isNull) { return EpisodeStarted.InsertOneAsync(episodeStartedModel); } return null;
        }

        /// <summary>
        ///     Töröl egy elkezdett epizódot egy megadott felhasználótól. Visszatérés a törlés sikerességével.
        /// </summary>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="season"></param>
        /// <param name="episode"></param>
        public async Task<bool> DeleteStartedEpisode(string tvmazeid, string tmdbid, int season, int episode)
        {
            var deleteEpisode =
                await EpisodeStarted.DeleteOneAsync(ep => (ep.TvMazeId == int.Parse(tvmazeid) ||
                                                           ep.TmdbId == int.Parse(tmdbid)) &&
                                                          ep.EpisodeNumber == episode && ep.SeasonNumber == season);
            return deleteEpisode.DeletedCount > 0;
        }

        /// <summary>
        ///     Visszaadja, hogy egy epizódot elkezdtünk-e már.
        /// </summary>
        /// <param name="internalEpisodeStarted"></param>
        public async Task<bool> IsEpisodeStarted(InternalEpisodeStartedModel internalEpisodeStarted)
        {
            var episodeStartedDao = _episodeStartedMapper.Map(internalEpisodeStarted);
            var s = await EpisodeStarted.CountDocumentsAsync(
                ep => (ep.TvMazeId == episodeStartedDao.TvMazeId || ep.TmdbId == episodeStartedDao.TmdbId) &&
                      ep.SeasonNumber == episodeStartedDao.SeasonNumber &&
                      ep.EpisodeNumber == episodeStartedDao.EpisodeNumber);
            return s > 0;
        }

        /// <summary>
        ///     Nem vagyok benne biztos, hogy ez kell még.
        ///     Nem is értem mire volt ez itt. ALapból logika is szar helyen van itt kb
        /// </summary>
        /// <param name="episodeStarted"></param>
        /// <param name="showTitle"></param>
        [Obsolete]
        public async Task<bool> GetShow(InternalEpisodeStartedModel episodeStarted, string showTitle)
        {
            var show = await Series.FindAsynchronous(series => series.Title.ToLower() == showTitle.ToLower());

            if (show != null)
            {
                //var episode = await EpisodeStarted.FindAsynchronous(ep => ep.Seriesid == Int32.Parse(show[0].Id));

                var coll = EpisodeStarted.Database.GetCollection<EpisodeStartedDao>("EpisodeStarted")
                    .AsQueryable();
                var series = new List<EpisodeStartedDao>();
                if (!coll.Empty())
                    series = coll.Where(b => b.TvMazeId.ToString() == show[0].TvMazeId.ToString() ||
                                             b.TmdbId.ToString() == show[0].TmdbId.ToString()).ToList();

                if (series.Count > 0)
                {
                    var ep = _episodeStartedMapper.Map(episodeStarted);

                    await EpisodeStarted.ReplaceOneAsync(
                        s => (s.TvMazeId == ep.TvMazeId || s.TmdbId == ep.TmdbId) &&
                             s.SeasonNumber == ep.SeasonNumber &&
                             s.EpisodeNumber == ep.EpisodeNumber, ep);
                }
                else
                {
                    var episodestarted = new EpisodeStartedDao();

                    await EpisodeStarted.InsertOneAsync(episodestarted);
                }
            }
            else
            {
                await new TvMazeParser().ImportSeries(showTitle);
            }

            return false;
        }

        /// <summary>
        ///     Megnézi az adatbázisban, hogy a sorozat amit paraméterben kap, hozzá van e már adva az adatbázishoz.
        /// </summary>
        /// <param name="title"></param>
        public async Task<bool> IsMediaExistInMongoDb(string title)
        {
            var exist = await Series.FindAsynchronous(x => x.Title.ToLower() == title.ToLower());
            return exist.Count > 0;
        }

        /// <summary>
        ///     Egy elkezdett epizód frissítése. Frissített értékek: Eltelt óra, eltelt percek, eltelt másodpercek.
        /// </summary>
        /// <param name="internalEpisode"></param>
        public async Task<bool> UpdateStartedEpisode(InternalEpisodeStartedModel internalEpisode)
        {
            var episodeDao = _episodeStartedMapper.Map(internalEpisode);
            var updateDef = Builders<EpisodeStartedDao>.Update.Set(o => o.HoursElapsed, internalEpisode.HoursElapsed)
                .Set(o => o.MinutesElapsed, episodeDao.MinutesElapsed)
                .Set(o => o.SecondsElapsed, episodeDao.SecondsElapsed)
                .Set(o => o.WatchedPercentage, episodeDao.WatchedPercentage);

            var s = await EpisodeStarted.UpdateOneAsync(
                episodeStarted => episodeStarted.TvMazeId == episodeDao.TvMazeId ||
                                  episodeStarted.TmdbId == episodeDao.TmdbId, updateDef);

            return s.ModifiedCount > 0;
        }

        /// <summary>
        ///     Egy sorozat kedvencelése. TODO kiegészíteni hogy levegye, ha már kedvencelve van.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        public async Task SetFavoriteSeries(int userid, int tvmazeid, int tmdbid)
        {
            await FavoriteSeries.InsertOneAsync(new FavoriteSeriesDao
            {
                UserId = userid,
                TvMazeId = tvmazeid.ToString(),
                TmdbId = tmdbid.ToString()
            });
        }

        /// <summary>
        ///     Az összes kedvencelt sorozat lekérése, adott felhaználóhoz.
        /// </summary>
        /// <param name="userid"></param>
        public async Task<List<InternalFavoriteSeries>> GetAllFavoritesSeries(int userid)
        {
            var seriesList = await FavoriteSeries.FindAsynchronous(favoriteseries => favoriteseries.UserId == userid);
            return seriesList.Select(x=>new InternalFavoriteSeries()
            {
                TmdbId = x.TmdbId,
                TvMazeId = x.TvMazeId,
                UserId = x.UserId
            }).ToList();
        }

        /// <summary>
        ///     Ellenőrzi, hogy egy adott sorozatot kedvenceltünk-e már. Visszatérés: igaz/hamis
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        public async Task<bool> IsSeriesFavoriteAlready(int userid, int tvmazeid, int tmdbid)
        {
            var isSeriesFavorite =
                await FavoriteSeries.CountDocumentsAsync(
                    favoriteseries => favoriteseries.UserId == userid &&
                                      (favoriteseries.TvMazeId == tvmazeid.ToString() ||
                                       favoriteseries.TmdbId == tmdbid.ToString()));
            return isSeriesFavorite > 0;
        }

        /// <summary>
        ///     Egy epizód beállítása kedvencként ha még nem a kedvencünk, ellenkező esetben kedvencelés levétele az epizódról.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="episodeNum"></param>
        /// <param name="seasonNum"></param>
        public async Task SetFavoriteEpisodes(int userid, int tvmazeid, int tmdbid, int episodeNum, int seasonNum)
        {
            var isItFavoriteAlready = await IsEpisodeFavoriteAlready(userid, tvmazeid, tmdbid, episodeNum, seasonNum);
            if (isItFavoriteAlready)
                await FavoriteEpisode.DeleteOneAsync(
                    episode => (episode.TvMazeId == tvmazeid.ToString() || episode.TmdbId == tmdbid.ToString()) &&
                               episode.UserId == userid && episode.SeasonNumber == seasonNum &&
                               episode.EpisodeNumber == episodeNum);
            else
                await FavoriteEpisode.InsertOneAsync(new FavoriteEpisode
                {
                    UserId = userid,
                    TvMazeId = tvmazeid.ToString(),
                    TmdbId = tmdbid.ToString(),
                    EpisodeNumber = episodeNum,
                    SeasonNumber = seasonNum
                });
        }

        /// <summary>
        ///     Ellenőrzi, hogy egy epizódot kedvenceltünk-e már. Viszatérési érték: igaz/hamis
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="episode"></param>
        /// <param name="season"></param>
        public async Task<bool> IsEpisodeFavoriteAlready(int userid, int tvmazeid, int tmdbid, int episode, int season)
        {
            var isEpisodeFound = await FavoriteEpisode.CountDocumentsAsync(
                favoriteEpisode => favoriteEpisode.UserId == userid &&
                                   (favoriteEpisode.TvMazeId == tvmazeid.ToString() ||
                                    favoriteEpisode.TmdbId == tmdbid.ToString())
                                   && favoriteEpisode.SeasonNumber == season &&
                                   favoriteEpisode.EpisodeNumber == episode);
            return isEpisodeFound > 0;
        }

        /// <summary>
        ///     Sorozat komment hozzáadása adott felhasználótól - akármennyi lehetséges
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task CommentOnSeries(int userid, int tvmazeid, int tmdbid, string message)
        {
            await SeriesComments.InsertOneAsync(new SeriesComment
            {
                UserId = userid,
                TvMazeId = tvmazeid.ToString(),
                TmdbId = tmdbid.ToString(),
                Message = message
            });
        }

        /// <summary>
        ///     Epizódhoz való kommentelés adott felhasználótól - akármennyi lehetséges
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="episode"></param>
        /// <param name="season"></param>
        /// <param name="message"></param>
        public async Task CommentOnEpisode(int userid, int tvmazeid, int tmdbid, int episode, int season,
            string message)
        {
            await EpisodeComments.InsertOneAsync(new EpisodeComment
            {
                UserId = userid,
                TvMazeId = tvmazeid.ToString(),
                TmdbId = tmdbid.ToString(),
                SeasonNumber = season,
                EpisodeNumber = episode,
                Message = message
            });
        }

        /// <summary>
        ///     Sorozat értékelése adott felhasználótól
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="rate"></param>
        public async Task RateSeries(int userid, int tvmazeid, int tmdbid, int rate)
        {
            var updateDef = Builders<SeriesRate>.Update
                .Set(o => o.UserId, userid)
                .Set(o => o.TvMazeId, tvmazeid)
                .Set(o => o.TmdbId, tmdbid)
                .Set(o => o.Rate, rate);

            var s = await SeriesRates.UpdateOneAsync(
                seriesRate => seriesRate.UserId == userid && seriesRate.TmdbId == tmdbid &&
                              seriesRate.TvMazeId == tvmazeid, updateDef, new UpdateOptions {IsUpsert = true});
        }

        /// <summary>
        ///     Epizód értékelése adott felhasználótól
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="episode"></param>
        /// <param name="season"></param>
        /// <param name="rate"></param>
        public async Task<bool> RateEpisode(int userid, int? tvmazeid, int? tmdbid, int episode, int season, int rate)
        {
            var updateDef = Builders<EpisodeRate>.Update
                .Set(o => o.UserId, userid)
                .Set(o => o.TvMazeId, tvmazeid)
                .Set(o => o.TmdbId, tmdbid)
                .Set(o => o.SeasonNumber, season)
                .Set(o => o.EpisodeNumber, episode)
                .Set(o => o.Rate, rate);

            var s = await EpisodeRates.UpdateOneAsync(episodeRate => episodeRate.UserId == userid &&
                                                                     episodeRate.TmdbId == tmdbid &&
                                                                     episodeRate.TvMazeId == tvmazeid
                                                                     && episodeRate.SeasonNumber == season &&
                                                                     episodeRate.EpisodeNumber == episode, updateDef,
                new UpdateOptions {IsUpsert = true});

            return s.ModifiedCount == 1;
        }


        /// <summary>
        ///     Adott felhasználótól lekérhető egy megadott napon belül elkezdett vagy befejezett sorozatainak listája
        /// </summary>
        /// <param name="days"></param>
        /// <param name="userid"></param>
        public async Task<InternalStartedAndSeenEpisodes> GetLastDaysEpisodes(int days, int userid)
        {
            var startedAndSeenEpisodes = new InternalStartedAndSeenEpisodes();
            var episodeStartedList = new List<InternalEpisodeStartedModel>();
            var episodeSeenList = new List<InternalEpisodeSeen>();

            var dateDaysBefore = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - days,
                DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            var daysInSeconds = days * 24 * 60 * 60;

            var diffInSeconds = (int) (DateTime.Now - dateDaysBefore).TotalSeconds;

            var startedEpisodesListDao = await EpisodeStarted.FindAsynchronous(ep => ep.Userid == userid);
            var internalStartedEpisodes = startedEpisodesListDao.Select(x => _episodeStartedMapper.Map(x)).ToList();

            var seenEpisodesListDao = await SeenEpisodes.FindAsynchronous(ep => ep.UserId == userid);
            var internalSeenEpisodes = seenEpisodesListDao.Select(x => new InternalEpisodeSeen()
            {
                TmdbId = x.TmdbId,
                TvMazeId = x.TvMazeId,
                EpisodeNumber = x.EpisodeNumber,
                SeasonNumber = x.SeasonNumber,
                UserId = x.UserId
            }).ToList();

            foreach (var internalStartedEpisode in internalStartedEpisodes)
            {
                var episodeDateDiffInSeconds = (int) (DateTime.Now - internalStartedEpisode.Date).TotalSeconds;

                if (episodeDateDiffInSeconds < diffInSeconds)
                    episodeStartedList.Add(internalStartedEpisode);
            }

            foreach (var internalSeenEpisode in internalSeenEpisodes)
            {
                var episodeDateDiffInSeconds = (int) (DateTime.Now - internalSeenEpisode.Date).TotalSeconds;

                if (episodeDateDiffInSeconds < diffInSeconds)
                    episodeSeenList.Add(internalSeenEpisode);
            }


            startedAndSeenEpisodes.seenEpisodeList = internalSeenEpisodes;
            startedAndSeenEpisodes.startedEpisodeList = internalStartedEpisodes;

            return startedAndSeenEpisodes;
        }

        /// <summary>
        ///     Ezt az oldalon való böngészéshez
        /// </summary>
        /// <returns></returns>
        public async Task<List<InternalSeries>> RecommendSeries(
            List<InternalSeriesGenre> genre, string username, int userid)
        {
            //TODO config recommendNumber
            const int recommendNumber = 3;
            
            //TODO checK/delete copy kód
            var startedEpisodes =
                await EpisodeStarted.FindAsynchronous(episodeStarted => episodeStarted.Userid == userid);

            var startedSeriesIds = new HashSet<int[]>();
            foreach (var startedEpisode in startedEpisodes)
                startedSeriesIds.Add(new int[2] {startedEpisode.TvMazeId, startedEpisode.TmdbId});

            var startedSeries = new List<InternalSeries>();
            foreach (var startedSeriesId in startedSeriesIds)
            {
                var searchedSeries =
                    await Series.FindAsynchronous(
                        series => series.TvMazeId == startedSeriesId[0].ToString() ||
                                  series.TmdbId == startedSeriesId[1].ToString());

                var converted = new Converter().ConvertMongoToInternalSeries(searchedSeries[0]);

                startedSeries.Add(converted);
            } // copy kód

            //1. Először pontosan azokat ajánljuk, amelyikben ugyanazok a genre-k vannak meg.

            var genreParameterCount = genre.Count;

            //var allSeriesFromDb = await Series.FindAsynchronous(s => s.Id == s.Id);
            var allSeriesFromDb = await Series.FindAsynchronous(s => s.Title != "");
            var allSeries = new List<MongoSeries>(allSeriesFromDb);

            //kiválogattam azokat amiket nem követünk
            foreach (var series in allSeriesFromDb)
            foreach (var startedSeries1 in startedSeries)
                if (series.TvMazeId == startedSeries1.TvMazeId || series.TmdbId == startedSeries1.TmdbId)
                    allSeries.Remove(series);


            //megszámoljuk melyik parameter genre hányszor szerepel a db sorozatokban
            var genreCountedSeriesList = new Dictionary<InternalSeries, int>();
            foreach (var series in allSeries)
            {
                var genreMatchCount = 0;
                foreach (var seriesGenre in series.Genres)
                foreach (var internalGenre in genre)
                    if (internalGenre.Name == seriesGenre.Name)
                        genreMatchCount++;

                //beadjuk melyik sorozatnál hányszor szerepelnek a paraméterben kapott genrek
                if (genreMatchCount != 0)
                    genreCountedSeriesList.Add(new Converter().ConvertMongoToInternalSeries(series), genreMatchCount);
            }

            //descending sort by genreCount
            var recommendedDescendedSort = genreCountedSeriesList.OrderByDescending(o => o.Value).ToList();


            //1. Abból ajánlunk ahol pontosan ugyanannyi genre szerepel a db sorozatban mint a parameter genreben
            //2. Ajánlunk olyat, amikben fellelhetőek azok a genre-k amiket nézünk
            var recommendedSeries = new List<InternalSeries>();
            List<InternalSeries> exactlyMatchingList = new List<InternalSeries>();
            List<InternalSeries> notExactlyMatchingList = new List<InternalSeries>();
            foreach (var series in recommendedDescendedSort)
                //kigyűjtöm ahol megegyezik a genre countja
                if (series.Value == genreParameterCount)
                    exactlyMatchingList.Add(series.Key);
                else
                    //kigyűjtöm ahol előfordul a parameterben kapott genréböl valamelyik
                if (series.Value < genreParameterCount && series.Value > 0)
                    notExactlyMatchingList.Add(series.Key);

            while (recommendedSeries.Count < recommendNumber)
            {
                foreach (var matchingSeries in exactlyMatchingList)
                    //TODO ebből még lehet randomizálni is, ha sok van
                    //plusz még a RATING-et is figyelembe lehet venni később
                    if (recommendedSeries.Count < recommendNumber)
                        recommendedSeries.Add(matchingSeries);

                foreach (var notExactlyMatching in notExactlyMatchingList)
                    //TODO ebből is lehet randomizálni
                    if (recommendedSeries.Count < recommendNumber)
                        recommendedSeries.Add(notExactlyMatching);

                //3. Bármit ajánlunk véletlenszerűen. Ez lesz az utolsó lehetőség
                if (recommendedSeries.Count < recommendNumber && allSeries.Count >= recommendNumber + 1)
                    while (recommendedSeries.Count < recommendNumber)
                    {
                        var random = new Random();
                        var randIndex = random.Next(0, allSeries.Count);

                        if (!recommendedSeries.Contains(
                            new Converter().ConvertMongoToInternalSeries(allSeries.ElementAt(randIndex))))
                            recommendedSeries.Add(
                                new Converter().ConvertMongoToInternalSeries(allSeries.ElementAt(randIndex)));
                    }
            }


            return recommendedSeries;
        }

        public async Task<InternalReturnSeriesEpisodeModel> GetSeriesByStartedEpisode(string showName, int seasonnum,
            int episodenum, int userid)
        {
            var seriesDaoList = await Series.FindAsynchronous(show => show.Title == showName);
            var internalSeriesList = seriesDaoList.Select(x => _seriesMapper.Map(x)).ToList();

            var startedEpisode =
                await EpisodeStarted.FindAsynchronous(
                    episodeStarted => episodeStarted.Userid == userid && episodeStarted.SeasonNumber == seasonnum &&
                                      episodeStarted.EpisodeNumber == episodenum);

            return new InternalReturnSeriesEpisodeModel
            {
                foundSeriesList = internalSeriesList,
                startedEpisodesList = _episodeStartedMapper.Map(startedEpisode.FirstOrDefault())
            };
        }

        public async Task<List<InternalEpisodeSeen>> PreviousEpisodeSeen(int seasonnum, int episodenum, int tvmazeid,
            int tmbdid, int userid)
        {
            var episodeSeenDaoList = await SeenEpisodes.FindAsynchronous(
                seen => seen.SeasonNumber == seasonnum && seen.EpisodeNumber < episodenum &&
                        (seen.TvMazeId == tvmazeid.ToString() || seen.TmdbId == tmbdid.ToString()) &&
                        seen.UserId == userid);

            return episodeSeenDaoList.Select(x=>new InternalEpisodeSeen()
            {
                UserId = x.UserId,
                Date = x.Date,
                EpisodeNumber = x.EpisodeNumber,
                SeasonNumber = x.SeasonNumber,
                TmdbId = x.TmdbId,
                TvMazeId = x.TvMazeId
            }).ToList();
        }

        /// <summary>
        ///     Egy sorozat felkérése a Mongo adatbázisból.
        /// </summary>
        /// <param name="title"></param>
        public async Task<InternalSeries> GetSeries(string title)
        {
            var seriesList = await Series.FindAsynchronous(x => x.Title == title);
            var series = seriesList.FirstOrDefault();
            //TODO replace mapper
            if (series != null)
                return new Converter().ConvertMongoToInternalSeries(series);

            return null;
        }

        /// <summary>
        ///     Ezt egy befejezett sorozat után esetleg. De az oldalon is igénybevehető lesz
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<List<InternalSeries>> RecommendSeries(int userid)
        {
            List<InternalSeries> recommendedSeries = new List<InternalSeries>();
            Dictionary<InternalSeriesGenre, int> mostWatchedGenres =
                new Dictionary<InternalSeriesGenre, int>();

            var startedEpisodes =
                await EpisodeStarted.FindAsynchronous(episodeStarted => episodeStarted.Userid == userid);

            var startedSeriesIds = new HashSet<int[]>();
            foreach (var startedEpisode in startedEpisodes)
                startedSeriesIds.Add(new int[2] {startedEpisode.TvMazeId, startedEpisode.TmdbId});

            var startedSeries = new List<InternalSeries>();
            foreach (var startedSeriesId in startedSeriesIds)
            {
                var searchedSeries =
                    await Series.FindAsynchronous(
                        series => series.TvMazeId == startedSeriesId[0].ToString() ||
                                  series.TmdbId == startedSeriesId[1].ToString());

                //var converted = new Converter().ConvertMongoToInternalSeries(searchedSeries[0]);
                var converted = _seriesMapper.Map(searchedSeries.FirstOrDefault());

                if (converted == null)
                    return null;

                startedSeries.Add(converted);

                //a genre egy adott filmnél és sorozatnál több elemből áll így ezeket szeparáljuk és hozzáadjuk a legtöbbet nézetthez
                foreach (var genre in converted.Genres)
                    if (!mostWatchedGenres.ContainsKey(genre))
                        mostWatchedGenres.Add(genre, 1);
                    else
                        mostWatchedGenres[genre] += 1;
            }

            //sorrendbe vágom a legtöbbet nzéett genréket
            mostWatchedGenres.OrderBy(o => o.Value);

            //Olyan sorozatot ajánlunk, ahol a genre a mostWatchedból megegyezik, és nem szerepel még a sorozat a listánkon

            //1 kiszedem azokat a sorozatokat amelyeknek megegyezik a genre-je a listából
            var allSeries = await Series.FindAsynchronous(s => s.Title != "");
            var listOfMatchingGenresSeries = new List<InternalSeries>();
            var mostWatchedGenresCount = mostWatchedGenres.Count();
            Dictionary<InternalSeries, int> genresCountChecked = new Dictionary<InternalSeries, int>();


            //összegyűjtöm azokat a sorozatokat amelyekben a legtöbb genre van meg a mostWatchedból, de abból is csak az első 3-4ből
            var cycleMax = 0;
            if (mostWatchedGenresCount >= 4)
                cycleMax = 4;
            else
                cycleMax = mostWatchedGenresCount;

            foreach (var series in allSeries)
            {
                var genreCounter = 0;
                for (var i = 0; i < cycleMax; i++)
                    if (series.Genres.Contains(mostWatchedGenres.ElementAt(i).Key))
                        genreCounter++;

                if (!genresCountChecked.Keys.Contains(new Converter().ConvertMongoToInternalSeries(series)))
                    genresCountChecked.Add(new Converter().ConvertMongoToInternalSeries(series), genreCounter);


                //foreach (var mostWatchedGenre in mostWatchedGenres)  MŰKÖDIK
                //{
                //    if (series.Genres.Contains(mostWatchedGenre.Key))
                //    {
                //        if (!listOfMatchingGenresSeries.Contains(ConvertMongoToInternalSeries(series)))
                //        {
                //            listOfMatchingGenresSeries.Add(ConvertMongoToInternalSeries(series));
                //        }

                //        break;
                //    }
                //}
            }

            //sorba vágom a genreCountCheck listát is, hogy tudjuk mi az amiben a legtöbbször szerepelnek a legtöbbet nézett genrek
            var genresCountOrdered = genresCountChecked.OrderByDescending(o => o.Value).ToList();
            var genreCountWatchedMax = genresCountOrdered.ElementAt(0).Value;
            const int genreMatchAtLeast = 2;

            //2 ezekből randomra ajánlok egyet ami nincs még felvéve


            //addig kell újat ajánlani amíg olyat találunk, amit nem vettünk még fel.
            //ez viszont oda vezethet, hogy nincs olyan a Series listájában amit nem vettünk fel. Ezért számon tartjuk hogy miket ellenőriztem le eddig, és ha már nincs
            // olyan amit megnézhetnénk akkor nem tudunk ajánlani

            // UPDATE először kitörlöm azokat a sorozatokat a Matchelt Genres sorozatokból, amelyeket felvettem már
            //MÉG AZT MEG KELL VIZSGÁLNI HOGY HA NINCS MEG A 3 SOROZAT AKKOR NE TUDJUNK AJÁNLANI 3AT
            var listOfMatchingGenresSeriesWithoutStarted =
                new List<KeyValuePair<InternalSeries, int>>(genresCountOrdered);
            foreach (var genreCountSeries in genresCountOrdered)
            foreach (var _startedSeries in startedSeries)
                if (_startedSeries.TmdbId == genreCountSeries.Key.TmdbId ||
                    _startedSeries.TvMazeId == genreCountSeries.Key.TvMazeId)
                    listOfMatchingGenresSeriesWithoutStarted.Remove(genreCountSeries);

            return listOfMatchingGenresSeriesWithoutStarted.Take(3).Select(x => x.Key).ToList();
        }

        //public async Task<List<InternalEpisode>> GetNotSeenEpisodes(int seasonNum, List<int> notSeenEpisodeIds, int tvmazeid, int tmdbid)
        //{
        //    var notSeenEpisodes = new List<InternalEpisode>();
        //    var theSeries = await GetSeriesById(tvmazeid,tmdbid);

        //    foreach (var notSeenEpisodeId in notSeenEpisodeIds)
        //    {
        //        if (theSeries != null)
        //        {
        //            var internalEpisode = ConvertMongoToInternalEpisode(theSeries.Seasons[seasonNum].Episodes[notSeenEpisodeId]);
        //            notSeenEpisodes.Add(internalEpisode);
        //        }
        //    }

        //    return notSeenEpisodes;
        //}

        public async Task<InternalSeries> GetSeriesById(int tvmazeid, int tmdbid)
        {
            var serieslist = await Series.FindAsynchronous(
                series => series.TvMazeId == tvmazeid.ToString() || series.TmdbId == tmdbid.ToString());
            return _seriesMapper.Map(serieslist.FirstOrDefault());
        }
    }
}