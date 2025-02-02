using HavekrigerenApp.Models.Classes;
using System.Text;

namespace HavekrigerenApp.ViewModels
{
    public class JobViewModel
    {
        // Properties to display
        public Job Job { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Category { get; set; }
        public bool HasDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Notes { get; set; }
        public string DateCreated { get; set; }

        private string _formattedPhoneNumber;

        public string FormattedPhoneNumber
        {
            get { return FormatPhoneNumber(_formattedPhoneNumber); }
        }


        public JobViewModel(Job job)
        {
            Job = job;
            ContactName = job.ContactName;
            Address = job.Address;
            PhoneNumber = job.PhoneNumber;
            Category = job.Category;
            HasDate = job.HasDate;
            StartDate = job.StartDate;
            EndDate = job.EndDate;
            Notes = job.Notes;
            DateCreated = job.DateCreated;

            _formattedPhoneNumber = job.PhoneNumber;
        }

        private string FormatPhoneNumber(string phoneNumber)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int i = 0;
            foreach (char c in phoneNumber)
            {
                stringBuilder.AppendFormat("{0}{1}", c, (i++ & 1) == 0 ? "" : ' ');
            }
            phoneNumber = "(+45) " + stringBuilder.ToString().Trim();

            return phoneNumber;
        }
    }
}
