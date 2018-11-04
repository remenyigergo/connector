using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Server.Service.Controllers
{
    [Route("api/[controller]")]
    public class SeriesMongoController : Controller
    {
//        private readonly ISeriesRepository _seriesRepository;
//
//        public SeriesMongoController(ISeriesRepository seriesRepository)
//        {
//            _seriesRepository = seriesRepository;
//        }
//
//        #region POST
//        [HttpPost("{id}")]
//        public async Task AddSeries([FromBody]MongoSeries mongoSeries)
//        {
////            await _seriesRepository.AddSeries(mongoSeries);
//        }
//
//        [HttpPost("addseason/{seriesid}")]
//        public async Task AddSeasonToExistingSeries([FromBody] Season season, int seriesid)
//        {
//            await _seriesRepository.AddSeason(season, seriesid);
//        }


//        #region GET
//        [HttpGet("mongoSeries/")]
//        public async Task<List<MongoSeries>> GetAllSeries()
//        {
//            return await _seriesRepository.GetAllSeries();
//        }
//
//        [HttpGet("{id}")]
//        public async Task<List<MongoSeries>> GetSeriesById(int id)
//        {
//            return await _seriesRepository.GetSeriesById(id);
//        }
//
//        [HttpGet("{name}")]
//        public async Task<List<MongoSeries>> GetSeriesByTitle(string title)
//        {
//            return await _seriesRepository.GetSeriesByTitle(title);
//        }
//
//
//
//        #endregion
//
//
//        #region DELETE
//        [HttpDelete("{id}")]
//        public async Task DeleteSeriesById(int id)
//        {
//            await _seriesRepository.DeleteSeriesById(id);
//
//        }
//
//        #endregion


    }
}