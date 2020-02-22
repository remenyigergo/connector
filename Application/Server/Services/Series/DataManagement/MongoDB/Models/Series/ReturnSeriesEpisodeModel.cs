using System.Collections.Generic;
using Series.Service.Models;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class ReturnSeriesEpisodeModel
    {
        public List<MongoSeriesDao> foundSeriesList;
        public EpisodeStartedDao startedEpisodesList;
    }
}