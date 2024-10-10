using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Animations;
using System.Diagnostics;

namespace HavekrigerenApp.Pages
{
    public partial class ViewAllCategoriesPage : ContentPage
    {
        private List<Category> categories;
        private string errorMessage;

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
            catch (Exception e)
            {
                categoryCount = -1;
                errorMessage = $"Der er sket en uventet fejl. \n Genstart appen og prøv igen. \n\n Fejlbesked: \n {e.Message}";
            }

            if (categoryCount < 0)
            {
                var errorLabel = new Label
                {
                    Text = errorMessage,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                viewAllCategoriesLayout.Children.Add(errorLabel);
            }
            else if (categoryCount == 0)
            {
                var noCategoriesLabel = new Label
                {
                    Text = "Kunne ikke finde nogen kategorier...\n\nTryk på knappen (+) i højre hjørne for at tilføje en kategori.",
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                viewAllCategoriesLayout.Children.Add(noCategoriesLabel);
            }
            else
            {
                foreach (Category category in categories)
                {
                    // Delete Swipe
                    var deleteSwipeItem = new SwipeItem
                    {
                        IconImageSource = "Resources/Icons/delete.png",
                        
                        BackgroundColor = Colors.Red,
                    };
                    deleteSwipeItem.Invoked += (sender, e) => DeleteCategory(sender, e, category.CategoryName);

                    List<SwipeItem> rightSwipeItems = new List<SwipeItem>() { deleteSwipeItem };

                    var swipeView = new SwipeView
                    {
                        RightItems = new SwipeItems(rightSwipeItems),
                    };

                    // SwipeView content
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

                    var categoryFrame = new Frame
                    {
                        CornerRadius = 10, // Set corner radius
                        Padding = new Thickness(10),
                        HasShadow = false,
                        Content = buttonGrid,
                        Margin = new Thickness(0, 5),

                        GestureRecognizers =
                        {
                            new TapGestureRecognizer
                            {
                                Command = new Command(() => OnCategoryClicked(category.CategoryName))
                            }
                        },
                    };

                    swipeView.Content = categoryFrame;

                    viewAllCategoriesLayout.Children.Add(swipeView);
                }
            }
        }

        private void OnReloadButtonClicked(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private async void OnCategoryClicked(string categoryName)
        {
            await Navigation.PushAsync(new ViewAllTasksPage(categoryName));
        }

        private async void OnCreateCategoryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateCategoryPage());
        }

        private async void DeleteCategory(object sender, EventArgs e, string categoryName)
        {
            bool answer = await DisplayAlert($"Er du sikker på du vil slette \"{categoryName}\"", "Denne handling kan ikke fortrydes", "Ja", "Nej");

            if (answer)
            {
                Category.DeleteCategory(categoryName);
                await DisplayAlert("Kategori Slettet", $"Kategorien: \"{categoryName}\" blev slettet", "OK");
                LoadCategories();
            }
        }
    }
}