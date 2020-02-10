using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Service.Requests
{
    public class RecommendByGenreRequest
    {
        public int UserId;
        public List<string> Genres;
    }
}
