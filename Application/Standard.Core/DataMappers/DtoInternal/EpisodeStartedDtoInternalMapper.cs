using Series.Dto.RequestDtoModels;
using Standard.Contracts.Models.Series;
using Standard.Core.DataMapping;

namespace Standard.Core.DataMappers.DtoInternal
{
    public class EpisodeStartedDtoInternalMapper : IDataMapper<EpisodeStartedDto, InternalEpisodeStartedModel>
    {
        public InternalEpisodeStartedModel Map(EpisodeStartedDto obj)
        {
            return new InternalEpisodeStartedModel
            {
                Date = obj.Date,
                EpisodeNumber = obj.EpisodeNumber,
                HoursElapsed = obj.HoursElapsed,
                MinutesElapsed = obj.MinutesElapsed,
                SeasonNumber = obj.SeasonNumber,
                SecondsElapsed = obj.SecondsElapsed,
                TmdbId = obj.TmdbId,
                TvMazeId = obj.TvMazeId,
                Userid = obj.Userid,
                WatchedPercentage = obj.WatchedPercentage
            };
        }

        public EpisodeStartedDto Map(InternalEpisodeStartedModel obj)
        {
            return new EpisodeStartedDto()
            {
                TmdbId = obj.TmdbId,
                TvMazeId = obj.TvMazeId,
                Date = obj.Date,
                EpisodeNumber = obj.EpisodeNumber,
                HoursElapsed = obj.HoursElapsed,
                MinutesElapsed = obj.MinutesElapsed,
                SeasonNumber = obj.SeasonNumber,
                SecondsElapsed = obj.SecondsElapsed,
                Userid = obj.Userid,
                WatchedPercentage = obj.WatchedPercentage
            };
        }
    }
}
