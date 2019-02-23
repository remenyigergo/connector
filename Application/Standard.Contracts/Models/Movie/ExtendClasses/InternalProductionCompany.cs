using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Standard.Contracts.Models.Movie.ExtendClasses
{
    public class InternalProductionCompany
    {
        public int Id;
        public string LogoPath;
        public string Name;
        public string OriginCountry;
    }
}
