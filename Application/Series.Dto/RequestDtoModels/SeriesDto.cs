using Series.Dto.RequestDtoModels.SeriesDtos;
using System.Collections.Generic;

namespace Series.Dto.RequestDtoModels.SeriesDto
{
    public class SeriesDto
    {
        #region IMDB
        public List<CreatorDto> CreatedBy { get; set; }
        public List<string> EpisodeRunTime { get; set; }
        public string FirstAirDate { get; set; }
        public List<GenreDto> Genres { get; set; }
        public EpisodeSimpleDto LastEpisodeSimpleToAir { get; set; }
        public List<NetworkDto> Networks { get; set; }
        public string OriginalLanguage { get; set; }
        public string Popularity { get; set; }
        public List<ProductionCompanyDto> ProductionCompanies { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int VoteCount { get; set; }
        #endregion

        #region TvMaze
        public string Id { get; set; }
        public string TvMazeId { get; set; }
        public string TmdbId { get; set; }
        public string Title { get; set; }
        public List<SeasonDto> Seasons { get; set; }
        public List<string> Runtime { get; set; }
        public double? Rating { get; set; }
        public string Year { get; set; }
        public List<string> Categories { get; set; }
        public string Description { get; set; }
        public int TotalSeasons { get; set; }
        public string LastUpdated { get; set; }
        public ShowCastDto Cast { get; set; }
        #endregion
    }
}
