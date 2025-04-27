using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp.Views
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

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            vm.LoadJobs();
        }
    }
}