using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Core.NetworkManager
{
    public class PostRequestModel
    {
        [JsonProperty("Title")]
        public string ShowTitle;
        //public string UserId;
        //public string SeriesId;
        public int SeasonNumber;
        public int EpisodeNumber;
        //public string Date;
        //public int WatchedPercentage;
    }
}
