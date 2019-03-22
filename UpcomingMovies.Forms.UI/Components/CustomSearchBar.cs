using System;
using Xamarin.Forms;

namespace UpcomingMovies.Forms.UI.Components
{
    public class CustomSearchBar : SearchBar
    {
        public event EventHandler CancelButtonTapped;

        public void RaiseCancelEvent()
        {
            CancelButtonTapped?.Invoke(this, EventArgs.Empty);
        }
    }
}
