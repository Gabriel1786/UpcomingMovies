using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views.InputMethods;
using MvvmCross.Forms.Platforms.Android.Views;
using UpcomingMovies.Core.ViewModels;
using UpcomingMovies.Droid.Renderers;
using UpcomingMovies.Forms.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.View;

[assembly: ExportRenderer(typeof(MoviesPage), typeof(MoviesPageRenderer))]
namespace UpcomingMovies.Droid.Renderers
{
    public class MoviesPageRenderer : MvxPageRenderer<MoviesViewModel>, IOnAttachStateChangeListener
    {
        Context context;
        SearchView searchView;

        public MoviesPageRenderer(Context context) : base(context)
        {
            this.context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            var moviesPage = Element as MoviesPage;
            if (moviesPage == null)
                return;

            if (e.NewElement != null && searchView != null)
            {
                searchView.QueryTextChange -= NativeSearchView_QueryTextChange;
                searchView.QueryTextSubmit -= NativeSearchView_QueryTextSubmit;
            }

            moviesPage.AppearingCalled += async (sender, ea) => 
            {
                // we are adding again because when we go to detail movie, the toolbar is cleared. 
                // the delay is added because it happens some time after OnAppearing event so we cant add it right away
                await Task.Delay(250);
                AddSearchView();
            };

            moviesPage.DisappearingCalled += (sender, ea) => 
            {
                RemoveSearchView();
            };

            AddSearchView();
        }

        void NativeSearchView_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            var moviesPage = Element as MoviesPage;
            //moviesPage.ViewModel.SearchCommand.Execute(e.NewText); //FIXME: this is causing keyboard to dismiss, find out why
        }

        void NativeSearchView_QueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            var moviesPage = Element as MoviesPage;
            moviesPage.ViewModel.SearchCommand.Execute(e.NewText);

            if (sender is SearchView searchView)
            {
                searchView.ClearFocus();
            }
        }

        void AddSearchView()
        {
            var mainActivity = (MainActivity)context;

            mainActivity.Toolbar = mainActivity.FindViewById<Toolbar>(Resource.Id.toolbar);
            mainActivity.Toolbar.Title = Element.Title;
            mainActivity.Toolbar.InflateMenu(Resource.Menu.search_menu);

            searchView = mainActivity.Toolbar.Menu.FindItem(Resource.Id.action_search).ActionView.JavaCast<SearchView>();
            searchView.QueryTextChange += NativeSearchView_QueryTextChange;
            searchView.QueryTextSubmit += NativeSearchView_QueryTextSubmit;
            searchView.AddOnAttachStateChangeListener(this);
            searchView.QueryHint = "Search";
            searchView.ImeOptions = (int)ImeAction.Search;
            searchView.InputType = (int)InputTypes.TextVariationNormal;
            searchView.MaxWidth = int.MaxValue;
        }

        void RemoveSearchView()
        {
            var mainActivity = (MainActivity)context;
            if (mainActivity == null || searchView == null)
                return;

            mainActivity?.Toolbar?.Menu?.RemoveItem(Resource.Id.action_search);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (searchView != null)
                {
                    searchView.QueryTextChange -= NativeSearchView_QueryTextChange;
                    searchView.QueryTextSubmit -= NativeSearchView_QueryTextSubmit;
                }

                var mainActivity = (MainActivity)context;
                mainActivity?.Toolbar?.Menu?.RemoveItem(Resource.Id.action_search);
            }

            base.Dispose(disposing);
        }

        void IOnAttachStateChangeListener.OnViewAttachedToWindow(Android.Views.View attachedView)
        {
        }

        void IOnAttachStateChangeListener.OnViewDetachedFromWindow(Android.Views.View detachedView)
        {
            if (detachedView == searchView)
            {
                var moviesPage = Element as MoviesPage;
                moviesPage.ViewModel.SearchCommand.Execute("");
            }
        }
    }
}
