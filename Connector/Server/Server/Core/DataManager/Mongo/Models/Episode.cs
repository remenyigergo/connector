using System;
using System.Collections.Generic;
using System.Text;
using Core.DataManager.Mongo.IRepository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Series.Parsers.Trakt.Models.TraktShowModels.SeasonModel;

namespace Core.DataManager.Mongo.Models
{
    public class Episode
    {
        public string Title;
        public string Length;
        public double? Rating;
        //public List<string> Cast;
        public string Description;

        public string Air_date;
        public string TMDB_Show_id;
        public int Vote_count;
        public List<InternalCrew> Crew;
        public List<InternalGuest> Guest_stars;
    }
}
