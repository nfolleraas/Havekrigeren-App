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

        private bool _showAllJobs = true;
        public bool ShowAllJobs
        {
            get => _showAllJobs;
            set
            {
                _showAllJobs = value;
                OnPropertyChanged();
            }
        }

        private bool _showIncomingJobs = false;
        public bool ShowIncomingJobs
        {
            get { return _showIncomingJobs; }
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
                ShowAllJobs = true;
                ShowIncomingJobs = false;
                SearchJob(_searchBoxInput);
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand RefreshCommand { get; }
        public ICommand ToggleAllJobsCommand { get; }
        public ICommand ToggleIncomingJobsCommand { get; }

        public HomeViewModel()
        {
            JobsVM = new ObservableCollection<JobViewModel>();
            JobsVMSortedByDate = new ObservableCollection<JobViewModel>();

            // Command registration
            RefreshCommand = new Command(RefreshPage);
            ToggleAllJobsCommand = new Command(ToggleAllJobs);
            ToggleIncomingJobsCommand = new Command(ToggleIncomingJobs);
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

            SortByDate();

            JobsVM.ToList().ForEach(n => Console.WriteLine("JobsVM " + n.ToString()));
            JobsVMSortedByDate.ToList().ForEach(n => Console.WriteLine("JobsVMSortedByDate " + n.ToString()));
        }

        private void SortByDate()
        {
            JobsVMSortedByDate.Clear();
            JobsVM
                .Where(jobVM => jobVM.StartDate != null)
                .OrderBy(jobVM => jobVM.StartDate)
                .ToList()
                .ForEach(jobVM => JobsVMSortedByDate.Add(jobVM)
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

            ShowSearchResults = string.IsNullOrEmpty(input) ? false : true;
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
        private void ToggleAllJobs()
        {
            ShowAllJobs = true;
            ShowIncomingJobs = false;
        }

        private void ToggleIncomingJobs()
        {
            ShowAllJobs = false;
            ShowIncomingJobs = true;
        }

    }
}
