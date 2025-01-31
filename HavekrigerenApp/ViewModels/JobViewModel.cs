using HavekrigerenApp.Models.Classes;

namespace HavekrigerenApp.ViewModels
{
    public class JobViewModel
    {
        // Properties to display
        private Job job;
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Category { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Notes { get; set; }

        public JobViewModel(Job job)
        {
            this.job = job;
            ContactName = job.ContactName;
            Address = job.Address;
            PhoneNumber = job.PhoneNumber;
            Category = job.Category;
            StartDate = job.StartDate;
            EndDate = job.EndDate;
            Notes = job.Notes;
        }
    }
}
