using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Standard.Contracts.Models.Series;
using Standard.Core.DataManager.MongoDB;
using Standard.Core.DataManager.MongoDB.DbModels;
using Standard.Core.DataManager.MongoDB.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NetFusion.Common.Extensions.Collections;
using Newtonsoft.Json;
using Series.DataManagement.MongoDB.Models.Series;
using Series.DataManagement.MongoDB.SeriesFunctionModels;
using Series.Parsers.TvMaze;
using Series.Service.Models;
using Standard.Contracts.Models.Series.ExtendClasses;

/*
    IDE SEMMILYEN LOGIKA NEM KERÜL
     */
namespace Series.DataManagement.MongoDB.Repositories
{
    public class SeriesRepository : BaseMongoDbDataManager, ISeriesRepository
    {
        public IMongoCollection<MongoSeries> Series => Database.GetCollection<MongoSeries>("Series");
        private IMongoCollection<EpisodeSeen> SeenEpisodes => Database.GetCollection<EpisodeSeen>("SeenEpisodes");
        private IMongoCollection<AddedSeries> AddedSeries => Database.GetCollection<AddedSeries>("AddedSeries");

        private IMongoCollection<EpisodeStarted> EpisodeStarted =>
            Database.GetCollection<EpisodeStarted>("EpisodeStarted");

        private IMongoCollection<FavoriteSeries> FavoriteSeries =>
            Database.GetCollection<FavoriteSeries>("FavoriteSeries");

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

        public SeriesRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
        }

        /// <summary>
        /// Sorozat letárolása adatbázisba.
        /// </summary>
        /// <param name="internalSeries"></param>
        public async Task AddInternalSeries(InternalSeries internalSeries)
        {
            //Valódi mongoSeries, ami letárolásra kerül
            var mongoSeries = ConvertInternalToMongoSeries(internalSeries);

            await Series.InsertOneAsync(mongoSeries);
        }


        /// <summary>
        /// Sorozat törlése ID alapján
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteSeriesById(int id)
        {
            //List<Series> seriesList = GetAllSeries().Result;
            var seriesExistCheck = Series.Find(x => x.TvMazeId == id.ToString() || x.TmdbId == id.ToString());
            if (seriesExistCheck != null)
            {
                await Series.DeleteOneAsync(Builders<MongoSeries>.Filter.Eq("TvMazeId", id));
            }
        }

        /// <summary>
        /// Cím alapján sorozat lekérése.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<List<MongoSeries>> GetSeriesByTitle(string title)
        {
            var coll = Series.AsQueryable();

            //var series = coll.Where(b => b.Title == title).ToList();
            var series = await Series.FindAsynchronous(b => b.Title.ToLower() == title.ToLower());

            return series;
        }

        /// <summary>
        /// Ellenőrzi, hogy egy sorozatot berendeltünk-e már? Megegyező névként keresve
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<bool> IsSeriesImported(string title)
        {
            var s = await Series.CountDocumentsAsync(x => x.Title.ToLower() == title.ToLower());

            return s > 0;
        }

