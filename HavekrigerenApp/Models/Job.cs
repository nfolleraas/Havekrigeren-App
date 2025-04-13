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
        public string Address { get; set; }
        [FirestoreProperty]
        public string PhoneNumber { get; set; }
        [FirestoreProperty]
        public Category Category { get; set; }
        [FirestoreProperty]
        public bool HasDate { get; set; }
        [FirestoreProperty]
        public DateTime? StartDate { get; set; }
        [FirestoreProperty]
        public DateTime? EndDate { get; set; }
        [FirestoreProperty]
        public string Notes { get; set; }
        [FirestoreProperty]
        public DateTime DateCreated { get; set; }

        public Job(int id, string contactName, string address, string phoneNumber, Category category, bool hasDate, DateTime? startDate, DateTime? endDate, string notes, DateTime dateCreated)
        {
            Id = id;
            ContactName = contactName;
            Address = address;
            PhoneNumber = phoneNumber;
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

        public override string ToString()
        {
            return $"{ContactName} {Address} {PhoneNumber} {Category}";
        }
    }
}
