using System;
using System.Collections.Generic;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using UpcomingMovies.Core.ViewModels;
using Xamarin.Forms;

namespace UpcomingMovies.Forms.UI.Views
{
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class MoviesPage : MvxContentPage<MoviesViewModel>
    {
        public MoviesPage()
        {
            InitializeComponent();
        }
    }
}
