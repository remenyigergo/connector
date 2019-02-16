using Series.Service.Models;
using Standard.Contracts.Models.Series;
using System;
using System.Collections.Generic;
using System.Text;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class ReturnSeriesEpisodeModel
    {
        public List<MongoSeries> foundSeriesList;
        public EpisodeStarted startedEpisodesList;
    }
}
