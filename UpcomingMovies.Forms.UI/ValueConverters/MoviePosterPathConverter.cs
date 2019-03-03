using System;
using System.Globalization;
using Flurl;
using MvvmCross.Converters;
using UpcomingMovies.Core.Configurations;
using Xamarin.Forms;

namespace UpcomingMovies.Forms.UI.ValueConverters
{
    public class MoviePosterPathConverter : MvxValueConverter<string, ImageSource>
    {
        protected override ImageSource Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value))
                return "movie_placeholder.png";

            var url = Url.Combine(AppConfig.ApiImageUrl, $"w{parameter ?? 300}", value);
            var imgSource = ImageSource.FromUri(new Uri(url));
            return imgSource;
        }
    }
}
