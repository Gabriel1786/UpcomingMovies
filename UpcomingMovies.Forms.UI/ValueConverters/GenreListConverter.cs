using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UpcomingMovies.Core.Models;
using Xamarin.Forms;

namespace UpcomingMovies.Forms.UI.ValueConverters
{
    public class GenreListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<Genre> genres)
            {
                return "Genres: " + string.Join(", ", genres.Select(x => x.Name));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
