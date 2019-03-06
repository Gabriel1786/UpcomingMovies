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
        MovieStateContainer _previousStateContainer;
        Dictionary<MovieListType, MovieStateContainer> _cachedMovies = new Dictionary<MovieListType, MovieStateContainer>();

        public MoviesViewModel(IMvxNavigationService navigationService, IMovieService movieService)
        {
            _navigationService = navigationService;
            _movieService = movieService;

            Movies = new MvxObservableCollection<Movie>();

            ShowMovieDetailViewModelCommand = new MvxAsyncCommand<Movie>(ShowMovieDetailView);
            SwitchMovieListTypeCommand = new MvxCommand<MovieListType>(SetMovieListToType);
            SearchCommand = new MvxAsyncCommand<string>(Search);
            RefreshCommand = new MvxAsyncCommand(Refresh);
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
            SetMovieListToType(MovieListType.Upcoming); //Setting first list type to load
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

        bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        bool _emptySearchResults;
        public bool EmptySearchResults
        {
            get => _emptySearchResults;
            set => SetProperty(ref _emptySearchResults, value);
        }

        public MvxNotifyTask LoadInitialMoviesTask { get; private set; }
        public MvxNotifyTask LoadMoreMoviesTask { get; private set; }

        // MVVM Commands
        public IMvxAsyncCommand<Movie> ShowMovieDetailViewModelCommand { get; }
        public IMvxCommand LoadMoreMoviesCommand { get; }
        public IMvxCommand SwitchMovieListTypeCommand { get; }
        public IMvxAsyncCommand<string> SearchCommand { get; }
        public IMvxAsyncCommand RefreshCommand { get; }

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
            else
            {
                //TODO: could load more but failed, alert user?
            }
        }

        async Task Search(string query)
        {
            Debug.WriteLine($"Searching for {query}");

            if (string.IsNullOrEmpty(query))
            {
                MovieListType previousType = _currentStateContainer.MovieListType;
                if (_previousStateContainer != null)
                    previousType = _previousStateContainer.MovieListType;

                SetMovieListToType(previousType);
                return;
            }

            SetMovieListToType(MovieListType.Search);

            var responseInfo = await _movieService.SearchMoviesAsync(new Dictionary<string, object>
            {
                { "query", query }
            });

            if (responseInfo.IsSuccess)
            {
                if (responseInfo.Result.Movies.Count > 0) // Circumventing AiForms.CollectionView crash on Android
                {
                    Movies.AddRange(responseInfo.Result.Movies);
                    EmptySearchResults = false;
                }
                else
                {
                    EmptySearchResults = true;
                }
                _currentStateContainer.Movies.AddRange(responseInfo.Result.Movies);
                _currentStateContainer.CurrentPage++;
                _currentStateContainer.TotalPages = responseInfo.Result.TotalPages;
            }
            else
            {
                SetMovieListToType(_previousStateContainer.MovieListType);
                //TODO: search failed, showing previous list
            }
        }

        async Task Refresh()
        {
            IsRefreshing = true;
            Movies.Clear();
            _currentStateContainer.Movies.Clear();
            _currentStateContainer.CurrentPage = 1;
            await LoadMovies();
            IsRefreshing = false;
        }

        MovieStateContainer GetMovieStateContainerForType(MovieListType movieListType)
        {
            if (_cachedMovies.ContainsKey(movieListType))
                return _cachedMovies[movieListType];

            var stateContainer = new MovieStateContainer { MovieListType = movieListType };
            _cachedMovies.Add(movieListType, stateContainer);
            return stateContainer;
        }

        /// <summary>
        /// Controls what list should be showing in the "Movies" property, returning cached Movies and state.
        /// </summary>
        /// <param name="movieListType">Movie list type.</param>
        void SetMovieListToType(MovieListType movieListType)
        {
            if (_currentStateContainer != null && _currentStateContainer.MovieListType != MovieListType.Search)
                _previousStateContainer = _currentStateContainer;

            _currentStateContainer = GetMovieStateContainerForType(movieListType);

            Movies.Clear();
            EmptySearchResults = false;

            if (movieListType == MovieListType.Search)
                _currentStateContainer.Movies.Clear();

            if (_currentStateContainer.Movies.Count > 0) // Circumventing AiForms.CollectionView crash on Android
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
                case MovieListType.Search:
                    Title = "Search Results";
                    break;
            }
        }

        async Task ShowMovieDetailView(Movie movie)
        {
            await _navigationService.Navigate<MovieDetailViewModel, Movie>(movie);
        }
    }
}
