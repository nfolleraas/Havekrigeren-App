using System.Linq;
using HavekrigerenApp.Models;

namespace HavekrigerenApp.Pages
{

    public partial class CreateCategoryPage : ContentPage
	{
		public CreateCategoryPage()
		{
			InitializeComponent();
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
            if (!string.IsNullOrEmpty(categoryEntry.Text))
            {
                createCategoryButton.IsEnabled = true;
            }
            else
            {
                createCategoryButton.IsEnabled = false;
            }
        }

		private async void OnCreateClicked(object sender, EventArgs e)
		{
            string categoryName = categoryEntry.Text;
            List<Category> categories = await Database.GetDocuments<Category>("Categories");
            bool categoryExists = categories.Any(c => c.CategoryName == categoryName);

            if (categoryExists)
            {
                await DisplayAlert("Fejl", $"Kategorien \"{categoryName}\" eksisterer allerede.", "OK");
            }
            else
            {
                Dictionary<string, object> categoryData = new Dictionary<string, object>()
                {
                    {"categoryName", categoryName}
                };

                Database.AddDocument("Categories", categoryData);

                await Navigation.PopAsync();

                await DisplayAlert("Opret Kategori", $"Oprettede kategori: \"{categoryName}\".", "OK");
            }
        }
    }
}