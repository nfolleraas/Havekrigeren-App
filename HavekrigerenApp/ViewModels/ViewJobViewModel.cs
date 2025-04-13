using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.Models.Services;
using System;
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
        private JobViewModel _jobVM;
        public JobViewModel JobVM
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

        public ViewJobViewModel(Job job)
        {
            JobVM = new JobViewModel(job);

            // Command registration
            PhoneNumberClickedCommand = new Command<string>(PhoneNumberClicked);
        }
        private async void PhoneNumberClicked(string phoneNumber)
        {
            if (PhoneDialer.Default.IsSupported)
            {
                try
                {
                    PhoneDialer.Default.Open(phoneNumber);
                }
                catch (ArgumentNullException ex)
                {
                    await _alertService.DisplayAlertAsync("Fejl!", $"Telefonnummeret \"{phoneNumber}\" er ikke gyldigt", "OK");
                }
            }
            else
            {
                await _alertService.DisplayAlertAsync("Fejl!", "Din telefon understøtter ikke denne funktion.", "OK");
            }
        }
    }
}
