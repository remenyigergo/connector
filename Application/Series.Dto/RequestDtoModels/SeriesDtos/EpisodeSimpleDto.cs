namespace Series.Dto.RequestDtoModels.SeriesDtos
{
    public class EpisodeSimpleDto
    {
        public string AirDate { get; }
        public int EpisodeNumber { get; }
        public string Name { get; }
        public string Overview { get; }
        public int SeasonNumber { get; }
        public double? VoteAverage { get; }
        public int VoteCount { get; }
    }
}
