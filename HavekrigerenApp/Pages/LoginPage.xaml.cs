namespace HavekrigerenApp.Pages;

public partial class LoginPage : ContentPage
{

    public LoginPage()
    {
        InitializeComponent();
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(usernameEntry.Text) && !string.IsNullOrEmpty(passwordEntry.Text))
        {
            loginButton.IsEnabled = true;
        }
        else
        {
            loginButton.IsEnabled = false;
        }
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        string password = passwordEntry.Text;

        User user = new User();
        string isLoginSuccessful = user.Login(username, password);

        if (isLoginSuccessful == "Success")
        {
            await Navigation.PushAsync(new FrontPage());
            usernameEntry.Text = null;
            passwordEntry.Text = null;
        }
        else
        {
            await DisplayAlert("Log ind", isLoginSuccessful, "Prøv igen");
        }
    }

    private void OnForgottonPassword(object sender, EventArgs e)
    {
        DisplayAlert("Glemt adgangskode", "", "OK");
    }

    private void OnShowPasswordToggled(object sender, CheckedChangedEventArgs e)
    {
        passwordEntry.IsPassword = !e.Value;
    }

}