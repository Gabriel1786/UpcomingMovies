using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        MovieStateContainer _currentStateContainer;
        Dictionary<MovieListType, MovieStateContainer> _cachedMovies = new Dictionary<MovieListType, MovieStateContainer>();

        public MoviesViewModel(IMvxNavigationService navigationService, IMovieService movieService)
        {
            _navigationService = navigationService;
            _movieService = movieService;

            Movies = new MvxObservableCollection<Movie>();

            ShowMovieDetailViewModelCommand = new MvxAsyncCommand<Movie>(ShowMovieDetailView);
            SwitchMovieListTypeCommand = new MvxCommand<MovieListType>(SetMovieListToType);
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
            SetMovieListToType(MovieListType.NowPlaying); //Setting first list type to load
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
        public IMvxCommand SwitchMovieListTypeCommand { get; }

        // Private Methods
        async Task LoadMovies()
        {
            if (!_currentStateContainer.CanLoadMore())
                return;

            var responseInfo = await _movieService.GetMoviesAsync(_currentStateContainer.MovieListType, new Dictionary<string, object>
            {
                { "page", _currentStateContainer.CurrentPage }
            });

            if (responseInfo.IsSuccess)
            {
                Movies.AddRange(responseInfo.Result.Movies);
                _currentStateContainer.Movies.AddRange(responseInfo.Result.Movies);

                _currentStateContainer.CurrentPage++;
                _currentStateContainer.TotalPages = responseInfo.Result.TotalPages;
            }

            //TODO: could load more but failed, alert user?
        }

        MovieStateContainer GetMovieStateContainerForType(MovieListType movieListType)
        {
            if (_cachedMovies.ContainsKey(movieListType))
                return _cachedMovies[movieListType];

            var stateContainer = new MovieStateContainer { MovieListType = movieListType };
            _cachedMovies.Add(movieListType, stateContainer);
            return stateContainer;
        }

        void SetMovieListToType(MovieListType movieListType)
        {
            _currentStateContainer = GetMovieStateContainerForType(movieListType);
            Movies.Clear();
            Movies.AddRange(_currentStateContainer.Movies);

            switch (movieListType)
            {
                case MovieListType.Latest:
                    Title = "Latest Movies";
                    break;
                case MovieListType.NowPlaying:
                    Title = "Now Playing";
                    break;
                case MovieListType.Popular:
                    Title = "Popular Movies";
                    break;
                case MovieListType.TopRated:
                    Title = "Top Rated Movies";
                    break;
                case MovieListType.Upcoming:
                    Title = "Upcoming Movies";
                    break;
            }
        }

        async Task ShowMovieDetailView(Movie movie)
        {
            await _navigationService.Navigate<MovieDetailViewModel, Movie>(movie);
        }
    }
}
