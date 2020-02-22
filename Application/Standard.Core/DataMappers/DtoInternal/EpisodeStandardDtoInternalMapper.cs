using Series.Dto.RequestDtoModels;
using Standard.Contracts.Models.Series;
using Standard.Core.DataMapping;
using System;

namespace Standard.Core.DataMappers.DtoInternal
{
    public class EpisodeStandardDtoInternalMapper : IDataMapper<EpisodeDto, InternalEpisode>
    {
        public InternalEpisode Map(EpisodeDto obj)
        {
            throw new NotImplementedException();
        }

        public EpisodeDto Map(InternalEpisode obj)
        {
            throw new NotImplementedException();
        }
    }
}
