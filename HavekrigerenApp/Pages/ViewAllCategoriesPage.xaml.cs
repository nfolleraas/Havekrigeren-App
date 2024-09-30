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

			Button categoryButton = new Button
			{
				Padding = new Thickness(10),
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Center,
				Command = new Command(() => OnCategoryClicked(category))
			};

			Frame categoryFrame = new Frame
			{
				CornerRadius = 10,
				Padding = new Thickness(10),
				HasShadow = false,
				GestureRecognizers =
				{
					new TapGestureRecognizer
					{
						Command = new Command(() => OnCategoryClicked(category))
					}
				},
				Margin = new Thickness(0, 5)
			};

			Grid buttonGrid = new Grid
			{
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Star},
					new ColumnDefinition { Width = GridLength.Auto}
				}
			};

			Label categoryLabel = new Label
			{
				Text = category,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Start
			};

			Label seeMoreLabel = new Label
			{
				Text = "Se mere",
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.End
			};

            buttonGrid.Children.Add(categoryLabel);
            Grid.SetColumn(categoryLabel, 0);

            buttonGrid.Children.Add(seeMoreLabel);
            Grid.SetColumnSpan(seeMoreLabel, 1);

			categoryFrame.Content = buttonGrid;

			viewAllCategoriesLayout.Children.Add(categoryFrame);
        }
	}

	private string categoryName;
    private void SetCategoryName(string category)
	{
        categoryName = category;
    }

	public string GetCategoryName()
	{
		return categoryName;
	}


	private async void OnCategoryClicked(string category)
	{
		// IT WORK! NO TOUCHY
        ViewAllCategoriesPage viewAllCategoriesPage = new ViewAllCategoriesPage();
        viewAllCategoriesPage.SetCategoryName(category);
        await Navigation.PushAsync(new ViewAllTasksPage(viewAllCategoriesPage));
    }

	private void OnCreateCategoryClicked(object sender, EventArgs e)
	{
        DisplayAlert("Opret Kategori", $"Du klikkede på knappen", "OK");
    }

}