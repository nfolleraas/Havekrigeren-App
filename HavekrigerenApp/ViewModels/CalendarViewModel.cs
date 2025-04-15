using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.ViewModels;
using Plugin.Maui.Calendar.Models;

namespace HavekrigerenApp.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        public ObservableCollection<JobViewModel> JobsVM { get; set; }
        public CultureInfo Culture { get; set; } = new CultureInfo("da-DK");
        public EventCollection Events { get; set; }

        public CalendarViewModel()
        {
            JobsVM = new ObservableCollection<JobViewModel>();
            Events = new EventCollection();
        }

        public void LoadJobs()
        {
            JobsVM.Clear();
            Events.Clear();

            foreach (Job job in _jobRepo.GetAll())
            {
                if (!string.IsNullOrEmpty(job.StartDate.ToString()))
                {
                    JobViewModel jobVM = new JobViewModel(job);
                    JobsVM.Add(jobVM);
                }
                AddJobsToEvents();
            }
        }

        private void AddJobsToEvents()
        {
            foreach (JobViewModel jobVM in JobsVM)
            {
                string? formattedDate = jobVM.StartDate.ToString();
                if (DateTime.TryParseExact(formattedDate, "dd/MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
                {
                    Events.Add(startDate, new List<JobViewModel>() { jobVM });
                }
                else
                {
                    Console.WriteLine($"The date {jobVM.StartDate} failed to parse as a DateTime");
                }

            }
        }
    }
}
