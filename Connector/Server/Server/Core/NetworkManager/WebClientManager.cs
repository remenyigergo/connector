using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.NetworkManager
{
    public class WebClientManager
    {
        public async Task<T> Get<T>(string url)
        {
            WebClient c = new WebClient();
            var data = await c.DownloadStringTaskAsync(new Uri(url));
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
