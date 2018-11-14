using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Standard.Core.NetworkManager
{
    public class PostRequestModel
    {
        [JsonProperty("Title")]
        public string ShowTitle;
        //public string UserId;
        //public string TvMazeId;
        public int SeasonNumber;
        public int EpisodeNumber;
        //public string Date;
        //public int WatchedPercentage;
    }
}
