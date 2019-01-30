using System.Collections.Generic;
using Standard.Contracts.Models.Series.ExtendClasses;

namespace Standard.Contracts.Models.Series
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
        public string AirDate;
        public string TmdbShowId;
        public int VoteCount;
        public List<InternalEpisodeCrew> Crew;
        public List<InternalEpisodeGuest> GuestStars;

    }
}
