using System.Collections.Generic;
using Series.Parsers.TMDB.Models.TmdbShowModels.SeasonModel;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;

namespace Series.Parsers.TMDB.Models.TmdbShowModels.ConvertHelper
{
    public static class InternalConverter
    {
        public static List<InternalSeason> ConvertTmdbSeasonToInternalSeason(List<TmdbSeason> tmdbSeasons)
        {
            List<InternalSeason> seasonsConverted = new List<InternalSeason>();

            foreach (var tmdbSeason in tmdbSeasons)
            {
                InternalSeason internalSeason = new InternalSeason();
                internalSeason.Id = tmdbSeason.Id;
                internalSeason.EpisodesCount = tmdbSeason.Episodes.Count;
                internalSeason.SeasonNumber = tmdbSeason.SeasonNumber;
                internalSeason.Summary = tmdbSeason.Overview;
                internalSeason.Name = tmdbSeason.Name;
                internalSeason.Airdate = tmdbSeason.AirDate;

                List<InternalEpisode> episodesConverted = new List<InternalEpisode>();

                foreach (var tmdbEpisode in tmdbSeason.Episodes)
                    episodesConverted.Add(new InternalEpisode
                    {
                        Title = tmdbEpisode.Name,
                        Length = "",
                        Rating = tmdbEpisode.VoteAverage,
                        Description = tmdbEpisode.Overview,
                        SeasonNumber = tmdbEpisode.SeasonNumber,
                        EpisodeNumber = tmdbEpisode.EpisodeNumber,

                        AirDate = tmdbEpisode.AirDate,
                        TmdbShowId = tmdbEpisode.ShowId,
                        VoteCount = tmdbEpisode.VoteCount,
                        Crew = ConvertTmdbCrewToInternal(tmdbEpisode.Crew),
                        GuestStars = ConvertTmdbGuestsToInternal(tmdbEpisode.GuestStars)
                    });
                internalSeason.Episodes = episodesConverted;
                seasonsConverted.Add(internalSeason);
            }

            return seasonsConverted;
        }

        public static List<InternalCreator> ConvertTmdbCreatorsToInternal(List<Creator> creators)
        {
            List<InternalCreator> internalCreatorList = new List<InternalCreator>();

            foreach (var creator in creators)
                internalCreatorList.Add(new InternalCreator
                {
                    Name = creator.Name
                });

            return internalCreatorList;
        }

        public static List<InternalSeriesGenre> ConvertTmdbGenreToInternal(List<Genre> genres)
        {
            List<InternalSeriesGenre> internalGenreList = new List<InternalSeriesGenre>();

            foreach (var genre in genres)
                internalGenreList.Add(new InternalSeriesGenre(genre.Name));

            return internalGenreList;
        }

        public static InternalEpisodeSimple ConvertTmdbEpisodeToInternal(TmdbEpisodeSimple tmdbEpisode)
        {
            return new InternalEpisodeSimple
            {
                Air_date = tmdbEpisode.AirDate,
                Episode_number = tmdbEpisode.EpisodeNumber,
                Season_number = tmdbEpisode.SeasonNumber,
                Name = tmdbEpisode.Name,
                Overview = tmdbEpisode.Overview,
                Vote_average = tmdbEpisode.VoteAverage,
                Vote_count = tmdbEpisode.VoteCount
            };
        }

        public static List<InternalEpisodeCrew> ConvertTmdbCrewToInternal(List<Crew> crew)
        {
            List<InternalEpisodeCrew> crewList = new List<InternalEpisodeCrew>();
            foreach (var crewMember in crew)
                crewList.Add(new InternalEpisodeCrew
                {
                    Name = crewMember.Name,
                    Department = crewMember.Department,
                    Job = crewMember.Job
                });
            return crewList;
        }

        public static List<InternalEpisodeGuest> ConvertTmdbGuestsToInternal(List<Guest> guests)
        {
            List<InternalEpisodeGuest> guestList = new List<InternalEpisodeGuest>();
            foreach (var guest in guests)
                guestList.Add(new InternalEpisodeGuest
                {
                    Name = guest.Name,
                    Character = guest.Character
                });
            return guestList;
        }

        public static List<InternalNetwork> ConvertTmdbNetworkToInternal(List<Network> networks)
        {
            List<InternalNetwork> networkList = new List<InternalNetwork>();
            foreach (var network in networks)
                networkList.Add(new InternalNetwork
                {
                    Name = network.Name,
                    Origin_country = network.OriginCountry
                });
            return networkList;
        }

        public static List<InternalProductionCompany> ConvertTmdbProductionCompanyToInternal(
            List<ProductionCompany> companies)
        {
            List<InternalProductionCompany> companyList = new List<InternalProductionCompany>();

            foreach (var productionCompany in companies)
                companyList.Add(new InternalProductionCompany
                {
                    Name = productionCompany.Name,
                    Origin_country = productionCompany.OriginCountry
                });
            return companyList;
        }
    }
}