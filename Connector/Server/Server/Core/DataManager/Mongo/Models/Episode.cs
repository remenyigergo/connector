using System;
using System.Collections.Generic;
using System.Text;
using Core.DataManager.Mongo.IRepository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.DataManager.Mongo.Models
{
    public class Episode
    {
        public string Title;
        public string Length;
        public double? Rating;
        public List<string> Cast;
        public string Description;
    }
}
