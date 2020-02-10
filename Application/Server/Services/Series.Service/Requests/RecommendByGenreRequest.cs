using System.Collections.Generic;

namespace Series.Service.Requests
{
    public class RecommendByGenreRequest
    {
        public List<string> Genres;
        public int userid;
        public string username;
    }
}