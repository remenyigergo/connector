using Series.DataManagement.MongoDB.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Series.DataManagement.InternalToDao
{
    public class CreatedByInternalDaoMapper : IDataMapper<InternalCreator, MongoCreator>
    {
        public MongoCreator Map(InternalCreator obj)
        {
            return new MongoCreator()
            {
                Name = obj.Name
            };
        }

        public InternalCreator Map(MongoCreator obj)
        {
            return new InternalCreator()
            {
                Name = obj.Name
            };
        }
    }
}
