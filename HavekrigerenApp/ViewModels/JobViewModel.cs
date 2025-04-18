using Google.Protobuf.Reflection;
using HavekrigerenApp.Models;
using System.Text;

namespace HavekrigerenApp.ViewModels
{
    public class JobViewModel : BaseViewModel
    {
        private Job _job;
        public Job Job
        {
            get { return _job; }
            private set
            {
                _job = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get => Job.Id;
            set
            {
                if (Job.Id != value)
                {
                    Job.Id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ContactName
        {
            get => Job.ContactName;
            set
            {
                if (Job.ContactName != value)
                {
                    Job.ContactName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Address
        {
            get => Job.Address;
            set
            {
                if (Job.Address != value)
                {
                    Job.Address = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PhoneNumber
        {
            get => Job.PhoneNumber;
            set
            {
                if (Job.PhoneNumber != value)
                {
                    Job.PhoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FormattedPhoneNumber
        {
            get => _formattedPhoneNumber;
            set
            {
                if (_formattedPhoneNumber != value)
                {
                    _formattedPhoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _formattedPhoneNumber; // Still local to the view model

        public Category Category
        {
            get => Job.Category;
            set
            {
                if (Job.Category != value)
                {
                    Job.Category = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool HasDate
        {
            get => Job.HasDate;
            set
            {
                if (Job.HasDate != value)
                {
                    Job.HasDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? StartDate
        {
            get => Job.StartDate;
            set
            {
                if (Job.StartDate != value)
                {
                    Job.StartDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? EndDate
        {
            get => Job.EndDate;
            set
            {
                if (Job.EndDate != value)
                {
                    Job.EndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Notes
        {
            get => Job.Notes;
            set
            {
                if (Job.Notes != value)
                {
                    Job.Notes = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DateCreated
        {
            get => Job.DateCreated;
            set
            {
                if (Job.DateCreated != value)
                {
                    Job.DateCreated = value;
                    OnPropertyChanged();
                }
            }
        }


        public JobViewModel(Job job)
        {
            Job = job;
            Id = job.Id;
            ContactName = job.ContactName;
            Address = job.Address;
            PhoneNumber = FormatPhoneNumber(job.PhoneNumber);
            Category = job.Category;
            HasDate = job.HasDate;
            StartDate = job.StartDate;
            EndDate = job.EndDate;
            Notes = job.Notes;
            DateCreated = job.DateCreated;
        }

        private string FormatPhoneNumber(string phoneNumber)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int i = 0;

            // Loop through each character in the phone number
            foreach (char c in phoneNumber)
            {
                // Add the character to the StringBuilder
                stringBuilder.Append(c);

                // Add a space after every two digits (excluding the last pair)
                if (++i % 2 == 0 && i < phoneNumber.Length)
                {
                    stringBuilder.Append(' ');
                }
            }

            // Prepend the country code (+45) and return the formatted phone number
            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            return Job.ToString();
        }

    }
}
