using HavekrigerenApp.Classes;
using Microsoft.Maui.Storage;

namespace HavekrigerenApp.Pages
{

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
                Console.WriteLine("Login button enabled");
            }
            else
            {
                loginButton.IsEnabled = false;
                Console.WriteLine("Login button disabled");
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;

            List<User> users = await Database.GetDocuments<User>("Users");

            bool isLoginSuccessful = User.Login(username, password, users);

            if (isLoginSuccessful)
            {
                try
                {
                    await Shell.Current.GoToAsync("///HomePage");
                    Console.WriteLine("Navigated to homepage");
                    SaveUserStatus(username, true);
                    usernameEntry.Text = null;
                    passwordEntry.Text = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Navigation error: {ex.Message}");
                }
            }
            else
            {
                await DisplayAlert("Log ind", "Forkert brugernavn eller adgangskode.", "Prøv igen");
            }
        }

        private void OnForgottonPassword(object sender, EventArgs e)
        {
            DisplayAlert("Glemt Adgangskode", "Glemt adgangskode.", "OK");
        }

        private void OnShowPasswordToggled(object sender, CheckedChangedEventArgs e)
        {
            passwordEntry.IsPassword = !e.Value;
        }

    }
}