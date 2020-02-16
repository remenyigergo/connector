using Microsoft.Extensions.DependencyInjection;
using Series.Dto.RequestDtoModels;
using Series.Dto.RequestDtoModels.SeriesDto;
using Series.Dto.RequestDtoModels.SeriesDtos;
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
            services.AddSingleton<IDataMapper<EpisodeStartedDto, InternalEpisodeStartedModel>, EpisodeStartedDtoInternalMapper>();
            services.AddSingleton<IDataMapper<SeriesDto, InternalSeries>, SeriesDtoInternalMapper>();
            services.AddSingleton<IDataMapper<ShowCastDto, InternalShowCast>, CastDtoInternalMapper>();
            services.AddSingleton<IDataMapper<EpisodeSimpleDto, InternalEpisodeSimple>, EpisodeSimpleDtoInternalMapper>();
            services.AddSingleton<IDataMapper<GenreDto, InternalSeriesGenre>, GenreDtoInternalMapper>();
            services.AddSingleton<IDataMapper<NetworkDto, InternalNetwork>, NetworkDtoInternalMapper>();
            services.AddSingleton<IDataMapper<ProductionCompanyDto, InternalProductionCompany>, ProductionCompanyDtoInternalMapper>();
            services.AddSingleton<IDataMapper<SeasonDto, InternalSeason>, SeasonDtoInternalMapper>();
        }

        public static void RegisterInternalDaoMappers(this IServiceCollection services)
        {
        }
    }
}
