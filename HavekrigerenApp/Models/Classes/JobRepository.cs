﻿namespace HavekrigerenApp.Models.Classes
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

        public async Task AddAsync(string contactName, string phoneNumber, string address, Category category, DateTime startDate, DateTime endDate, string notes = "")
        {
            // Check if inputs have content
            if (!string.IsNullOrEmpty(contactName)
                && !string.IsNullOrEmpty(phoneNumber)
                && !string.IsNullOrEmpty(address)
                && !string.IsNullOrEmpty(category.ToString())
                && !string.IsNullOrEmpty(startDate.ToString())
                && !string.IsNullOrEmpty(endDate.ToString()))
            {
                int id = 0;
                await LoadAllAsync();
                if (jobs.Count != 0)
                {
                    id = GetHighestId() + 1;
                }
                Job newJob = new Job(id, contactName, phoneNumber, address, category.ToString(), startDate.ToString(), endDate.ToString(), notes);
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
            jobs.Sort((x, y) => x.ContactName.CompareTo(y.ContactName));
        }

        public List<Job> GetAll()
        {
            return jobs;
        }

        public Job Get(int id)
        {
            Job? result = null;

            foreach (Job job in jobs)
            {
                if (job.Id == id)
                {
                    result = job;
                    break;
                }
            }
            return result;
        }

        public async Task DeleteAsync(Job job)
        {
            jobs.Remove(job);
            await databaseRepo.DeleteAsync(collectionName, "Id", job.Id);
        }
    }
}
