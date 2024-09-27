namespace HavekrigerenApp;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(UsernameEntry.Text) && !string.IsNullOrEmpty(PasswordEntry.Text))
        {
            LoginButton.IsEnabled = true;
        }
        else
        {
            LoginButton.IsEnabled = false;
        }
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        User user = new User();
        bool isLoginSuccessful = user.Login(username, password);

        if (isLoginSuccessful)
        {
            DisplayAlert("Login", "Login successful!", "OK");
        }
        else
        {
            DisplayAlert("Login", "Incorrect username or password", "OK");
        }
    }

}