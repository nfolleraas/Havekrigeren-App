using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.Classes
{
    public class Job
    {
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Category { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Notes { get; set; }

        public Job(string contactName, string address, string phoneNumber, string category, DateOnly startDate, DateOnly endDate, string notes)
        {
            ContactName = contactName;
            Address = address;
            PhoneNumber = phoneNumber;
            Category = category;
            StartDate = startDate;
            EndDate = endDate;
            Notes = notes;
        }

        public Job()
        {
        }
    }
}
