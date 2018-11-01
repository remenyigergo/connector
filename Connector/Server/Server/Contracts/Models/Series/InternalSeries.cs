using System.Collections.Generic;
using Series.Parsers.Trakt.Models;
using Series.Parsers.Trakt.Models.TraktShowModels;

namespace Contracts.Models.Series
{
    public class InternalSeries
    {
        //TVMAZE
        public string Id { get; set; }
        public string SeriesId { get; set; }
        public string Title { get; set; }
        public List<InternalSeason> Seasons { get; set; }
        public List<string> Runtime { get; set; }
        public double? Rating { get; set; }
        public string Year { get; set; }
        public List<string> Categories { get; set; }
        public string Description { get; set; }

        public int TotalSeasons { get; set; }
        public string LastUpdated { get; set; }
        //TODO: EXTERNAL ID FELKÉRÉS

        //TMDB
        public List<InternalCreator> Created_by;
        public List<string> Episode_run_time;
        public string First_air_date;
        public List<InternalGenre> Genres;
        public string Original_language;
        public InternalEpisodeSimple LastEpisodeSimpleToAir;
        public List<InternalNetwork> Networks;
        public string Popularity;
        public List<InternalProductionCompany> Production_companies;
        public string Status;
        public string Type;
        public int Vote_count;
    }
}
