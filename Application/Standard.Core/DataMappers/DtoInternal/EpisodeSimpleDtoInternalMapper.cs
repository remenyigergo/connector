using Series.Dto.RequestDtoModels.SeriesDtos;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Standard.Core.DataMappers.DtoInternal
{
    public class EpisodeSimpleDtoInternalMapper : IDataMapper<EpisodeSimpleDto, InternalEpisodeSimple>
    {
        public InternalEpisodeSimple Map(EpisodeSimpleDto obj)
        {
            return new InternalEpisodeSimple() {
                Name = obj.Name,
                Air_date = obj.AirDate,
                Episode_number = obj.EpisodeNumber,
                Overview = obj.Overview,
                Season_number = obj.SeasonNumber,
                Vote_average = obj.VoteAverage,
                Vote_count = obj.VoteCount
            };
        }

        public EpisodeSimpleDto Map(InternalEpisodeSimple obj)
        {
            return new EpisodeSimpleDto() {
                Name = obj.Name,
                AirDate = obj.Air_date,
                EpisodeNumber = obj.Episode_number,
                Overview = obj.Overview,
                SeasonNumber = obj.Season_number,
                VoteAverage = obj.Vote_average,
                VoteCount = obj.Vote_count
            };
        }
    }
}
