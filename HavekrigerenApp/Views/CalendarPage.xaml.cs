using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp.Views
{

	public partial class CalendarPage : ContentPage
	{
		CalendarViewModel vm;
		public CalendarPage()
		{
			InitializeComponent();

			vm = new CalendarViewModel();
			BindingContext = vm;
		}

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            vm.LoadJobs();
        }
    }
}