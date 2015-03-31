﻿using System;
using NControl.Abstractions;
using NGraphics;
using Xamarin.Forms;
using System.Reflection;
using System.Linq;
using NControl.Controls.Demo.FormsApp.Resources.Fonts;

namespace NControl.Controls.Demo.FormsApp
{
	public class MyApp : Application
	{
		public MyApp ()
		{
			var controlList = typeof(RoundCornerView).GetTypeInfo ().Assembly.DefinedTypes				
				.Where(t => !t.IsAbstract && t.ImplementedInterfaces.Contains(typeof(IDemonstratableControl)))
				.Select (t => t.Name).ToArray ();

			var listView = new ListView {
				ItemsSource = controlList,
			};

			var startPage = new ContentPage {
				Title = "NControl.Controls",
                Content = new StackLayout
                {
                    Children =
                    {
						new CustomFontLabel()
                        {
                            HeightRequest = 55,
							FontDataResourcePath = typeof(ClinkClankFont).Namespace + ".ClinkClank.ttf",
							FontDataAssembly = typeof(ClinkClankFont).GetTypeInfo().Assembly,
							Text = "DEMONSTRATION",
							FontFamily = "Clink Clank",
							XAlign = Xamarin.Forms.TextAlignment.Center,
							YAlign = Xamarin.Forms.TextAlignment.Center,
							FontSize = 24,
                        },
                        listView
                    }
                }
			};

			listView.ItemSelected += async (sender, e) => {

				if(listView.SelectedItem == null)
					return;
				
				// Create control
				var type = typeof(RoundCornerView).GetTypeInfo().Assembly.DefinedTypes.FirstOrDefault(
					t => t.Name.EndsWith((string)listView.SelectedItem)).AsType();
				
				var control = (View)Activator.CreateInstance(type);
				var demonstratable = control as IDemonstratableControl;
				if(demonstratable != null)
					demonstratable.Initialize();

				await startPage.Navigation.PushAsync(
					new ContentPage{
						Title = (string)listView.SelectedItem,
						Content = new ScrollView { 
							Padding = 25,
							Content = control 
						}
					});							
			};

			listView.ItemTapped += (sender, e) => listView.SelectedItem = null;
				
			// The root page of your application
			MainPage = new NavigationPage(startPage);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

