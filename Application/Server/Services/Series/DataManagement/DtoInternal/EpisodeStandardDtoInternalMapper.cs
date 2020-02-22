using Series.Dto.RequestDtoModels;
using Series.Dto.RequestDtoModels.SeriesDtos.EpisodeDtos;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;
using System.Linq;

namespace Series.DataManagement.DtoInternal
{
    public class EpisodeStandardDtoInternalMapper : IDataMapper<EpisodeDto, InternalEpisode>
    {
        private readonly IDataMapper<EpisodeCrewDto, InternalEpisodeCrew> _crewMapper;
        private readonly IDataMapper<EpisodeGuestDto, InternalEpisodeGuest> _guestMapper;

        public EpisodeStandardDtoInternalMapper(
                IDataMapper<EpisodeCrewDto, InternalEpisodeCrew> crewMapper,
                IDataMapper<EpisodeGuestDto, InternalEpisodeGuest> guestMapper
            )
        {
            _crewMapper = crewMapper;
            _guestMapper = guestMapper;
        }

        public InternalEpisode Map(EpisodeDto obj)
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

        public EpisodeDto Map(InternalEpisode obj)
        {
            return new EpisodeDto()
            {
                AirDate = obj.AirDate,
                Crew = obj.Crew?.Select(x=> _crewMapper.Map(x)).ToList(),
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
