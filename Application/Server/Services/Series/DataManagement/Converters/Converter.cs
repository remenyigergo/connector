using System;
using System.Collections.Generic;
using System.Text;
using Series.DataManagement.MongoDB.Models.Series;
using Series.DataManagement.MongoDB.SeriesFunctionModels;
using Standard.Contracts.Models.Series;

namespace Series.DataManagement.Converters
{
    public class Converter
    {
        /// <summary>
        /// Sorozat átalakítás:   Mongo -> Internal  TODO ezt is kiszedni külön fájlba
        /// </summary>
        /// <param name="mongoSeries"></param>
        /// <returns></returns>
        public InternalSeries ConvertMongoToInternalSeries(MongoSeries mongoSeries)
        {
            //var seasons = ConvertInternalSeasonToMongoSeason(mongoSeries.Seasons);

            var internalSeries = new InternalSeries()
            {
                Title = mongoSeries.Title,
                TvMazeId = mongoSeries.TvMazeId,
                TmdbId = mongoSeries.TmdbId,
                Runtime = mongoSeries.Runtime,
                TotalSeasons = mongoSeries.TotalSeasons,
                Categories = mongoSeries.Categories,
                Description = mongoSeries.Description,
                Rating = mongoSeries.Rating,
                Year = mongoSeries.Year,
                LastUpdated = mongoSeries.LastUpdated,
                Seasons = ConvertMongoSeasonToInternalSeason(mongoSeries.Seasons),
                Cast = mongoSeries.Cast,
                CreatedBy = mongoSeries.CreatedBy,
                EpisodeRunTime = mongoSeries.EpisodeRunTime,
                FirstAirDate = mongoSeries.FirstAirDate,
                Genres = mongoSeries.Genres,
                OriginalLanguage = mongoSeries.OriginalLanguage,
                LastEpisodeSimpleToAir = mongoSeries.LastEpisodeSimpleToAir,
                Networks = mongoSeries.Networks,
                Popularity = mongoSeries.Popularity,
                ProductionCompanies = mongoSeries.ProductionCompanies,
                Status = mongoSeries.Status,
                Type = mongoSeries.Type,
                VoteCount = mongoSeries.VoteCount
            };
            return internalSeries;
        }


        /// <summary>
        /// Évad átalakítás:  Internal -> Mongo    TODO ezt szerintem ki lehetne szedni külön fájlba
        /// </summary>
        /// <param name="internalSeasons"></param>
        public List<InternalSeason> ConvertMongoSeasonToInternalSeason(List<MongoSeason> mongoSeasons)
        {
            var seasonsList = new List<InternalSeason>();
            foreach (var mongoseason in mongoSeasons)
            {
                var episodeList = new List<InternalEpisode>();
                foreach (var episode in mongoseason.Episodes)
                {
                    episodeList.Add(new InternalEpisode()
                    {
                        Title = episode.Title,
                        Length = episode.Length,
                        Rating = episode.Rating,
                        Description = episode.Description,
                        SeasonNumber = episode.SeasonNumber,
                        EpisodeNumber = episode.EpisodeNumber,
                        AirDate = episode.AirDate,
                        TmdbShowId = episode.TmdbShowId,
                        VoteCount = episode.VoteCount,
                        Crew = episode.Crew,
                        GuestStars = episode.GuestStars
                    });
                }

                seasonsList.Add(new InternalSeason()
                {
                    SeasonNumber = mongoseason.SeasonNumber,
                    EpisodesCount = mongoseason.EpisodesCount,
                    Episodes = episodeList,
                    Summary = mongoseason.Summary,
                    Airdate = mongoseason.Airdate,
                    Name = mongoseason.Name
                });
            }
            return seasonsList;
        }


        public InternalEpisode ConvertMongoToInternalEpisode(MongoEpisode mongoEpisode)
        {
            var internalEpisode = new InternalEpisode()
            {
                Title = mongoEpisode.Title,
                Length = mongoEpisode.Length,
                Rating = mongoEpisode.Rating,
                Description = mongoEpisode.Description,
                SeasonNumber = mongoEpisode.SeasonNumber,
                EpisodeNumber = mongoEpisode.EpisodeNumber,
                AirDate = mongoEpisode.AirDate,
                TmdbShowId = mongoEpisode.TmdbShowId,
                Crew = mongoEpisode.Crew,
                GuestStars = mongoEpisode.GuestStars,
                VoteCount = mongoEpisode.VoteCount
            };

            return internalEpisode;
        }

