using Android.Webkit;
using System.Linq;

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
            string createdCategory = categoryEntry.Text;
            List<Category> categories = await Category.GetCategories();
            bool categoryExists = categories.Any(c => c.CategoryName == createdCategory);

            if (categoryExists)
            {
                await DisplayAlert("Fejl", $"Kategorien {createdCategory} eksisterer allerede", "OK");
            }
            else
            {
                Category.AddCategory(createdCategory);

                await Navigation.PopAsync();

                await DisplayAlert("Opret Kategori", $"Oprettede kategori: {createdCategory}", "OK");
            }
        }
    }
}