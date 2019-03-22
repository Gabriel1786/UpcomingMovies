using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UpcomingMovies.Core.Models
{
    public class Credits
    {
        [JsonProperty("cast")]
        public List<Cast> Cast { get; set; }
    }
}
