using Series.DataManagement.MongoDB.Models.Series;
using Standard.Contracts.Models.Series;
using Standard.Core.DataMapping;
using System;
using System.Linq;

namespace Series.DataManagement.InternalToDao
{
    public class SeasonInternalDaoMapper : IDataMapper<InternalSeason, MongoSeason>
    {
        private readonly IDataMapper<InternalEpisode, MongoEpisode> _episodeMapper;

        public SeasonInternalDaoMapper(
                IDataMapper<InternalEpisode, MongoEpisode> episodeMapper
            )
        {
            _episodeMapper = episodeMapper;
        }

        public MongoSeason Map(InternalSeason obj)
        {
            return new MongoSeason()
            {
                Airdate = obj.AirDate,
                Episodes = obj.Episodes.Select(x=>_episodeMapper.Map(x)).ToList(),
                EpisodesCount = obj.EpisodesCount,
                Name = obj.Name,
                SeasonNumber = obj.SeasonNumber,
                Summary = obj.Summary
            };
        }

        public InternalSeason Map(MongoSeason obj)
        {
            return new InternalSeason() {
                AirDate = obj.Airdate,
                Episodes = obj.Episodes.Select(x=>_episodeMapper.Map(x)).ToList(),
                EpisodesCount = obj.EpisodesCount,
                Name = obj.Name,
                SeasonNumber = obj.SeasonNumber,
                Summary = obj.Summary,
                TvMazeId = -999
            };
        }
    }
}
