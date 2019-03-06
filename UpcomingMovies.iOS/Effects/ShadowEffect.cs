using System;
using System.ComponentModel;
using System.Diagnostics;
using CoreGraphics;
using UpcomingMovies.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(ShadowEffect), "ShadowEffect")]
namespace UpcomingMovies.iOS.Effects
{
    public class ShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                SetNativeShadowColor();
                SetNativeShadowRadius();
                SetNativeShadowOffset();
                SetNativeShadowOpacity();
            }
            catch
            {
            }
        }

        protected override void OnDetached()
        {
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == UpcomingMovies.Forms.UI.Effects.ShadowEffect.ShadowColorProperty.PropertyName)
            {
                SetNativeShadowColor();
            }
            else if (args.PropertyName == UpcomingMovies.Forms.UI.Effects.ShadowEffect.ShadowRadiusProperty.PropertyName)
            {
                SetNativeShadowRadius();
            }
            else if (args.PropertyName == UpcomingMovies.Forms.UI.Effects.ShadowEffect.ShadowOpacityProperty.PropertyName)
            {
                SetNativeShadowOpacity();
            }
            else if (args.PropertyName == UpcomingMovies.Forms.UI.Effects.ShadowEffect.ShadowXOffsetProperty.PropertyName ||
                     args.PropertyName == UpcomingMovies.Forms.UI.Effects.ShadowEffect.ShadowYOffsetProperty.PropertyName)
            {
                SetNativeShadowOffset();
            }
        }

        void SetNativeShadowColor()
        {
            var color = UpcomingMovies.Forms.UI.Effects.ShadowEffect.GetShadowColor(Element);
            Container.Layer.ShadowColor = color.ToCGColor();
        }

        void SetNativeShadowRadius()
        {
            Container.Layer.ShadowRadius = UpcomingMovies.Forms.UI.Effects.ShadowEffect.GetShadowRadius(Element);
        }

        void SetNativeShadowOffset()
        {
            Container.Layer.ShadowOffset = new CGSize(UpcomingMovies.Forms.UI.Effects.ShadowEffect.GetShadowXOffset(Element), 
                                                    UpcomingMovies.Forms.UI.Effects.ShadowEffect.GetShadowYOffset(Element));
        }

        void SetNativeShadowOpacity()
        {
            Container.Layer.ShadowOpacity = UpcomingMovies.Forms.UI.Effects.ShadowEffect.GetShadowOpacity(Element);
        }
    }
}
