using CommunityToolkit.Mvvm.ComponentModel;
using HavekrigerenApp.Models;
using HavekrigerenApp.Persistance;
using HavekrigerenApp.Services;
using HavekrigerenApp.Views;
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

        private bool _showNoJobsMessage;
        public bool ShowNoJobsMessage
        {
            get { return _showNoJobsMessage; }
            set
            {
                _showNoJobsMessage = value;
                OnPropertyChanged();
            }
        }
        public Category SelectedCategory { get; set; }

        // Commands
        public ICommand JobClickedCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteSelectedCategoryCommand { get; }

        public ViewAllJobsViewModel(Category selectedCategory)
        {
            JobsVM = new ObservableCollection<JobViewModel>();

            SelectedCategory = selectedCategory;

            // Command registration
            JobClickedCommand = new Command<Job>(JobClicked);
            RefreshCommand = new Command(RefreshPage);
            DeleteSelectedCategoryCommand = new Command<Category>(DeleteSelectedCategory);
        }

        public void LoadJobs()
        {
            JobsVM.Clear();
            // Instatiate new JobViewModel for each job if the job is in the category
            foreach (Job job in JobRepository.GetAll())
            {
                if (job.Category.Name == SelectedCategory.Name)
                {
                    JobViewModel jobVM = new JobViewModel(job);
                    JobsVM.Add(jobVM);
                }
            }
            ShowNoJobsMessage = JobsVM.Count <= 0 ? true : false;
        }

        public void RefreshPage()
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
                await NavigationService.PushAsync(new ViewJobPage(job));
            }
            catch (InvalidOperationException ex)
            {
                await AlertService.DisplayAlertAsync("Fejl!", ex.Message);
            }
            catch (Exception ex)
            {
                await AlertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }

        private async void DeleteSelectedCategory(Category category)
        {
            try
            {
                bool answer = await AlertService.DisplayAlertAsync("Slet Kategori", $"Er du sikker på, du vil slette kategorien \"{category.Name}\"?\nDette vil også slette alle opgaver i kategorien.\nDenne handling kan ikke fortrydes.", "Ja", "Nej");

                if (answer)
                {
                    // Delete every job in the category
                    foreach (JobViewModel job in JobsVM)
                    {
                        if (category.Name == job.Category.Name)
                        {
                            JobRepository.Delete(job.Job.Id);
                        }
                    }
                    // Then delete the category
                    CategoryRepository.Delete(category.Id);
                    await AlertService.DisplayAlertAsync("Slet Kategori", $"Kategorien \"{category.Name}\" blev slettet.");
                    await NavigationService.PopAsync();
                }
            }
            catch (ArgumentException ex)
            {
                await AlertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }
    }
}
