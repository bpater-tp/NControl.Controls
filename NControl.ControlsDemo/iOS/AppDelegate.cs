using Foundation;
using NControl.Controls.iOS;
using UIKit;
using XLabs.Platform.Device;

namespace NControl.Controls.Demo.FormsApp.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			NControls.Init ();

			LoadApplication (new MyApp ());
            MyApp.RegisterDeviceResolver(AppleDevice.CurrentDevice);
			return base.FinishedLaunching (app, options);
		}
	}
}

