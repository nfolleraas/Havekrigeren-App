using System.Globalization;

namespace HavekrigerenApp.Models.Classes
{
    public class JobRepository
    {
        private static List<Job> jobs = new List<Job>();
        private DatabaseRepository databaseRepo;
        private string collectionName = "Jobs";

        public JobRepository()
        {
            databaseRepo = new DatabaseRepository();
        }

        public async Task AddAsync(string contactName, string address, string phoneNumber, Category category, bool hasDate, DateTime? startDate, DateTime? endDate, string notes, DateTime dateCreated)
        {
            // Check if inputs have content
            if (!string.IsNullOrEmpty(contactName)
                && !string.IsNullOrEmpty(address)
                && !string.IsNullOrEmpty(phoneNumber)
                && !string.IsNullOrEmpty(category.ToString()))
            {
                // Set job id to highest id + 1
                int id = 0;
                await LoadAllAsync();
                if (jobs.Count != 0)
                {
                    id = GetHighestId() + 1;
                }

                // Converts notes to empty string if null
                if (string.IsNullOrEmpty(notes))
                {
                    notes = string.Empty;
                }
                // Converts start and end dates to either date format or empty string
                string formattedStartDate = startDate?.ToString("dd/MM-yyyy") ?? string.Empty;
                string formattedEndDate = endDate?.ToString("dd/MM-yyyy") ?? string.Empty;

                Job newJob = new Job(id, contactName, address, phoneNumber, category.ToString(), hasDate, formattedStartDate, formattedEndDate, notes, dateCreated.ToString("dd/MM-yyyy HH:mm:ss"));
                jobs.Add(newJob);

                await databaseRepo.AddAsync(collectionName, newJob);
            }
            else
            {
                throw new ArgumentException($"Job arguments cannot be null or empty!");
            }
        }

        public int GetHighestId()
        {
            int highestId = 0;

            foreach (Job job in jobs)
            {
                if (job.Id > highestId)
                {
                    highestId = job.Id;
                }
            }
            return highestId;
        }

        public async Task LoadAllAsync()
        {
            jobs = await databaseRepo.GetAllAsync<Job>(collectionName);
            jobs = SortJobsBy(job => job.ContactName);
            
        }

        public List<Job> SortJobsBy(Func<Job, object> propertyName)
        {
            return jobs.OrderBy(propertyName).ToList();
        }

        public List<Job> GetAll()
        {
            return jobs;
       }

        public Job? Get(string contactName)
        {
            foreach (Job job in jobs)
            {
                if (job.ContactName == contactName)
                {
                    return job;
                }
            }
            return null;
        }

        public List<Job> PerformSearch(string query)
        {
            List<Job> filteredJobs = jobs
                                    .Where(job => job.ToString().ToLower()
                                    .Contains(query?.ToLower() ?? ""))
                                    .ToList();

            return filteredJobs;
        }

        public async Task DeleteAsync(Job job)
        {
            jobs.Remove(job);
            await databaseRepo.DeleteAsync(collectionName, "Id", job.Id);
        }
    }
}
