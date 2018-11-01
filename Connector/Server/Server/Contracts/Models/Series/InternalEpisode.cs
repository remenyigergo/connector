using System.Collections.Generic;
using Series.Parsers.Trakt.Models.TraktShowModels.SeasonModel;

namespace Contracts.Models.Series
{
    public class InternalEpisode
    {
        //TVMAZE
        public string Title;
        public string Length;
        public double? Rating;
        public string Description;
        public int SeasonNumber;
        public int EpisodeNumber;


        //TMDB

        public string Air_date;
        public string Show_id;
        public int Vote_count;
        public List<InternalCrew> Crew;
        public List<InternalGuest> Guest_stars;

    }
}
