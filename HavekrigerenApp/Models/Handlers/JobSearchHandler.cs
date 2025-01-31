using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.Models.Services;
using System.Collections.ObjectModel;

namespace HavekrigerenApp.Models.Handlers
{
    public class JobSearchHandler : SearchHandler
    {
        private readonly NavigationService navigationService = new NavigationService();

        public static readonly BindableProperty JobsProperty = BindableProperty.Create(
            nameof(Jobs),
            typeof(IList<Job>),
            typeof(JobSearchHandler),
            null);

        public IList<Job> Jobs
        {
            get => (IList<Job>)GetValue(JobsProperty);
            set => SetValue(JobsProperty, value);
        }

        // Event that will notify when the filtered list needs to be updated
        public event Action<IList<Job>> FilteredJobsUpdated;

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            Console.WriteLine($"Query changed. Old Value: '{oldValue}', New Value: '{newValue}'");

            if (string.IsNullOrWhiteSpace(newValue))
            {
                FilteredJobsUpdated?.Invoke(new List<Job>());
            }
            else
            {
                Console.WriteLine($"Total Jobs count before filtering: '{Jobs.Count}'");
                var results = Jobs.Where(job => job.ContactName.Contains(newValue, StringComparison.OrdinalIgnoreCase)).ToList();

                Console.WriteLine($"Filtered results count: '{results.Count}'");

                FilteredJobsUpdated?.Invoke(results);
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            if (item is Job selectedJob)
            {
                await navigationService.PushAsync(new ViewJobPage(selectedJob));
            }
        }
    }
}
