using Microsoft.Maui.Storage;

namespace HavekrigerenApp.Pages;

public partial class LoginPage : ContentPage
{

    public LoginPage()
    {
        InitializeComponent();
    }

    public static void SaveUserStatus(string username, bool isLoggedIn)
    {
        Preferences.Default.Set("ActiveUser", username);
        Preferences.Default.Set("IsLoggedIn", isLoggedIn);
        Console.WriteLine($"{Preferences.Default.Get("ActiveUser", "Unknown")}, {Preferences.Default.Get("IsLoggedIn", false)}");
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
        bool isLoginSuccessful = user.Login(username, password);

        if (isLoginSuccessful)
        {
            await Shell.Current.GoToAsync("///HomePage");
            SaveUserStatus(username, true);
            usernameEntry.Text = null;
            passwordEntry.Text = null;
        }
        else
        {
            await DisplayAlert("Log ind", "Forkert brugernavn eller adgangskode", "Prøv igen");
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