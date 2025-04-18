using HavekrigerenApp.Models;
using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp
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

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            vm.LoadJobs();
        }
    }
}