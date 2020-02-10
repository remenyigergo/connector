using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Contracts.Requests.Movie
{
    public class InternalMovieRateRequest
    {
        public int? TmdbId;
        public int? ImdbId;
        public int Rating;
        public int UserId;
    }
}
