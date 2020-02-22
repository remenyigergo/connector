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
using Standard.Core.DataMapping;
using MongoDB.Bson.Serialization;
using Series.DataManagement.MongoDB.Models.Series.MongoSeriesModels;
using Series.Dto.RequestDtoModels.SeriesDtos.EpisodeDtos;
using Series.DataManagement.DtoInternal;

namespace Series.Service.ServiceConfigurationExtensions
{
    public static class RegisterMappersExtension
    {
        public static void RegisterDtoInternalMappers(this IServiceCollection services)
        {
            services.AddSingleton<IDataMapper<EpisodeDto, InternalEpisode>, EpisodeStandardDtoInternalMapper>();
            services.AddSingleton<IDataMapper<EpisodeStartedDto, InternalEpisodeStartedModel>, EpisodeStartedDtoInternalMapper>();
            services.AddSingleton<IDataMapper<ShowCastDto, InternalShowCast>, CastDtoInternalMapper>();
            services.AddSingleton<IDataMapper<CreatorDto, InternalCreator>, CreatedByDtoInternalMapper>();
            services.AddSingleton<IDataMapper<EpisodeSimpleDto, InternalEpisodeSimple>, EpisodeSimpleDtoInternalMapper>();
            services.AddSingleton<IDataMapper<GenreDto, InternalSeriesGenre>, GenreDtoInternalMapper>();
            services.AddSingleton<IDataMapper<NetworkDto, InternalNetwork>, NetworkDtoInternalMapper>();
            services.AddSingleton<IDataMapper<ProductionCompanyDto, InternalProductionCompany>, ProductionCompanyDtoInternalMapper>();
            services.AddSingleton<IDataMapper<SeasonDto, InternalSeason>, SeasonDtoInternalMapper>();
            services.AddSingleton<IDataMapper<SeriesDto, InternalSeries>, SeriesDtoInternalMapper>();
            services.AddSingleton<IDataMapper<EpisodeCrewDto, InternalEpisodeCrew>, CrewDtoInternalMapper>();
            services.AddSingleton<IDataMapper<EpisodeGuestDto, InternalEpisodeGuest>, GuestDtoInternalMapper>();
        }

        public static void RegisterInternalDaoMappers(this IServiceCollection services)
        {
            services.AddSingleton<IDataMapper<InternalSeries, MongoSeriesDao>, SeriesInternalDaoMapper>();
            services.AddSingleton<IDataMapper<InternalEpisodeStartedModel, EpisodeStartedDao>, EpisodeStartedInternalDaoMapper>();
            services.AddSingleton<IDataMapper<InternalShowCast, MongoShowCast>, CastInternalDaoMapper>();
            services.AddSingleton<IDataMapper<InternalCreator, MongoCreator>, CreatedByInternalDaoMapper>();
            services.AddSingleton<IDataMapper<InternalSeriesGenre, MongoSeriesGenre>, GenreInternalDaoMapper>();
            services.AddSingleton<IDataMapper<InternalSeason, MongoSeason>, SeasonInternalDaoMapper>();
            services.AddSingleton<IDataMapper<InternalEpisode, MongoEpisode>, EpisodeStandardInternalDaoMapper>();
            services.AddSingleton<IDataMapper<InternalEpisodeSimple, MongoEpisodeSimple>, EpisodeSimpleInternalDaoMapper>();
            services.AddSingleton<IDataMapper<InternalNetwork, MongoNetwork>, NetworkInternalDaoMapper>();
            services.AddSingleton<IDataMapper<InternalProductionCompany, MongoProductionCompany>, ProductionCompanyInternalDaoMapper>();
            services.AddSingleton<IDataMapper<InternalEpisodeCrew, MongoCrew>, CrewInternalDaoMapper>();
            services.AddSingleton<IDataMapper<InternalEpisodeGuest, MongoEpisodeGuest>, GuestInternalDaoMapper>();
        }

        public static void RegisterSupportedMappers(this IServiceCollection services)
        {
            BsonClassMap.RegisterClassMap<MongoSeriesDao>();
        }
    }
}
