using Series.Dto.RequestDtoModels.SeriesDtos.EpisodeDtos;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Series.DataManagement.DtoInternal
{
    public class CrewDtoInternalMapper : IDataMapper<EpisodeCrewDto, InternalEpisodeCrew>
    {
        public InternalEpisodeCrew Map(EpisodeCrewDto obj)
        {
            return new InternalEpisodeCrew()
            {
                Department = obj.Department,
                Job = obj.Job,
                Name = obj.Name
            };
        }

        public EpisodeCrewDto Map(InternalEpisodeCrew obj)
        {
            return new EpisodeCrewDto()
            {
                Department = obj.Department,
                Job = obj.Job,
                Name = obj.Name
            };
        }
    }
}
