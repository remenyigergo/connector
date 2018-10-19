using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataManager.Mongo.IRepository;
using Core.DataManager.Mongo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Server.Service.Controllers
{
    [Route("api/[controller]")]
    public class SeriesMongoController : Controller
    {
        private readonly ISeriesRepository _seriesRepository;

        public SeriesMongoController(ISeriesRepository seriesRepository)
        {
            _seriesRepository = seriesRepository;
        }

        #region POST
        [HttpPost("{id}")]
        public async Task AddSeries([FromBody]Series series)
        {
            await _seriesRepository.AddSeries(series);
        }

        [HttpPost("addseason/{seriesid}")]
        public async Task AddSeasonToExistingSeries([FromBody] Season season, int seriesid)
        {
            await _seriesRepository.AddSeason(season, seriesid);
        }

        
        #endregion


        #region GET
        [HttpGet("series/")]
        public async Task<List<Series>> GetAllSeries()
        {
            return await _seriesRepository.GetAllSeries();
        }

        [HttpGet("{id}")]
        public async Task<List<Series>> GetSeriesById(int id)
        {
            return await _seriesRepository.GetSeriesById(id);
        }

        [HttpGet("{name}")]
        public async Task<List<Series>> GetSeriesByTitle(string title)
        {
            return await _seriesRepository.GetSeriesByTitle(title);
        }



        #endregion


        #region DELETE
        [HttpDelete("{id}")]
        public async Task DeleteSeriesById(int id)
        {
            await _seriesRepository.DeleteSeriesById(id);

        }

        #endregion


    }
}