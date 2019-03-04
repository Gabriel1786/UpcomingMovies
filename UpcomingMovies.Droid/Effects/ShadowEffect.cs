using System;
using System.Diagnostics;
using Android.Content;
using Android.Graphics;
using Android.Views;
using UpcomingMovies.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(ShadowEffect), "ShadowEffect")]
namespace UpcomingMovies.Droid.Effects
{
    public class ShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            return; //FIXME
            try
            {
                Console.WriteLine($"Applying Shadow Effect on {Element}");
                SetNativeShadow();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Not able to attach 'ShadowEffect' in {Element}.");
                Console.WriteLine(e);
            }
        }

        protected override void OnDetached()
        {
            Container.OutlineProvider = ViewOutlineProvider.Background;
        }

        void SetNativeShadow()
        {
            var radius = UpcomingMovies.Forms.UI.Effects.ShadowEffect.GetShadowRadius(Element);
            var shadowX = UpcomingMovies.Forms.UI.Effects.ShadowEffect.GetShadowXOffset(Element);
            var shadowY = UpcomingMovies.Forms.UI.Effects.ShadowEffect.GetShadowYOffset(Element);
            var color = UpcomingMovies.Forms.UI.Effects.ShadowEffect.GetShadowColor(Element).ToAndroid();

            //if (Control is Android.Widget.ImageView nativeImageView)
            //{
            //    //nativeImageView.Elevation = radius;
            //    //nativeImageView.TranslationZ = radius;
            //    //nativeImageView.ClipToOutline = true;
            //    //nativeImageView.OutlineProvider = new ShadowOutlineProvider(radius);
            //    Context context = Android.App.Application.Context;
            //    //nativeImageView.SetBackground(context.GetDrawable(Resource.Drawable.shadow));
            //    Bitmap tempbitmap = nativeImageView.GetDrawingCache(false);
            //    Bitmap bitmap = Bitmap.CreateBitmap(tempbitmap.Width+6, 
            //                                        tempbitmap.Height+6, 
            //                                        Bitmap.Config.Argb8888);
            //    Canvas canvas = new Canvas(bitmap);

            //    Paint paint = new Paint();
            //    paint.AntiAlias = true;
            //    paint.SetShadowLayer(radius, shadowX, shadowY, color);
            //    canvas.DrawRect(new Rect(radius, radius, tempbitmap.Width, tempbitmap.Height), paint);
            //    canvas.DrawBitmap(bitmap, 0, 0, null);

            //    //nativeImageView.SetLayerType(LayerType.Software, null);
            //    nativeImageView.SetLayerPaint(paint);
            //}
            //else
            {
                Container.OutlineProvider = new ShadowOutlineProvider(radius);
            }
        }
    }

    class ShadowOutlineProvider : ViewOutlineProvider
    {
        readonly float _elevation;

        public ShadowOutlineProvider(float elevation)
        {
            _elevation = elevation;
        }

        public override void GetOutline(Android.Views.View view, Outline outline)
        {
            outline?.SetRect(0, 0, view.Width, view.Height);
            view.Elevation = _elevation;
            view.TranslationZ = _elevation * 5; //TODO: needs work
        }
    }
}
