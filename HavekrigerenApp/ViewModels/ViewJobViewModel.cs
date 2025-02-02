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
    public class ViewJobViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private AlertService alertService = new AlertService();

        private JobViewModel _jobVM;
        public JobViewModel JobVM
        {
            get => _jobVM;
            set
            {
                _jobVM = value;
                OnPropertyChanged(nameof(JobVM));
            }
        }

        // Commands
        public ICommand PhoneNumberClickedCommand { get; set; }

        public ViewJobViewModel(Job job)
        {
            _jobVM = new JobViewModel(job);

            JobVM.PhoneNumber = FormatPhoneNumber(job.PhoneNumber);

            // Command registration
            PhoneNumberClickedCommand = new Command<string>(PhoneNumberClicked);
        }

        private string FormatPhoneNumber(string phoneNumber)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int i = 0;
            foreach (char c in phoneNumber)
            {
                stringBuilder.AppendFormat("{0}{1}", c, (i++ & 1) == 0 ? "" : ' ');
            }
            phoneNumber = "(+45) " + stringBuilder.ToString().Trim();

            return phoneNumber;
        }

        // Commands
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
                    await alertService.DisplayAlertAsync("Fejl!", $"Telefonnummeret \"{phoneNumber}\" er ikke gyldigt", "OK");
                }
            }
            else
            {
                await alertService.DisplayAlertAsync("Fejl!", "Din telefon understøtter ikke denne funktion.", "OK");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