        public InternalEpisodeSeen ConvertMongoToInternalEpisode(EpisodeSeen mongoEpisode)
        {
            var internalEpisode = new InternalEpisodeSeen()
            {
                UserId = mongoEpisode.UserId,
                EpisodeNumber = mongoEpisode.EpisodeNumber,
                SeasonNumber = mongoEpisode.SeasonNumber,
                TmdbId = mongoEpisode.TmdbId,
                TvMazeId = mongoEpisode.TvMazeId
            };

            return internalEpisode;
        }

        /// <summary>
        /// Sorozat átalakítás:   Internal -> Mongo TODO kiszedni
        /// </summary>
        /// <param name="internalSeries"></param>
        /// <returns></returns>
        public MongoSeries ConvertInternalToMongoSeries(InternalSeries internalSeries)
        {
            var seasons = ConvertInternalSeasonToMongoSeason(internalSeries.Seasons);

            var mongoSeries = new MongoSeries()
            {
                TvMazeId = internalSeries.TvMazeId,
                TmdbId = internalSeries.TmdbId,
                Title = internalSeries.Title,
                //TvMazeId = internalSeries.Id,
                Runtime = internalSeries.Runtime,
                TotalSeasons = internalSeries.TotalSeasons,
                Categories = internalSeries.Categories,
                Description = internalSeries.Description,
                Rating = internalSeries.Rating,
                Year = internalSeries.Year,
                LastUpdated = internalSeries.LastUpdated,
                Seasons = seasons,
                Cast = internalSeries.Cast,
                CreatedBy = internalSeries.CreatedBy,
                EpisodeRunTime = internalSeries.EpisodeRunTime,
                FirstAirDate = internalSeries.FirstAirDate,
                Genres = internalSeries.Genres,
                OriginalLanguage = internalSeries.OriginalLanguage,
                LastEpisodeSimpleToAir = internalSeries.LastEpisodeSimpleToAir,
                Networks = internalSeries.Networks,
                Popularity = internalSeries.Popularity,
                ProductionCompanies = internalSeries.ProductionCompanies,
                Status = internalSeries.Status,
                Type = internalSeries.Type,
                VoteCount = internalSeries.VoteCount
            };
            return mongoSeries;
        }


        /// <summary>
        /// Évad átalakítás:  Internal -> Mongo    TODO ezt szerintem ki lehetne szedni külön fájlba
        /// </summary>
        /// <param name="internalSeasons"></param>
        public List<MongoSeason> ConvertInternalSeasonToMongoSeason(List<InternalSeason> internalSeasons)
        {
            var seasonsList = new List<MongoSeason>();
            foreach (var internalSeason in internalSeasons)
            {
                var episodeList = new List<MongoEpisode>();
                foreach (var episode in internalSeason.Episodes)
                {
                    episodeList.Add(new MongoEpisode()
                    {
                        Title = episode.Title,
                        Length = episode.Length,
                        Rating = episode.Rating,
                        Description = episode.Description,
                        SeasonNumber = episode.SeasonNumber,
                        EpisodeNumber = episode.EpisodeNumber,
                        AirDate = episode.AirDate,
                        TmdbShowId = episode.TmdbShowId,
                        VoteCount = episode.VoteCount,
                        Crew = episode.Crew,
                        GuestStars = episode.GuestStars
                    });
                }

                seasonsList.Add(new MongoSeason()
                {
                    SeasonNumber = internalSeason.SeasonNumber,
                    EpisodesCount = internalSeason.EpisodesCount,
                    Episodes = episodeList,
                    Summary = internalSeason.Summary,
                    Airdate = internalSeason.Airdate,
                    Name = internalSeason.Name
                });
            }
            return seasonsList;
        }
    }
}
