using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp
{
    public class TaskManager
    {
        public string ContactName { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public string Category { get; set; }
        public string Date { get; set; }
        public string Notes { get; set; }

        public TaskManager(string contactName, string address, int phoneNumber, string category, string date, string notes)
        {
            ContactName = contactName;
            Address = address;
            PhoneNumber = phoneNumber;
            Category = category;
            Date = date;
            Notes = notes;
        }
    }
}
