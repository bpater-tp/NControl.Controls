using System;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NControl.Controls.iOS.CalculateTextWidth_iOS))]
namespace NControl.Controls.iOS
{
    public class CalculateTextWidth_iOS : CalculateTextWidth
    {
        public double CalculateWidth(string text)
        {
            var uiLabel = new UILabel();
            uiLabel.Text = text;
            var length = uiLabel.Text.StringSize(uiLabel.Font);
            return length.Width;
        }
    }
}
