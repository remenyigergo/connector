using Series.DataManagement.MongoDB.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Series.DataManagement.InternalToDao
{
    public class GenreInternalDaoMapper : IDataMapper<InternalSeriesGenre, MongoSeriesGenre>
    {
        public MongoSeriesGenre Map(InternalSeriesGenre obj)
        {
            return new MongoSeriesGenre()
            {
                Name = obj.Name
            };
        }

        public InternalSeriesGenre Map(MongoSeriesGenre obj)
        {
            return new InternalSeriesGenre(obj.Name);
        }
    }
}
