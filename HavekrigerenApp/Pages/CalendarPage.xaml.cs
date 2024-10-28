using System.Globalization;

namespace HavekrigerenApp.Pages
{

	public partial class CalendarPage : ContentPage
	{
		public CalendarPage()
		{
			InitializeComponent();
			calendar.Culture = App.Culture;
		}
	}
}