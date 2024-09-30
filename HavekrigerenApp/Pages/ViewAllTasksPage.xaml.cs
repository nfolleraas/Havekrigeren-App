namespace HavekrigerenApp.Pages;

public partial class ViewAllTasksPage : ContentPage
{
	public ViewAllTasksPage(ViewAllCategoriesPage viewAllCategoriesPage)
	{
        InitializeComponent();

        string pageTitle = viewAllCategoriesPage.GetCategoryName();
        Title = pageTitle;
        Console.WriteLine($"Title: {Title}");
	}
}