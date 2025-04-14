using Google.Cloud.Firestore;

namespace HavekrigerenApp.Models.Classes
{
    [FirestoreData]
    public class Job
    {
        public int Id { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Category Category { get; set; }
        public bool HasDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }
        public DateTime DateCreated { get; set; }

        public Job(string contactName, string address, string phoneNumber, Category category, bool hasDate, DateTime? startDate, DateTime? endDate, string notes, DateTime dateCreated)
        {
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

        public override string ToString()
        {
            return $"{ContactName} {Address} {PhoneNumber} {Category.ToString()}";
        }
    }
}
