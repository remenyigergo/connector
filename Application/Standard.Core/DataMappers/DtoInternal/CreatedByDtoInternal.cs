using Series.Dto.RequestDtoModels.SeriesDtos;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Standard.Core.DataMappers.DtoInternal
{
    public class CreatedByDtoInternal : IDataMapper<CreatorDto, InternalCreator>
    {
        public InternalCreator Map(CreatorDto obj)
        {
            return new InternalCreator()
            {
                Name = obj.Name
            };
        }

        public CreatorDto Map(InternalCreator obj)
        {
            return new CreatorDto() {
                Name = obj.Name
            };
        }
    }
}
