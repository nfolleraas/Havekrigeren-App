using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp;

public class TaskManager
{
    public string ContactName { get; private set; }
    public string Address { get; private set; }
    public int PhoneNumber { get; private set; }
    public string Category { get; private set; }
    public string Date { get; private set; }
    public string Notes { get; private set; }

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
