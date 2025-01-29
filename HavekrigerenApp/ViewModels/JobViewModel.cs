using HavekrigerenApp.Models.Classes;

namespace HavekrigerenApp.ViewModels
{
    public class JobViewModel
    {
        // Properties to display
        public Job Job { get; set; }
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Notes { get; set; }

        public JobViewModel(Job job)
        {
            Job = job;
            ContactName = job.ContactName;
            PhoneNumber = job.PhoneNumber;
            Address = job.Address;
            Category = job.Category;
            StartDate = job.StartDate;
            EndDate = job.EndDate;
            Notes = job.Notes;
        }
    }
}
