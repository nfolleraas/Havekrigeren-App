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

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            await vm.LoadCategories();
        }
    }
}