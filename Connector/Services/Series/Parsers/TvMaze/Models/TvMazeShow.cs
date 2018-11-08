using System.Collections.Generic;
using Series.Parsers.TMDB.Models.TmdbShowModels;

namespace Series.Parsers.TvMaze.Models
{
    public class TvMazeShow
    {
        public string Id;
        public string Runtime;
        public string Name;
        public List<string> Genres;
        public List<string> ExternalIds;
        public TvMazeRating Rating;
        public string Language;
        public string Status;
        public string Summary;
        public string Updated;
        public TvMazeShowCast Cast;
    }
}