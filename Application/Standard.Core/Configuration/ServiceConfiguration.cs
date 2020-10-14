namespace Standard.Core.Configuration
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        public MongoConnection MongoConnection { get; set; }
        public GatheringSites GatheringSites { get; set; }
        public Endpoints Endpoints { get; set; }
    }
}
