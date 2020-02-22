using NUnit.Framework;
using Series.DataManagement.DtoInternal;
using Series.Dto.RequestDtoModels;
using Series.Dto.RequestDtoModels.SeriesDto;
using Series.Dto.RequestDtoModels.SeriesDtos;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Contracts.Models.Series.ExtendClasses.Cast;
using System.Collections.Generic;

namespace UnitTest.Series.MapperTests
{
    class SeriesDtoInternalMapperTests
    {
        [Test]
        public void SeriesDtoInternalMap()
        {
            #region Required Mappers

            var showCastMapper = new CastDtoInternalMapper();
            var creatorMapper = new CreatedByDtoInternalMapper();
            var genreMapper = new GenreDtoInternalMapper();
            var episodeSimpleMapper = new EpisodeSimpleDtoInternalMapper();
            var networkMapper = new NetworkDtoInternalMapper();
            var productionCompanyMapper = new ProductionCompanyDtoInternalMapper();


            var crewMapper = new CrewDtoInternalMapper();
            var guestMapper = new GuestDtoInternalMapper();
            var episodeStandardMapper = new EpisodeStandardDtoInternalMapper(crewMapper, guestMapper);
            var seasonMapper = new SeasonDtoInternalMapper(episodeStandardMapper);

            #endregion

            var seriesMapper = new SeriesDtoInternalMapper(showCastMapper, creatorMapper, genreMapper, episodeSimpleMapper, networkMapper, productionCompanyMapper, seasonMapper);

            var result = seriesMapper.Map(GetNewSeriesDto());

            Assert.AreEqual(GetExpectedInternalSeries(), result);
        }


        public SeriesDto GetNewSeriesDto()
        {
            return new SeriesDto()
            {
                Cast = new ShowCastDto
                {
                    Persons = new List<ActorDto>() { new ActorDto() {
                        CharacterName = "Harry",
                        RealName = "Peter"
                    }
                    }
                },
                Categories = new List<string>() { "Horror" },
                CreatedBy = new List<CreatorDto>() { new CreatorDto()
                {
                    Name = "creator"
                } },
                Description = "description",
                EpisodeRunTime = new List<string>() { "1" },
                FirstAirDate = "2012.1.1",
                Genres = new List<GenreDto>() { new GenreDto() { Name = "genre1" } },
                LastEpisodeSimpleToAir = new EpisodeSimpleDto()
                {
                    AirDate= "2000.1.1",
                    EpisodeNumber = 1,
                    Name = "name",
                    Overview = "overview",
                    SeasonNumber = 1,
                    VoteAverage = 1,
                    VoteCount = 1
                },
                LastUpdated = "2000.1.1",
                Networks = new List<NetworkDto> () { new NetworkDto() { Name = "network", OriginCountry = "us" } },
                OriginalLanguage = "us",
                Popularity = "popularity",
                ProductionCompanies = new List<ProductionCompanyDto> () { new ProductionCompanyDto () { Name = "productionCompany", OriginCountry = "us"} },
                Rating = 1,
                Runtime = new List<string>() { "1" },
                Seasons = new List<SeasonDto> () { new SeasonDto()
                {
                    Airdate = "2000.1.1",
                    Episodes = new List<EpisodeDto>()
                    {
                        new EpisodeDto ()  {}
                    },
                    EpisodesCount = 1,
                    Name = "pilot",
                    SeasonNumber = 1,
                    Summary = "summary",
                    TvMazeId = 1
                } },
                Status = "status",
                Title = "title",
                TmdbId = "1",
                TotalSeasons = 1,
                TvMazeId = "1",
                Type = "type",
                VoteCount = 1,
                Year = "2019"
            };
        }

        public InternalSeries GetExpectedInternalSeries()
        {
            return new InternalSeries()
            {
                Cast = new InternalShowCast
                {
                    Persons = new List<InternalActor>() { new InternalActor() {
                        CharacterName = "Harry",
                        RealName = "Peter"
                    }
                    }
                },
                Categories = new List<string>() { "Horror" },
                CreatedBy = new List<InternalCreator>() { new InternalCreator()
                {
                    Name = "creator"
                } },
                Description = "description",
                EpisodeRunTime = new List<string>() { "1" },
                FirstAirDate = "2012.1.1",
                Genres = new List<InternalSeriesGenre>() { new InternalSeriesGenre("genre1") },
                LastEpisodeSimpleToAir = new InternalEpisodeSimple()
                {
                    Air_date = "2000.1.1",
                    Episode_number = 1,
                    Name = "name",
                    Overview = "overview",
                    Season_number = 1,
                    Vote_average = 1,
                    Vote_count = 1
                },
                LastUpdated = "2000.1.1",
                Networks = new List<InternalNetwork>() { new InternalNetwork() { Name = "network", Origin_country = "us" } },
                OriginalLanguage = "us",
                Popularity = "popularity",
                ProductionCompanies = new List<InternalProductionCompany>() { new InternalProductionCompany() { Name = "productionCompany", Origin_country = "us" } },
                Rating = 1,
                Runtime = new List<string>() { "1" },
                Seasons = new List<InternalSeason>() { new InternalSeason()
                {
                    AirDate = "2000.1.1",
                    Episodes = new List<InternalEpisode>()
                    {
                        new InternalEpisode ()  {}
                    },
                    EpisodesCount = 1,
                    Name = "pilot",
                    SeasonNumber = 1,
                    Summary = "summary",
                    TvMazeId = 1
                } },
                Status = "status",
                Title = "title",
                TmdbId = "1",
                TotalSeasons = 1,
                TvMazeId = "1",
                Type = "type",
                VoteCount = 1,
                Year = "2019"
            };
        }
    }
}
