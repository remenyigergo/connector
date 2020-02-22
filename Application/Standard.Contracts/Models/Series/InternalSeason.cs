using System.Collections.Generic;

namespace Standard.Contracts.Models.Series
{
    public class InternalSeason
    {
        //TMDB
        public string AirDate { get; set; }
        public List<InternalEpisode> Episodes { get; set; }
        public int EpisodesCount { get; set; }

        //TVMAZE
        public int TvMazeId { get; set; }
        public string Name { get; set; }
        public int SeasonNumber { get; set; }
        public string Summary { get; set; }
    }
}