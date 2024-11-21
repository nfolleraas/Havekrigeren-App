using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp
{
    public partial class ViewAllCategoriesPage : ContentPage
    {
        ViewAllCategoriesViewModel viewModel = new ViewAllCategoriesViewModel();

        public ViewAllCategoriesPage()
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}