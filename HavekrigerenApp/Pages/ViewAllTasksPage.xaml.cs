namespace HavekrigerenApp.Pages;

public partial class ViewAllTasksPage : ContentPage
{
	public ViewAllTasksPage(ViewAllCategoriesPage viewAllCategoriesPage)
	{
        InitializeComponent();

        string pageTitle = viewAllCategoriesPage.GetCategoryName();
        Title = pageTitle;

        DisplayTasks();
	}

    private void DisplayTasks()
    {
        var tsk = new Task();
        Console.WriteLine(Title);

        int taskCount = tsk.GetTasks(Title).Count();

        if (taskCount <= 0)
        {

            var noTasksLabel = new Label
            {
                Text = "Kunne ikke finde nogen opgaver...\n\nTryk på knappen (+) i midten af navigationslinjen for at tilføje en opgave.",
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            viewAllTasksLayout.Children.Add(noTasksLabel);

        }
        else
        {
            foreach (TaskManager task in tsk.GetTasks(Title))
            {
                if (task.Category == Title)
                {
                    var taskFrame = new Frame
                    {
                        CornerRadius = 10,
                        Padding = new Thickness(10),
                        HasShadow = false,
                        GestureRecognizers =
                {
                    new TapGestureRecognizer
                    {
                        Command = new Command(() => OnTaskClicked(task))
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

                    taskFrame.Content = buttonGrid;

                    viewAllTasksLayout.Children.Add(taskFrame);
                }
            }
        }
    }

    private void OnTaskClicked(TaskManager task)
    {
        DisplayAlert("Opgave", $"{task.ContactName}, {task.Address}, {task.PhoneNumber}, {task.Category}, {task.Date}, {task.Notes}", "OK");
    }
}