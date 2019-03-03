using System;
using MvvmCross.ViewModels;

namespace UpcomingMovies.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        string _title;
        public virtual string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
