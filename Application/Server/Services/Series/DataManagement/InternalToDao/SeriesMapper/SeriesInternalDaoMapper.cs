using Series.DataManagement.MongoDB.Models.Series;
using Series.DataManagement.MongoDB.Models.Series.MongoSeriesModels;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;
using System.Linq;

namespace Series.DataManagement.InternalToDao.SeriesMapper
{
    public class SeriesInternalDaoMapper : IDataMapper<InternalSeries, MongoSeriesDao>
    {
        private readonly IDataMapper<InternalShowCast, MongoShowCast> _castMapper;
        private readonly IDataMapper<InternalCreator, MongoCreator> _createdByMapper;
        private readonly IDataMapper<InternalSeriesGenre, MongoSeriesGenre> _genreMapper;
        private readonly IDataMapper<InternalSeason, MongoSeason> _seasonMapper;
        private readonly IDataMapper<InternalEpisodeSimple, MongoEpisodeSimple> _episodeSimpleMapper;
        private readonly IDataMapper<InternalNetwork, MongoNetwork> _networkMapper;
        private readonly IDataMapper<InternalProductionCompany, MongoProductionCompany> _productionCompanyMapper;

        public SeriesInternalDaoMapper(
                IDataMapper<InternalShowCast,MongoShowCast> castMapper,
                IDataMapper<InternalCreator, MongoCreator> createdByMapper,
                IDataMapper<InternalSeriesGenre, MongoSeriesGenre> genreMapper,
                IDataMapper<InternalSeason, MongoSeason> seasonMapper,
                IDataMapper<InternalEpisodeSimple, MongoEpisodeSimple> episodeSimpleMapper,
                IDataMapper<InternalNetwork, MongoNetwork> networkMapper,
                IDataMapper<InternalProductionCompany, MongoProductionCompany> productionCompanyMapper
            )
        {
            _castMapper = castMapper;
            this._createdByMapper = createdByMapper;
            this._genreMapper = genreMapper;
            this._seasonMapper = seasonMapper;
            this._episodeSimpleMapper = episodeSimpleMapper;
            this._networkMapper = networkMapper;
            this._productionCompanyMapper = productionCompanyMapper;
        }

        public MongoSeriesDao Map(InternalSeries obj)
        {
            return new MongoSeriesDao()
            {
                Guid = obj.Guid,
                ExternalIds = obj.ExternalIds,
                Title = obj.Title,
                Cast = _castMapper.Map(obj.Cast),
                Categories = obj.Categories,
                CreatedBy = obj.CreatedBy.Select(x=>_createdByMapper.Map(x)).ToList(),
                Description = obj.Description,
                EpisodeRunTime = obj.EpisodeRunTime,
                FirstAirDate = obj.FirstAirDate,
                Genres = obj.Genres.Select(x=>_genreMapper.Map(x)).ToList(),
                LastEpisodeSimpleToAir = _episodeSimpleMapper.Map(obj.LastEpisodeSimpleToAir),
                LastUpdated = obj.LastUpdated,
                Networks = obj.Networks.Select(x=>_networkMapper.Map(x)).ToList(),
                OriginalLanguage = obj.OriginalLanguage,
                Popularity = obj.Popularity,
                ProductionCompanies = obj.ProductionCompanies.Select(x=>_productionCompanyMapper.Map(x)).ToList(),
                Rating = obj.Rating,
                Runtime = obj.Runtime,
                Seasons = obj.Seasons.Select(x=>_seasonMapper.Map(x)).ToList(),
                Status = obj.Status,
                TmdbId = obj.TmdbId,
                TotalSeasons = obj.TotalSeasons,
                TvMazeId = obj.TvMazeId,
                Type = obj.Type,
                VoteCount = obj.VoteCount,
                Year = obj.Year
            };
        }

        public InternalSeries Map(MongoSeriesDao obj)
        {
            return new InternalSeries() {
                Guid = obj.Guid,
                ExternalIds = obj.ExternalIds,
                Cast = _castMapper.Map(obj.Cast),
                Categories = obj.Categories,
                CreatedBy = obj.CreatedBy.Select(x=>_createdByMapper.Map(x)).ToList(),
                Description = obj.Description,
                EpisodeRunTime = obj.EpisodeRunTime,
                FirstAirDate = obj.FirstAirDate,
                Genres = obj.Genres.Select(x => _genreMapper.Map(x)).ToList(),
                LastEpisodeSimpleToAir = _episodeSimpleMapper.Map(obj.LastEpisodeSimpleToAir),
                LastUpdated = obj.LastUpdated,
                Networks = obj.Networks.Select(x=>_networkMapper.Map(x)).ToList(),
                OriginalLanguage = obj.OriginalLanguage,
                Popularity = obj.Popularity,
                ProductionCompanies = obj.ProductionCompanies.Select(x=>_productionCompanyMapper.Map(x)).ToList(),
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
    }
}
