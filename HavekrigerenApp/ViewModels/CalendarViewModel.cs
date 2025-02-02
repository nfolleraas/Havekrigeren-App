using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.ViewModels;
using Plugin.Maui.Calendar.Models;
using HavekrigerenApp.Models.Misc;

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

        public CalendarViewModel()
        {
            _jobsVM = new ObservableCollection<JobViewModel>();

            Events = new EventCollection
            {
                [DateTime.Now] = new List<Job>
                {
                    new() { ContactName = "Cool event1", Address = "vfdba", PhoneNumber = "35316" },
                    new() { ContactName = "Cool event1", Address = "vfdba", PhoneNumber = "35316" }
                },

                // 5 days from today
                [DateTime.Now.AddDays(5)] = new List<Job>
                {
                    new() { ContactName = "Cool event1", Address = "vfdba", PhoneNumber = "35316" },
                    new() {ContactName = "Cool event1", Address = "vfdba", PhoneNumber = "35316"}
                },

                // 3 days ago
                [DateTime.Now.AddDays(-3)] = new List<Job>
                {
                    new() {ContactName = "Cool event1", Address = "vfdba", PhoneNumber = "35316"}
                },

                // custom date
                [new DateTime(2024, 3, 16)] = new List<Job>
                {
                    new() { ContactName = "Cool event1", Address = "vfdba", PhoneNumber = "35316" }
                }
            };
        }

        public async Task LoadJobs()
        {
            await jobRepo.LoadAllAsync();

            _jobsVM.Clear();

            foreach (Job job in jobRepo.GetAll())
            {
                if (!string.IsNullOrEmpty(job.StartDate))
                {
                    JobViewModel jobVM = new JobViewModel(job);
                    _jobsVM.Add(jobVM);
                    
                    /*Events.Add
                    (
                        DateTime.ParseExact(job.StartDate, "dd/MM-yyyy", CultureInfo.InvariantCulture),
                        new List<JobViewModel>
                        {
                            new(job)
                        }
                    );
                    */
                }
            }

            foreach (var item in Events)
            {
                Console.WriteLine(item);
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
