using System;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using UpcomingMovies.Core.Models;

namespace UpcomingMovies.Core.ViewModels
{
    public class MovieDetailViewModel : BaseViewModel, IMvxViewModel<Movie>
    {
        readonly IMvxNavigationService _navigationService;
        Movie _movie;

        public MovieDetailViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        // MvvmCross Lifecycle
        void IMvxViewModel<Movie>.Prepare(Movie parameter)
        {
            _movie = parameter;
        }

        // MVVM Properties

        // MVVM Commands

        // Private Methods
    }
}
