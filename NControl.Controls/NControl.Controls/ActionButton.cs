﻿/************************************************************************
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2025 - Christian Falch
 * 
 * Permission is hereby granted, free of charge, to any person obtaining 
 * a copy of this software and associated documentation files (the 
 * "Software"), to deal in the Software without restriction, including 
 * without limitation the rights to use, copy, modify, merge, publish, 
 * distribute, sublicense, and/or sell copies of the Software, and to 
 * permit persons to whom the Software is furnished to do so, subject 
 * to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be 
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 ************************************************************************/

using System;
using NControl.Abstractions;
using NGraphics;
using Xamarin.Forms;
using System.Windows.Input;
using Color = Xamarin.Forms.Color;
using TextAlignment = Xamarin.Forms.TextAlignment;

namespace NControl.Controls
{
	/// <summary>
	/// Implements a floating action button with a font awesome label as
	/// the icon. An action button is part of Google Material Design and 
	/// therefore has a few attributes that should be set correctly.
	/// 
	/// http://www.google.com/design/spec/components/buttons.html
	/// 
	/// 
	/// </summary>
	public class ActionButton: NControlView
	{
		#region Protected Members

		/// <summary>
		/// The button shadow element.
		/// </summary>
		protected readonly NControlView ButtonShadowElement;

		/// <summary>
		/// The button element.
		/// </summary>
		protected readonly NControlView ButtonElement;

		/// <summary>
		/// The button icon label.
		/// </summary>
		protected readonly FontAwesomeLabel ButtonIconLabel;

        /// <summary>
        /// The button side text.
        /// </summary>
        protected Label SideLabel;

        protected RoundCornerView SideLabelBackground;

	    protected Grid SideLabelGrid;

        // used on android as ncontrols seems to operate on hardware pixels
        protected float AndroidScale = 1.0f;

        private double labelWidth;

        private double marginSize = Device.RuntimePlatform == Device.Android ? 2 : 8;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.ActionButton"/> class.
		/// </summary>
		public ActionButton(string sideLabelText="")
		{
			var layout = new Grid{Padding = 0, ColumnSpacing = 0, RowSpacing = 0};

			ButtonShadowElement = new NControlView
            { 
				DrawingFunction = (canvas, rect) => 
                {
					// Draw shadow
					rect.Inflate(new NGraphics.Size(-4));
					rect.Y += 4;
                    double pos_x = SideLabel.Width > rect.Width  ? -SideLabelBackground.Width : -rect.Width - 8;

                    switch (Device.RuntimePlatform)
                    {
                        case Device.iOS:
                            canvas.DrawEllipse(rect, null, new NGraphics.RadialGradientBrush(
                            new NGraphics.Color(0, 0, 0, 200), NGraphics.Colors.Clear));

                            break;
                        case Device.Android:
                            canvas.DrawEllipse(rect, null, new NGraphics.RadialGradientBrush(
                            new NGraphics.Point(rect.Width / 2, (rect.Height / 2) + 2),
                            new NGraphics.Size(rect.Width, rect.Height),
                            new NGraphics.Color(0, 0, 0, 200), NGraphics.Colors.Clear));
                            var rectScaled = rect.Width / AndroidScale;
                            pos_x = SideLabel.Width > rectScaled ? -SideLabel.Width - marginSize - marginSize : -rectScaled - (8 / AndroidScale);

                            break;
                        default:
                            canvas.DrawEllipse(rect, null, new NGraphics.RadialGradientBrush(
                                new NGraphics.Color(0, 0, 0, 200), NGraphics.Colors.Clear));

                            break;
                    }
                    SideLabelGrid.TranslationX = pos_x;
				},
			};

			ButtonElement = new NControlView{ 
				DrawingFunction = (canvas, rect) => {

					// Draw button circle
					rect.Inflate (new NGraphics.Size (-8));
					canvas.DrawEllipse (rect, null, new NGraphics.SolidBrush(ButtonColor.ToNColor()));
				}
			};

			ButtonIconLabel = new FontAwesomeLabel{
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				Text = FontAwesomeLabel.FAPlus,
				FontSize = 14.0,
			};


            labelWidth = DependencyService.Get<CalculateTextWidth>().CalculateWidth(sideLabelText);
            SideLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.NoWrap,
                TextColor = Color.Black,
                Text = sideLabelText,
                FontSize = 10.0,
                Margin = new Thickness { Top=2, Bottom=2, Left=marginSize, Right=marginSize },
                WidthRequest = labelWidth,
            };

