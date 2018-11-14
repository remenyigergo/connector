﻿using Standard.Contracts.Models.Series.ExtendClasses;
using System.Collections.Generic;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class MongoEpisode
    {
        public string Title;
        public string Length;
        public double? Rating;
        //public List<string> MongoCast;
        public string Description;
        public int SeasonNumber;
        public int EpisodeNumber;


        public string AirDate;
        public string TmdbShowId;
        public int VoteCount;
        public List<InternalEpisodeCrew> Crew;
        public List<InternalEpisodeGuest> GuestStars;
    }
}
