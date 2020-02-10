using System.Collections.Generic;

namespace Movie.DataManagement.Parsers.TMDB.Models
{
    public class TmdbQueryMovies
    {
        public string Page;
        public List<TmdbMovieSimple> Results;
    }
}