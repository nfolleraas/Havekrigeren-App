using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp
{
    public partial class ViewAllCategoriesPage : ContentPage
    {
        ViewAllCategoriesViewModel vm;

        public ViewAllCategoriesPage()
        {
            InitializeComponent();

            vm = new ViewAllCategoriesViewModel();
            BindingContext = vm;
        }
    }
}