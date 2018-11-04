using System.Collections.Generic;
using Series.Parsers.TMDB.Models.TmdbShowModels;

namespace Series.Parsers.TMDB.Models
{
    public class TmdbShow
    {
        public string Id;
        public List<Creator> Created_by;
        public List<string> Episode_run_time;
        public string First_air_date;
        public List<Genre> Genres;
        public string Original_language;
        public TmdbEpisodeSimple Last_Episode_To_Air;
        public string Name;
        public List<Network> Networks;
        public string Overview;
        public string Popularity;
        public List<ProductionCompany> Production_companies;
        public List<TmdbSeasonSimple> Seasons;
        public string Status;
        public string Type;
        public double? Vote_average;
        public int Vote_count;
    } 
}
