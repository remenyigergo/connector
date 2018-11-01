using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataManager.Mongo.DbModels;
using Core.DataManager.Mongo.IRepository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;
using Contracts.Models.Series;
using Core.DataManager.Mongo.Models;
using NetFusion.Common.Extensions;
using Season = Core.DataManager.Mongo.Models.Season;

namespace Core.DataManager.Mongo.Repository
{
    public class SeriesRepository : ISeriesRepository
    {

        private readonly ObjectContext _context = null;

        public SeriesRepository(IOptions<Settings> settings)
        {
            _context = new ObjectContext(settings);
        }

        public async Task AddSeason(Season season, int seriesId)  //OLYAT TUDUNK CSINÁLNI, HOGY HA EZ NEM MEGY, HOGY KIKÉREM A SOROZATOT,
                                                                  // KIVESZEM AZ ÉVADOKAT EGY LISTÁBA, A LISTÁT MEGPÓTOLOM AZZAL AMIT BE AKARUNK ADNI, ÉS INSERTÁLOM ÚJ SOROZATKÉNT
                                                                  // viszont így felmerül az a hibalehetőség, hogy majd a megtekintett epizódok nem töltődnek be, amíg 
                                                                  // kitörlődve van az adott sorozat a csere miatt, szóval nem a legjobb eddig ez így.
        {
            var series = GetSeriesById(seriesId).Result;
            var filter = Builders<MongoSeries>.Filter.Eq(series[0].SeriesId, seriesId);
            var update = Builders<MongoSeries>.Update.AddToSet(s => s.Seasons, season);
            var result = await _context.Series.FindOneAndUpdateAsync(filter, update);

        }

        public async Task AddTvMazeSeries(InternalSeries internalSeries)
        {
            List<Season> mongoSeasonList = new List<Season>();
            
            //Átalakítjuk a szezont mongo modellre
            foreach (var season in internalSeries.Seasons)
            {
                List<Episode> episodeslist = new List<Episode>(season.EpisodesCount);
                foreach (var episode in season.Episodes)
                {
                    var mongoEpisode = new Episode()
                    {
                        Title = episode.Title,
                        Rating = episode.Rating,
                        Description = episode.Description,
                        Length = episode.Length,
                        //Cast = null TODO
                    };
                    episodeslist.Add(mongoEpisode);
                }

                var mongoSeason = new Season()
                {
                    EpisodesCount = season.EpisodesCount,
                    SeasonNumber = season.SeasonNumber,
                    Episodes = episodeslist
                };

                mongoSeasonList.Add(mongoSeason);
            }

            //Valódi mongoSeries, ami letárolásra kerül
            var mongoSeries = ConvertInternalToMongoSeries(internalSeries);

            await _context.Series.InsertOneAsync(mongoSeries.Result);
        }


        public async Task AddInternalSeries(InternalSeries internalSeries)
        {
            List<Season> mongoSeasonList = new List<Season>();

            //Átalakítjuk a szezont mongo modellre
            foreach (var season in internalSeries.Seasons)
            {
                List<Episode> episodeslist = new List<Episode>(season.EpisodesCount);
                foreach (var episode in season.Episodes)
                {
                    var mongoEpisode = new Episode()
                    {
                        Title = episode.Title,
                        Rating = episode.Rating,
                        Description = episode.Description,
                        Length = episode.Length,
                        //Cast = null

                        Air_date = episode.Air_date,
                        TMDB_Show_id = episode.Show_id,
                        Vote_count = episode.Vote_count,
                        Crew = episode.Crew,
                        Guest_stars = episode.Guest_stars 
                    };
                    episodeslist.Add(mongoEpisode);
                }

                var mongoSeason = new Season()
                {
                    EpisodesCount = season.EpisodesCount,
                    SeasonNumber = season.SeasonNumber,
                    Episodes = episodeslist,
                    
                    Airdate = season.Airdate,
                    Name = season.Name,
                    
                };

                mongoSeasonList.Add(mongoSeason);
            }

            

            //Valódi mongoSeries, ami letárolásra kerül
            var mongoSeries = ConvertInternalToMongoSeries(internalSeries);

            await _context.Series.InsertOneAsync(mongoSeries.Result);
        }

        public async Task AddIMDBSeries(InternalSeries internalSeries)
        {
            var mongoSeries = ConvertInternalToMongoSeries(internalSeries);

            await _context.Series.InsertOneAsync(mongoSeries.Result);
        }

        public async Task AddEpisodes(List<InternalEpisode> episodeList)
        {
            foreach (var episode in episodeList)
            {
                var mongoEpisode =
                    new Episode()
                    {
                        Title = episode.Title,
                        Rating = episode.Rating,
                        Length = episode.Length,
                        Description = episode.Description,
                        //Cast = new List<string>()
                    };
                await _context.Episodes.InsertOneAsync(mongoEpisode);
            }
            
        }


