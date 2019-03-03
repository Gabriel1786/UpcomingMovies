using MvvmCross.Forms.Views;
using MvvmCross.Forms.Presenters.Attributes;
using UpcomingMovies.Core.ViewModels;

namespace UpcomingMovies.Forms.UI.Views
{
    [MvxContentPagePresentation]
    public partial class MovieDetailPage : MvxContentPage<MovieDetailViewModel>
    {
        public MovieDetailPage()
        {
            InitializeComponent();
        }
    }
}
