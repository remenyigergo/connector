using Series.Dto.RequestDtoModels.SeriesDto;
using Series.Dto.RequestDtoModels.SeriesDtos;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;
using System.Linq;

namespace Standard.Core.DataMappers.DtoInternal
{
    public class SeriesDtoInternalMapper : IDataMapper<SeriesDto, InternalSeries>
    {
        private readonly IDataMapper<ShowCastDto, InternalShowCast> _showCastMapper;
        private readonly IDataMapper<CreatorDto, InternalCreator> _creatorMapper;
        private readonly IDataMapper<GenreDto, InternalSeriesGenre> _genreMapper;
        private readonly IDataMapper<EpisodeSimpleDto, InternalEpisodeSimple> _episodeSimpleMapper;
        private readonly IDataMapper<NetworkDto, InternalNetwork> _networkMapper;
        private readonly IDataMapper<ProductionCompanyDto, InternalProductionCompany> _productionCompanyMapper;
        private readonly IDataMapper<SeasonDto, InternalSeason> _seasonMapper;

        public SeriesDtoInternalMapper(
            IDataMapper<ShowCastDto, InternalShowCast> showCastMapper,
            IDataMapper<CreatorDto, InternalCreator> creatorMapper,
            IDataMapper<GenreDto, InternalSeriesGenre> genreMapper,
            IDataMapper<EpisodeSimpleDto, InternalEpisodeSimple> episodeSimpleMapper,
            IDataMapper<NetworkDto, InternalNetwork> networkMapper,
            IDataMapper<ProductionCompanyDto, InternalProductionCompany> productionCompanyMapper,
            IDataMapper<SeasonDto, InternalSeason> seasonMapper
            )
        {
            _showCastMapper = showCastMapper;
            _creatorMapper = creatorMapper;
            _genreMapper = genreMapper;
            _episodeSimpleMapper = episodeSimpleMapper;
            _networkMapper = networkMapper;
            _productionCompanyMapper = productionCompanyMapper;
            _seasonMapper = seasonMapper;
        }

        public InternalSeries Map(SeriesDto obj)
        {
            return new InternalSeries()
            {
                Cast = _showCastMapper.Map(obj.Cast),
                Categories = obj.Categories,
                CreatedBy = obj.CreatedBy.Select(x=> _creatorMapper.Map(x)).ToList(),
                Description = obj.Description,
                EpisodeRunTime = obj.EpisodeRunTime,
                FirstAirDate = obj.FirstAirDate,
                Genres = obj.Genres.Select(x=>_genreMapper.Map(x)).ToList(),
                Id = obj.Id,
                LastEpisodeSimpleToAir = _episodeSimpleMapper.Map(obj.LastEpisodeSimpleToAir),
                LastUpdated = obj.LastUpdated,
                Networks = obj.Networks.Select(x=>_networkMapper.Map(x)).ToList(),
                OriginalLanguage = obj.OriginalLanguage,
                Popularity = obj.Popularity,
                ProductionCompanies = obj.ProductionCompanies.Select(x=> _productionCompanyMapper.Map(x)).ToList(),
                Rating = obj.Rating,
                Runtime = obj.Runtime,
                Seasons = obj.Seasons.Select(x=>_seasonMapper.Map(x)).ToList(),
                Status = obj.Status,
                Title = obj.Title,
                TmdbId = obj.TmdbId,
                TotalSeasons = obj.TotalSeasons,
                TvMazeId = obj.TvMazeId,
                Type = obj.Type,
                VoteCount = obj.VoteCount,
                Year = obj.Year
            };
        }

        public SeriesDto Map(InternalSeries obj)
        {
            return new SeriesDto()
            {
                Cast = _showCastMapper.Map(obj.Cast),
                Categories = obj.Categories,
                CreatedBy = obj.CreatedBy.Select(x => _creatorMapper.Map(x)).ToList(),
                Description = obj.Description,
                EpisodeRunTime = obj.EpisodeRunTime,
                FirstAirDate = obj.FirstAirDate,
                Genres = obj.Genres.Select(x => _genreMapper.Map(x)).ToList(),
                Id = obj.Id,
                LastEpisodeSimpleToAir = _episodeSimpleMapper.Map(obj.LastEpisodeSimpleToAir),
                LastUpdated = obj.LastUpdated,
                Networks = obj.Networks.Select(x => _networkMapper.Map(x)).ToList(),
                OriginalLanguage = obj.OriginalLanguage,
                Popularity = obj.Popularity,
                ProductionCompanies = obj.ProductionCompanies.Select(x => _productionCompanyMapper.Map(x)).ToList(),
                Rating = obj.Rating,
                Runtime = obj.Runtime,
                Seasons = obj.Seasons.Select(x => _seasonMapper.Map(x)).ToList(),
                Status = obj.Status,
                Title = obj.Title,
                TmdbId = obj.TmdbId,
                TotalSeasons = obj.TotalSeasons,
                TvMazeId = obj.TvMazeId,
                Type = obj.Type,
                VoteCount = obj.VoteCount,
                Year = obj.Year
            };
        }
    }
}
