using System;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using UpcomingMovies.Core.Models;
using UpcomingMovies.Core.Services;

namespace UpcomingMovies.Core.ViewModels
{
    public class MovieDetailViewModel : BaseViewModel, IMvxViewModel<Movie>
    {
        readonly IMvxNavigationService _navigationService;
        readonly IMovieService _movieService;

        public MovieDetailViewModel(IMvxNavigationService navigationService, IMovieService movieService)
        {
            _navigationService = navigationService;
            _movieService = movieService;
        }

        // MvvmCross Lifecycle
        void IMvxViewModel<Movie>.Prepare(Movie parameter)
        {
            Movie = parameter;
            LoadMovieDetailTask = MvxNotifyTask.Create(LoadMovieDetail);
        }

        // MVVM Properties
        Movie _movie;
        public Movie Movie
        {
            get => _movie;
            set => SetProperty(ref _movie, value);
        }

        // MVVM Commands
        public MvxNotifyTask LoadMovieDetailTask { get; private set; }

        // Private Methods
        async Task LoadMovieDetail()
        {
            var responseInfo = await _movieService.GetMovieDetailAsync(Movie.Id.ToString());
            if (responseInfo.IsSuccess)
            {
                Movie = responseInfo.Result;
            }
        }
    }
}