        /// <summary>
        /// Sorozat cseréje sorozattal, frissítésként használva.
        /// </summary>
        /// <param name="internalSeries"></param>
        /// <returns></returns>
        public async Task<bool> Update(InternalSeries internalSeries)
        {
            var mongoSeries = ConvertInternalToMongoSeries(internalSeries);
            var result = await Series.ReplaceOneAsync(x => x.Title == internalSeries.Title, mongoSeries);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Ellenőrzés, hogy a sorozat a legfrissebb adatokat tartalmazza-e?
        /// </summary>
        /// <param name="title"></param>
        /// <param name="updateCode"></param>
        public async Task<bool> IsUpToDate(string title, string updateCode)
        {
            var series = (await Series.FindAsync(x => x.Title == title)).FirstOrDefault();
            //var a = await Series.Find(x => x.Title == showTitle).Project(x => new MongoSeries() { LastUpdated = x.LastUpdated }).ToListAsync();            
            var a = await Series.FindAndProject(x => x.Title == title, x => x.LastUpdated);

            if (a.FirstOrDefault() == updateCode)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sorozat átalakítás:   Internal -> Mongo TODO kiszedni
        /// </summary>
        /// <param name="internalSeries"></param>
        /// <returns></returns>
        public MongoSeries ConvertInternalToMongoSeries(InternalSeries internalSeries)
        {
            var seasons = ConvertInternalSeasonToMongoSeason(internalSeries.Seasons);

            var mongoSeries = new MongoSeries()
            {
                TvMazeId = internalSeries.TvMazeId,
                TmdbId = internalSeries.TmdbId,
                Title = internalSeries.Title,
                //TvMazeId = internalSeries.Id,
                Runtime = internalSeries.Runtime,
                TotalSeasons = internalSeries.TotalSeasons,
                Categories = internalSeries.Categories,
                Description = internalSeries.Description,
                Rating = internalSeries.Rating,
                Year = internalSeries.Year,
                LastUpdated = internalSeries.LastUpdated,
                Seasons = seasons,
                Cast = internalSeries.Cast,
                CreatedBy = internalSeries.CreatedBy,
                EpisodeRunTime = internalSeries.EpisodeRunTime,
                FirstAirDate = internalSeries.FirstAirDate,
                Genres = internalSeries.Genres,
                OriginalLanguage = internalSeries.OriginalLanguage,
                LastEpisodeSimpleToAir = internalSeries.LastEpisodeSimpleToAir,
                Networks = internalSeries.Networks,
                Popularity = internalSeries.Popularity,
                ProductionCompanies = internalSeries.ProductionCompanies,
                Status = internalSeries.Status,
                Type = internalSeries.Type,
                VoteCount = internalSeries.VoteCount
            };
            return mongoSeries;
        }

        /// <summary>
        /// Sorozat átalakítás:   Mongo -> Internal  TODO ezt is kiszedni külön fájlba
        /// </summary>
        /// <param name="mongoSeries"></param>
        /// <returns></returns>
        public InternalSeries ConvertMongoToInternalSeries(MongoSeries mongoSeries)
        {
            //var seasons = ConvertInternalSeasonToMongoSeason(mongoSeries.Seasons);

            var internalSeries = new InternalSeries()
            {
                Title = mongoSeries.Title,
                TvMazeId = mongoSeries.Id,
                Runtime = mongoSeries.Runtime,
                TotalSeasons = mongoSeries.TotalSeasons,
                Categories = mongoSeries.Categories,
                Description = mongoSeries.Description,
                Rating = mongoSeries.Rating,
                Year = mongoSeries.Year,
                LastUpdated = mongoSeries.LastUpdated,
                //Seasons = seasons,
                Cast = mongoSeries.Cast,
                CreatedBy = mongoSeries.CreatedBy,
                EpisodeRunTime = mongoSeries.EpisodeRunTime,
                FirstAirDate = mongoSeries.FirstAirDate,
                Genres = mongoSeries.Genres,
                OriginalLanguage = mongoSeries.OriginalLanguage,
                LastEpisodeSimpleToAir = mongoSeries.LastEpisodeSimpleToAir,
                Networks = mongoSeries.Networks,
                Popularity = mongoSeries.Popularity,
                ProductionCompanies = mongoSeries.ProductionCompanies,
                Status = mongoSeries.Status,
                Type = mongoSeries.Type,
                VoteCount = mongoSeries.VoteCount
            };
            return internalSeries;
        }

        /// <summary>
        /// Évad átalakítás:  Internal -> Mongo    TODO ezt szerintem ki lehetne szedni külön fájlba
        /// </summary>
        /// <param name="internalSeasons"></param>
        public List<MongoSeason> ConvertInternalSeasonToMongoSeason(List<InternalSeason> internalSeasons)
        {
            var seasonsList = new List<MongoSeason>();
            foreach (var internalSeason in internalSeasons)
            {
                var episodeList = new List<MongoEpisode>();
                foreach (var episode in internalSeason.Episodes)
                {
                    episodeList.Add(new MongoEpisode()
                    {
                        Title = episode.Title,
                        Length = episode.Length,
                        Rating = episode.Rating,
                        Description = episode.Description,
                        SeasonNumber = episode.SeasonNumber,
                        EpisodeNumber = episode.EpisodeNumber,
                        AirDate = episode.AirDate,
                        TmdbShowId = episode.TmdbShowId,
                        VoteCount = episode.VoteCount,
                        Crew = episode.Crew,
                        GuestStars = episode.GuestStars
                    });
                }

                seasonsList.Add(new MongoSeason()
                {
                    SeasonNumber = internalSeason.SeasonNumber,
                    EpisodesCount = internalSeason.EpisodesCount,
                    Episodes = episodeList,
                    Summary = internalSeason.Summary,
                    Airdate = internalSeason.Airdate,
                    Name = internalSeason.Name
                });
            }
            return seasonsList;
        }

        /// <summary>
        /// Látottként jelöl egy epizódot. (teljesen láttuk)
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="seasonNumber"></param>
        /// <param name="episodeNumber"></param>
        public async Task MarkAsSeen(int userid, string tvmazeid, string tmdbid, int seasonNumber, int episodeNumber)
        {
            await SeenEpisodes.InsertOneAsync(new EpisodeSeen()
            {
                UserId = userid,
                TvMazeId = tvmazeid,
                TmdbId = tmdbid,
                EpisodeNumber = episodeNumber,
                SeasonNumber = seasonNumber
            });
        }

        /// <summary>
        /// Ellenőrzi, hogy láttunk-e már egy adott epizódot, azaz teljesen végignéztük-e.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="season"></param>
        /// <param name="episode"></param>
        public async Task<bool> IsItSeen(int userid, string tvmazeid, string tmdbid, int season, int episode)
        {
            var s = await SeenEpisodes.CountDocumentsAsync(ep => ep.TmdbId == tmdbid || ep.TvMazeId == tvmazeid && ep.UserId == userid && ep.SeasonNumber == season && ep.EpisodeNumber == episode);
            return s > 0;
        }

        /// <summary>
        /// Sorozatfelvétel a sorozatlistához, megadott felhasználóhoz.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="seriesid"></param>
        public async Task AddSeriesToUser(int userid, int seriesid)
        {
            await AddedSeries.InsertOneAsync(new AddedSeries()
            {
                Userid = userid,
                Seriesid = seriesid
            });
        }

        /// <summary>
        /// Ellenőrzi, hogy egy felhasználó felvette-e már az adott sorozatot.
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
        /// Megjelöl egy epizódot, mint elkezdett epizód.
        /// </summary>
        public async Task MarkEpisodeStarted(InternalEpisodeStartedModel episodeStartedModel)
        {
            //bool isNull = episodeStartedModel.GetType().GetProperties()
            //    .All(p => p.GetValue(episodeStartedModel) != null);

            await EpisodeStarted.InsertOneAsync(new EpisodeStarted()
            {
                Date = episodeStartedModel.Date,
                EpisodeNumber = episodeStartedModel.EpisodeNumber,
                SeasonNumber = episodeStartedModel.SeasonNumber,
                HoursElapsed = episodeStartedModel.HoursElapsed,
                MinutesElapsed = episodeStartedModel.MinutesElapsed,
                SecondsElapsed = episodeStartedModel.SecondsElapsed,
                TmdbId = episodeStartedModel.TmdbId,
                TvMazeId = episodeStartedModel.TvMazeId,
                Userid = episodeStartedModel.Userid,
                WatchedPercentage = episodeStartedModel.WatchedPercentage
            });

            //            if (!isNull) { return EpisodeStarted.InsertOneAsync(episodeStartedModel); } return null;
        }

        /// <summary>
        /// Töröl egy elkezdett epizódot egy megadott felhasználótól. Visszatérés a törlés sikerességével.
        /// </summary>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="season"></param>
        /// <param name="episode"></param>
        public async Task<bool> DeleteStartedEpisode(string tvmazeid, string tmdbid, int season, int episode)
        {
            var deleteEpisode = await EpisodeStarted.DeleteOneAsync(ep => ep.TvMazeId == Int32.Parse(tvmazeid) || ep.TmdbId == Int32.Parse(tmdbid) && ep.EpisodeNumber == episode && ep.SeasonNumber == season);
            return deleteEpisode.DeletedCount > 0;
        }

        /// <summary>
        /// Visszaadja, hogy egy epizódot elkezdtünk-e már.
        /// </summary>
        /// <param name="episodeStartedModel"></param>
        public async Task<bool> IsEpisodeStarted(InternalEpisodeStartedModel episodeStartedModel)
        {
            var s = await EpisodeStarted.CountDocumentsAsync(ep => ep.TvMazeId == episodeStartedModel.TvMazeId || ep.TmdbId == episodeStartedModel.TmdbId);
            return s > 0;
        }

        /// <summary>
        /// Nem vagyok benne biztos, hogy ez kell még.
        /// </summary>
        /// <param name="episodeStarted"></param>
        /// <param name="showTitle"></param>
        public async Task<bool> GetShow(EpisodeStarted episodeStarted, string showTitle)
        {
            var show = await Series.FindAsynchronous(series => series.Title.ToLower() == showTitle.ToLower());

            if (show != null)
            {
                //var episode = await EpisodeStarted.FindAsynchronous(ep => ep.Seriesid == Int32.Parse(show[0].Id));

                var coll = EpisodeStarted.Database.GetCollection<EpisodeStarted>("EpisodeStarted")
                    .AsQueryable<EpisodeStarted>();
                List<EpisodeStarted> series = new List<EpisodeStarted>();
                if (!coll.Empty())
                {
                    series = coll.Where(b => b.TvMazeId.ToString() == show[0].TvMazeId.ToString() || b.TmdbId.ToString() == show[0].TmdbId.ToString()).ToList();
                }

                if (series.Count > 0)
                {
                    EpisodeStarted ep = new EpisodeStarted()
                    {
                        Userid = episodeStarted.Userid,
                        TvMazeId = episodeStarted.TvMazeId,
                        TmdbId = episodeStarted.TmdbId,
                        SeasonNumber = episodeStarted.SeasonNumber,
                        EpisodeNumber = episodeStarted.EpisodeNumber,
                        Date = episodeStarted.Date,
                        HoursElapsed = episodeStarted.HoursElapsed,
                        MinutesElapsed = episodeStarted.MinutesElapsed,
                        SecondsElapsed = episodeStarted.SecondsElapsed,
                        WatchedPercentage = episodeStarted.WatchedPercentage
                    };
                    await EpisodeStarted.ReplaceOneAsync(
                        s => (s.TvMazeId == ep.TvMazeId || s.TmdbId == ep.TmdbId) && s.SeasonNumber == ep.SeasonNumber &&
                             s.EpisodeNumber == ep.EpisodeNumber, ep);
                }
                else
                {
                    //var episodestarted = new EpisodeStarted()
                    //{
                    //    Userid = episodeStarted.Userid,
                    //    Seriesid = episodeStarted.Seriesid,
                    //    SeasonNumber = episodeStarted.SeasonNumber,
                    //    EpisodeNumber = episodeStarted.EpisodeNumber,
                    //    Date = episodeStarted.Date,
                    //    TimeElapsed = episodeStarted.TimeElapsed,
                    //    WatchedPercentage = episodeStarted.WatchedPercentage
                    //};
                    var episodestarted = new EpisodeStarted() { };

                    await EpisodeStarted.InsertOneAsync(episodestarted);
                }
            }
            else
            {
                await new TvMazeParser().ImportSeriesFromTvMaze(showTitle);
            }

            return false;
        }

        /// <summary>
        /// Megnézi az adatbázisban, hogy a sorozat amit paraméterben kap, hozzá van e már adva az adatbázishoz.
        /// </summary>
        /// <param name="title"></param>
        public async Task<bool> IsShowExistInMongoDb(string title)
        {
            var exist = await Series.FindAsynchronous(x => x.Title.ToLower() == title.ToLower());
            return exist.Count > 0;
        }

        /// <summary>
        /// Egy sorozat felkérése a Mongo adatbázisból.
        /// </summary>
        /// <param name="title"></param>
        public async Task<InternalSeries> GetSeries(string title)
        {
            var seriesList = await Series.FindAsynchronous(x => x.Title == title);
            var series = seriesList.FirstOrDefault();
            if (series != null)
            {
                return ConvertMongoToInternalSeries(series);
            }

            return null;
        }

        /// <summary>
        /// Egy elkezdett epizód frissítése. Frissített értékek: Eltelt óra, eltelt percek, eltelt másodpercek.
        /// </summary>
        /// <param name="internalEpisode"></param>
        /// <param name="showName"></param>
        public async Task<bool> UpdateStartedEpisode(InternalEpisodeStartedModel internalEpisode, string showName)
        {
            var updateDef = Builders<EpisodeStarted>.Update.Set(o => o.HoursElapsed, internalEpisode.HoursElapsed).Set(o => o.MinutesElapsed, internalEpisode.MinutesElapsed).Set(o => o.SecondsElapsed, internalEpisode.SecondsElapsed).Set(o => o.WatchedPercentage, internalEpisode.WatchedPercentage);
            var s = await EpisodeStarted.UpdateOneAsync(episodeStarted => (episodeStarted.TvMazeId == internalEpisode.TvMazeId) || (episodeStarted.TmdbId == internalEpisode.TmdbId), updateDef);

            return s.ModifiedCount > 0;
        }

        /// <summary>
        /// Egy sorozat kedvencelése. TODO kiegészíteni hogy levegye, ha már kedvencelve van.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        public async Task SetFavoriteSeries(int userid, int tvmazeid, int tmdbid)
        {
            await FavoriteSeries.InsertOneAsync(new FavoriteSeries()
            {
                UserId = userid,
                TvMazeId = tvmazeid.ToString(),
                TmdbId = tmdbid.ToString()
            });
        }

        /// <summary>
        /// Az összes kedvencelt sorozat lekérése, adott felhaználóhoz.
        /// </summary>
        /// <param name="userid"></param>
        public async Task<List<FavoriteSeries>> GetAllFavoritesSeries(int userid)
        {
            var seriesList = await FavoriteSeries.FindAsynchronous(favoriteseries => favoriteseries.UserId == userid);
            return seriesList.ToList();

        }

        /// <summary>
        /// Ellenőrzi, hogy egy adott sorozatot kedvenceltünk-e már. Visszatérés: igaz/hamis
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        public async Task<bool> IsSeriesFavoriteAlready(int userid, int tvmazeid, int tmdbid)
        {
            var isSeriesFavorite = await FavoriteSeries.CountDocumentsAsync(favoriteseries => favoriteseries.UserId == userid && (favoriteseries.TvMazeId == tvmazeid.ToString() || favoriteseries.TmdbId == tmdbid.ToString()));
            return isSeriesFavorite > 0;
        }

        /// <summary>
        /// Egy epizód beállítása kedvencként ha még nem a kedvencünk, ellenkező esetben kedvencelés levétele az epizódról.
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
            {
                await FavoriteEpisode.DeleteOneAsync(episode => (episode.TvMazeId == tvmazeid.ToString() || episode.TmdbId == tmdbid.ToString()) && episode.UserId == userid && episode.SeasonNumber == seasonNum && episode.EpisodeNumber == episodeNum);
            }
            else
            {
                await FavoriteEpisode.InsertOneAsync(new FavoriteEpisode()
                {
                    UserId = userid,
                    TvMazeId = tvmazeid.ToString(),
                    TmdbId = tmdbid.ToString(),
                    EpisodeNumber = episodeNum,
                    SeasonNumber = seasonNum
                });
            }


        }

        /// <summary>
        /// Ellenőrzi, hogy egy epizódot kedvenceltünk-e már. Viszatérési érték: igaz/hamis
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="episode"></param>
        /// <param name="season"></param>
        public async Task<bool> IsEpisodeFavoriteAlready(int userid, int tvmazeid, int tmdbid, int episode, int season)
        {
            var isEpisodeFound = await FavoriteEpisode.CountDocumentsAsync(favoriteEpisode => favoriteEpisode.UserId == userid && (favoriteEpisode.TvMazeId == tvmazeid.ToString() || favoriteEpisode.TmdbId == tmdbid.ToString())
            && favoriteEpisode.SeasonNumber == season && favoriteEpisode.EpisodeNumber == episode);
            return isEpisodeFound > 0;
        }

        /// <summary>
        /// Sorozat komment hozzáadása adott felhasználótól - akármennyi lehetséges
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task CommentOnSeries(int userid, int tvmazeid, int tmdbid, string message)
        {
            await SeriesComments.InsertOneAsync(new SeriesComment()
            {
                UserId = userid,
                TvMazeId = tvmazeid.ToString(),
                TmdbId = tmdbid.ToString(),
                Message = message
            });
        }

        /// <summary>
        /// Epizódhoz való kommentelés adott felhasználótól - akármennyi lehetséges
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="episode"></param>
        /// <param name="season"></param>
        /// <param name="message"></param>
        public async Task CommentOnEpisode(int userid, int tvmazeid, int tmdbid, int episode, int season, string message)
        {
            await EpisodeComments.InsertOneAsync(new EpisodeComment()
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
        /// Sorozat értékelése adott felhasználótól
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

            var s = await SeriesRates.UpdateOneAsync(seriesRate => (seriesRate.UserId == userid) && (seriesRate.TmdbId == tmdbid) && (seriesRate.TvMazeId == tvmazeid), updateDef, new UpdateOptions { IsUpsert = true });
        }

        /// <summary>
        /// Epizód értékelése adott felhasználótól
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tvmazeid"></param>
        /// <param name="tmdbid"></param>
        /// <param name="episode"></param>
        /// <param name="season"></param>
        /// <param name="rate"></param>
        public async Task RateEpisode(int userid, int? tvmazeid, int? tmdbid, int episode, int season, int rate)
        {

            var updateDef = Builders<EpisodeRate>.Update
                .Set(o => o.UserId, userid)
                .Set(o => o.TvMazeId, tvmazeid)
                .Set(o => o.TmdbId, tmdbid)
                .Set(o => o.SeasonNumber, season)
                .Set(o => o.EpisodeNumber, episode)
                .Set(o => o.Rate, rate);

            var s = await EpisodeRates.UpdateOneAsync(episodeRate => (episodeRate.UserId == userid) && (episodeRate.TmdbId == tmdbid) && (episodeRate.TvMazeId == tvmazeid)
            && (episodeRate.SeasonNumber == season) && (episodeRate.EpisodeNumber == episode), updateDef, new UpdateOptions { IsUpsert = true });
        }


        /// <summary>
        /// Adott felhasználótól lekérhető egy megadott napon belül elkezdett vagy befejezett sorozatainak listája
        /// </summary>
        /// <param name="days"></param>
        /// <param name="userid"></param>
        public async Task<StartedAndSeenEpisodes> GetLastDaysEpisodes(int days, int userid)
        {
            StartedAndSeenEpisodes startedAndSeenEpisodes = new StartedAndSeenEpisodes();
            List<EpisodeStarted> episodeStartedList = new List<EpisodeStarted>();
            List<EpisodeSeen> episodeSeenList = new List<EpisodeSeen>();

            DateTime dateDaysBefore = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - days, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            int daysInSeconds = days * (24 * 60) * 60;

            var diffInSeconds = (int)(DateTime.Now - dateDaysBefore).TotalSeconds;

            var startedEpisodes = await EpisodeStarted.FindAsynchronous(ep => ep.Userid == userid);
            var seenEpisodes = await SeenEpisodes.FindAsynchronous(ep => ep.UserId == userid);

            foreach (var startedEpisode in startedEpisodes)
            {
                var episodeDateDiffInSeconds = (int)(DateTime.Now - startedEpisode.Date).TotalSeconds;

                if (episodeDateDiffInSeconds < diffInSeconds)
                {
                    episodeStartedList.Add(startedEpisode);
                }
            }

            foreach (var seenEpisode in seenEpisodes)
            {
                var episodeDateDiffInSeconds = (int)(DateTime.Now - seenEpisode.Date).TotalSeconds;

                if (episodeDateDiffInSeconds < diffInSeconds)
                {
                    episodeSeenList.Add(seenEpisode);
                }
            }


            startedAndSeenEpisodes.seenEpisodes = seenEpisodes;
            startedAndSeenEpisodes.startedEpisodes = startedEpisodes;

            return startedAndSeenEpisodes;
        }

        public Task<List<InternalSeries>> RecommendSeries(int userid, int genre)
        {
            throw new NotImplementedException();
        }
    }
}