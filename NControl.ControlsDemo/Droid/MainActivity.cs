using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using NControl.Controls.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Platform.Device;

namespace NControl.Controls.Demo.FormsApp.Droid
{
	[Activity (Label = "NControl.Controls Demo", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
			FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;
            MyApp.DensityScale = Resources.DisplayMetrics.ScaledDensity;

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			NControls.Init ();
            MyApp.RegisterDeviceResolver(AndroidDevice.CurrentDevice);
			LoadApplication (new MyApp ());
		}
	}
}

