using Android.App;
using Android.OS;
using Android.Content.PM;
using MvvmCross.Forms.Platforms.Android.Views;
using System.Net.Http;

namespace UpcomingMovies.Droid
{
    [Activity(Label = "@string/app_name",
              Icon = "@mipmap/icon",
              Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
              LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : MvxFormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);

            var config = new FFImageLoading.Config.Configuration
            {
                HttpClient = new HttpClient()
            };
            FFImageLoading.ImageService.Instance.Initialize(config);

            base.OnCreate(bundle);
        }
    }
}

