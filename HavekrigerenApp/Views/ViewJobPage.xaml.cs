using HavekrigerenApp.ViewModels;
using HavekrigerenApp.Models;

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
    }
}