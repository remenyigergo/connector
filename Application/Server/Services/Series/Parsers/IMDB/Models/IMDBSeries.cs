using System.Collections.Generic;

namespace Series.Parsers.IMDB.Models
{
    public class IMDBSeries
    {
        public string Actors;
        public string Genre;
        public string ImdbId;
        public double ImdbRating;
        public string Plot;
        public List<string> Runtime;
        public string Title;
        public int TotalSeasons;
        public string Year;
    }
}