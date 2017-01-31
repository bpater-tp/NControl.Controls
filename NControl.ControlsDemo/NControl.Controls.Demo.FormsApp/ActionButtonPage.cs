using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class ActionButtonPage: ContentPage
	{
		private Command _command;

		public ActionButtonPage ()
		{
			Title = "ActionButton";
			BackgroundColor = Color.White;
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

			var playButton = new ActionButton{ 
				ButtonColor = Color.FromHex ("#2196F3"), 
                ButtonIcon = FontAwesomeLabel.FAUpload,
                ButtonText = "lalala",
                ButtonTextColor = Color.Black,
                Command = _command,
			};

			var abex = new ExpandableActionButton {
				ButtonColor = Color.FromHex("#FF9800"),
				ButtonIcon = FontAwesomeLabel.FAPlusSquare,
				Buttons = {
					playButton,
					new ActionButton{ ButtonColor = Color.FromHex("#009688"), ButtonIcon = FontAwesomeLabel.FATag, Command = _command},
					new ActionButton{ ButtonColor = Color.FromHex("#CDDC39"), ButtonIcon = FontAwesomeLabel.FARoad, Command = _command},
				}
			};
			layout.Children.Add(abex, () => new Rectangle(((layout.Width/4)*3)-(56/2), (layout.Height/2)-(200), 56, 250));
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

