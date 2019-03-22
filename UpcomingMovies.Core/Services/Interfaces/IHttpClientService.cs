using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace UpcomingMovies.Core.Services
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
