namespace Standard.Core.Configuration
{
    public interface IServiceConfiguration
    {
        MongoConnection MongoConnection { get; set; }
        GatheringSites GatheringSites { get; set; }
        Endpoints Endpoints { get; set; }
    }
}
