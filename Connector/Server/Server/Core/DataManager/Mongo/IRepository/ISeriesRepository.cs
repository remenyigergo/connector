using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.DataManager.Mongo.Models;
using MongoDB.Driver;

namespace Core.DataManager.Mongo.IRepository
{
    public interface ISeriesRepository
    {
        Task<List<Series>> GetAllSeries();
        Task<List<Series>> GetSeriesById(int id);
        Task<List<Series>> GetSeriesByTitle(string name);
        Task AddSeries(Series s);
        Task AddSeason(Season season, int seriesId);
        Task DeleteSeriesById(int id);
    }
}
