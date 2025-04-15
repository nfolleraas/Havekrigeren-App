using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.Models.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly CategoryRepository _categoryRepo;
        protected readonly JobRepository _jobRepo;
        protected readonly AlertService _alertService;
        protected readonly NavigationService _navigationService;

        public BaseViewModel()
        {
            _categoryRepo = new CategoryRepository();
            _jobRepo = new JobRepository();
            _alertService = new AlertService();
            _navigationService = new NavigationService();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
