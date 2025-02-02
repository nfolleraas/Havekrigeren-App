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
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            await vm.LoadJobs();
        }
    }
}