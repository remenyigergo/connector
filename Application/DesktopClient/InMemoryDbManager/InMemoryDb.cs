using System.Collections.Generic;

namespace DesktopClient.InMemoryDbManager
{
    public class InMemoryDb : IInMemoryDb
    {
        protected Dictionary<string, Dictionary<string, string>> cache = new Dictionary<string, Dictionary<string, string>>();

        public Dictionary<string, Dictionary<string, string>> SeriesCache
        {
            get => cache;
        }

        public void Update(string guid, Dictionary<string, string> externalIds)
        {
            cache.Add(guid, externalIds);
        }

    }
}
