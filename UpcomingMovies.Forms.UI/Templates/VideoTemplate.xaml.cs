using System;
using System.Collections.Generic;
using UpcomingMovies.Core.Models;
using Xamarin.Forms;

namespace UpcomingMovies.Forms.UI.Templates
{
    public partial class VideoTemplate : StackLayout
    {
        public VideoTemplate()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (sender is ImageButton imageButton)
            {
                var param = imageButton.CommandParameter;
                if (param is Video video)
                {
                    var uri = new Uri(video.VideoUrl());
                    Device.OpenUri(uri);
                }
            }
        }
    }
}
