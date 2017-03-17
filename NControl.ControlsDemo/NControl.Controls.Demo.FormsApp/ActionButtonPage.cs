using System;
using System.Diagnostics;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;

namespace NControl.Controls.Demo.FormsApp
{
	public class ActionButtonPage: ContentPage
	{
		private Command _command;

		public ActionButtonPage ()
		{
			Title = "ActionButton";
            BackgroundColor = Color.Gray;
		}

		protected override void OnAppearing ()
		{
            Debug.WriteLine("starting");
			base.OnAppearing ();

			var layout = new RelativeLayout ();
			Content = layout;

            _command = new Command((ext) =>
            {
                if (ext != null)
                {
                    ((ExpandableActionButton)ext).HideButtonsAsync();
                }
                Debug.WriteLine("sdsdf");
            });

			var ab = new ActionButton {
				ButtonColor = Color.FromHex("#E91E63"),
				ButtonIcon = FontAwesomeLabel.FAThumbsUp,
                Command = _command,
			};
			layout.Children.Add(ab, () => new Rectangle((layout.Width/4)-(56/2), (layout.Height/2)-(56/2), 56, 56));

			var abtgl = new ToggleActionButton {
				ButtonColor = Color.FromHex("#FF5722"),
				ButtonIcon = FontAwesomeLabel.FAPlus,
			};
			//abtgl.SetBinding (IsToggledProperty, "IsToggled");
			layout.Children.Add(abtgl, () => new Rectangle((layout.Width/2)-(56/2), (layout.Height/2)-(56/2), 56, 56));

            var device = Resolver.Resolve<IDevice>();
            var display = device.Display;
            double iconFontSize = display.HeightRequestInInches(0.125);
            double sideFontSize = display.HeightRequestInInches(0.08);

            var playButton = new ActionButton("z galerii"){
            //var playButton = new ActionButton("from gallery"){ 
				ButtonColor = Color.FromHex ("#2196F3"), 
                ButtonIcon = FontAwesomeLabel.FAUpload,
                ButtonIconSize = iconFontSize,
                //SideLabelText = "from gallery",
                SideLabelTextColor = Color.White,
                SideLabelBackgroundColor = Color.FromHex("#804444"),
                SideLabelFontSize = sideFontSize,
                Command = _command,
			};
            var bubButton = new ActionButton("z aparatu")
            //var bubButton = new ActionButton("from camera")
            {
                ButtonColor = Color.FromHex("#009688"),
                ButtonIcon = FontAwesomeLabel.FATag,
                ButtonIconSize = iconFontSize,
                //SideLabelText = "from camera",
                SideLabelTextColor = Color.White,
                SideLabelBackgroundColor = Color.FromHex("#804444"),
                SideLabelFontSize = sideFontSize,
                Command = _command,
            };

            var abex = new ExpandableActionButton {
                DensityScale = MyApp.DensityScale,
				ButtonColor = Color.FromHex("#FF9800"),
				ButtonIcon = FontAwesomeLabel.FAPlusSquare,
				Buttons = {
					playButton,
                    bubButton,
				}
			};
            double buttonSize = Device.OnPlatform(0.45, 0.4, 0.4);
            double diameter = display.WidthRequestInInches(buttonSize);
            double padding_x = display.WidthRequestInInches(0.08);
            double padding_y = display.HeightRequestInInches(0.1);
            var multiplier = 1 + abex.Buttons.Count;
            layout.Children.Add(abex, () => new Rectangle(this.Width - diameter - padding_x, this.Height - (diameter * multiplier) - padding_y, diameter, diameter * multiplier));

			//layout.Children.Add(abex, () => new Rectangle(((layout.Width/4)*3)-(56/2), (layout.Height/2)-(200), 56, 250));
		}

		/// <summary>
		/// The IsToggled property.
		/// </summary>
		public static BindableProperty IsToggledProperty = 
			BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(ActionButtonPage), false,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (ActionButtonPage)bindable;
					ctrl.IsToggled = (bool)newValue;
				});

		/// <summary>
		/// Gets or sets the IsToggled of the ActionButtonPage instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public bool IsToggled {
			get{ return (bool)GetValue (IsToggledProperty); }
			set {
				SetValue (IsToggledProperty, value);
				_command.ChangeCanExecute ();
			}
		}

	}
}

