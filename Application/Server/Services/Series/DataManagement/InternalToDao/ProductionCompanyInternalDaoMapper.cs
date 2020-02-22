using Series.DataManagement.MongoDB.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Series.DataManagement.InternalToDao
{
    public class ProductionCompanyInternalDaoMapper : IDataMapper<InternalProductionCompany, MongoProductionCompany>
    {
        public MongoProductionCompany Map(InternalProductionCompany obj)
        {
            return new MongoProductionCompany() {
                Name = obj.Name,
                OriginCountry = obj.Origin_country
            };
        }

        public InternalProductionCompany Map(MongoProductionCompany obj)
        {
            return new InternalProductionCompany()
            {
                Name = obj.Name,
                Origin_country = obj.OriginCountry,
            };
        }
    }
}
