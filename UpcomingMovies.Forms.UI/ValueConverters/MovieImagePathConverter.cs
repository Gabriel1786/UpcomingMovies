﻿using System;
using System.Globalization;
using Flurl;
using UpcomingMovies.Core.Configurations;
using Xamarin.Forms;

namespace UpcomingMovies.Forms.UI.ValueConverters
{
    public class MovieImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return "movie_placeholder.png";

            var url = Url.Combine(AppConfig.ApiImageUrl, $"w{parameter ?? 500}", value.ToString());
            var imgSource = ImageSource.FromUri(new Uri(url));
            return imgSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
