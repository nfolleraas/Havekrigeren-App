using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp
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
	}
}