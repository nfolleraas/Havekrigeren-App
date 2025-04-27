using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp.Views;

public partial class ViewAllNotesPage : ContentPage
{
	ViewAllNotesViewModel vm;
	public ViewAllNotesPage()
	{
		InitializeComponent();

		vm = new ViewAllNotesViewModel();
		BindingContext = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        vm.LoadNotes();
    }
}