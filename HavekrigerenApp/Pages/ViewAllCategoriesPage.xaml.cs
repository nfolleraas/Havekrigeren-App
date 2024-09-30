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
		foreach (string category in cat.GetCategories())
		{
            Label categoryLabel = new Label
            {
                Text = category,
                FontSize = 20,
                TextColor = Colors.Black
            };

			Button categoryButton = new Button
			{
				Text = "Se mere",
				FontSize = 20,
				TextColor = Colors.Black
			};

			viewAllCategoriesLayout.Children.Add(categoryLabel);
            viewAllCategoriesLayout.Children.Add(categoryButton);
        }
	}

}