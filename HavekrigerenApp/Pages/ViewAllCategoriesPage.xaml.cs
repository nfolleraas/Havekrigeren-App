using Microsoft.Maui.Animations;

namespace HavekrigerenApp.Pages
{
    public partial class ViewAllCategoriesPage : ContentPage
    {
        public ViewAllCategoriesPage()
        {
            InitializeComponent();

            DisplayAllCategories();
        }

        private void DisplayAllCategories()
        {
            var cat = new Category();

            int categoryCount = cat.GetCategories().Count();

            if (categoryCount <= 0)
            {

                var noCategoriesLabel = new Label
                {
                    Text = "Kunne ikke finde nogen kategorier...\n\nTryk på knappen (+) i højre hjørne for at tilføje en kategori.",
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                };

                viewAllCategoriesLayout.Children.Add(noCategoriesLabel);

            }
            else
            {
                foreach (string category in cat.GetCategories())
                {
                    var categoryFrame = new Frame
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

                    var buttonGrid = new Grid
                    {
                        ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star},
                    new ColumnDefinition { Width = GridLength.Auto}
                }
                    };

                    var categoryLabel = new Label
                    {
                        Text = category,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Start,
                        LineBreakMode = LineBreakMode.CharacterWrap,
                        MaximumWidthRequest = 300
                    };

                    var seeMoreLabel = new Label
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
            var viewAllCategoriesPage = new ViewAllCategoriesPage();
            viewAllCategoriesPage.SetCategoryName(category);
            await Navigation.PushAsync(new ViewAllTasksPage(viewAllCategoriesPage));
        }

        private void OnCreateCategoryClicked(object sender, EventArgs e)
        {
            DisplayAlert("Opret Kategori", $"Du klikkede på knappen", "OK");
        }

    }
}