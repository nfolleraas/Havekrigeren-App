namespace HavekrigerenApp.Pages;

public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        string activeUser = Preferences.Default.Get("ActiveUser", "Unknown");
        welcomeLabel.Text = $"Velkommen {activeUser}";
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
	{
        Preferences.Default.Clear();
        await Shell.Current.GoToAsync("///LoginPage");
    }
}