            SideLabelBackground = new RoundCornerView
            {
                BackgroundColor = Color.Transparent,
                CornerRadius = Device.RuntimePlatform == Device.Android ? 6 : 3,
                BorderColor = Color.Transparent,
                BorderWidth = 2,
                WidthRequest = labelWidth,
            };

            labelWidth += Device.RuntimePlatform == Device.Android ? 8 : 0;
            SideLabelGrid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = labelWidth },
                },
            };
            SideLabelGrid.Children.Add(SideLabelBackground, 0, 1);
            SideLabelGrid.Children.Add(SideLabel, 0, 1);

            layout.Children.Add(ButtonShadowElement);
			layout.Children.Add(ButtonElement);
			layout.Children.Add(ButtonIconLabel);
            layout.Children.Add(SideLabelGrid);

            Content = layout;
		}

        #region Properties

		/// <summary>
		/// The command property.
		/// </summary>
		public static BindableProperty CommandProperty = 
			BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ActionButton), null,
				BindingMode.TwoWay, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
					ctrl.Command = (ICommand)newValue;
				});

		/// <summary>
		/// Gets or sets the color of the buton.
		/// </summary>
		/// <value>The color of the buton.</value>
		public ICommand Command
		{
			get {  return (ICommand)GetValue (CommandProperty);}
			set {

				if (Command != null)
					Command.CanExecuteChanged -= HandleCanExecuteChanged;
				
				SetValue (CommandProperty, value);

				if (Command != null)
					Command.CanExecuteChanged += HandleCanExecuteChanged;
				
			}
		}

		/// <summary>
		/// The command parameter property.
		/// </summary>
		public static BindableProperty CommandParameterProperty = 
			BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ActionButton), null,
				BindingMode.TwoWay, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
					ctrl.CommandParameter = newValue;
				});

		/// <summary>
		/// Gets or sets the color of the buton.
		/// </summary>
		/// <value>The color of the buton.</value>
		public object CommandParameter
		{
			get {  return GetValue (CommandParameterProperty);}
			set {
				SetValue (CommandParameterProperty, value);
			}
		}

		/// <summary>
		/// The button color property.
		/// </summary>
		public static BindableProperty ButtonColorProperty = 
			BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(ActionButton), Color.Gray,
				BindingMode.TwoWay, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
					ctrl.ButtonColor = (Color)newValue;
				});

		/// <summary>
		/// Gets or sets the color of the buton.
		/// </summary>
		/// <value>The color of the buton.</value>
		public Color ButtonColor
		{
			get {  return (Color)GetValue (ButtonColorProperty);}
			set {
				SetValue (ButtonColorProperty, value);
				ButtonElement.Invalidate ();
			}
		}

		/// <summary>
		/// The button font family property.
		/// </summary>
		public static BindableProperty ButtonFontFamilyProperty = 
			BindableProperty.Create(nameof(ButtonFontFamily), typeof(string), typeof(ActionButton), null,
				BindingMode.TwoWay, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
					ctrl.ButtonFontFamily = (string)newValue;
				});

		/// <summary>
		/// Gets or sets the color of the buton.
		/// </summary>
		/// <value>The color of the buton.</value>
		public string ButtonFontFamily
		{
			get {  return (string)GetValue (ButtonFontFamilyProperty);}
			set {
				SetValue (ButtonFontFamilyProperty, value);
				ButtonIconLabel.FontFamily = value;
			}
		}

		/// <summary>
		/// The button icon property.
		/// </summary>
		public static BindableProperty ButtonIconProperty = 
			BindableProperty.Create(nameof(ButtonIcon), typeof(string), typeof(ActionButton), FontAwesomeLabel.FAPlus,
				BindingMode.TwoWay, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
					ctrl.ButtonIcon = (string)newValue;
				});

        /// <summary>
        /// The button text property.
        /// </summary>
        //public static BindableProperty SideLabelTextProperty =
        //    BindableProperty.Create(nameof(SideLabelText), typeof(string), typeof(ActionButton), "",
        //        BindingMode.TwoWay, null, (bindable, oldValue, newValue) =>
        //        {
        //            var ctrl = (ActionButton)bindable;
        //            ctrl.SideLabelText = (string)newValue;
        //        });

        //public string SideLabelText
        //{
        //    get { return (string)GetValue(SideLabelTextProperty); }
        //    set
        //    {
        //        SetValue(SideLabelTextProperty, value);
        //        SideLabel.Text = value;
        //    }
        //}

        /// <summary>
        /// The side label text color property.
        /// </summary>
        public static BindableProperty SideLabelTextColorProperty =
            BindableProperty.Create(nameof(SideLabelTextColor), typeof(Color), typeof(ActionButton), Color.Black,
                BindingMode.TwoWay, null, (bindable, oldValue, newValue) =>
                {
                    var ctrl = (ActionButton)bindable;
                    ctrl.SideLabelTextColor = (Color)newValue;
                });

        /// <summary>
        /// Gets or sets the color of the side label text.
        /// </summary>
        /// <value>The color of the buton.</value>
        public Color SideLabelTextColor
        {
            get { return (Color)GetValue(SideLabelTextColorProperty); }
            set
            {
                SetValue(SideLabelTextColorProperty, value);
                SideLabel.TextColor = value;
            }
        }

	    /// <summary>
	    /// The side label background color property.
	    /// </summary>
	    public static BindableProperty SideLabelBackgroundColorProperty =
	        BindableProperty.Create(nameof(SideLabelBackgroundColor), typeof(Color), typeof(ActionButton), Color.White,
	            BindingMode.TwoWay, null, (bindable, oldValue, newValue) =>
	            {
	                var ctrl = (ActionButton)bindable;
	                ctrl.SideLabelBackgroundColor = (Color)newValue;
	            });

	    /// <summary>
	    /// Gets or sets the background color of the side label.
	    /// </summary>
	    /// <value>The color of the buton.</value>
	    public Color SideLabelBackgroundColor
	    {
	        get { return (Color)GetValue(SideLabelBackgroundColorProperty); }
	        set
	        {
	            SetValue(SideLabelBackgroundColorProperty, value);
	            SideLabelBackground.BackgroundColor = value;
	            SideLabelBackground.BorderColor = value;
	        }
	    }

	    /// <summary>
	    /// The side label text font size property.
	    /// </summary>
	    public static BindableProperty SideLabelFontSizeProperty =
	        BindableProperty.Create(nameof(SideLabelFontSize), typeof(double), typeof(ActionButton), 10.0,
	            BindingMode.TwoWay, null, (bindable, oldValue, newValue) =>
	            {
	                var ctrl = (ActionButton)bindable;
	                ctrl.SideLabelFontSize = (double)newValue;
	            });

	    /// <summary>
	    /// Gets or sets the font size of the side label text.
	    /// </summary>
	    /// <value>The font size of the buton.</value>
	    public double SideLabelFontSize
	    {
	        get { return (double)GetValue(SideLabelFontSizeProperty); }
	        set
	        {
	            SetValue(SideLabelFontSizeProperty, value);
	            SideLabel.FontSize = value;
	        }
	    }

	    /// <summary>
		/// Gets or sets the button icon.
		/// </summary>
		/// <value>The button icon.</value>
		public string ButtonIcon
		{
			get {  return (string)GetValue (ButtonIconProperty);}
			set {
				SetValue (ButtonIconProperty, value);
				ButtonIconLabel.Text = value;
			}
		}

        /// <summary>
        /// The button icon size property.
        /// </summary>
        public static BindableProperty ButtonIconSizeProperty =
            BindableProperty.Create(nameof(ButtonIconSize), typeof(Color), typeof(ActionButton), Color.Black,
                BindingMode.TwoWay, null, (bindable, oldValue, newValue) =>
                {
                    var ctrl = (ActionButton)bindable;
                    ctrl.ButtonIconSize = (double)newValue;
                });

        /// <summary>
        /// Gets or sets the icon size.
        /// </summary>
        /// <value>The font size of the buton.</value>
        public double ButtonIconSize
        {
            get { return (double)GetValue(ButtonIconSizeProperty); }
            set
            {
                SetValue(ButtonIconSizeProperty, value);
                ButtonIconLabel.FontSize = value;
            }
        }

		/// <summary>
		/// The button icon property.
		/// </summary>
		public static BindableProperty HasShadowProperty = 
			BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(ActionButton), true,
				BindingMode.TwoWay, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
					ctrl.HasShadow = (bool)newValue;
				});

		/// <summary>
		/// Gets or sets a value indicating whether this instance has shadow.
		/// </summary>
		/// <value><c>true</c> if this instance has shadow; otherwise, <c>false</c>.</value>
		public bool HasShadow
		{
			get {  return (bool)GetValue (HasShadowProperty);}
			set
			{
			    SetValue (HasShadowProperty, value);
			    ButtonShadowElement.FadeTo(value ? 1.0 : 0.0, 250);
			}
		}

		/// <summary>
		/// The button icon color property.
		/// </summary>
		public static BindableProperty ButtonIconColorProperty = 
			BindableProperty.Create(nameof(ButtonIconColor), typeof(Color), typeof(ActionButton), Color.White,
				BindingMode.TwoWay, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
					ctrl.ButtonIconColor = (Color)newValue;
				});

		/// <summary>
		/// Gets or sets the button icon color.
		/// </summary>
		/// <value>The button icon.</value>
		public Color ButtonIconColor
		{
			get {  return (Color)GetValue (ButtonIconColorProperty);}
			set {
				SetValue (ButtonIconColorProperty, value);
				ButtonIconLabel.TextColor = value;
			}
		}

        /// <summary>
        /// The density scale property.
        /// </summary>
        public static BindableProperty DensityScaleProperty =
            BindableProperty.Create(nameof(DensityScale), typeof(float), typeof(ActionButton), 1.0f,
                BindingMode.TwoWay, null, (bindable, oldValue, newValue) =>
                {
                    var ctrl = (ActionButton)bindable;
                    ctrl.DensityScale = (float)newValue;
                });

        /// <summary>
        /// Gets or sets the density scale.
        /// </summary>
        public float DensityScale
        {
            get { return (float)GetValue(DensityScaleProperty); }
            set
            {
                SetValue(DensityScaleProperty, value);
                AndroidScale = value;
            }
        }

		#endregion

		#region Private Members

		/// <summary>
		/// Handles the can execute changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void HandleCanExecuteChanged(object sender, EventArgs args)
		{
			IsEnabled = Command.CanExecute (CommandParameter);
		}
		#endregion

		#region Touch Events

		/// <summary>
		/// Toucheses the began.
		/// </summary>
		/// <param name="points">Points.</param>
		public override bool TouchesBegan (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesBegan (points);

			if (!IsEnabled)
				return false;

			ButtonElement.ScaleTo (1.15, 140, Easing.CubicInOut);

			ButtonIconLabel.ScaleTo (1.2, 140, Easing.CubicInOut);

			if (HasShadow) {
				ButtonShadowElement.TranslateTo (0.0, 3, 140, Easing.CubicInOut);
				ButtonShadowElement.ScaleTo (1.2, 140, Easing.CubicInOut);
				ButtonShadowElement.FadeTo (0.44, 140, Easing.CubicInOut);
			}

			return true;
		}

		/// <summary>
		/// Toucheses the cancelled.
		/// </summary>
		/// <param name="points">Points.</param>
		public override bool TouchesCancelled (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesCancelled (points);

			if (!IsEnabled)
				return false;

			ButtonElement.ScaleTo (1.0, 140, Easing.CubicInOut);
			ButtonIconLabel.ScaleTo (1.0, 140, Easing.CubicInOut);

			if (HasShadow) {
				ButtonShadowElement.TranslateTo (0.0, 0.0, 140, Easing.CubicInOut);
				ButtonShadowElement.ScaleTo (1.0, 140, Easing.CubicInOut);
				ButtonShadowElement.FadeTo (1.0, 140, Easing.CubicInOut);
			}                		

			return true;
		}

		/// <summary>
		/// Toucheses the ended.
		/// </summary>
		/// <param name="points">Points.</param>
		public override bool TouchesEnded (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesEnded (points);

			if (!IsEnabled)
				return false;

			if (Command != null && Command.CanExecute (CommandParameter))
				Command.Execute (CommandParameter);

			ButtonElement.ScaleTo (1.0, 140, Easing.CubicInOut);

			ButtonIconLabel.ScaleTo (1.0, 140, Easing.CubicInOut);

			if (HasShadow) {
				ButtonShadowElement.TranslateTo (0.0, 0.0, 140, Easing.CubicInOut);
				ButtonShadowElement.ScaleTo (1.0, 140, Easing.CubicInOut);
				ButtonShadowElement.FadeTo (1.0, 140, Easing.CubicInOut);
			}

			return true;
		}

        #endregion

        /// <summary>
        /// On Measure
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var retVal = base.OnMeasure(widthConstraint, heightConstraint);

            if (retVal.Request.Width > retVal.Request.Height)
                retVal.Request = new Xamarin.Forms.Size(retVal.Request.Width, retVal.Request.Width);
            else if (retVal.Request.Height > retVal.Request.Width)
                retVal.Request = new Xamarin.Forms.Size(retVal.Request.Height, retVal.Request.Height);

            retVal.Minimum = retVal.Request;
            return retVal;
        }	
	}
}

