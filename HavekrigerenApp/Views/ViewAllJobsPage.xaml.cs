using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp
{
    public partial class ViewAllJobsPage : ContentPage
    {
        ViewAllJobsViewModel vm;

        public ViewAllJobsPage(string categoryName)
        {
            InitializeComponent();

            vm = new ViewAllJobsViewModel(categoryName);
            BindingContext = vm;

            Title = categoryName;
        }
    }
}