using System;
using Newtonsoft.Json;

namespace UpcomingMovies.Core.Models
{
    public class Cast
    {
        [JsonProperty("character")]
        public string Character { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }
    }
}
