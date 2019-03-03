using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpcomingMovies.Core.Models;

namespace UpcomingMovies.Core.Services
{
    public interface IMovieService
    {
        Task<ResponseInfo<MovieListResponse>> GetMoviesAsync(MovieListType category, Dictionary<string, object> parameters = null);
    }
}
