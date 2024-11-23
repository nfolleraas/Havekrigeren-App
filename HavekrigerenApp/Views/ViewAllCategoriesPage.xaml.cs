using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp
{
    public partial class ViewAllCategoriesPage : ContentPage
    {
        ViewAllCategoriesViewModel vm = new ViewAllCategoriesViewModel();

        public ViewAllCategoriesPage()
        {
            InitializeComponent();

            BindingContext = vm;
        }
    }
}