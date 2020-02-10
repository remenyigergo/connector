using System.Collections.Generic;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class MongoEpisode
    {
        public string AirDate;
        public List<InternalEpisodeCrew> Crew;

        //public List<string> MongoCast;
        public string Description;

        public int EpisodeNumber;
        public List<InternalEpisodeGuest> GuestStars;
        public string Length;

        public double? Rating;

        public int SeasonNumber;
        public string Title;
        public string TmdbShowId;
        public int VoteCount;
    }
}