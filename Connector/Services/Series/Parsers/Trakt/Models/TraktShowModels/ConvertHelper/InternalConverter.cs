using System;
using System.Collections.Generic;
using System.Text;
using Contracts.Models.Series;
using Series.Parsers.Trakt.Models.TraktShowModels.SeasonModel;

namespace Series.Parsers.Trakt.Models.TraktShowModels.ConvertHelper
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
                internalSeason.SeasonNumber = tmdbSeason.Season_number;
                internalSeason.Summary = tmdbSeason.Overview;
                internalSeason.Name = tmdbSeason.Name;
                internalSeason.Airdate = tmdbSeason.Air_date;

                List<InternalEpisode> episodesConverted = new List<InternalEpisode>();

                foreach (var tmdbEpisode in tmdbSeason.Episodes)
                {
                    episodesConverted.Add(new InternalEpisode()
                    {
                        Title = tmdbEpisode.Name,
                        SeasonNumber = tmdbEpisode.Season_number,
                        EpisodeNumber = tmdbEpisode.Episode_number,
                        Description = tmdbEpisode.Overview,
                        Length = "",
                        Rating = tmdbEpisode.Vote_average
                    });
                }
                internalSeason.Episodes = episodesConverted;
                seasonsConverted.Add(internalSeason);
            }

            return seasonsConverted;
        }

        public static List<InternalCreator> ConvertTmdbCreatorsToInternal(List<Creator> creators)
        {
            List<InternalCreator> internalCreatorList = new List<InternalCreator>();

            foreach (var creator in creators)
            {
                internalCreatorList.Add(new InternalCreator()
                {
                    Name = creator.Name
                });
            }

            return internalCreatorList;
        }

        public static List<InternalGenre> ConvertTmdbGenreToInternal(List<Genre> genres)
        {
            List<InternalGenre> internalGenreList = new List<InternalGenre>();

            foreach (var genre in genres)
            {
                internalGenreList.Add(new InternalGenre()
                {
                    Name = genre.Name
                });
            }

            return internalGenreList;
        }

        public static InternalEpisodeSimple ConvertTmdbEpisodeToInternal(TmdbEpisodeSimple tmdbEpisode)
        {
            return new InternalEpisodeSimple()
            {
                Air_date = tmdbEpisode.Air_date,
                Episode_number = tmdbEpisode.Episode_number,
                Season_number = tmdbEpisode.Season_number,
                Name = tmdbEpisode.Name,
                Overview = tmdbEpisode.Overview,
                Vote_average = tmdbEpisode.Vote_average,
                Vote_count = tmdbEpisode.Vote_count
            };
        }

        public static List<InternalCrew> ConvertTmdbCrewToInternal(List<Crew> crew)
        {
            List<InternalCrew> crewList =new List<InternalCrew>();
            foreach (var crewMember in crew)
            {
                crewList.Add(new InternalCrew()
                {
                    Name = crewMember.Name,
                    Department = crewMember.Department,
                    Job = crewMember.Job
                });
            }
            return crewList;
        }

        public static List<InternalGuest> ConvertTmdbGuestsToInternal(List<Guest> guests)
        {
            List<InternalGuest> guestList = new List<InternalGuest>();
            foreach (var guest in guests)
            {
                guestList.Add(new InternalGuest()
                {
                    Name = guest.Name,
                    Character = guest.Character
                });
            }
            return guestList;
        }

        public static List<InternalNetwork> ConvertTmdbNetworkToInternal(List<Network> networks)
        {
            List<InternalNetwork> networkList = new List<InternalNetwork>();
            foreach (var network in networks)
            {
                networkList.Add(new InternalNetwork()
                {
                    Name = network.Name,
                    Origin_country = network.Origin_country
                });
            }
            return networkList;
        }

        public static List<InternalProductionCompany> ConvertTmdbProductionCompanyToInternal(List<ProductionCompany> companies)
        {
            List<InternalProductionCompany> companyList= new List<InternalProductionCompany>();

            foreach (var productionCompany in companies)
            {
                companyList.Add(new InternalProductionCompany()
                {
                    Name = productionCompany.Name,
                    Origin_country = productionCompany.Origin_country
                });
            }
            return companyList;
        }
    }
}
