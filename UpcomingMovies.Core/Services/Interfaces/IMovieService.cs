using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpcomingMovies.Core.Models;
using UpcomingMovies.Core.Models.Dto;

namespace UpcomingMovies.Core.Services
{
    public interface IMovieService
    {
        Task<ResponseInfo<MovieListResponse>> GetMoviesAsync(MovieListType category, Dictionary<string, object> parameters = null);
    }
}
