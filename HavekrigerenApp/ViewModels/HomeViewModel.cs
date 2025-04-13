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
        private List<Job> _jobsSortedByDate;
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
            JobsVMSortedByDate = new ObservableCollection<JobViewModel>();

            // Command registration
            RefreshCommand = new Command(async () => await RefreshPage());
        }

        public async Task LoadJobs()
        {
            await _jobRepo.LoadAllAsync();

            JobsVM.Clear();
            // Instatiate new JobViewModel for each job
            foreach (Job job in _jobRepo.GetAll())
            {
                JobViewModel jobVM = new JobViewModel(job);
                JobsVM.Add(jobVM);
            }

            // Sort by date
            _jobsSortedByDate = _jobRepo.SortJobsBy(job => job.StartDate);

            JobsVMSortedByDate.Clear();
            foreach (Job job in _jobsSortedByDate)
            {
                if (!string.IsNullOrEmpty(job.StartDate.ToString()))
                {
                    JobViewModel jobVM = new JobViewModel(job);
                    JobsVMSortedByDate.Add(jobVM);
                }
            }
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
    }
}
