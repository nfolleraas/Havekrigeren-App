using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using HavekrigerenApp.Models;
using HavekrigerenApp.Persistance;
using HavekrigerenApp.Services;
using Plugin.Maui.Calendar.Models;

namespace HavekrigerenApp.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        public ObservableCollection<JobViewModel> JobsVM { get; set; }
        public CultureInfo Culture { get; set; } = new CultureInfo("da-DK");
        public EventCollection Events { get; set; }

        public ICommand JobClickedCommand { get; }

        public CalendarViewModel()
        {
            JobsVM = new ObservableCollection<JobViewModel>();
            Events = new EventCollection();

            JobClickedCommand = new Command<Job>(JobClicked);
        }

        public void LoadJobs()
        {
            JobsVM.Clear();
            Events.Clear();

            foreach (Job job in JobRepository.GetAll())
            {
                if (job.HasDate)
                {
                    JobViewModel jobVM = new JobViewModel(job);
                    JobsVM.Add(jobVM);
                    AddJobsToEvents(jobVM);
                }
            }
        }

        private void AddJobsToEvents(JobViewModel jobVM)
        {
            DateTime startDate = (DateTime)jobVM.StartDate;
            DateTime endDate = (DateTime)jobVM.EndDate;

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {

                if (!Events.ContainsKey(date))
                {
                    Events.Add(date, new ArrayList());
                }

                ((ArrayList)Events[date]).Add(jobVM);
            }
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
