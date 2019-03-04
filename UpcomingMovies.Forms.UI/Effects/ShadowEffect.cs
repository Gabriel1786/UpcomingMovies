using System;
using System.Linq;
using Xamarin.Forms;

namespace UpcomingMovies.Forms.UI.Effects
{
    public class ShadowEffect : RoutingEffect
    {
        public ShadowEffect() : base("Mads.ShadowEffect") { }

        public static readonly BindableProperty ShadowColorProperty = BindableProperty.CreateAttached("ShadowColor",
                                                                                                      typeof(Color),
                                                                                                      typeof(ShadowEffect),
                                                                                                      Color.Black);
        public static Color GetShadowColor(BindableObject view) => (Color)view.GetValue(ShadowColorProperty);
        public static void SetShadowColor(BindableObject view, Color value) => view.SetValue(ShadowColorProperty, value);


        public static readonly BindableProperty ShadowOpacityProperty = BindableProperty.CreateAttached("ShadowOpacity",
                                                                                                        typeof(float),
                                                                                                        typeof(ShadowEffect),
                                                                                                        1.0f);
        public static float GetShadowOpacity(BindableObject view) => (float)view.GetValue(ShadowOpacityProperty);
        public static void SetShadowOpacity(BindableObject view, float value) => view.SetValue(ShadowOpacityProperty, value);


        public static readonly BindableProperty ShadowRadiusProperty = BindableProperty.CreateAttached("ShadowRadius",
                                                                                                       typeof(int),
                                                                                                       typeof(ShadowEffect),
                                                                                                       0);
        public static int GetShadowRadius(BindableObject view) => (int)view.GetValue(ShadowRadiusProperty);
        public static void SetShadowRadius(BindableObject view, int value) => view.SetValue(ShadowRadiusProperty, value);


        public static readonly BindableProperty ShadowXOffsetProperty = BindableProperty.CreateAttached("ShadowXOffset",
                                                                                                       typeof(int),
                                                                                                       typeof(ShadowEffect),
                                                                                                       0);
        public static int GetShadowXOffset(BindableObject view) => (int)view.GetValue(ShadowXOffsetProperty);
        public static void SetShadowXOffset(BindableObject view, int value) => view.SetValue(ShadowXOffsetProperty, value);

        public static readonly BindableProperty ShadowYOffsetProperty = BindableProperty.CreateAttached("ShadowYOffset",
                                                                                                       typeof(int),
                                                                                                       typeof(ShadowEffect),
                                                                                                       0);
        public static int GetShadowYOffset(BindableObject view) => (int)view.GetValue(ShadowYOffsetProperty);
        public static void SetShadowYOffset(BindableObject view, int value) => view.SetValue(ShadowYOffsetProperty, value);

        public static readonly BindableProperty EnabledProperty = BindableProperty.CreateAttached("Enabled",
                                                                                                   typeof(bool),
                                                                                                   typeof(ShadowEffect),
                                                                                                   false,
                                                                                                   propertyChanged: OnEnabledChanged);

        public static bool GetEnabled(BindableObject view) => (bool)view.GetValue(EnabledProperty);
        public static void SetEnabled(BindableObject view, bool value) => view.SetValue(EnabledProperty, value);

        static void OnEnabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is View view))
                return;

            var hasShadow = (bool)newValue;
            var effect = view.Effects.FirstOrDefault(e => e is ShadowEffect);

            if (hasShadow && effect == null)
            {
                view.Effects.Add(new ShadowEffect());
            }
            else if (!hasShadow && effect != null)
            {
                view.Effects.Remove(effect);
            }
        }
    }
}
