using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json;
using UpcomingMovies.Core.Configurations;
using UpcomingMovies.Core.Models;
using UpcomingMovies.Core.Models.Dto;

namespace UpcomingMovies.Core.Services
{
    public class MovieService : IMovieService
    {
        HttpClient _httpClient;

        public MovieService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ResponseInfo<MovieListResponse>> GetMoviesAsync(MovieListType type, Dictionary<string, object> parameters = null)
        {
            var api = $"/movie/{MovieListTypeToString(type)}";

            var url = Url.Combine(AppConfigurations.ApiUrl, AppConfigurations.ApiVersion, api)
                         .SetQueryParams(parameters ?? new Dictionary<string, object>());

            var result = new ResponseInfo<MovieListResponse>();
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    result.Result = JsonConvert.DeserializeObject<MovieListResponse>(responseString);
                }
                else
                {
                    result.Error = "Service unavailable.";
                }
            }
            catch
            {
                result.Error = "Uh oh, something went wrong!";
            }

            return result;
        }

        string MovieListTypeToString(MovieListType type)
        {
            switch (type)
            {
                case MovieListType.Latest:
                    return "latest";
                case MovieListType.NowPlaying:
                    return "now_playing";
                case MovieListType.Popular:
                    return "popular";
                case MovieListType.TopRated:
                    return "top_rated";
                case MovieListType.Upcoming:
                    return "upcoming";
            }

            return string.Empty;
        }
    }
}
