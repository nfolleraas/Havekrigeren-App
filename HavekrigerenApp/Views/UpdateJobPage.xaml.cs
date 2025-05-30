using HavekrigerenApp.Models;
using HavekrigerenApp.ViewModels;

namespace HavekrigerenApp.Views;

public partial class UpdateJobPage : ContentPage
{
	UpdateJobViewModel vm;
	public UpdateJobPage(JobViewModel jobVM)
	{
		InitializeComponent();
		vm = new UpdateJobViewModel(jobVM);
		BindingContext = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        vm.LoadCategories();
    }
}