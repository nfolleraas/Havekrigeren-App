using Android.Webkit;
using CommunityToolkit.Mvvm.ComponentModel;
using HavekrigerenApp.Models;
using HavekrigerenApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.ViewModels
{
    [QueryProperty(nameof(CategoryName), "categoryName")]
    public class ViewAllJobsViewModel : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private JobRepository jobRepo = new JobRepository();
        private AlertService alertService = new AlertService();

        private ObservableCollection<JobViewModel>? _jobsVM;
        public ObservableCollection<JobViewModel>? JobsVM
        {
            get => _jobsVM;
            set 
            { 
                _jobsVM = value;
                OnPropertyChanged(nameof(JobsVM));
            }
        }
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set 
            { 
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private string _categoryName;

        public string CategoryName
        {
            get => _categoryName;
            set => SetProperty(ref _categoryName, value);
        }

        private string _path;

        public string Path
        {
            get => _path;   
            set => SetProperty(ref _path, value);
        }

        // Commands


        public ViewAllJobsViewModel()
        {
            Path = $"Alle kategorier / {CategoryName} /";

            JobsVM = new ObservableCollection<JobViewModel>();
            RefreshPage();

            // Command registration

        }

        private async Task LoadJobs()
        {
            await jobRepo.LoadAllAsync();

            JobsVM.Clear();
            foreach (Job job in jobRepo.GetAll())
            {
                JobViewModel jobVM = new JobViewModel(job);
                JobsVM.Add(jobVM);
            }
        }

        private async Task RefreshPage()
        {
            try
            {
                IsRefreshing = true;
                await LoadJobs();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
