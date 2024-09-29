namespace HavekrigerenApp.Pages;

public partial class ViewAllCategoriesPage : ContentPage
{
	public ViewAllCategoriesPage()
	{
		InitializeComponent();

		DisplayAllCategories();
	}

	private void DisplayAllCategories()
	{
		Category cat = new Category();
		foreach (string category in cat.categories)
		{
            Entry categoryLabel = new Entry
            {
                Text = category,
                FontSize = 20,
                TextColor = Colors.Black
				
            };

			viewAllCategoriesLayout.Children.Add(categoryLabel);
        }
	}

}