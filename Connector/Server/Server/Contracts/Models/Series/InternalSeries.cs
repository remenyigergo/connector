using System.Collections.Generic;

namespace Contracts.Models.Series
{
    public class InternalSeries
    {
        public string Id { get; set; }
        public string SeriesId { get; set; }
        public string Title { get; set; }
        public List<InternalSeason> Seasons { get; set; }
        public string Runtime { get; set; }
        public double? Rating { get; set; }
        public string Year { get; set; }
        public List<string> Categories { get; set; }
        public string Description { get; set; }

        public int TotalSeasons { get; set; }
        public string LastUpdated { get; set; }


    }
}
