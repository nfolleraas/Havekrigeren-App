using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp
{
    public partial class ViewAllJobsPage : ContentPage
    {
        ViewAllJobsViewModel vm = new ViewAllJobsViewModel();

        public ViewAllJobsPage()
        {
            InitializeComponent();

            BindingContext = vm;
        }
    }
}