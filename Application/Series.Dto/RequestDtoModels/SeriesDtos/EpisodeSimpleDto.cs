namespace Series.Dto.RequestDtoModels.SeriesDtos
{
    public class EpisodeSimpleDto
    {
        public string AirDate { get; set; }
        public int EpisodeNumber { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public int SeasonNumber { get; set; }
        public double? VoteAverage { get; set; }
        public int VoteCount { get; set; }
    }
}
