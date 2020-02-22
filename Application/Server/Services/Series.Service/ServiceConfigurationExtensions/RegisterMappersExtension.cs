using Microsoft.Extensions.DependencyInjection;
using Series.DataManagement.InternalToDao;
using Series.DataManagement.InternalToDao.SeriesMapper;
using Series.DataManagement.MongoDB.Models.Series;
using Series.Dto.RequestDtoModels;
using Series.Dto.RequestDtoModels.SeriesDto;
using Series.Dto.RequestDtoModels.SeriesDtos;
using Series.Service.Models;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMappers.DtoInternal;
using Standard.Core.DataMapping;

namespace Series.Service.ServiceConfigurationExtensions
{
    public static class RegisterMappersExtension
    {
        public static void RegisterDtoInternalMappers(this IServiceCollection services)
        {
            services.AddSingleton<IDataMapper<EpisodeDto, InternalEpisode>, EpisodeStandardDtoInternalMapper>();
            services.AddSingleton<IDataMapper<EpisodeStartedDto, InternalEpisodeStartedModel>, EpisodeStartedDtoInternalMapper>();
            services.AddSingleton<IDataMapper<ShowCastDto, InternalShowCast>, CastDtoInternalMapper>();
            services.AddSingleton<IDataMapper<CreatorDto, InternalCreator>, CreatedByDtoInternal>();
            services.AddSingleton<IDataMapper<EpisodeSimpleDto, InternalEpisodeSimple>, EpisodeSimpleDtoInternalMapper>();
            services.AddSingleton<IDataMapper<GenreDto, InternalSeriesGenre>, GenreDtoInternalMapper>();
            services.AddSingleton<IDataMapper<NetworkDto, InternalNetwork>, NetworkDtoInternalMapper>();
            services.AddSingleton<IDataMapper<ProductionCompanyDto, InternalProductionCompany>, ProductionCompanyDtoInternalMapper>();
            services.AddSingleton<IDataMapper<SeasonDto, InternalSeason>, SeasonDtoInternalMapper>();
            services.AddSingleton<IDataMapper<SeriesDto, InternalSeries>, SeriesDtoInternalMapper>();
        }

        public static void RegisterInternalDaoMappers(this IServiceCollection services)
        {
            services.AddSingleton<IDataMapper<InternalSeries, MongoSeriesDao>, SeriesInternalDaoMapper>();
            services.AddSingleton<IDataMapper<InternalEpisodeStartedModel, EpisodeStartedDao>, EpisodeStartedInternalDaoMapper>();
        }
    }
}
