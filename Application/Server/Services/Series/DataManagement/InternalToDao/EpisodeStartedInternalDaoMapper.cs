using Series.Service.Models;
using Standard.Contracts.Models.Series;
using Standard.Core.DataMapping;

namespace Series.DataManagement.InternalToDao
{
    public class EpisodeStartedInternalDaoMapper : IDataMapper<InternalEpisodeStartedModel, EpisodeStartedDao>
    {
        public EpisodeStartedDao Map(InternalEpisodeStartedModel obj)
        {
            throw new System.NotImplementedException();
        }

        public InternalEpisodeStartedModel Map(EpisodeStartedDao obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
