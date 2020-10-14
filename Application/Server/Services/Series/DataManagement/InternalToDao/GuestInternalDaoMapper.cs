using Series.DataManagement.MongoDB.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Series.DataManagement.InternalToDao
{
    public class GuestInternalDaoMapper : IDataMapper<InternalEpisodeGuest, MongoEpisodeGuest>
    {
        public MongoEpisodeGuest Map(InternalEpisodeGuest obj)
        {
            return new MongoEpisodeGuest()
            {
                Character = obj.Character,
                Name = obj.Name
            };
        }

        public InternalEpisodeGuest Map(MongoEpisodeGuest obj)
        {
            return new InternalEpisodeGuest()
            {
                Character = obj.Character,
                Name = obj.Name
            };
        }
    }
}
