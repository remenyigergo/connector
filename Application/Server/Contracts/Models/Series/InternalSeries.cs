using System.Collections.Generic;
using System.Linq;
using Contracts.Models.Series.ExtendClasses;
using Contracts.Models.Series.ExtendClasses.Cast;
using Series.Parsers.Trakt.Models;
using Series.Parsers.Trakt.Models.TraktShowModels;

namespace Contracts.Models.Series
{
    public class InternalSeries
    {
        //TVMAZE
        public string Id { get; set; }
        public string TvMazeId { get; set; }
        public string TmdbId { get; set; }
        public string Title { get; set; }
        public List<InternalSeason> Seasons { get; set; }
        public List<string> Runtime { get; set; }
        public double? Rating { get; set; }
        public string Year { get; set; }
        public List<string> Categories { get; set; }
        public string Description { get; set; }
        public int TotalSeasons { get; set; }
        public string LastUpdated { get; set; }
        public InternalShowCast Cast { get; set; }
        //TODO: EXTERNAL ID FELKÉRÉS

        //TMDB
        public List<InternalCreator> CreatedBy;
        public List<string> EpisodeRunTime;
        public string FirstAirDate;
        public List<InternalGenre> Genres;
        public string OriginalLanguage;
        public InternalEpisodeSimple LastEpisodeSimpleToAir;
        public List<InternalNetwork> Networks;
        public string Popularity;
        public List<InternalProductionCompany> ProductionCompanies;
        public string Status;
        public string Type;
        public int VoteCount;

        public void Merge(InternalSeries from)
        {
            Id = from.TvMazeId;
            TmdbId = from.TmdbId;
            EpisodeRunTime = from.EpisodeRunTime;
            FirstAirDate = from.FirstAirDate;
            Genres = from.Genres;
            LastEpisodeSimpleToAir = from.LastEpisodeSimpleToAir;
            CreatedBy = from.CreatedBy;
            Networks = from.Networks;
            OriginalLanguage = from.OriginalLanguage;
            Popularity = from.Popularity;
            ProductionCompanies = from.ProductionCompanies;
            Rating = from.Rating;
            VoteCount = from.VoteCount;
            Status = from.Status;
            Year = from.Year;
            Type = from.Type;
            OriginalLanguage = from.OriginalLanguage;
            TotalSeasons = from.TotalSeasons;            

            foreach (var season in from.Seasons)
            {
                var matchingSeason = Seasons.FirstOrDefault(t => t.SeasonNumber == season.SeasonNumber);
                if (matchingSeason == null)
                {
                    Seasons.Add(season);
                }
                else
                {
                    matchingSeason.Airdate = season.Airdate;
                    matchingSeason.Name = matchingSeason.Name ?? season.Name;
                    matchingSeason.Summary = matchingSeason.Summary ?? season.Summary;
                    matchingSeason.EpisodesCount = (matchingSeason.EpisodesCount == 0) ? season.EpisodesCount : matchingSeason.EpisodesCount;

                    if (season.Episodes != null && season.Episodes.Count > 0)
                    {
                        foreach (var episode in matchingSeason.Episodes)
                        {
                            var matchingEpisode = season.Episodes.FirstOrDefault(e => e.EpisodeNumber == episode.EpisodeNumber);
                            if (matchingEpisode == null)
                            {
                                season.Episodes.Add(episode);
                            }
                            else
                            {
                                matchingEpisode.Title = matchingEpisode.Title ?? episode.Title;
                                // TODO folytatni
                            }
                        }
                    }
                }
            }
        }
    }
}
