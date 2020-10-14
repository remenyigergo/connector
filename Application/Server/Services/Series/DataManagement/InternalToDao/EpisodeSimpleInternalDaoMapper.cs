using Series.DataManagement.MongoDB.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Series.DataManagement.InternalToDao
{
    public class EpisodeSimpleInternalDaoMapper : IDataMapper<InternalEpisodeSimple, MongoEpisodeSimple>
    {
        public MongoEpisodeSimple Map(InternalEpisodeSimple obj)
        {
            return new MongoEpisodeSimple()
            {
                AirDate = obj.Air_date,
                EpisodeNumber = obj.Episode_number,
                Name = obj.Name,
                Overview = obj.Overview,
                SeasonNumber = obj.Season_number,
                VoteAverage = obj.Vote_average,
                VoteCount = obj.Vote_count
            };
        }

        public InternalEpisodeSimple Map(MongoEpisodeSimple obj)
        {
            return new InternalEpisodeSimple() {
                Air_date = obj.AirDate,
                Episode_number = obj.EpisodeNumber,
                Name = obj.Name,
                Overview = obj.Overview,
                Season_number = obj.SeasonNumber,
                Vote_average = obj.VoteAverage,
                Vote_count = obj.VoteCount
            };
        }
    }
}
