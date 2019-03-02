using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UpcomingMovies.Core.Models
{
    public class Video
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("site")]
        public string Site { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        public string VideoUrl()
        {
            switch (Site)
            {
                case "YouTube":
                    return $"https://www.youtube.com/watch?v={Key}";
            }

            return string.Empty;
        }
    }

    public class VideoResults
    {
        [JsonProperty("results")]
        public List<Video> Videos { get; set; }
    }
}
