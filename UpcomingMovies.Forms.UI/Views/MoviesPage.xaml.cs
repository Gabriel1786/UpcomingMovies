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
            if (info.Orientation == DisplayOrientation.Portrait)
            {
                var dipWidth = info.Width / info.Density;
                var numberOfCellsP = dipWidth / desiredCellWidth;

                collectionView.PortraitColumns = (int)Math.Floor(numberOfCellsP);

                var dipHeight = info.Height / info.Density;
                var numberOfCellsL = dipHeight / desiredCellWidth;

                collectionView.LandscapeColumns = (int)Math.Floor(numberOfCellsL);
            }
        }
    }
}
