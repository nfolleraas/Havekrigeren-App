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
    public class Task
    {
        public string ContactName { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public string Category { get; set; }
        public DateOnly Date { get; set; }
        public string Notes { get; set; }

        public Task(string contactName, string address, int phoneNumber, string category, DateOnly date, string notes)
        {
            ContactName = contactName;
            Address = address;
            PhoneNumber = phoneNumber;
            Category = category;
            Date = date;
            Notes = notes;
        }

        public static async void AddTask(string contactName, string address, int phoneNumber, string category, DateOnly date, string notes)
        {
            CollectionReference collRef = App.db.Collection("Tasks");
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"contactName", contactName},
                {"address", address},
                {"phoneNumber", phoneNumber.ToString()},
                {"category", category},
                {"date", date.ToString("dd/MM-yyyy")},
                {"notes", notes}
            };
            try
            {
                await collRef.AddAsync(data);
                Console.WriteLine($"Data added successfully: {data}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding data: {ex.Message}");
            }
            
        }

        public static async Task<List<Task>> GetTasks()
        {
            List<Task> tasks = new List<Task>();

            string contactName = "";
            string address = "";
            int phoneNumber = 0;
            string category = "";
            string date = "";
            string notes = "";

            CollectionReference catRef = App.db.Collection("Tasks");
            QuerySnapshot snap = await catRef.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snap.Documents)
            {
                if (document.Exists)
                {
                    Dictionary<string, object> tasksDict = document.ToDictionary();
                    foreach (object field in tasksDict.Keys)
                    {
                        Console.WriteLine(field);
                        if (tasksDict.TryGetValue(field.ToString(), out object fieldValue))
                        {
                            switch (field.ToString())
                            {
                                case "contactName":
                                    contactName = fieldValue as string ?? "";
                                    break;

                                case "address":
                                    address = fieldValue as string ?? "";
                                    break;

                                case "phoneNumber":
                                    if (fieldValue is string phoneStr && int.TryParse(phoneStr, out int parsedPhoneNumber))
                                    {
                                        phoneNumber = parsedPhoneNumber;
                                    }
                                    else
                                    {
                                        phoneNumber = 0;
                                    }
                                    break;

                                case "category":
                                    category = fieldValue as string ?? "";
                                    break;

                                case "date":
                                    date = fieldValue as string ?? "";
                                    break;

                                case "notes":
                                    notes = fieldValue as string ?? "";
                                    break;
                            }
                        }
                    }
                    DateOnly dateTime = DateOnly.ParseExact(date, "dd/MM-yyyy");

                    tasks.Add(new Task(contactName, address, phoneNumber, category, dateTime, notes));
                }
            }

            return tasks;
        }

        public static async void DeleteTask(string contactName)
        {
            CollectionReference collRef = App.db.Collection("Tasks");
            Query query = collRef.WhereEqualTo("contactName", contactName);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                await document.Reference.DeleteAsync();
            }
        }

        public override string ToString()
        {
            return $"{ContactName}, {Address}, {PhoneNumber}, {Category}, {Date}, {Notes}";
        }
    }
}
