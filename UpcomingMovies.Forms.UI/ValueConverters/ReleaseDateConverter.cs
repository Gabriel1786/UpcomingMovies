using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace UpcomingMovies.Forms.UI.ValueConverters
{
    public class ReleaseDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString)
            {
                DateTime date = DateTime.MinValue;
                try
                {
                    date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                catch
                {
                    return dateString;
                }
                return $"Release Date: {date.ToString("dd MMMM yyyy")}";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
