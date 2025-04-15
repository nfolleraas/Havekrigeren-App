using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp
{

    public partial class CreateJobPage : ContentPage
	{
		CreateJobViewModel vm;
		public CreateJobPage()
		{
			InitializeComponent();
			
			vm = new CreateJobViewModel();
			BindingContext = vm;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            vm.LoadCategories();
        }
    }
}