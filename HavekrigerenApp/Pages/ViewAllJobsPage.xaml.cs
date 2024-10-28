using HavekrigerenApp.Classes;

namespace HavekrigerenApp.Pages
{
    public partial class ViewAllJobsPage : ContentPage
    {
        private List<Job> jobs;
        private string errorMessage;

        public ViewAllJobsPage(string categoryName)
        {
            InitializeComponent();

            Title = categoryName;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await LoadJobs();
        }

        public async Task LoadJobs()
        {
            jobs = await GetJobsByCategory(Title);

            DisplayAllJobs();
        }

        private async Task<List<Job>> GetJobsByCategory(string categoryName)
        {
            List<Job> jobs = await Database.GetDocuments<Job>("Jobs");
            List<Job> jobsByCategory = new List<Job>();

            foreach (Job job in jobs)
            {
                if (job.Category == categoryName)
                {
                    jobsByCategory.Add(job);
                }
            }
            return jobsByCategory;
        }

        private void DisplayAllJobs()
        {
            viewAllJobsLayout.Children.Clear();

            int jobCount;
            try
            {
                jobCount = jobs.Count();
            }
            catch (Exception e)
            {
                jobCount = -1;
                errorMessage = $"Der er sket en uventet fejl. \n Genstart appen og prøv igen. \n\n Fejlbesked: \n {e.Message}";
            }

            if (jobCount < 0)
            {
                var errorLabel = new Label
                {
                    Text = errorMessage,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                viewAllJobsLayout.Children.Add(errorLabel);
            }
            else if (jobCount == 0)
            {
                var noJobsLabel = new Label
                {
                    Text = "Kunne ikke finde nogen opgaver...\n\nTryk på knappen (+) i midten af navigationslinjen for at tilføje en opgave.",
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                viewAllJobsLayout.Children.Add(noJobsLabel);
            }
            else
            {
                foreach (Job job in jobs)
                {
                    // Delete Swipe
                    var deleteSwipeItem = new SwipeItem
                    {
                        IconImageSource = "Resources/Icons/delete.png",

                        BackgroundColor = Colors.Red,
                    };

                    deleteSwipeItem.Invoked += (sender, e) =>
                    {
                        DeleteJob(sender, e, job.ContactName, job.Address);
                    };

                    List<SwipeItem> rightSwipeItems = new List<SwipeItem>() { deleteSwipeItem };

                    var swipeView = new SwipeView
                    {
                        LeftItems = new SwipeItems(rightSwipeItems),
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

                    var jobLabel = new Label
                    {
                        Text = $"{job.ContactName}\n{job.Address}",
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

                    buttonGrid.Children.Add(jobLabel);
                    Grid.SetColumn(jobLabel, 0);

                    buttonGrid.Children.Add(seeMoreLabel);
                    Grid.SetColumnSpan(seeMoreLabel, 1);

                    var jobFrame = new Frame
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
                                Command = new Command(() => OnJobClicked(job))
                            }
                        },
                    };

                    swipeView.Content = jobFrame;

                    viewAllJobsLayout.Children.Add(swipeView);
                }

                var infoLabel = new Label
                {
                    Text = "Tryk på + for at tilføje flere\nSwipe til venstre for at slette en opgave",
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Colors.Gray
                };
                viewAllJobsLayout.Children.Add(infoLabel);
            }
        }

        private async void RefreshCommand(object? sender, EventArgs e)
        {
            jobsRefreshView.IsRefreshing = true;
            Console.WriteLine("Started reloading");

            await LoadJobs();

            jobsRefreshView.IsRefreshing = false;
            Console.WriteLine("Done reloading");
        }

        private async void OnJobClicked(Job job)
        {
            await Navigation.PushAsync(new ViewJobPage(job));
        }

        private async void DeleteJob(object? sender, EventArgs e, string contactName, string address)
        {
            bool answer = await DisplayAlert("Slet Opgave", $"Er du sikker på du vil slette opgaven: \"{contactName} på {address}\"?\nDenne handling kan ikke fortrydes.", "Ja", "Nej");

            if (answer)
            {
                Database.DeleteDocument("Jobs", "contactName", contactName);
                await DisplayAlert("Slet Opgave", $"Opgaven: \"{contactName} på {address}\" blev slettet.", "OK");
                LoadJobs();
            }
        }
    }
}