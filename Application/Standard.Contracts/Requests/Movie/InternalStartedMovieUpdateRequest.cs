using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Contracts.Requests.Movie
{
    public class InternalStartedMovieUpdateRequest
    {
        public int UserId;
        public string Title;
        public int? TmdbId;
        public string ImdbId;
        public int HoursElapsed;
        public int MinutesElapsed;
        public int SecondsElapsed;
        public DateTime Date;
        public double WatchedPercentage;
    }
}
