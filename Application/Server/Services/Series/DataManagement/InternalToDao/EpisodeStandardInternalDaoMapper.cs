using Series.DataManagement.MongoDB.Models.Series;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;
using System.Linq;

namespace Series.DataManagement.InternalToDao
{
    public class EpisodeStandardInternalDaoMapper : IDataMapper<InternalEpisode, MongoEpisode>
    {
        private readonly IDataMapper<InternalEpisodeCrew, MongoCrew> _crewMapper;
        private readonly IDataMapper<InternalEpisodeGuest, MongoEpisodeGuest> _guestMapper;

        public EpisodeStandardInternalDaoMapper(
                IDataMapper<InternalEpisodeCrew, MongoCrew> crewMapper,
                IDataMapper<InternalEpisodeGuest, MongoEpisodeGuest> guestMapper
            )
        {
            _crewMapper = crewMapper;
            _guestMapper = guestMapper;
        }

        public MongoEpisode Map(InternalEpisode obj)
        {
            return new MongoEpisode()
            {
                AirDate = obj.AirDate,
                Crew = obj.Crew?.Select(x=>_crewMapper.Map(x)).ToList(),
                Description = obj.Description,
                EpisodeNumber = obj.EpisodeNumber,
                GuestStars = obj.GuestStars?.Select(x=>_guestMapper.Map(x)).ToList(),
                Length = obj.Length,
                Rating = obj.Rating,
                SeasonNumber = obj.SeasonNumber,
                Title = obj.Title,
                TmdbShowId = obj.TmdbShowId,
                VoteCount = obj.VoteCount,
            };
        }

        public InternalEpisode Map(MongoEpisode obj)
        {
            return new InternalEpisode()
            {
                AirDate = obj.AirDate,
                Crew = obj.Crew?.Select(x=>_crewMapper.Map(x)).ToList(),
                Description = obj.Description,
                EpisodeNumber = obj.EpisodeNumber,
                GuestStars = obj.GuestStars?.Select(x=>_guestMapper.Map(x)).ToList(),
                Length = obj.Length,
                Rating = obj.Rating,
                SeasonNumber = obj.SeasonNumber,
                Title = obj.Title,
                TmdbShowId = obj.TmdbShowId,
                VoteCount = obj.VoteCount
            };
        }
    }
}