        public async Task DeleteSeriesById(int id)
        {
            //List<Series> seriesList = GetAllSeries().Result;
            var seriesExistCheck = _context.Series.Find(x => x.SeriesId == id.ToString());
            if (seriesExistCheck != null)
            {
                await _context.Series.DeleteOneAsync(Builders<MongoSeries>.Filter.Eq("SeriesId", id));
            }
            

            //foreach (var internalSeries in seriesList)
            //{
            //    if (Int32.Parse(internalSeries.SeriesId) == id)
            //    {
            //        await _context.Series.DeleteOneAsync(Builders<Series>.Filter.Eq("Id", id));
            //    }
            //}

        }

        public async Task<List<MongoSeries>> GetAllSeries()
        {
            //List<Series> seriesList = new List<Series>();
            //await _context.Series.Find(x => true).ForEachAsync(doc => seriesList.Add(doc));   //EZ IS JÓL MŰKÖDIK, VISZONT NINCSENEK NESTED BLOKKOK
            var coll = _context.Series.Database.GetCollection<MongoSeries>("internalSeries").AsQueryable<MongoSeries>();

            return coll.Where(b => true).ToList();

        }

        //public async Task<Series> GetSeriesById(int id)
        //{
        //    var seriesList = GetAllSeries().Result;

        //    foreach (var internalSeries in seriesList)
        //    {
        //        if (Int32.Parse(internalSeries.SeriesId) == id)
        //        {
        //            return internalSeries;
        //        }
        //    }
        //    return null;
        //}

        public async Task<List<MongoSeries>> GetSeriesById(int id)  //IGAZÁBÓL ITT CSAK EGY DB SERIEST KÉNE VISSZAADNI, HELYETTESITENI KÉNE A TOLISTET
        {
            var coll = _context.Series.Database.GetCollection<MongoSeries>("internalSeries").AsQueryable<MongoSeries>();

            var series = coll.Where(b => b.SeriesId == id.ToString()).ToList();
            return series;
        }

        //public async Task<Series> GetSeriesByTitle(string title)  //EZ JÓ ÍGY?    - MŰKÖDNI MŰKÖDIK
        //{
        //    var seriesList = GetAllSeries().Result;

        //    foreach (var internalSeries in seriesList)
        //    {
        //        if (internalSeries.Title == title)
        //        {
        //            return internalSeries;
        //        }
        //    }
        //    return null;
        //}


        public async Task<List<MongoSeries>> GetSeriesByTitle(string title)
        {
            var coll = _context.Series.Database.GetCollection<MongoSeries>("series").AsQueryable<MongoSeries>();

            var series = coll.Where(b => b.Title == title).ToList();
            return series;
        }


        public async Task<bool> IsSeriesAdded(string title)
        {
            var s = await _context.Series.CountDocumentsAsync(x => x.Title == title);

            return s > 0;
        }

        public async Task<bool> Update(InternalSeries tvMazeseries)
        {
            var mongoTvMazeSeries = await ConvertInternalToMongoSeries(tvMazeseries);
            var result = await _context.Series.ReplaceOneAsync(x => x.Title == tvMazeseries.Title, mongoTvMazeSeries);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> IsUpToDate(string title, string updateCode)
        {
            var series = (await _context.Series.FindAsync(x => x.Title == title)).FirstOrDefault();
            var a = await _context.Series.Find(x => x.Title == title).Project(x => new MongoSeries() {LastUpdated = x.LastUpdated}).ToListAsync();

            if (a[0].LastUpdated == updateCode)
            {
                return true;
            }
            return false;
        }

        public async Task<MongoSeries> ConvertInternalToMongoSeries(InternalSeries internalSeries)
        {
            
                var mongoSeries= new Core.DataManager.Mongo.Models.MongoSeries()
                {
                    Title = internalSeries.Title,
                    SeriesId = internalSeries.Id,
                    Runtime = internalSeries.Runtime,
                    TotalSeasons = internalSeries.TotalSeasons,
                    Category = internalSeries.Categories,
                    Description = internalSeries.Description,
                    Rating = internalSeries.Rating,
                    Year = internalSeries.Year,
                    LastUpdated = internalSeries.LastUpdated,

                    Created_by = internalSeries.Created_by,
                    Episode_run_time = internalSeries.Episode_run_time,
                    First_air_date = internalSeries.First_air_date,
                    Genres = internalSeries.Genres,
                    Original_language = internalSeries.Original_language,
                    LastEpisodeSimpleToAir = internalSeries.LastEpisodeSimpleToAir,
                    Networks = internalSeries.Networks,
                    Popularity = internalSeries.Popularity,
                    Production_companies = internalSeries.Production_companies,
                    Status = internalSeries.Status,
                    Type = internalSeries.Type,
                    Vote_count = internalSeries.Vote_count
                };
            return mongoSeries;
        }
    }
}
