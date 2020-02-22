using Series.Dto.RequestDtoModels.SeriesDtos;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Standard.Core.DataMappers.DtoInternal
{
    public class GenreDtoInternalMapper : IDataMapper<GenreDto, InternalSeriesGenre>
    {
        public InternalSeriesGenre Map(GenreDto obj)
        {
            return new InternalSeriesGenre(obj.Name);
        }

        public GenreDto Map(InternalSeriesGenre obj)
        {
            return new GenreDto() {
                Name = obj.Name
            };
        }
    }
}
