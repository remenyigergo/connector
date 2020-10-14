using System.Collections.Generic;

namespace DesktopClient.InMemoryDbManager
{
    public interface IInMemoryDb
    {
        Dictionary<string,Dictionary<string, string>> SeriesCache { get; }
        void Update(string guid, Dictionary<string,string> externalIds);
    }
}
