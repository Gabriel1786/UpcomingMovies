using System;
using MvvmCross.Navigation;

namespace UpcomingMovies.Core.ViewModels
{
    public class MovieDetailViewModel : BaseViewModel
    {
        readonly IMvxNavigationService _navigationService;

        public MovieDetailViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        // MvvmCross Lifecycle

        // MVVM Properties

        // MVVM Commands

        // Private Methods
    }
}
