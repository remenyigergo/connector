using Series.Dto.RequestDtoModels.SeriesDtos.EpisodeDtos;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Series.DataManagement.DtoInternal
{
    public class GuestDtoInternalMapper : IDataMapper<EpisodeGuestDto, InternalEpisodeGuest>
    {
        public InternalEpisodeGuest Map(EpisodeGuestDto obj)
        {
            return new InternalEpisodeGuest()
            {
                Character = obj.Character,
                Name = obj.Name
            };
        }

        public EpisodeGuestDto Map(InternalEpisodeGuest obj)
        {
            return new EpisodeGuestDto()
            {
                Character = obj.Character,
                Name = obj.Name
            };
        }
    }
}
