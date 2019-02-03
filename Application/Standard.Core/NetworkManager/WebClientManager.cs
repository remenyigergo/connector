using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Standard.Contracts.Requests;
using Newtonsoft.Json;
using Series.Service.Models;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO;
using Standard.Contracts.Models.Books;

namespace Standard.Core.NetworkManager
{
    public class WebClientManager
    {
        public async Task<T> Get<T>(string url)
        {
            using (var c = new WebClient())
            {
                var data = await c.DownloadStringTaskAsync(new Uri(url));
                return JsonConvert.DeserializeObject<T>(data);
            }
        }

        public async Task<string> GetFeliratokInfoHtml(string url)
        {
            using (WebClient client = new WebClient())
            {
                var data = await client.DownloadStringTaskAsync(new Uri(url));
                return data;
            }
        }

        //public async Task<string> Post<T>(string url, PostRequestModel body)
        //{
        //    HttpClient c = new HttpClient();

        //    try
        //    {
        //        string json = JsonConvert.SerializeObject(body);
        //        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        //        var request = await c.PostAsync(new Uri(url), httpContent);

        //        var response = await request.Content.ReadAsStringAsync();                

        //        return response;
        //    }
        //    catch (TaskCanceledException ex)
        //    {
        //        if (ex.CancellationToken.IsCancellationRequested)
        //        {
        //            return "cancelledout";
        //        }
        //        else
        //        {
        //            return "timed out";
        //        }
        //    }
            
        //    //var data = await c.DownloadStringTaskAsync(new Uri(url));
        //    //return JsonConvert.DeserializeObject<T>(data);
        //}

        public async Task<int> Post<T>(string url, InternalImportRequest body)
        {
            
            HttpClient c = new HttpClient();
            string json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();

            return Int32.Parse(response);
            //return bool.TryParse(response, out var result);
        }

        public async Task<string> PostMarkAsSeen<T>(string url, InternalMarkRequest body)
        {

            HttpClient c = new HttpClient();
            string json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();

            return response;
            //return bool.TryParse(response, out var result);
        }

        public async Task<bool> Post<T>(string url, InternalEpisodeStartedModel body)
        {

            HttpClient c = new HttpClient();
            string json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();

            return bool.Parse(response);
            //return bool.TryParse(response, out var result);
        }

        public async Task<string> GetShowPost<T>(string url, InternalImportRequest body)
        {

            HttpClient c = new HttpClient();
            string json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();

            return response;
            //return bool.TryParse(response, out var result);
        }

        public async Task<string> InsertPrograms(string url, List<string> body)
        {
            HttpClient c = new HttpClient();
            string json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();

            return response;
            //return bool.TryParse(response, out var result);
        }

        public async Task<string> UpdateProgramsFollowedRequest(string url, Dictionary<int, int> body)
        {
            HttpClient c = new HttpClient();
            string json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();

            return response;
            //return bool.TryParse(response, out var result);
        }

        public async Task<List<string>> GetAllPrograms(string url)
        {
            var request = WebRequest.Create(url);
            string text;
            var response = (HttpWebResponse)request.GetResponse();
            request.ContentType = "application/json; charset=utf-8";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
                var result = JsonConvert.DeserializeObject<List<string>>(text);

                return result;
            }
        }

        public async Task<Dictionary<string, int>> GetFollowedProgramsByUser(string url)
        {
            var request = WebRequest.Create(url);
            string text;
            var response = (HttpWebResponse)request.GetResponse();
            request.ContentType = "application/json; charset=utf-8";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
                var result = JsonConvert.DeserializeObject<Dictionary<string, int>>(text);

                return result;
            }
        }

        public async Task<List<InternalBook>> RecommendBooksByString(string url)
        {
            var request = WebRequest.Create(url);
            string text;
            var response = (HttpWebResponse)request.GetResponse();
            request.ContentType = "application/json; charset=utf-8";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
                var result = JsonConvert.DeserializeObject<List<InternalBook>>(text);

                return result;
            }
        }

    }
}
