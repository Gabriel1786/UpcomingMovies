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
            CancelSearchCommand = new MvxCommand(CancelSearch);
            RefreshCommand = new MvxAsyncCommand(Refresh);
            LoadMoreMoviesCommand = new MvxCommand(() =>
            {
                LoadMoreMoviesTask = MvxNotifyTask.Create(LoadMore);
                RaisePropertyChanged(() => LoadMoreMoviesTask);
            });
        }

        // MvvmCross Lifecycle
        public override Task Initialize()
        {
            var task = base.Initialize();
            SetMovieListToType(MovieListType.Upcoming);
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

        bool _loadFailed;
        public bool LoadFailed
        {
            get => _loadFailed;
            set => SetProperty(ref _loadFailed, value);
        }

        string _failMessage;
        public string FailMessage
        {
            get => _failMessage;
            set => SetProperty(ref _failMessage, value);
        }

        bool _isNavigating;
        public bool IsNavigating
        {
            get => _isNavigating;
            set => SetProperty(ref _isNavigating, value);
        }

        public MvxNotifyTask LoadInitialMoviesTask { get; private set; }
        public MvxNotifyTask LoadMoreMoviesTask { get; private set; }

        // MVVM Commands
        public IMvxAsyncCommand<Movie> ShowMovieDetailViewModelCommand { get; }
        public IMvxCommand LoadMoreMoviesCommand { get; }
        public IMvxCommand SwitchMovieListTypeCommand { get; }
        public IMvxAsyncCommand<string> SearchCommand { get; }
        public IMvxCommand CancelSearchCommand { get; }
        public IMvxAsyncCommand RefreshCommand { get; }

        // Private Methods
        async Task LoadMovies()
        {
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
                FailMessage = responseInfo.Error;
                LoadFailed = true;
            }
        }

        async Task LoadMore()
        {
            if (!_currentStateContainer.CanLoadMore())
                return;

            if (_currentStateContainer.MovieListType == MovieListType.Search)
                await Search(_currentStateContainer.Query);
            else
                await LoadMovies();
        }

        async Task Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                CancelSearch();
                return;
            }

            SetMovieListToType(MovieListType.Search);

            if (_currentStateContainer.Query != query)
            {
                _currentStateContainer.Query = query;
                _currentStateContainer.CurrentPage = 1;
                _currentStateContainer.TotalPages = 0;
                _currentStateContainer.Movies.Clear();
                Movies.Clear();
            }

            var responseInfo = await _movieService.SearchMoviesAsync(new Dictionary<string, object>
            {
                { "query", _currentStateContainer.Query },
                { "page", _currentStateContainer.CurrentPage }
            });

            if (responseInfo.IsSuccess)
            {
                if (responseInfo.Result.Movies.Count > 0) // Circumventing AiForms.CollectionView crash on Android
                {
                    Movies.AddRange(responseInfo.Result.Movies);
                    _currentStateContainer.Movies.AddRange(responseInfo.Result.Movies);
                    _currentStateContainer.CurrentPage++;
                    _currentStateContainer.TotalPages = responseInfo.Result.TotalPages;
                    EmptySearchResults = false;
                }
                else
                {
                    EmptySearchResults = true;
                }
            }
            else
            {
                FailMessage = responseInfo.Error;
                LoadFailed = true;
            }
        }

        void CancelSearch()
        {
            MovieListType previousType = _currentStateContainer.MovieListType;
            if (_previousStateContainer != null)
                previousType = _previousStateContainer.MovieListType;

            SetMovieListToType(previousType);
        }

        async Task Refresh()
        {
            IsRefreshing = true;
            LoadFailed = false;
            FailMessage = null;

            if (_currentStateContainer.MovieListType == MovieListType.Search)
            {
                SetMovieListToType(_previousStateContainer.MovieListType);
            }
            else
            {
                Movies.Clear();
                _currentStateContainer.Movies.Clear();
                _currentStateContainer.CurrentPage = 1;
            }

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
            if (_currentStateContainer != null)
            {
                if (_currentStateContainer.MovieListType != MovieListType.Search)
                    _previousStateContainer = _currentStateContainer;
                else if (_currentStateContainer.MovieListType == movieListType)
                    return;
            }

            _currentStateContainer = GetMovieStateContainerForType(movieListType);

            Movies.Clear();
            EmptySearchResults = false;
            LoadFailed = false;
            FailMessage = null;

            if (_currentStateContainer.Movies.Count > 0) // Circumventing AiForms.CollectionView crash on Android
                Movies.AddRange(_currentStateContainer.Movies);

            SetTitle(movieListType);
        }

        void SetTitle(MovieListType movieListType)
        {
            switch (movieListType)
            {
                case MovieListType.Latest:
                    Title = UiMessages.LatestMovies;
                    break;
                case MovieListType.NowPlaying:
                    Title = UiMessages.NowPlaying;
                    break;
                case MovieListType.Popular:
                    Title = UiMessages.PopularMovies;
                    break;
                case MovieListType.TopRated:
                    Title = UiMessages.TopRatedMovies;
                    break;
                case MovieListType.Upcoming:
                    Title = UiMessages.UpcomingMovies;
                    break;
                case MovieListType.Search:
                    Title = UiMessages.SearchResults;
                    break;
            }
        }

        async Task ShowMovieDetailView(Movie movie)
        {
            IsNavigating = true;
            try
            {
                await _navigationService.Navigate<MovieDetailViewModel, Movie>(movie);
            }
            catch
            {
                LoadFailed = true;
                FailMessage = UiMessages.MovieDetailNavigationFailed;
            }
            finally
            {
                IsNavigating = false;
            }
        }
    }
}