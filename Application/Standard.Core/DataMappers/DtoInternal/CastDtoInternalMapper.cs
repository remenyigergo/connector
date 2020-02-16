using Series.Dto.RequestDtoModels.SeriesDtos;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Contracts.Models.Series.ExtendClasses.Cast;
using Standard.Core.DataMapping;
using System.Linq;

namespace Standard.Core.DataMappers.DtoInternal
{
    public class CastDtoInternalMapper : IDataMapper<ShowCastDto, InternalShowCast>
    {
        public InternalShowCast Map(ShowCastDto obj)
        {
            var actors = obj.Persons.Select(x=> new InternalActor() {
                CharacterName = x.CharacterName,
                RealName = x.RealName
            }).ToList();

            return new InternalShowCast() {
                Persons = actors
            };
        }

        public ShowCastDto Map(InternalShowCast obj)
        {
            return new ShowCastDto()
            {
                Persons = obj.Persons.Select(x=> new ActorDto() {
                    CharacterName = x.CharacterName,
                    RealName = x.RealName
                }).ToList()
            };
        }
    }
}
