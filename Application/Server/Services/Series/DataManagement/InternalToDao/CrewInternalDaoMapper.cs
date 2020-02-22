using Series.DataManagement.MongoDB.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Core.DataMapping;

namespace Series.DataManagement.InternalToDao
{
    public class CrewInternalDaoMapper : IDataMapper<InternalEpisodeCrew, MongoCrew>
    {
        public MongoCrew Map(InternalEpisodeCrew obj)
        {
            return new MongoCrew()
            {
                Department = obj.Department,
                Job = obj.Job,
                Name = obj.Name
            };

        }

        public InternalEpisodeCrew Map(MongoCrew obj)
        {
            return new InternalEpisodeCrew()
            {
                Department = obj.Department,
                Job = obj.Job,
                Name = obj.Name
            };
        }
    }
}
