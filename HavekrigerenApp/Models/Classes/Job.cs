using Google.Cloud.Firestore;

namespace HavekrigerenApp.Models.Classes
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
        public bool HasDate { get; set; }
        [FirestoreProperty]
        public string StartDate { get; set; }
        [FirestoreProperty]
        public string EndDate { get; set; }
        [FirestoreProperty]
        public string Notes { get; set; }
        [FirestoreProperty]
        public string DateCreated { get; set; }

        public Job(int id, string contactName, string phoneNumber, string address, string category, bool hasDate, string startDate, string endDate, string notes, string dateCreated)
        {
            Id = id;
            ContactName = contactName;
            PhoneNumber = phoneNumber;
            Address = address;
            Category = category;
            HasDate = hasDate;
            StartDate = startDate;
            EndDate = endDate;
            Notes = notes;
            DateCreated = dateCreated;
        }

        // Parameterless contructor for Firebase cus they stupid
        public Job()
        {
        }
    }
}
