﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json;
using UpcomingMovies.Core.Configurations;
using UpcomingMovies.Core.Models;

namespace UpcomingMovies.Core.Services
{
    public class MovieService : IMovieService
    {
        IHttpClientService _httpClientService;

        public MovieService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<ResponseInfo<MovieListResponse>> GetMoviesAsync(MovieListType type, Dictionary<string, object> parameters = null)
        {
            var url = BuildUrl($"/movie/{MovieListTypeToString(type)}", parameters);

            var result = new ResponseInfo<MovieListResponse>();
            try
            {
                var response = await _httpClientService.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    result.Result = JsonConvert.DeserializeObject<MovieListResponse>(responseString);
                    result.IsSuccess = true;
                }
                else
                {
                    result.Error = UiMessages.ServiceUnavailable;
                }
            }
            catch (Exception e)
            {
                result.Error = e.Message;
            }

            return result;
        }

        public async Task<ResponseInfo<Movie>> GetMovieDetailAsync(string id, Dictionary<string, object> parameters = null)
        {
            var url = BuildUrl($"/movie/{id}", parameters);
            url = Url.Combine(url).SetQueryParam("append_to_response", "videos,credits");

            var result = new ResponseInfo<Movie>();
            try
            {
                var response = await _httpClientService.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    result.Result = JsonConvert.DeserializeObject<Movie>(responseString);
                    result.IsSuccess = true;
                }
                else
                {
                    result.Error = UiMessages.ServiceUnavailable;
                }
            }
            catch (Exception e)
            {
                result.Error = e.Message;
            }

            return result;
        }

        public async Task<ResponseInfo<MovieListResponse>> SearchMoviesAsync(Dictionary<string, object> parameters = null)
        {
            var url = BuildUrl($"/search/movie", parameters);

            var result = new ResponseInfo<MovieListResponse>();
            try
            {
                var response = await _httpClientService.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    result.Result = JsonConvert.DeserializeObject<MovieListResponse>(responseString);
                    result.IsSuccess = true;
                }
                else
                {
                    result.Error = UiMessages.ServiceUnavailable;
                }
            }
            catch (Exception e)
            {
                result.Error = e.Message;
            }

            return result;
        }

        string BuildUrl(string api, Dictionary<string, object> parameters = null)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();

            parameters.Add("api_key", AppConfig.ApiKey);

            var url = Url.Combine(AppConfig.ApiUrl, AppConfig.ApiVersion, api)
                         .SetQueryParams(parameters);

            return url;
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
