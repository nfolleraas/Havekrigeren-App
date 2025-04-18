using HavekrigerenApp.Models;
using HavekrigerenApp.Persistance;
using HavekrigerenApp.Services;
using HavekrigerenApp.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HavekrigerenApp.ViewModels
{
    public class ViewJobViewModel : BaseViewModel
    {
        private JobViewModel? _jobVM;
        public JobViewModel? JobVM
        {
            get => _jobVM;
            set
            {
                _jobVM = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand PhoneNumberClickedCommand { get; }
        public ICommand DeleteJobCommand { get; }
        public ICommand NavigateToUpdateJobCommand { get; }

        public ViewJobViewModel(Job job)
        {
            JobVM = new JobViewModel(JobRepository.Get(job.Id));

            // Command registration
            PhoneNumberClickedCommand = new Command<string>(PhoneNumberClicked);
            DeleteJobCommand = new Command<JobViewModel>(DeleteJob);
            NavigateToUpdateJobCommand = new Command<JobViewModel>(NavigateToUpdateJob);
        }

        private async void PhoneNumberClicked(string phoneNumber)
        {
            if (PhoneDialer.Default.IsSupported)
            {
                try
                {
                    PhoneDialer.Default.Open(phoneNumber);
                }
                catch (ArgumentNullException)
                {
                    await AlertService.DisplayAlertAsync("Fejl!", $"Telefonnummeret \"{phoneNumber}\" er ikke gyldigt", "OK");
                }
            }
            else
            {
                await AlertService.DisplayAlertAsync("Fejl!", "Din telefon understøtter ikke denne funktion.", "OK");
            }
        }

        private async void DeleteJob(JobViewModel jobVM)
        {
            try
            {
                bool answer = await AlertService.DisplayAlertAsync("Slet Opgave", $"Er du sikker på, du vil slette opgaven \"{jobVM?.ContactName}, {jobVM?.Address}\"?\nDenne handling kan ikke fortrydes.", "Ja", "Nej");

                if (answer)
                {
                    // Delete the job
                    JobRepository.Delete(jobVM.Id);
                    await AlertService.DisplayAlertAsync("Slet Opgave", $"Opgaven \"{jobVM?.ContactName}, {jobVM?.Address}\" blev slettet.");
                    await NavigationService.PopAsync();
                }
            }
            catch (ArgumentException ex)
            {
                await AlertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }

        private async void NavigateToUpdateJob(JobViewModel jobVM)
        {
            try
            {
                await NavigationService.PushAsync(new UpdateJobPage(jobVM));
            }
            catch (InvalidOperationException ex)
            {
                await AlertService.DisplayAlertAsync("Fejl!", ex.Message);
            }
        }
    }
}
