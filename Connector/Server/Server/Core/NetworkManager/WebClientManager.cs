using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Contracts.Requests;
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

        public async Task<string> Post<T>(string url, PostRequestModel body)
        {
            HttpClient c = new HttpClient();

            try
            {
                string json = JsonConvert.SerializeObject(body);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var request = await c.PostAsync(new Uri(url), httpContent);

                var response = await request.Content.ReadAsStringAsync();

                return response;
            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken.IsCancellationRequested)
                {
                    return "cancelledout";
                }
                else
                {
                    return "timed out";
                }
            }
            
            //var data = await c.DownloadStringTaskAsync(new Uri(url));
            //return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<string> PostCheckShowExist<T>(string url, InternalImportRequest body)
        {
            
            HttpClient c = new HttpClient();
            string json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();

            return response;
        }
    }
}
