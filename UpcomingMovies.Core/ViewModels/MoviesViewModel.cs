using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using UpcomingMovies.Core.Models;
using UpcomingMovies.Core.Services;

namespace UpcomingMovies.Core.ViewModels
{
    public class MoviesViewModel : BaseViewModel
    {
        readonly IMvxNavigationService _navigationService;
        readonly IMovieService _movieService;

        int _currentPage = 1;
        bool _canLoadMore = true;

        public MoviesViewModel(IMvxNavigationService navigationService, IMovieService movieService)
        {
            _navigationService = navigationService;
            _movieService = movieService;

            Movies = new MvxObservableCollection<Movie>();

            ShowMovieDetailViewModelCommand = new MvxAsyncCommand<Movie>(ShowMovieDetailView);
            LoadMoreMoviesCommand = new MvxCommand(() =>
            {
                LoadMoreMoviesTask = MvxNotifyTask.Create(LoadMovies);
                RaisePropertyChanged(() => LoadMoreMoviesTask);
            });
        }

        // MvvmCross Lifecycle
        public override Task Initialize()
        {
            var task = base.Initialize();
            LoadInitialMoviesTask = MvxNotifyTask.Create(LoadMovies);
            return task;
        }

        // MVVM Properties
        MvxObservableCollection<Movie> _movies;
        public MvxObservableCollection<Movie> Movies
        {
            get => _movies;
            set => SetProperty(ref _movies, value);
        }

        public MvxNotifyTask LoadInitialMoviesTask { get; private set; }
        public MvxNotifyTask LoadMoreMoviesTask { get; private set; }

        // MVVM Commands
        public IMvxAsyncCommand<Movie> ShowMovieDetailViewModelCommand { get; }
        public IMvxCommand LoadMoreMoviesCommand { get; }

        // Private Methods
        async Task LoadMovies()
        {
            if (!_canLoadMore)
                return;

            var responseInfo = await _movieService.GetMoviesAsync(MovieListType.Upcoming, new Dictionary<string, object>
            {
                { "page", _currentPage }
            });

            if (responseInfo.IsSuccess)
            {
                Movies.AddRange(responseInfo.Result.Movies);

                _currentPage++;

                if (_currentPage > responseInfo.Result.TotalPages)
                    _canLoadMore = false;
            }
        }

        async Task ShowMovieDetailView(Movie movie)
        {
            await _navigationService.Navigate<MovieDetailViewModel, Movie>(movie);
        }
    }
}
