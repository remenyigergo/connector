using System.Collections.Generic;

namespace Series.Dto.RequestDtoModels
{
    public class RecommendByGenreRequestDto
    {
        public List<string> Genres;
        public int userid;
        public string username;
    }
}