using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.DataManagement.MongoDB.Models
{
    public class Rate
    {
        public int UserId;
        public int? TmdbId;
        public int? ImdbId;
        public int Rating;
    }
}
