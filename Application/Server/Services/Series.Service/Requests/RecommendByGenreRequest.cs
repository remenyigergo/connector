using Standard.Contracts.Models.Series.ExtendClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Series.Service.Requests
{
    public class RecommendByGenreRequest
    {
        public List<string> Genres;
        public string username;
        public int userid;
    }
}
