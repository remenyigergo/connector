using System.Collections.Generic;

namespace Series.Dto.RequestDtoModels.SeriesDtos
{
    public class SeasonDto
    {
        //TMDB
        public string Airdate { get; }
        public List<EpisodeDto> Episodes { get; }
        public int EpisodesCount { get; }
        //TVMAZE
        public int TvMazeId { get; }
        public string Name { get; }
        public int SeasonNumber { get; }
        public string Summary { get; }
    }
}
