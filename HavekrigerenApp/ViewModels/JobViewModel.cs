using Google.Protobuf.Reflection;
using HavekrigerenApp.Models.Classes;
using System.Text;

namespace HavekrigerenApp.ViewModels
{
    public class JobViewModel : BaseViewModel
    {
        private Job _job;
        public Job Job
        {
            get { return _job; }
            set
            {
                _job = value;
                OnPropertyChanged();
            }
        }

        private string _contactName;
        public string ContactName
        {
            get { return _contactName; }
            set
            {
                _contactName = value;
                OnPropertyChanged();
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return FormatPhoneNumber(_phoneNumber); }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private Category _category;
        public Category Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dateCreated;
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set
            {
                _dateCreated = value;
                OnPropertyChanged();
            }
        }

        public JobViewModel(Job job)
        {
            Job = job;
            ContactName = job.ContactName;
            Address = job.Address;
            PhoneNumber = job.PhoneNumber;
            Category = job.Category;
            StartDate = job.StartDate;
            EndDate = job.EndDate;
            Notes = job.Notes;
            DateCreated = job.DateCreated;
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
