using System.Globalization;

namespace HavekrigerenApp.Models.Classes
{
    public class JobRepository
    {
        private List<Job> _jobs = new List<Job>();
        private DatabaseRepository _databaseRepo;
        private string _collectionName = "Jobs";

        public JobRepository(DatabaseRepository databaseRepo)
        {
            _databaseRepo = databaseRepo;
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
                if (_jobs.Count != 0)
                {
                    id = GetHighestId() + 1;
                }

                // Converts notes to empty string if null
                if (string.IsNullOrEmpty(notes))
                {
                    notes = string.Empty;
                }

                Job newJob = new Job(id, contactName, address, phoneNumber, category, hasDate, startDate, endDate, notes, dateCreated);
                _jobs.Add(newJob);

                await _databaseRepo.AddAsync(_collectionName, newJob);
            }
            else
            {
                throw new ArgumentException($"Job arguments cannot be null or empty!");
            }
        }

        public int GetHighestId()
        {
            int highestId = 0;

            foreach (Job job in _jobs)
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
            _jobs = await _databaseRepo.GetAllAsync<Job>(_collectionName);
            _jobs = SortJobsBy(job => job.ContactName);
            
        }

        public List<Job> SortJobsBy(Func<Job, object> propertyName)
        {
            return _jobs.OrderBy(propertyName).ToList();
        }

        public List<Job> GetAll()
        {
            return _jobs;
       }

        public Job? Get(string contactName)
        {
            foreach (Job job in _jobs)
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
            List<Job> filteredJobs = _jobs
                                    .Where(job => job.ToString().ToLower()
                                    .Contains(query?.ToLower() ?? ""))
                                    .ToList();

            return filteredJobs;
        }

        public async Task DeleteAsync(Job job)
        {
            _jobs.Remove(job);
            await _databaseRepo.DeleteAsync(_collectionName, "Id", job.Id);
        }
    }
}
