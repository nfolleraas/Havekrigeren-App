using HavekrigerenApp.Services;
using HavekrigerenApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HavekrigerenApp.ViewModels
{
    public class CreateChoiceViewModel
    {
        public ICommand NavigateToCreateJobCommand { get; }
        public ICommand NavigateToCreateNoteCommand { get; }

        public CreateChoiceViewModel()
        {
            NavigateToCreateJobCommand = new Command(NavigateToCreateJob);
            NavigateToCreateNoteCommand = new Command(NavigateToCreateNote);
        }

        private async void NavigateToCreateJob()
        {
            await NavigationService.PushAsync(new CreateJobPage());
        }

        private async void NavigateToCreateNote()
        {
            await NavigationService.PushAsync(new CreateNotePage());
        }
    }
}
