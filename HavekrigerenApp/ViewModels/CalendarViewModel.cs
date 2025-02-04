using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.ViewModels;
using Plugin.Maui.Calendar.Models;

namespace HavekrigerenApp.ViewModels
{
    public class CalendarViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public CultureInfo Culture { get; set; } = new CultureInfo("da-DK");
        public EventCollection Events { get; set; }

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

        /*private List<Job> _jobs;
        public List<Job> Jobs
        {
            get => _jobs;
            set
            {
                _jobs = value;
                OnPropertyChanged(nameof(Jobs));
            }
        }*/


        public CalendarViewModel()
        {
            _jobsVM = new ObservableCollection<JobViewModel>();
            Events = new EventCollection();

        }

        public async Task LoadJobs()
        {
            await jobRepo.LoadAllAsync();

            _jobsVM.Clear();
            Events.Clear();

            foreach (Job job in jobRepo.GetAll())
            {
                if (!string.IsNullOrEmpty(job.StartDate))
                {
                    JobViewModel jobVM = new JobViewModel(job);
                    _jobsVM.Add(jobVM);
                }
            }
            
            foreach (JobViewModel jobVM in _jobsVM)
            {
                if (DateTime.TryParseExact(jobVM.StartDate, "dd/MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
                {
                    Events.Add(startDate, new List<JobViewModel>() { jobVM });
                }
                else
                {
                    Console.WriteLine($"The date {jobVM.StartDate} failed to parse as a DateTime");
                }

            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
