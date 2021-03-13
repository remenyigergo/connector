using System;
using System.Collections.Generic;
using System.Linq;
using Standard.Contracts.Models.Series.ExtendClasses;

namespace Standard.Contracts.Models.Series
{
    public class InternalSeries
    {
        public Dictionary<string, string> ExternalIds { get; set; }
        public string Guid { get; set; }

        //TMDB
        public List<InternalCreator> CreatedBy { get; set; }
        public List<string> EpisodeRunTime { get; set; }
        public string FirstAirDate { get; set; }
        public List<InternalSeriesGenre> Genres { get; set; }
        public InternalEpisodeSimple LastEpisodeSimpleToAir { get; set; }
        public List<InternalNetwork> Networks { get; set; }
        public string OriginalLanguage { get; set; }
        public string Popularity { get; set; }
        public List<InternalProductionCompany> ProductionCompanies { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int VoteCount { get; set; }

        //TVMAZE
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

        public void Merge(InternalSeries from)
        {
            if (from != null)
            {
                Guid = System.Guid.NewGuid().ToString();
                if (ExternalIds == null)
                    ExternalIds = new Dictionary<string, string>();

                ExternalIds = ExternalIds.Union(from.ExternalIds).ToDictionary(x=>x.Key,y=>y.Value);
                
                TvMazeId = string.IsNullOrEmpty(from.TvMazeId) ? TvMazeId : from.TvMazeId;
                TmdbId = string.IsNullOrEmpty(from.TmdbId) ? TmdbId : from.TmdbId;
                EpisodeRunTime = from.EpisodeRunTime ?? EpisodeRunTime;
                FirstAirDate = string.IsNullOrEmpty(from.FirstAirDate) ? FirstAirDate : from.FirstAirDate;
                LastEpisodeSimpleToAir = from.LastEpisodeSimpleToAir ?? LastEpisodeSimpleToAir;
                CreatedBy = from.CreatedBy ?? CreatedBy;
                Networks = from.Networks ?? Networks;
                OriginalLanguage = string.IsNullOrEmpty(from.OriginalLanguage) ? OriginalLanguage : from.OriginalLanguage;
                Popularity = string.IsNullOrEmpty(from.Popularity) ? Popularity : from.Popularity;
                ProductionCompanies = from.ProductionCompanies ?? ProductionCompanies;
                Rating = from.Rating ?? Rating;
                VoteCount = from.VoteCount <= 0 ? VoteCount : from.VoteCount;
                Status = string.IsNullOrEmpty(from.Status) ? Status : from.Status;
                Year = string.IsNullOrEmpty(from.Year) ? Year : from.Year;
                Type = string.IsNullOrEmpty(from.Type) ? Type : from.Type;
                OriginalLanguage = string.IsNullOrEmpty(from.OriginalLanguage) ? OriginalLanguage : from.OriginalLanguage;
                TotalSeasons = from.TotalSeasons <= 0 && from.TotalSeasons != TotalSeasons ? TotalSeasons : from.TotalSeasons;

                MergeGenreCategory();
                MergeSeasons(from);
            }
        }

        private void MergeSeasons(InternalSeries from)
        {
            foreach (var season in from.Seasons)
            {
                var matchingSeason = Seasons.FirstOrDefault(t => t.SeasonNumber == season.SeasonNumber);
                if (matchingSeason == null)
                {
                    Seasons.Add(season);
                }
                else
                {
                    matchingSeason.AirDate = string.IsNullOrEmpty(matchingSeason.AirDate) ? season.AirDate : matchingSeason.AirDate;
                    matchingSeason.Name = matchingSeason.Name ?? season.Name;
                    matchingSeason.Summary = matchingSeason.Summary ?? season.Summary;
                    matchingSeason.EpisodesCount = matchingSeason.EpisodesCount == 0
                        ? season.EpisodesCount
                        : matchingSeason.EpisodesCount;

                    if (season.Episodes != null && season.Episodes.Count > 0 && matchingSeason.Episodes != null)
                    {
                        MergeEpisodes(matchingSeason, season);
                    }
                }
            }
        }

        private void MergeEpisodes(InternalSeason actualMatchingSeason, InternalSeason fromSeason)
        {
            foreach (var episode in actualMatchingSeason.Episodes)
            {
                var matchingEpisode =
                    fromSeason.Episodes.FirstOrDefault(e => e.EpisodeNumber == episode.EpisodeNumber);
                if (matchingEpisode == null)
                {
                    fromSeason.Episodes.Add(episode);
                }
                else
                {
                    matchingEpisode.Title = matchingEpisode.Title ?? episode.Title;
                    matchingEpisode.Length = matchingEpisode.Length ?? episode.Length;
                    matchingEpisode.Rating = matchingEpisode.Rating ?? episode.Rating;
                    matchingEpisode.Description = matchingEpisode.Description ?? episode.Description;
                    if (matchingEpisode.SeasonNumber == 0 && matchingEpisode.EpisodeNumber == 0)
                    {
                        matchingEpisode.SeasonNumber = episode.SeasonNumber;
                        matchingEpisode.EpisodeNumber = episode.EpisodeNumber;
                    }
                    matchingEpisode.AirDate = matchingEpisode.AirDate ?? episode.AirDate;
                    matchingEpisode.TmdbShowId = matchingEpisode.TmdbShowId ?? episode.AirDate;
                    if (matchingEpisode.VoteCount == 0 && episode.VoteCount > 0)
                        matchingEpisode.VoteCount = episode.VoteCount;
                    matchingEpisode.Crew = matchingEpisode.Crew ?? episode.Crew;
                    matchingEpisode.GuestStars = matchingEpisode.GuestStars ?? episode.GuestStars;
                }
            }
        }

        private void MergeGenreCategory()
        {
            var genreList = new HashSet<InternalSeriesGenre>();
            Categories?.ForEach(x => genreList.Add(new InternalSeriesGenre(x)));
            if (Genres == null)
                Genres = new List<InternalSeriesGenre>();
            Genres = Genres.Union(genreList).ToList();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is InternalSeries series))
                return false;

            return Title.Equals(series.Title);
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }
}