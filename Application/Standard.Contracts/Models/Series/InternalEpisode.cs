using System.Collections.Generic;
using Standard.Contracts.Models.Series.ExtendClasses;

namespace Standard.Contracts.Models.Series
{
    public class InternalEpisode
    {
        //TMDB
        public string AirDate;

        public List<InternalEpisodeCrew> Crew;
        public string Description;
        public int EpisodeNumber;
        public List<InternalEpisodeGuest> GuestStars;

        public string Length;
        public double? Rating;

        public int SeasonNumber;

        //TVMAZE
        public string Title;

        public string TmdbShowId;
        public int VoteCount;
    }
}