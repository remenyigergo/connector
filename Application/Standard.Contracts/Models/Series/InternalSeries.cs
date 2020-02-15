using System.Collections.Generic;
using System.Linq;
using Standard.Contracts.Models.Series.ExtendClasses;

namespace Standard.Contracts.Models.Series
{
    public class InternalSeries
    {
        //TODO: EXTERNAL ID FELKÉRÉS

        //TMDB
        public List<InternalCreator> CreatedBy;

        public List<string> EpisodeRunTime;
        public string FirstAirDate;
        public List<InternalSeriesGenre> Genres;
        public InternalEpisodeSimple LastEpisodeSimpleToAir;
        public List<InternalNetwork> Networks;
        public string OriginalLanguage;
        public string Popularity;
        public List<InternalProductionCompany> ProductionCompanies;
        public string Status;
        public string Type;

        public int VoteCount;

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

        public void Merge(InternalSeries from)
        {
            Id = from.TvMazeId;
            TmdbId = from.TmdbId;
            EpisodeRunTime = from.EpisodeRunTime;
            FirstAirDate = from.FirstAirDate;
            //Genres = from.Genres;
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

            //Mergeljük a Categoriest és Genret
            var genreList = new List<InternalSeriesGenre>();
            foreach (var category in Categories)
            {
                var genre = new InternalSeriesGenre(category);
                genreList.Add(genre);
            }
            Genres = genreList;

            if (from.Genres.Count > 0)
                foreach (var internalGenre in from.Genres)
                    if (!Genres.Contains(internalGenre))
                        Genres.Add(internalGenre);


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
                    matchingSeason.EpisodesCount = matchingSeason.EpisodesCount == 0
                        ? season.EpisodesCount
                        : matchingSeason.EpisodesCount;

                    if (season.Episodes != null && season.Episodes.Count > 0)
                        foreach (var episode in matchingSeason.Episodes)
                        {
                            var matchingEpisode =
                                season.Episodes.FirstOrDefault(e => e.EpisodeNumber == episode.EpisodeNumber);
                            if (matchingEpisode == null)
                            {
                                season.Episodes.Add(episode);
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
                                if (matchingEpisode.VoteCount == 0)
                                    matchingEpisode.VoteCount = episode.VoteCount;
                                matchingEpisode.Crew = matchingEpisode.Crew ?? episode.Crew;
                                matchingEpisode.GuestStars = matchingEpisode.GuestStars ?? episode.GuestStars;

                                // TODO folytatni
                            }
                        }
                }
            }
        }


        public override bool Equals(object obj)
        {
            var series = obj as InternalSeries;

            if (series == null)
                return false;

            return Title.Equals(series.Title);
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }
}