using HavekrigerenApp.Classes;

namespace HavekrigerenApp.Pages;

public partial class AdminPage : ContentPage
{
	public AdminPage()
	{
		InitializeComponent();
	}

    private void OnTextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(usernameEntry.Text) && !string.IsNullOrEmpty(passwordEntry.Text))
        {
            createUserButton.IsEnabled = true;
        }
        else
        {
            createUserButton.IsEnabled = false;
        }
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        string password = passwordEntry.Text;

        Dictionary<string, object> userData = new Dictionary<string, object>()
        {
            {"name", username},
            {"password", password}
        };

        Database.AddDocument("Users", userData);

        usernameEntry.Text = "";
        passwordEntry.Text = "";

        await DisplayAlert("ADMIN", $"Oprettede bruger: \"{username}\" med adgangskode: \"{password}\".", "OK");
    }
}