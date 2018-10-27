using System.Collections.Generic;


namespace Contracts.Models.Series
{    
    public class InternalSeason
    {
        public int Id;        
        public int SeasonNumber;
        public int EpisodesCount;
        public List<InternalEpisode> Episodes;
        public string Summary;
    }
}
