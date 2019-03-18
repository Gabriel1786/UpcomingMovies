using System;
using UpcomingMovies.Forms.UI.Components;
using UpcomingMovies.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
namespace UpcomingMovies.iOS.Renderers
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.OldElement != null)
            {
                Control.CancelButtonClicked -= Control_CancelButtonClicked;
                Control.TextChanged -= Control_TextChanged;
                return;
            }

            if (e.NewElement != null)
            {
                Control.CancelButtonClicked += Control_CancelButtonClicked;
                Control.TextChanged += Control_TextChanged;
            }
        }

        void Control_CancelButtonClicked(object sender, EventArgs e)
        {
            if (Element is CustomSearchBar customSearchBar)
            {
                customSearchBar.RaiseCancelEvent();
            }
        }

        void Control_TextChanged(object sender, UIKit.UISearchBarTextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.SearchText) && Element is CustomSearchBar customSearchBar)
            {
                customSearchBar.RaiseCancelEvent();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Control != null)
                { 
                    Control.CancelButtonClicked -= Control_CancelButtonClicked;
                    Control.TextChanged -= Control_TextChanged;
                }
            }

            base.Dispose(disposing);
        }
    }
}
