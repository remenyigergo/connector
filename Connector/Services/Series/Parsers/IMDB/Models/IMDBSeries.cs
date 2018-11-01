using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Series.Parsers.IMDB.Models
{
    public class IMDBSeries
    {
        public string Title;
        public List<string> Runtime;
        public int TotalSeasons;
        public string Actors;
        public string ImdbId;
        public double ImdbRating;
        public string Plot;
        public string Year;
        public string Genre;
    }
}
