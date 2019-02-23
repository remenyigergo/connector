using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.DataManagement.Parsers.TMDB.Models
{
    public class TmdbQueryMovies
    {
        public string Page;
        public List<TmdbMovieSimple> Results;
    }
}
