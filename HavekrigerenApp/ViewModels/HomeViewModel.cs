using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.Models.Handlers;
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
        private JobSearchHandler jobSearchHandler;

        private ObservableCollection<Job> _jobs;
        public ObservableCollection<Job> Jobs
        {
            get { return _jobs; }
            set 
            { 
                _jobs = value;
                OnPropertyChanged(nameof(Jobs));
            }
        }

        private ObservableCollection<Job> _filteredJobs = new ObservableCollection<Job>();

        public ObservableCollection<Job> FilteredJobs
        {
            get { return _filteredJobs; }
            set 
            { 
                _filteredJobs = value; 
                OnPropertyChanged(nameof(FilteredJobs));   
            }
        }


        public HomeViewModel()
        {
            _jobs = new ObservableCollection<Job>();
            jobSearchHandler = new JobSearchHandler();

            LoadJobs();

            

            jobSearchHandler.FilteredJobsUpdated += (_jobs) =>
            {
                Console.WriteLine($"FilteredJobsUpdated triggered with {_jobs.Count} items");
                FilteredJobs.Clear();
                foreach (var job in _jobs)
                {
                    FilteredJobs.Add(job);
                }
            };
        }

        private async void LoadJobs()
        {
            await jobRepo.LoadAllAsync();

            _jobs.Clear();
            // Instatiate new JobViewModel for each job
            foreach (Job job in jobRepo.GetAll())
            {
                //JobViewModel jobVM = new JobViewModel(job);
                _jobs.Add(job);
            }

            jobSearchHandler.Jobs = _jobs.ToList();
        }

        // Method for updating the UI on changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
