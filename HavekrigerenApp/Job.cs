using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp
{
    public class Job
    {
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Category { get; set; }
        public DateOnly Date { get; set; }
        public string Notes { get; set; }

        public Job(string contactName, string address, string phoneNumber, string category, DateOnly date, string notes)
        {
            ContactName = contactName;
            Address = address;
            PhoneNumber = phoneNumber;
            Category = category;
            Date = date;
            Notes = notes;
        }

        public Job() 
        { 
        }
    }
}
