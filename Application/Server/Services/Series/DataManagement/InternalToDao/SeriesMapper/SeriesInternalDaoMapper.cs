using Series.DataManagement.MongoDB.Models.Series;
using Standard.Contracts.Models.Series;
using Standard.Core.DataMapping;

namespace Series.DataManagement.InternalToDao.SeriesMapper
{
    public class SeriesInternalDaoMapper : IDataMapper<InternalSeries, MongoSeriesDao>
    {
        public MongoSeriesDao Map(InternalSeries obj)
        {
            return new MongoSeriesDao()
            {
                Title = obj.Title,
                Cast = obj.Cast,
                
            };
        }

        public InternalSeries Map(MongoSeriesDao obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
