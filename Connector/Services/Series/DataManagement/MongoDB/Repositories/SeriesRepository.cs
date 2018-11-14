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

namespace Series.DataManagement.MongoDB.Repositories
{
    public class SeriesRepository : BaseMongoDbDataManager, ISeriesRepository
    {
        public IMongoCollection<MongoSeries> Series => Database.GetCollection<MongoSeries>("series");
        private IMongoCollection<MongoEpisode> Episodes => Database.GetCollection<MongoEpisode>("episodes");
        private IMongoCollection<SeenEpisode> SeenEpisodes => Database.GetCollection<SeenEpisode>("SeenEpisodes");
        private IMongoCollection<AddedSeries> AddedSeries => Database.GetCollection<AddedSeries>("AddedSeries");

        private IMongoCollection<EpisodeStarted> EpisodeStarted =>
            Database.GetCollection<EpisodeStarted>("EpisodeStarted");

        public SeriesRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
            //_context = new BaseMongoDbDataManager(settings);
        }

        public async Task AddSeason(MongoSeason mongoSeason, int seriesId)
            //OLYAT TUDUNK CSINÁLNI, HOGY HA EZ NEM MEGY, HOGY KIKÉREM A SOROZATOT,
            // KIVESZEM AZ ÉVADOKAT EGY LISTÁBA, A LISTÁT MEGPÓTOLOM AZZAL AMIT BE AKARUNK ADNI, ÉS INSERTÁLOM ÚJ SOROZATKÉNT
            // viszont így felmerül az a hibalehetőség, hogy majd a megtekintett epizódok nem töltődnek be, amíg 
            // kitörlődve van az adott sorozat a csere miatt, szóval nem a legjobb eddig ez így.
        {
            //            var series = GetSeriesById(seriesId).Result;
            //            var filter = Builders<MongoSeries>.Filter.Eq(series[0].TvMazeId, seriesId);
            //            var update = Builders<MongoSeries>.Update.AddToSet(s => s.Seasons, mongoSeason);
            //            var result = await Series.FindOneAndUpdateAsync(filter, update);
        }


        public async Task AddInternalSeries(InternalSeries internalSeries)
        {
            //Valódi mongoSeries, ami letárolásra kerül
            var mongoSeries = ConvertInternalToMongoSeries(internalSeries);

            //            var json = JsonConvert.SerializeObject(mongoSeries);
            //            var mongoSeries = JsonConvert.DeserializeObject<MongoSeries>(json);

            await Series.InsertOneAsync(mongoSeries);
        }

        public async Task AddEpisodes(List<InternalEpisode> episodeList)
        {
            foreach (var episode in episodeList)
            {
                var mongoEpisode =
                    new MongoEpisode()
                    {
                        Title = episode.Title,
                        Rating = episode.Rating,
                        Length = episode.Length,
                        Description = episode.Description,
                        //MongoCast = new List<string>()
                    };
                await Episodes.InsertOneAsync(mongoEpisode);
            }
        }


        public async Task DeleteSeriesById(int id)
        {
            //List<Series> seriesList = GetAllSeries().Result;
            var seriesExistCheck = Series.Find(x => x.SeriesId == id.ToString());
            if (seriesExistCheck != null)
            {
                await Series.DeleteOneAsync(Builders<MongoSeries>.Filter.Eq("TvMazeId", id));
            }


            //foreach (var mongoSeries in seriesList)
            //{
            //    if (Int32.Parse(mongoSeries.TvMazeId) == id)
            //    {
            //        await _context.Series.DeleteOneAsync(Builders<Series>.Filter.Eq("Id", id));
            //    }
            //}
        }

        //        public async Task<List<MongoSeries>> GetAllSeries()
        //        {
        //            //List<Series> seriesList = new List<Series>();
        //            //await _context.Series.Find(x => true).ForEachAsync(doc => seriesList.Add(doc));   //EZ IS JÓL MŰKÖDIK, VISZONT NINCSENEK NESTED BLOKKOK
        //            var coll = IMongoCollectionExtensions.AsQueryable<MongoSeries>(Series.Database.GetCollection<MongoSeries>("mongoSeries"));
        //
        //            return coll.Where(b => true).ToList();
        //        }

        //public async Task<Series> GetSeriesById(int id)
        //{
        //    var seriesList = GetAllSeries().Result;

        //    foreach (var mongoSeries in seriesList)
        //    {
        //        if (Int32.Parse(mongoSeries.TvMazeId) == id)
        //        {
        //            return mongoSeries;
        //        }
        //    }
        //    return null;
        //}

