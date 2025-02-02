using HavekrigerenApp.Models.Classes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

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

        // Commands
        public ICommand JobClickedCommand { get; set; }
        public ICommand RefreshCommand { get; set; }


        public HomeViewModel()
        {
            _jobsVM = new ObservableCollection<JobViewModel>();

            RefreshPage();

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
        }

        public void SearchJob(object input)
        {
            ObservableCollection<Job> foundJobs = new ObservableCollection<Job>(jobRepo.PerformSearch(((SearchBar)input).Text));

            _jobsVM.Clear();
            foreach (Job job in foundJobs)
            {
                JobViewModel jobVM = new JobViewModel(job);
                _jobsVM.Add(jobVM);
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
