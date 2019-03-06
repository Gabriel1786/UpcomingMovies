using System.Collections.Generic;
using Newtonsoft.Json;

namespace UpcomingMovies.Core.Models
{
    public class VideoResults
    {
        [JsonProperty("results")]
        public List<Video> Videos { get; set; }
    }
}
