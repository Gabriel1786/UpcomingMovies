using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UpcomingMovies.Core.Models
{
    public class MovieListResponse
    {
        [JsonProperty("results")]
        public List<Movie> Movies { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
    }
}
