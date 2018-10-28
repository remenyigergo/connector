using System;
using System.Collections.Generic;
using System.Text;
using Series.Parsers.Trakt.Models.TraktShowModels;

namespace Series.Parsers.Trakt.Models
{
    public class TraktShow
    {
        public string Id;
        public List<Creator> Created_by;
        public List<int> Episode_run_time;
        public string First_air_date;
        public List<Genre> Genres;
        public string Original_language;
        public TraktEpisodeSimple LastEpisodeSimpleToAir;
        public string Name;
        public List<Network> Networks;
        public string Overview;
        public string Popularity;
        public List<ProductionCompany> Production_companies;
        public List<TraktSeasonSimple> Seasons;
        public string Status;
        public string Type;
        public string Vote_average;
        public string Vote_count;
    } 
}
