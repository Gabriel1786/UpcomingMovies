using System;
using MvvmCross.Navigation;

namespace UpcomingMovies.Core.ViewModels
{
    public class MoviesViewModel : BaseViewModel
    {
        readonly IMvxNavigationService _navigationService;

        public MoviesViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        // MvvmCross Lifecycle

        // MVVM Properties

        // MVVM Commands

        // Private Methods
    }
}