        //        public async Task<List<MongoSeries>>GetSeriesById(int id) //IGAZÁBÓL ITT CSAK EGY DB SERIEST KÉNE VISSZAADNI, HELYETTESITENI KÉNE A TOLISTET
        //        {
        //            var coll = IMongoCollectionExtensions.AsQueryable<MongoSeries>(_context.Series.Database.GetCollection<MongoSeries>("mongoSeries"));
        //
        //            var series = coll.Where(b => b.TvMazeId == id.ToString()).ToList();
        //            return series;
        //        }

        //public async Task<Series> GetSeriesByTitle(string showTitle)  //EZ JÓ ÍGY?    - MŰKÖDNI MŰKÖDIK
        //{
        //    var seriesList = GetAllSeries().Result;

        //    foreach (var mongoSeries in seriesList)
        //    {
        //        if (mongoSeries.Title == showTitle)
        //        {
        //            return mongoSeries;
        //        }
        //    }
        //    return null;
        //}


        public async Task<List<MongoSeries>> GetSeriesByTitle(string title)
        {
            var coll = Series.AsQueryable();

            var series = coll.Where(b => b.Title == title).ToList();
            return series;
        }


        public async Task<bool> IsSeriesImported(string title)
        {
            var s = await Series.CountDocumentsAsync(x => x.Title.ToLower() == title.ToLower());

            return s > 0;
        }

        public async Task<bool> Update(InternalSeries internalSeries)
        {
            var mongoSeries = ConvertInternalToMongoSeries(internalSeries);
            var result = await Series.ReplaceOneAsync(x => x.Title == internalSeries.Title, mongoSeries);
            return result.ModifiedCount > 0;
        }

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

        public MongoSeries ConvertInternalToMongoSeries(InternalSeries internalSeries)
        {
            var seasons = ConvertInternalSeasonToMongoSeason(internalSeries.Seasons);

            var mongoSeries = new MongoSeries()
            {
                TvMazeId = internalSeries.TvMazeId,
                TmdbId = internalSeries.TmdbId,
                Title = internalSeries.Title,
                SeriesId = internalSeries.Id,
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

        public async Task MarkAsSeen(int userid, int seriesid, int seasonNumber, int episodeNumber)
        {
            await SeenEpisodes.InsertOneAsync(new SeenEpisode()
            {
                UserId = userid,
                SeriesId = seriesid,
                EpisodeNumber = episodeNumber,
                SeasonNumber = seasonNumber
            });
        }

        public async Task AddSeriesToUser(int userid, int seriesid)
        {
            await AddedSeries.InsertOneAsync(new AddedSeries()
            {
                Userid = userid,
                Seriesid = seriesid
            });
        }

        public async Task<bool> IsSeriesAddedToUser(int userid, int seriesid)
        {
            var s = await AddedSeries.CountDocumentsAsync(x => x.Userid == userid && x.Seriesid == seriesid);

            return s > 0;
        }

        public Task MarkEpisodeStarted(EpisodeStartedModel episodeStartedModel)
        {
            //TODO: ÁTALAKTÍANI EPISODESTARTED-re

            bool isNull = episodeStartedModel.GetType().GetProperties()
                .All(p => p.GetValue(episodeStartedModel) != null);

            //            if (!isNull) { return EpisodeStarted.InsertOneAsync(episodeStartedModel); } return null;
            return null;
        }

        public Task<bool> IsEpisodeStarted(EpisodeStartedModel episodeStartedModel)
        {
            throw new System.NotImplementedException();
        }


        public Task<bool> IsEpisodeStarted(EpisodeStarted episodeStartedModel)
        {
            return null;
        }

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
                    series = coll.Where(b => b.Seriesid.ToString() == show[0].SeriesId.ToString()).ToList();
                }

                if (series.Count > 0)
                {
                    EpisodeStarted ep = new EpisodeStarted()
                    {
                        Userid = episodeStarted.Userid,
                        Seriesid = episodeStarted.Seriesid,
                        SeasonNumber = episodeStarted.SeasonNumber,
                        EpisodeNumber = episodeStarted.EpisodeNumber,
                        Date = episodeStarted.Date,
                        TimeElapsed = episodeStarted.TimeElapsed,
                        WatchedPercentage = episodeStarted.WatchedPercentage
                    };
                    await EpisodeStarted.ReplaceOneAsync(
                        s => s.Seriesid == ep.Seriesid && s.SeasonNumber == ep.SeasonNumber &&
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
        }  //már nem tudom mihez kell .. EXIST?

        public async Task<bool> IsShowExistInMongoDb(string title)
        {
            var exist = await Series.FindAsynchronous(x => x.Title.ToLower() == title.ToLower());
            return exist.Count > 0;
        }

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
    }
}