using System.Collections.Generic;

namespace Series.Dto.RequestDtoModels.SeriesDtos
{
    public class SeasonDto
    {
        //TMDB
        public string Airdate { get; set; }
        public List<EpisodeDto> Episodes { get; set; }
        public int EpisodesCount { get; set; }
        //TVMAZE
        public int TvMazeId { get; set; }
        public string Name { get; set; }
        public int SeasonNumber { get; set; }
        public string Summary { get; set; }
    }
}
