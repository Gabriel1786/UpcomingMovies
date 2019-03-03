using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using UpcomingMovies.Core.ViewModels;

namespace UpcomingMovies.Forms.UI.Views
{
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class MoviesPage : MvxContentPage<MoviesViewModel>
    {
        public MoviesPage()
        {
            InitializeComponent();

            SetupCollectionView();
        }

        void SetupCollectionView()
        {
            //TODO: Determine right amount of columns depending on device screen
        }
    }
}
