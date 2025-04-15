using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.Models.Misc;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace HavekrigerenApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<JobViewModel> JobsVM { get; set; }
        public ObservableCollection<JobViewModel> JobsVMSortedByDate { get; set; }

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

        private bool _showSearchResults = false;
        public bool ShowSearchResults
        {
            get { return _showSearchResults; }
            set
            {
                _showSearchResults = value;
                OnPropertyChanged();
            }
        }

        private bool _showIncomingJobs = true;
        public bool ShowIncomingJobs
        {
            get => _showIncomingJobs;
            set
            {
                _showIncomingJobs = value;
                OnPropertyChanged();
            }
        }

        private string _searchBoxInput;
        public string SearchBoxInput
        {
            get { return _searchBoxInput; }
            set
            {
                _searchBoxInput = value;
                SearchJob(_searchBoxInput);
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand RefreshCommand { get; }

        public HomeViewModel()
        {
            JobsVM = new ObservableCollection<JobViewModel>();

            // Command registration
            RefreshCommand = new Command(RefreshPage);
        }

        public void LoadJobs()
        {
            JobsVM.Clear();
            // Instatiate new JobViewModel for each job
            foreach (Job job in _jobRepo.GetAll())
            {
                JobViewModel jobVM = new JobViewModel(job);
                JobsVM.Add(jobVM);
            }

            // Sort by date
            JobsVMSortedByDate = new ObservableCollection<JobViewModel>(
                JobsVM.OrderBy(job => job.StartDate)
                );
        }

        public void SearchJob(string input)
        {
            ObservableCollection<Job> foundJobs = new ObservableCollection<Job>(_jobRepo.PerformSearch(input));

            JobsVM.Clear();
            foreach (Job job in foundJobs)
            {
                JobViewModel jobVM = new JobViewModel(job);
                JobsVM.Add(jobVM);
            }

            if (string.IsNullOrEmpty(input))
            {
                ShowIncomingJobs = true;
            }
            else
            {
                ShowIncomingJobs = false;
            }
        }

        // Commands
        private void RefreshPage()
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
    }
}
