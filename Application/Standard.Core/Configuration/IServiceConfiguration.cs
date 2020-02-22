namespace Standard.Core.Configuration
{
    public interface IServiceConfiguration
    {
        MongoConnection Connection { get; set; }
    }
}
