using HavekrigerenApp.Models;
using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp.Views;

public partial class ViewNotePage : ContentPage
{
	ViewNoteViewModel vm;
	public ViewNotePage(NoteViewModel selectedNote)
	{
		InitializeComponent();

		vm = new ViewNoteViewModel(selectedNote);
		BindingContext = vm;
	}
}