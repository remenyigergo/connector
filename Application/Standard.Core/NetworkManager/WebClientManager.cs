using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Series.Service.Models;
using Standard.Contracts;
using Standard.Contracts.Models.Books;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Requests;
using Standard.Contracts.Requests.Movie;
using Standard.Contracts.Requests.Series;

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
            using (var client = new WebClient())
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
            var c = new HttpClient();
            var json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result<int>>(response);
            return res.Data;
            //return bool.TryParse(response, out var result);
        }

        public async Task<int> Exist<T>(string url, InternalImportRequest body)
        {
            var c = new HttpClient();
            var json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();

            var res = JsonConvert.DeserializeObject<Result<int>>(response);

            return res.Data;
            //return bool.TryParse(response, out var result);
        }

        public async Task<bool> PostMarkAsSeen<T>(string url, InternalMarkRequest body)
        {
            var c = new HttpClient();
            var json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result<bool>>(response);

            return res.Data;
            //return bool.TryParse(response, out var result);
        }

        public async Task<bool> Post<T>(string url, InternalEpisodeStartedModel body)
        {
            var c = new HttpClient();
            var json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result<bool>>(response);

            return res.Data;
            //return bool.TryParse(response, out var result);
        }

        public async Task<Result<bool>> Post<T>(string url, InternalStartedMovieUpdateRequest body)
        {
            var c = new HttpClient();
            var json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result<bool>>(response);

            return res;
            //return bool.TryParse(response, out var result);
        }

        public async Task<bool> SeenMovie<T>(string url, InternalMovieSeenRequest body)
        {
            var c = new HttpClient();
            var json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result<bool>>(response);

            return res.Data;
            //return bool.TryParse(response, out var result);
        }

        public async Task<InternalSeries> GetShowPost<T>(string url, InternalImportRequest body)
        {
            var c = new HttpClient();
            var json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result<InternalSeries>>(response);

            return res.Data;
            //return bool.TryParse(response, out var result);
        }

        public async Task<bool> InsertPrograms(string url, List<string> body)
        {
            var c = new HttpClient();
            var json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result<bool>>(response);
            return res.Data;
            //return bool.TryParse(response, out var result);
        }

        public async Task<bool> UpdateProgramsFollowedRequest(string url, Dictionary<int, int> body)
        {
            var c = new HttpClient();
            var json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result<bool>>(response);

            return res.Data;
            //return bool.TryParse(response, out var result);
        }

        public async Task<List<string>> GetAllPrograms(string url)
        {
            var request = WebRequest.Create(url);
            string text;
            var response = (HttpWebResponse) request.GetResponse();
            request.ContentType = "application/json; charset=utf-8";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = await sr.ReadToEndAsync(); //átírtam asyncra
                var result = JsonConvert.DeserializeObject<Result<List<string>>>(text);
                return result.Data;
            }
        }

        public async Task<Dictionary<string, int>> GetFollowedProgramsByUser(string url)
        {
            var request = WebRequest.Create(url);
            string text;
            var response = (HttpWebResponse) request.GetResponse();
            request.ContentType = "application/json; charset=utf-8";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = await sr.ReadToEndAsync(); //átírtam asyncra
                var result = JsonConvert.DeserializeObject<Result<Dictionary<string, int>>>(text);

                return result.Data;
            }
        }

        public async Task<List<InternalBook>> RecommendBooksByString(string url, string title)
        {
            var c = new HttpClient();
            var json = JsonConvert.SerializeObject(title);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();

            var res = JsonConvert.DeserializeObject<Result<List<InternalBook>>>(response);
            return res.Data;
        }


        //public async Task<string> IsShowExist(string url, InternalImportRequest body)
        //{
        //    HttpClient c = new HttpClient();
        //    string json = JsonConvert.SerializeObject(body);
        //    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        //    var request = await c.PostAsync(new Uri(url), httpContent);

        //    var response = await request.Content.ReadAsStringAsync();

        //    return response;
        //    //return bool.TryParse(response, out var result);
        //}

        public async Task<bool> IsBookModuleActivated(string url, int userid)
        {
            var c = new HttpClient();
            var json = JsonConvert.SerializeObject(userid);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);
            var response = await request.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Result<bool>>(response);

            return result.Data;

            //return bool.TryParse(response, out var result);
        }

        public async Task<int> GetUserIdFromUsername(string url)
        {
            var request = WebRequest.Create(url);
            string text;
            var response = (HttpWebResponse) request.GetResponse();
            request.ContentType = "application/json; charset=utf-8";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Result<int>>(text);

                return res.Data;
            }
        }

        public async Task<List<InternalEpisode>> PreviousEpisodesSeen<T>(string url,
            InternalPreviousEpisodeSeenRequest model)
        {
            var c = new HttpClient();
            var json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await c.PostAsync(new Uri(url), httpContent);

            var response = await request.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result<List<InternalEpisode>>>(response);

            return res.Data;
        }
    }
}