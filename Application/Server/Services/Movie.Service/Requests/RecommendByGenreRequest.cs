using System.Collections.Generic;

namespace Movie.Service.Requests
{
    public class RecommendByGenreRequest
    {
        public List<string> Genres;
        public int UserId;
    }
}