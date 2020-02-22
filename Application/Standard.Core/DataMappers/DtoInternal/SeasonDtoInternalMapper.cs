using Series.Dto.RequestDtoModels;
using Series.Dto.RequestDtoModels.SeriesDtos;
using Standard.Contracts.Models.Series;
using Standard.Core.DataMapping;
using System.Linq;

namespace Standard.Core.DataMappers.DtoInternal
{
    public class SeasonDtoInternalMapper : IDataMapper<SeasonDto, InternalSeason>
    {
        private readonly IDataMapper<EpisodeDto, InternalEpisode> _episodeMapper;

        public SeasonDtoInternalMapper(IDataMapper<EpisodeDto, InternalEpisode> episodeMapper)
        {
            _episodeMapper = episodeMapper;
        }

        public InternalSeason Map(SeasonDto obj)
        {
            return new InternalSeason() {
                AirDate = obj.Airdate,
                Episodes = obj.Episodes.Select(x=> _episodeMapper.Map(x)).ToList(),
                EpisodesCount = obj.EpisodesCount,
                Name = obj.Name,
                SeasonNumber = obj.SeasonNumber,
                Summary = obj.Summary,
                TvMazeId = obj.TvMazeId
            };
        }

        public SeasonDto Map(InternalSeason obj)
        {
            return new SeasonDto()
            {
                Airdate = obj.AirDate,
                Episodes = obj.Episodes.Select(x => _episodeMapper.Map(x)).ToList(),
                EpisodesCount = obj.EpisodesCount,
                Name = obj.Name,
                SeasonNumber = obj.SeasonNumber,
                Summary = obj.Summary,
                TvMazeId = obj.TvMazeId
            };
        }
    }
}
