using CommunityToolkit.Mvvm.ComponentModel;
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
    public class ViewAllJobsViewModel : BaseViewModel
    {
        public ObservableCollection<JobViewModel> JobsVM { get; set; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set 
            { 
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCategoryName { get; set; }

        // Commands
        public ICommand JobClickedCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteJobCommand { get; }


        public ViewAllJobsViewModel(string selectedCategoryName)
        {
            JobsVM = new ObservableCollection<JobViewModel>();

            SelectedCategoryName = selectedCategoryName;

            // Command registration
            JobClickedCommand = new Command<Job>(JobClicked);
            RefreshCommand = new Command(async () => await RefreshPage());
            DeleteJobCommand = new Command<Job>(DeleteJob);
        }

        public void LoadJobs()
        {
            JobsVM.Clear();
            // Instatiate new JobViewModel for each job if the job is in the category
            foreach (Job job in _jobRepo.GetAll())
            {
                if (job.Category.ToString() == SelectedCategoryName)
                { 
                    JobViewModel jobVM = new JobViewModel(job);
                    JobsVM.Add(jobVM);
                }
            }
        }

        public async Task RefreshPage()
        {
            try
            {
                IsRefreshing = true;
                LoadJobs();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async void JobClicked(Job job)
        {
            try
            {
                await _navigationService.PushAsync(new ViewJobPage(job));
            }
            catch (InvalidOperationException ex)
            {
                await _alertService.DisplayAlertAsync("Fejl!", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _alertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }

        private async void DeleteJob(Job job)
        {
            try
            {
                bool answer = await _alertService.DisplayAlertAsync("Slet Opgave", $"Er du sikker på, du vil slette opgaven \"{job.ContactName}, {job.Address}\"?\nDenne handling kan ikke fortrydes.", "Ja", "Nej");

                if (answer)
                {
                    // Delete the job
                    _jobRepo.Delete(job.Id);
                    await _alertService.DisplayAlertAsync("Slet Opgave", $"Opgaven \"{job.ContactName}, {job.Address}\" blev slettet.");
                    await RefreshPage();
                }
            }
            catch (ArgumentException ex)
            {
                await _alertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }
    }
}
