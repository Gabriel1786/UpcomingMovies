using System;
using System.Collections.Generic;

namespace UpcomingMovies.Core.Models
{
    public class MovieStateContainer
    {
        public MovieListType MovieListType { get; set; }

        public List<Movie> Movies { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public string Query { get; set; }

        public MovieStateContainer()
        {
            Movies = new List<Movie>();
            CurrentPage = 1;
        }

        public bool CanLoadMore()
        {
            return CurrentPage <= TotalPages || TotalPages == 0;
        }
    }
}
