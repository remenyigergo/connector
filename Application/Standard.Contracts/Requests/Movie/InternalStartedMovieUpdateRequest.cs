using System;

namespace Standard.Contracts.Requests.Movie
{
    public class InternalStartedMovieUpdateRequest
    {
        public DateTime Date;
        public int HoursElapsed;
        public string ImdbId;
        public int MinutesElapsed;
        public int SecondsElapsed;
        public string Title;
        public int? TmdbId;
        public int UserId;
        public double WatchedPercentage;
    }
}