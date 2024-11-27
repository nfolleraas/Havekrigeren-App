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
    public class ViewAllJobsViewModel : INotifyPropertyChanged
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

        public string CategoryName { get; set; }

            
        // Commands


        public ViewAllJobsViewModel(string categoryName)
        {
            JobsVM = new ObservableCollection<JobViewModel>();

            CategoryName = categoryName;

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
