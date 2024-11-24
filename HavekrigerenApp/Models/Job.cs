using Google.Cloud.Firestore;

namespace HavekrigerenApp.Models
{
    [FirestoreData]
    public class Job
    {
        [FirestoreProperty]
        public int Id { get; set; }
        [FirestoreProperty]
        public string ContactName { get; set; }
        [FirestoreProperty]
        public string PhoneNumber { get; set; }
        [FirestoreProperty]
        public string Address { get; set; }
        [FirestoreProperty]
        public string Category { get; set; }
        [FirestoreProperty]
        public string StartDate { get; set; }
        [FirestoreProperty]
        public string EndDate { get; set; }
        [FirestoreProperty]
        public string Notes { get; set; }

        public Job(int id, string contactName, string phoneNumber, string address, string category, string startDate, string endDate, string notes)
        {
            Id = id;
            ContactName = contactName;
            PhoneNumber = phoneNumber;
            Address = address;
            Category = category;
            StartDate = startDate;
            EndDate = endDate;
            Notes = notes;
        }

        // Parameterless contructor for Firebase cus they stupid
        public Job()
        {
        }
    }
}
