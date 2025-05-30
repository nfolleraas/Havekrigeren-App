using HavekrigerenApp.Models;
using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp.Views
{
    public partial class ViewAllJobsPage : ContentPage
    {
        ViewAllJobsViewModel vm;

        public ViewAllJobsPage(Category selectedCategory)
        {
            InitializeComponent();

            vm = new ViewAllJobsViewModel(selectedCategory);
            BindingContext = vm;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            vm.LoadJobs();
        }
    }
}