using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace UpcomingMovies.Core.Services
{
    public class HttpClientService : IHttpClientService
    {
        HttpClient _client;

        public HttpClientService()
        {
            _client = new HttpClient();
            _client.Timeout = TimeSpan.FromSeconds(10);
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return _client.GetAsync(url);
        }
    }
}
