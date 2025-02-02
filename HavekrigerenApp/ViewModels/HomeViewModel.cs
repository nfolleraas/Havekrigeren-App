using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.Models.Misc;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace HavekrigerenApp.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private JobRepository jobRepo = new JobRepository();

        private ObservableCollection<JobViewModel> _jobsVM;
        public ObservableCollection<JobViewModel> JobsVM
        {
            get => _jobsVM;
            set 
            { 
                _jobsVM = value;
                OnPropertyChanged(nameof(JobsVM));
            }
        }

        private ObservableCollection<JobViewModel> _jobsVMSortedByDate;

        public ObservableCollection<JobViewModel> JobsVMSortedByDate
        {
            get { return _jobsVMSortedByDate; }
            set 
            { 
                _jobsVMSortedByDate = value; 
                OnPropertyChanged(nameof(JobsVMSortedByDate));
            }
        }

        private List<Job> jobsSortedByDate;


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

        private InvertableBool _showIncomingJobs = true;

        public InvertableBool ShowIncomingJobs
        {
            get => _showIncomingJobs;
            set 
            {
                _showIncomingJobs = value;
                OnPropertyChanged(nameof(ShowIncomingJobs));
            }
        }


        // Commands
        public ICommand JobClickedCommand { get; set; }
        public ICommand RefreshCommand { get; set; }


        public HomeViewModel()
        {
            _jobsVM = new ObservableCollection<JobViewModel>();
            _jobsVMSortedByDate = new ObservableCollection<JobViewModel>();

            // Command registration
            //JobClickedCommand = new Command<Job>(JobClicked);
            RefreshCommand = new Command(async () => await RefreshPage());
        }

        public async Task LoadJobs()
        {
            await jobRepo.LoadAllAsync();

            _jobsVM.Clear();
            // Instatiate new JobViewModel for each job
            foreach (Job job in jobRepo.GetAll())
            {
                JobViewModel jobVM = new JobViewModel(job);
                _jobsVM.Add(jobVM);
            }

            // Sort by date
            jobsSortedByDate = jobRepo.SortJobsBy(job => job.StartDate);

            _jobsVMSortedByDate.Clear();
            foreach (Job job in jobsSortedByDate)
            {
                if (!string.IsNullOrEmpty(job.StartDate))
                {
                    JobViewModel jobVM = new JobViewModel(job);
                    _jobsVMSortedByDate.Add(jobVM);
                }
            }
        }

        public void SearchJob(object input)
        {
            string searchText = ((SearchBar)input).Text;
            Console.WriteLine(searchText);

            ObservableCollection<Job> foundJobs = new ObservableCollection<Job>(jobRepo.PerformSearch(searchText));

            _jobsVM.Clear();
            foreach (Job job in foundJobs)
            {
                JobViewModel jobVM = new JobViewModel(job);
                _jobsVM.Add(jobVM);
            }

            if (string.IsNullOrEmpty(searchText))
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

        // Method for updating the UI on changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
