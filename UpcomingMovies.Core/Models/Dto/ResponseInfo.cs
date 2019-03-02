using System;

namespace UpcomingMovies.Core.Models.Dto
{
    public class ResponseInfo
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
    }

    public class ResponseInfo<T> : ResponseInfo
    {
        public T Result { get; set; }
    }
}
