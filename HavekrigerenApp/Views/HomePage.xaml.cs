namespace HavekrigerenApp.Pages
{

	public partial class HomePage : ContentPage
	{
		public HomePage()
		{
			try
			{
				InitializeComponent();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Homepage error: {ex.Message}");
			}
        }
    }
}