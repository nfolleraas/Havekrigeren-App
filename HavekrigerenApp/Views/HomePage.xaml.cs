using HavekrigerenApp.Models.Handlers;
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
    }
}