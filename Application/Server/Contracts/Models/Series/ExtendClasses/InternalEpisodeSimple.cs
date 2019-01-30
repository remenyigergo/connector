using System;
using System.Collections.Generic;
using System.Text;

namespace Series.Parsers.Trakt.Models
{
    public class InternalEpisodeSimple
    {
        public string Air_date;
        public int Episode_number;
        public int Season_number;
        public string Name;
        public string Overview;
        public double? Vote_average;
        public int Vote_count;
    }
}
