using Series.DataManagement.MongoDB.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;
using System;

namespace Series.DataManagement.InternalToDao
{
    public class NetworkInternalDaoMapper : IDataMapper<InternalNetwork, MongoNetwork>
    {
        public MongoNetwork Map(InternalNetwork obj)
        {
            return new MongoNetwork()
            {
                Name = obj.Name,
                OriginCountry =obj.Origin_country
            };
        }

        public InternalNetwork Map(MongoNetwork obj)
        {
            return new InternalNetwork() {
                Name = obj.Name,
                Origin_country = obj.OriginCountry
            };
        }
    }
}
