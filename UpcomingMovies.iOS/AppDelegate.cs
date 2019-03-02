using Foundation;
using MvvmCross.Forms.Platforms.Ios.Core;
using UIKit;
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
            //CustomizeAppearance();

            return finished;
        }

        void CustomizeAppearance()
        {
            //var primaryDarkColor = Xamarin.Forms.Application.Current.Resources["primaryDarkColor"] as Color?;
            //var primaryLightColor = Xamarin.Forms.Application.Current.Resources["primaryLightColor"] as Color?;

            //UINavigationBar.Appearance.BarTintColor = primaryDarkColor.Value.ToUIColor();
            ////UINavigationBar.Appearance.TintColor = primaryLightColor.Value.ToUIColor();
            //UINavigationBar.Appearance.TintColor = UIColor.White;

            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
            {
                //TextColor = primaryLightColor.Value.ToUIColor()
                TextColor = UIColor.White
            });

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                UINavigationBar.Appearance.LargeTitleTextAttributes = new UIStringAttributes
                {
                    //ForegroundColor = primaryLightColor.Value.ToUIColor()
                    ForegroundColor = UIColor.White
                };
            }
        }
    }
}
