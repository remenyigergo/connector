using Series.Dto.RequestDtoModels.SeriesDtos.EpisodeDtos;
using System.Collections.Generic;

namespace Series.Dto.RequestDtoModels
{
    public class EpisodeDto
    {
        #region Imdb
        public string AirDate { get; set; }
        public List<EpisodeCrewDto> Crew { get; set; }
        public string Description { get; set; }
        public int EpisodeNumber { get; set; }
        public List<EpisodeGuestDto> GuestStars { get; set; }
        public string Length { get; set; }
        public double? Rating { get; set; }
        public int SeasonNumber { get; set; }
        #endregion

        #region TvMaze
        public string Title { get; set; }
        public string TmdbShowId { get; set; }
        public int VoteCount { get; set; }
        #endregion
    }
}