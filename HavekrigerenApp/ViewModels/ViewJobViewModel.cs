using HavekrigerenApp.Models.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.ViewModels
{
    public class ViewJobViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

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

        public ViewJobViewModel(Job job)
        {
            JobVM = new JobViewModel(job);

            JobVM.PhoneNumber = FormatPhoneNumber(job.PhoneNumber);
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


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
