using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp.Views;

public partial class CreateNotePage : ContentPage
{
	CreateNoteViewModel vm;
	public CreateNotePage()
	{
		InitializeComponent();

		vm = new CreateNoteViewModel();
		BindingContext = vm;
	}
}