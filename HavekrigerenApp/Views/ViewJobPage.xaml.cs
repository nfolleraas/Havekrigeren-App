using HavekrigerenApp.ViewModels;
using HavekrigerenApp.Models.Classes;

namespace HavekrigerenApp
{
    public partial class ViewJobPage : ContentPage
    {
        ViewJobViewModel vm;
        public ViewJobPage(Job job)
        {
            InitializeComponent();

            vm = new ViewJobViewModel(job);
            BindingContext = vm;
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);
            Navigation.PopAsync();
        }
    }
}