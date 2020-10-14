using Series.Service.Models;
using Standard.Contracts.Models.Series;
using Standard.Core.DataMapping;

namespace Series.DataManagement.InternalToDao
{
    public class EpisodeStartedInternalDaoMapper : IDataMapper<InternalEpisodeStartedModel, EpisodeStartedDao>
    {
        public EpisodeStartedDao Map(InternalEpisodeStartedModel obj)
        {
            return new EpisodeStartedDao()
            {
                Date = obj.Date,
                EpisodeNumber = obj.EpisodeNumber,
                HoursElapsed = obj.HoursElapsed,
                TvMazeId = obj.TvMazeId,
                TmdbId = obj.TmdbId,
                MinutesElapsed = obj.MinutesElapsed,
                SeasonNumber = obj.SeasonNumber,
                SecondsElapsed = obj.SecondsElapsed,
                UserId = obj.Userid,
                WatchedPercentage = obj.WatchedPercentage
            };
        }

        public InternalEpisodeStartedModel Map(EpisodeStartedDao obj)
        {
            return new InternalEpisodeStartedModel()
            {
                Date = obj.Date,
                WatchedPercentage = obj.WatchedPercentage,
                Userid = obj.UserId,
                SecondsElapsed = obj.SecondsElapsed,
                SeasonNumber = obj.SeasonNumber,
                EpisodeNumber = obj.EpisodeNumber,
                HoursElapsed = obj.HoursElapsed,
                MinutesElapsed = obj.MinutesElapsed,
                TmdbId = obj.TmdbId,
                TvMazeId = obj.TvMazeId
            };
        }
    }
}
