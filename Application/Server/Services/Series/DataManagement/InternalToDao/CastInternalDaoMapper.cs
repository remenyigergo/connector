using Series.DataManagement.MongoDB.Models.Series.MongoSeriesModels;
using Series.Dto.RequestDtoModels.SeriesDtos;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;
using System.Linq;

namespace Series.DataManagement.InternalToDao
{
    public class CastInternalDaoMapper : IDataMapper<InternalShowCast, MongoShowCast>
    {
        //more mapper maybe? ActorMapper?


        public InternalShowCast Map(MongoShowCast obj)
        {
            return new InternalShowCast()
            {
                Persons = obj.Persons.Select(x => new Standard.Contracts.Models.Series.ExtendClasses.Cast.InternalActor()
                {
                    CharacterName = x.CharacterName,
                    RealName = x.RealName
                }).ToList()
            };
        }

        public MongoShowCast Map(InternalShowCast obj)
        {
            return new MongoShowCast()
            {
                Persons = obj.Persons.Select(x => new MongoActor()
                {
                    CharacterName = x.CharacterName,
                    RealName = x.RealName
                }).ToList()
            };
        }
    }
}
