using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp
{

	public partial class HomePage : ContentPage
	{
		HomeViewModel vm;
		public HomePage()
		{
			InitializeComponent();

			vm = new HomeViewModel();
			BindingContext = vm;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
			vm.SearchJob(sender);
        }
    }
}