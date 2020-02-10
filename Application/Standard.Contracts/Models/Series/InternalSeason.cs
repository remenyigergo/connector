using System.Collections.Generic;

namespace Standard.Contracts.Models.Series
{
    public class InternalSeason
    {
        //TMDB
        public string Airdate;

        public List<InternalEpisode> Episodes;

        public int EpisodesCount;

        //TVMAZE
        public int Id;

        public string Name;

        public int SeasonNumber;
        public string Summary;
    }
}