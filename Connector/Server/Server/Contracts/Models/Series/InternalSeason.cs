using System.Collections.Generic;
using Series.Parsers.Trakt.Models.TraktShowModels.SeasonModel;


namespace Contracts.Models.Series
{    
    public class InternalSeason
    {
        //TVMAZE
        public int Id;        
        public int SeasonNumber;
        public int EpisodesCount;
        public List<InternalEpisode> Episodes;
        public string Summary;

        //TMDB
        public string Airdate;
        public string Name;
    }


}
