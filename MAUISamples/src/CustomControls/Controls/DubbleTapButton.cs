using CustomControls.Resources.Fonts;
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace CustomControls.Controls
{
    public class DubbleTapButton : Button
    {
        private bool isFirstClick = true;
        private string fontFamily;
        private int width = 30;

        public new static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(DubbleTapButton),
            null,
            propertyChanged: OnCommandChanged);

        public new static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(DubbleTapButton),
            null);

        public new ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public new object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var button = (DubbleTapButton)bindable;
            button.UpdateCommand();
        }

        private void UpdateCommand()
        {
        }

        public new event EventHandler Clicked;

        public DubbleTapButton()
        {
            fontFamily = FontFamily;
            CornerRadius = 18;
            Text = FontAwesomeIcons.Xmark;
            Padding = 0;
            FontSize = 16;
            TextColor = Colors.Black;
            WidthRequest = width;
            HeightRequest = width;
            BackgroundColor = Colors.LightGray;
            FontFamily = "FontAwesome-Solid";

            base.Clicked += OnButtonClicked;
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            if (isFirstClick)
            {
                RunFirstClickAnimation();
            }
            else
            {
                ExecuteUserCommand();
            }
        }

        private void RunFirstClickAnimation()
        {
            TextColor = Colors.Transparent;
            Text = "Clear";
            new Animation(v => WidthRequest = v, width, 70).Commit(this, "Animate", 16, 200, Easing.SinOut, finished: (v, c) =>
            {
                FontFamily = fontFamily;
                TextColor = Colors.Black;
                FontSize = 14;
                CornerRadius = 18;
                isFirstClick = false; // Set the flag to indicate the first click has been handled
            });
        }

        private void ExecuteUserCommand()
        {
            if (Command != null)
            {
                if (Command.CanExecute(CommandParameter))
                {
                    Command.Execute(CommandParameter);
                }
            }
            Clicked?.Invoke(this, EventArgs.Empty);
            ReSet();
        }

        public void ReSet()
        {
            TextColor = Colors.Transparent;
            Text = FontAwesomeIcons.Xmark;
            new Animation(v => WidthRequest = v, 70, width).Commit(this, "AnimateReset", 16, 200, Easing.SinOut, finished: (v, c) =>
            {
                FontFamily = "FontAwesome-Solid";
                TextColor = Colors.Black;
                CornerRadius = 18;
                Padding = 0;
                FontSize = 16;
                isFirstClick = true; // Reset the flag when needed
            });
        }
    }
}
