using System.Net.Http;
using Foundation;
using MvvmCross.Forms.Platforms.Ios.Core;
using UIKit;
using UpcomingMovies.Core.Configurations;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace UpcomingMovies.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxFormsApplicationDelegate<Setup, Core.App, Forms.UI.App>
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            var finished = base.FinishedLaunching(uiApplication, launchOptions);

            AiForms.Renderers.iOS.CollectionViewInit.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            var config = new FFImageLoading.Config.Configuration
            {
                HttpClient = new HttpClient()
            };
            FFImageLoading.ImageService.Instance.Initialize(config);

            return finished;
        }
    }
}
