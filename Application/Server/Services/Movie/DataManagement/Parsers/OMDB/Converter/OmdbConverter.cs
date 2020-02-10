using System.Collections.Generic;
using Movie.DataManagement.Parsers.OMDB.Models.OmdbMovieExtendModels;
using Standard.Contracts.Models.Movie.ExtendClasses;

namespace Movie.DataManagement.Parsers.OMDB.Converter
{
    public class OmdbConverter
    {
        public List<InternalGenre> SplitGenre(string genres)
        {
            var splitGenres = genres.Split(',');
            var genreList = new List<InternalGenre>();

            foreach (var splitGenre in splitGenres)
                genreList.Add(new InternalGenre
                {
                    //ID NINCS
                    Name = splitGenre
                });

            return genreList;
        }

        public List<string> SplitString(string stringToSplit)
        {
            var splitGenres = stringToSplit.Split(',');
            var list = new List<string>();

            foreach (var splitGenre in splitGenres)
                list.Add(splitGenre);

            return list;
        }

        public List<InternalRating> ConvertOmdbToInternalRatings(List<Rating> ratings)
        {
            var internalRatings = new List<InternalRating>();
            foreach (var rating in ratings)
                internalRatings.Add(new InternalRating
                {
                    Source = rating.Source,
                    Value = rating.Value
                });

            return internalRatings;
        }
    }
}