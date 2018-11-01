using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contracts.Models.Series;
using Core.DataManager.Mongo.Models;
using MongoDB.Driver;
using Season = Core.DataManager.Mongo.Models.Season;

namespace Core.DataManager.Mongo.IRepository
{
    public interface ISeriesRepository
    {
        Task<List<MongoSeries>> GetAllSeries();
        Task<List<MongoSeries>> GetSeriesById(int id);
        Task<List<MongoSeries>> GetSeriesByTitle(string name);
        Task AddTvMazeSeries(InternalSeries series);
        Task AddInternalSeries(InternalSeries series);
        Task AddIMDBSeries(InternalSeries series);
        Task AddEpisodes(List<InternalEpisode> episodes);
        Task AddSeason(Season season, int seriesId);
        Task DeleteSeriesById(int id);
        Task<bool> IsSeriesAdded(string title);
        Task<bool> IsUpToDate(string title, string updateCode);
        Task<bool> Update(InternalSeries series);
        //Task<bool> CheckSeasonsCountMatch(string title);
    }
}
