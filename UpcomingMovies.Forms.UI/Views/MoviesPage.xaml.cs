using System;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using UpcomingMovies.Core.ViewModels;
using Xamarin.Essentials;

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
            var desiredCellWidth = 160;

            DisplayInfo info = DeviceDisplay.MainDisplayInfo;

            var dipWidth = info.Width / info.Density;
            var dipHeight = info.Height / info.Density;

            if (info.Orientation == DisplayOrientation.Portrait)
            {
                collectionView.PortraitColumns = (int)Math.Floor(dipWidth / desiredCellWidth);
                collectionView.LandscapeColumns = (int)Math.Floor(dipHeight / desiredCellWidth);
            }
            else
            {
                collectionView.PortraitColumns = (int)Math.Floor(dipHeight / desiredCellWidth);
                collectionView.LandscapeColumns = (int)Math.Floor(dipWidth / desiredCellWidth);
            }
        }
    }
}
