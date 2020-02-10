using Newtonsoft.Json;

namespace Standard.Core.NetworkManager
{
    public class PostRequestModel
    {
        public int EpisodeNumber;

        //public string UserId;
        //public string TvMazeId;
        public int SeasonNumber;

        [JsonProperty("Title")] public string ShowTitle;

        //public int WatchedPercentage;
        //public string Date;
    }
}