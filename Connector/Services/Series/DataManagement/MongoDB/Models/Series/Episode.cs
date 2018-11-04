﻿using System.Collections.Generic;
using Series.Parsers.Trakt.Models.TraktShowModels.SeasonModel;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class Episode
    {
        public string Title;
        public string Length;
        public double? Rating;
        //public List<string> Cast;
        public string Description;
        public int SeasonNumber;
        public int EpisodeNumber;


        public string AirDate;
        public string TmdbShowId;
        public int VoteCount;
        public List<InternalCrew> Crew;
        public List<InternalGuest> GuestStars;
    }
}
