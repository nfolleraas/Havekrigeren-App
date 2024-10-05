using Microsoft.Maui.Animations;
using System.Diagnostics;

namespace HavekrigerenApp.Pages
{
    public partial class ViewAllCategoriesPage : ContentPage
    {
        private List<Category> categories;

        public ViewAllCategoriesPage()
        {
            InitializeComponent();

            LoadCategories();
        }

        public async void LoadCategories()
        {
            var activityIndicator = new ActivityIndicator
            {
                IsVisible = true,
                IsRunning = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            viewAllCategoriesLayout.Children.Add(activityIndicator);
            
            categories = await Category.GetCategories();

            viewAllCategoriesLayout.Children.Remove(activityIndicator);

            DisplayAllCategories();
        }

        private void DisplayAllCategories()
        {
            viewAllCategoriesLayout.Children.Clear();

            int categoryCount;
            try
            {
                categoryCount = categories.Count();
            }
            catch
            {
                categoryCount = 0;
            }

            if (categoryCount < 1)
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
                foreach (Category category in categories)
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
                        Command = new Command(() => OnCategoryClicked(category.CategoryName))
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
                        Text = category.CategoryName,
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

        private async void OnReloadButtonClicked(object sender, EventArgs e)
        {
            LoadCategories();
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


        private async void OnCategoryClicked(string categoryName)
        {
            // IT WORK! NO TOUCHY
            //var viewAllCategoriesPage = new ViewAllCategoriesPage();
            //viewAllCategoriesPage.SetCategoryName(category);


            await Navigation.PushAsync(new ViewAllTasksPage(categoryName));
        }

        private void OnCreateCategoryClicked(object sender, EventArgs e)
        {
            string temp = "Test Kategori";
            Category.AddCategory(temp);
            DisplayAlert("Opret Kategori", $"Oprettede kategori: {temp}", "OK");
        }
    }
}