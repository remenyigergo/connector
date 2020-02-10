using System.Collections.Generic;
using Series.Parsers.TMDB.Models.TmdbShowModels;

namespace Series.Parsers.TvMaze.Models
{
    public class TvMazeShow
    {
        public TvMazeShowCast Cast;
        public List<string> ExternalIds;
        public List<string> Genres;
        public string Id;
        public string Language;
        public string Name;
        public TvMazeRating Rating;
        public string Runtime;
        public string Status;
        public string Summary;
        public string Updated;
    }
}