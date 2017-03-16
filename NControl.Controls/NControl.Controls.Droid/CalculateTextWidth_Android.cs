using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(NControl.Controls.Droid.CalculateTextWidth_Android))]
namespace NControl.Controls.Droid
{
    public class CalculateTextWidth_Android : CalculateTextWidth
    {
        public double CalculateWidth(string text)
        {
            Rect bounds = new Rect();
            TextView textView = new TextView(Forms.Context);
            textView.Paint.GetTextBounds(text, 0, text.Length, bounds);
            var length = bounds.Width();
            return length / Resources.System.DisplayMetrics.ScaledDensity;
        }
    }
}
