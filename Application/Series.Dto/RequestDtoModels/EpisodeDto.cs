using Series.Dto.RequestDtoModels.SeriesDtos.EpisodeDtos;
using System.Collections.Generic;

namespace Series.Dto.RequestDtoModels
{
    public class EpisodeDto
    {
        #region Imdb
        public string AirDate { get; }
        public List<EpisodeCrewDto> Crew { get; }
        public string Description { get; }
        public int EpisodeNumber { get; }
        public List<EpisodeGuestDto> GuestStars { get; }
        public string Length { get; }
        public double? Rating { get; }
        public int SeasonNumber { get; }
        #endregion

        #region TvMaze
        public string Title { get; }
        public string TmdbShowId { get; }
        public int VoteCount { get; }
        #endregion
    }
}