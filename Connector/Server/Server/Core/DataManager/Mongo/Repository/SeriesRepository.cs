using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.DataManager.Mongo.DbModels;
using Core.DataManager.Mongo.IRepository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Configuration;
using System.Linq;
using Core.DataManager.Mongo.Models;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using NetFusion.Common.Extensions;
using MongoDB.Driver.Linq;

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
            var filter = Builders<Series>.Filter.Eq(series[0].SeriesId, seriesId);
            var update = Builders<Series>.Update.AddToSet(s => s.Seasons, season);
            var result = await _context.Series.FindOneAndUpdateAsync(filter, update);

        }

        public async Task AddSeries(Series series)
        {
            await _context.Series.InsertOneAsync(series);
        }

        public async Task DeleteSeriesById(int id)
        {
            //List<Series> seriesList = GetAllSeries().Result;
            var seriesExistCheck = _context.Series.Find(x => x.SeriesId == id.ToString());
            if (seriesExistCheck != null)
            {
                await _context.Series.DeleteOneAsync(Builders<Series>.Filter.Eq("SeriesId", id));
            }
            

            //foreach (var series in seriesList)
            //{
            //    if (Int32.Parse(series.SeriesId) == id)
            //    {
            //        await _context.Series.DeleteOneAsync(Builders<Series>.Filter.Eq("Id", id));
            //    }
            //}

        }

        public async Task<List<Series>> GetAllSeries()
        {
            //List<Series> seriesList = new List<Series>();
            //await _context.Series.Find(x => true).ForEachAsync(doc => seriesList.Add(doc));   //EZ IS JÓL MŰKÖDIK, VISZONT NINCSENEK NESTED BLOKKOK
            var coll = _context.Series.Database.GetCollection<Series>("series").AsQueryable<Series>();

            return coll.Where(b => true).ToList();

        }

        //public async Task<Series> GetSeriesById(int id)
        //{
        //    var seriesList = GetAllSeries().Result;

        //    foreach (var series in seriesList)
        //    {
        //        if (Int32.Parse(series.SeriesId) == id)
        //        {
        //            return series;
        //        }
        //    }
        //    return null;
        //}

        public async Task<List<Series>> GetSeriesById(int id)  //IGAZÁBÓL ITT CSAK EGY DB SERIEST KÉNE VISSZAADNI, HELYETTESITENI KÉNE A TOLISTET
        {
            var coll = _context.Series.Database.GetCollection<Series>("series").AsQueryable<Series>();

            var series = coll.Where(b => b.SeriesId == id.ToString()).ToList();
            return series;
        }

        //public async Task<Series> GetSeriesByTitle(string title)  //EZ JÓ ÍGY?    - MŰKÖDNI MŰKÖDIK
        //{
        //    var seriesList = GetAllSeries().Result;

        //    foreach (var series in seriesList)
        //    {
        //        if (series.Title == title)
        //        {
        //            return series;
        //        }
        //    }
        //    return null;
        //}


        public async Task<List<Series>> GetSeriesByTitle(string title)
        {
            var coll = _context.Series.Database.GetCollection<Series>("series").AsQueryable<Series>();

            var series = coll.Where(b => b.Title == title).ToList();
            return series;
        }
    }
}
