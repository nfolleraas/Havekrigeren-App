namespace HavekrigerenApp.Pages
{
    public partial class ViewAllTasksPage : ContentPage
    {
        private List<Task> tasks;
        private string errorMessage;

        public ViewAllTasksPage(string categoryName)
        {
            InitializeComponent();

            Title = categoryName;

            LoadTasks();
        }

        public async void LoadTasks()
        {
            var activityIndicator = new ActivityIndicator
            {
                IsVisible = true,
                IsRunning = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            viewAllTasksLayout.Children.Add(activityIndicator);

            tasks = await Task.GetTasks();

            viewAllTasksLayout.Children.Remove(activityIndicator);

            DisplayAllTasks();
        }

        private void DisplayAllTasks()
        {
            viewAllTasksLayout.Children.Clear();

            int taskCount;
            try
            {
                taskCount = tasks.Count();
            }
            catch (Exception e)
            {
                taskCount = -1;
                errorMessage = $"Der er sket en uventet fejl. \n Genstart appen og prøv igen. \n\n Fejlbesked: \n {e.Message}";
            }

            if (taskCount < 0)
            {
                var errorLabel = new Label
                {
                    Text = errorMessage,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                viewAllTasksLayout.Children.Add(errorLabel);
            }
            else if (taskCount == 0)
            {
                var noTasksLabel = new Label
                {
                    Text = "Kunne ikke finde nogen opgaver...\n\nTryk på knappen (+) i midten af navigationslinjen for at tilføje en opgave.",
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                viewAllTasksLayout.Children.Add(noTasksLabel);
            }
            else
            {
                foreach (Task task in tasks)
                {
                    // Delete Swipe
                    var deleteSwipeItem = new SwipeItem
                    {
                        IconImageSource = "Resources/Icons/delete.png",

                        BackgroundColor = Colors.Red,
                    };
                    deleteSwipeItem.Invoked += (sender, e) => DeleteTask(sender, e, task.ContactName, task.Address);

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

                    var taskLabel = new Label
                    {
                        Text = $"{task.ContactName}\n{task.Address}",
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

                    buttonGrid.Children.Add(taskLabel);
                    Grid.SetColumn(taskLabel, 0);

                    buttonGrid.Children.Add(seeMoreLabel);
                    Grid.SetColumnSpan(seeMoreLabel, 1);

                    var taskFrame = new Frame
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
                                Command = new Command(() => OnTaskClicked(task))
                            }
                        },
                    };

                    swipeView.Content = taskFrame;

                    viewAllTasksLayout.Children.Add(swipeView);
                }
            }
        }

        private void OnReloadButtonClicked(object sender, EventArgs e)
        {
            LoadTasks();
        }

        private async void OnTaskClicked(Task task)
        {
            await Navigation.PushAsync(new ViewTaskPage(task));
        }

        private async void DeleteTask(object sender, EventArgs e, string contactName, string address)
        {
            bool answer = await DisplayAlert($"Er du sikker på du vil slette opgaven hos {contactName} på {address}?", "Denne handling kan ikke fortrydes", "Ja", "Nej");

            if (answer)
            {
                Task.DeleteTask(contactName);
                await DisplayAlert("Opgave Slettet", $"Opgaven: \"{contactName} på {address}\" blev slettet", "OK");
                LoadTasks();
            }
        }
    }
}