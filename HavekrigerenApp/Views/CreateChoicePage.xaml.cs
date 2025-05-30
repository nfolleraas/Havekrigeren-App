using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp.Views;

public partial class CreateChoicePage : ContentPage
{
	CreateChoiceViewModel vm;
	public CreateChoicePage()
	{
		InitializeComponent();
		vm = new CreateChoiceViewModel();
		BindingContext = vm;
	}
}