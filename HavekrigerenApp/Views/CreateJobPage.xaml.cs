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
    }
}