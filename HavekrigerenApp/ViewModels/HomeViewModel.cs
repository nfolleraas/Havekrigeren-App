using HavekrigerenApp.Models;
using HavekrigerenApp.Persistance;
using HavekrigerenApp.Services;
using HavekrigerenApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

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

        private bool _showSearchResults;
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

        private bool _showAllJobsLabel = true;
        public bool ShowAllJobsLabel
        {
            get { return _showAllJobsLabel; }
            set
            {
                _showAllJobsLabel = value;
                OnPropertyChanged();
            }
        }

        private bool _showIncomingJobs;
        public bool ShowIncomingJobs
        {
            get { return _showIncomingJobs; }
            set
            {
                _showIncomingJobs = value;
                OnPropertyChanged();
            }
        }

        private string? _searchBoxInput;
        public string? SearchBoxInput
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


        // Commands
        public ICommand RefreshCommand { get; }
        public ICommand ToggleAllJobsCommand { get; }
        public ICommand ToggleIncomingJobsCommand { get; }
        public ICommand JobClickedCommand { get; }

        public HomeViewModel()
        {
            JobsVM = new ObservableCollection<JobViewModel>();
            JobsVMSortedByDate = new ObservableCollection<JobViewModel>();

            // Command registration
            RefreshCommand = new Command(RefreshPage);
            ToggleAllJobsCommand = new Command(ToggleAllJobs);
            ToggleIncomingJobsCommand = new Command(ToggleIncomingJobs);
            JobClickedCommand = new Command<Job>(JobClicked);

        }

        public void LoadJobs()
        {
            JobsVM.Clear();

            // Instatiate new JobViewModel for each job
            foreach (Job job in JobRepository.GetAll())
            {
                JobViewModel jobVM = new JobViewModel(job);
                JobsVM.Add(jobVM);
            }

            ShowNoJobsMessage = JobsVM.Count <= 0;

            JobsVMSortedByDate.Clear();
            //JobsVMSortedByDate = SortByDate(JobsVM);
            foreach (JobViewModel jobVM in SortByDate(JobsVM))
            {
                JobsVMSortedByDate.Add(jobVM);
            }
        }

        public void SearchJob(string input)
        {
            ObservableCollection<Job> foundJobs = new ObservableCollection<Job>(JobRepository.PerformSearch(input));

            JobsVM.Clear();
            foreach (Job job in foundJobs)
            {
                JobViewModel jobVM = new JobViewModel(job);
                JobsVM.Add(jobVM);
            }

            ShowSearchResults = string.IsNullOrEmpty(input) ? false : true;
            ShowAllJobsLabel = string.IsNullOrEmpty(input) ? true : false;
        }

        private List<JobViewModel> SortByDate(ObservableCollection<JobViewModel> jobsVM)
        {
            return jobsVM
                .Where(jobVM => jobVM.StartDate != null)
                .OrderBy(jobVM => jobVM.StartDate)
                .ToList();
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
            ShowAllJobsLabel = true;
            ShowIncomingJobs = false;
        }

        private void ToggleIncomingJobs()
        {
            ShowAllJobs = false;
            ShowAllJobsLabel = false;
            ShowIncomingJobs = true;
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

    }
}
