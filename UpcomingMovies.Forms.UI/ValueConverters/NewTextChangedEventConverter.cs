using System;
using System.Globalization;
using MvvmCross.Converters;
using Xamarin.Forms;

namespace UpcomingMovies.Forms.UI.ValueConverters
{
    public class NewTextChangedEventConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Xamarin.Forms.TextChangedEventArgs))
                return null;

            var textChanged = value as TextChangedEventArgs;
            return textChanged.NewTextValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
