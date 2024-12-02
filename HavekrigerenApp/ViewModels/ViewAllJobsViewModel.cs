﻿using CommunityToolkit.Mvvm.ComponentModel;
using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.Models.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HavekrigerenApp.ViewModels
{
    public class ViewAllJobsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private JobRepository jobRepo = new JobRepository();
        private AlertService alertService = new AlertService();
        private NavigationService navigationService = new NavigationService();

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
        public ICommand JobClickedCmd { get; set; }
        public ICommand RefreshCmd { get; set; }
        public ICommand DeleteJobCmd { get; set; }


        public ViewAllJobsViewModel(string categoryName)
        {
            JobsVM = new ObservableCollection<JobViewModel>();

            CategoryName = categoryName;

            RefreshPage();

            // Command registration
            JobClickedCmd = new Command<Job>(JobClicked);
            RefreshCmd = new Command(async () => await RefreshPage());
            DeleteJobCmd = new Command<Job>(DeleteJob);

        }

        private async Task LoadJobs()
        {
            await jobRepo.LoadAllAsync();

            JobsVM.Clear();
            // Instatiate new JobViewModel for each job if the job is in the category
            foreach (Job job in jobRepo.GetAll())
            {
                if (job.Category == CategoryName)
                { 
                    JobViewModel jobVM = new JobViewModel(job);
                    JobsVM.Add(jobVM);
                }
            }
        }

        private async void JobClicked(Job job)
        {
            try
            {
                //await navigationService.PushAsync(new ViewJobPage());
                await alertService.DisplayAlertAsync("Opgave", $"Trykkede på {job.ContactName}, {job.Address}");
            }
            catch (InvalidOperationException ex)
            {
                await alertService.DisplayAlertAsync("Fejl!", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await alertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
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

        private async void DeleteJob(Job job)
        {
            try
            {
                bool answer = await alertService.DisplayAlertAsync("Slet Opgave", $"Er du sikker på, du vil slette opgaven \"{job.ContactName}, {job.Address}\"?\nDenne handling kan ikke fortrydes.", "Ja", "Nej");

                if (answer)
                {
                    // Delete the job
                    await jobRepo.DeleteAsync(job);
                    await alertService.DisplayAlertAsync("Slet Opgave", $"Opgaven \"{job.ContactName}, {job.Address}\" blev slettet.");
                    await RefreshPage();
                }
            }
            catch (ArgumentException ex)
            {
                await alertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
