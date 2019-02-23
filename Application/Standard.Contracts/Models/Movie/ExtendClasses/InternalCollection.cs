using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Contracts.Models.Movie.ExtendClasses
{
    public class InternalCollection
    {
        public int? Id;
        public string Name;
        public string PosterPath;
        public string BackdropPath;
    }
}
