using HavekrigerenApp.Exceptions;
using HavekrigerenApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;

namespace HavekrigerenApp.Persistance
{
    public static class JobRepository
    {
        private static List<Job> _jobs = new List<Job>();

        public static void Add(Job job)
        {
            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_InsertJob", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ContactName", job.ContactName);
                    command.Parameters.AddWithValue("@Address", job.Address);
                    command.Parameters.AddWithValue("@PhoneNumber", job.PhoneNumber);
                    command.Parameters.AddWithValue("@HasDate", job.HasDate);
                    command.Parameters.AddWithValue("@StartDate", job.StartDate.HasValue ? job.StartDate.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", job.EndDate.HasValue ? job.EndDate.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Notes", (object)job.Notes ?? string.Empty);
                    command.Parameters.AddWithValue("@DateCreated", job.DateCreated);
                    command.Parameters.AddWithValue("@CategoryId", job.Category.Id);
                    job.Id = (int)command.ExecuteScalar();
                }
            }
            _jobs.Add(job);
        }

        public static List<Job> GetAll()
        {
            _jobs.Clear();

            List<Job> foundJobs = new List<Job>();
            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_SelectAllJobs", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int jobId = (int)reader["JobId"];
                        string contactName = (string)reader["ContactName"];
                        string address = (string)reader["Address"];
                        string phoneNumber = (string)reader["PhoneNumber"];

                        int categoryId = (int)reader["CategoryId"];
                        string categoryName = (string)reader["CategoryName"];
                        Category category = new Category(categoryName) { Id = categoryId };

                        bool hasDate = (bool)reader["HasDate"];
                        DateTime? startDate = reader["StartDate"] != DBNull.Value ? (DateTime)reader["StartDate"] : null;
                        DateTime? endDate = reader["EndDate"] != DBNull.Value ? (DateTime)reader["EndDate"] : null;
                        string notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : string.Empty;
                        DateTime dateCreated = (DateTime)reader["DateCreated"];

                        Job job = new Job(contactName, address, phoneNumber, category, hasDate, startDate, endDate, notes, dateCreated)
                        {
                            Id = jobId
                        };

                        foundJobs.Add(job);
                        _jobs.Add(job);
                    }
                }
            }
            foundJobs = foundJobs
                .OrderByDescending(job => job.DateCreated)
                .ToList();

            return foundJobs;
        }

        public static Job Get(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("The parameter id cannot be less than 0");
            }

            Job? foundJob = null;

            foreach (Job job in _jobs)
            {
                if (job.Id == id)
                {
                    foundJob = job;
                }
            }

            if (foundJob == null)
            {
                throw new NotFoundException($"Category with the id '{id}' was not found in the database.");
            }

            return foundJob;
        }

        public static void Update(Job updatedJob)
        {
            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_UpdateJob", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", updatedJob.Id);
                    command.Parameters.AddWithValue("@ContactName", updatedJob.ContactName);
                    command.Parameters.AddWithValue("@Address", updatedJob.Address);
                    command.Parameters.AddWithValue("@PhoneNumber", updatedJob.PhoneNumber);
                    command.Parameters.AddWithValue("@HasDate", updatedJob.HasDate);
                    command.Parameters.AddWithValue("@StartDate", updatedJob.StartDate.HasValue ? updatedJob.StartDate.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", updatedJob.EndDate.HasValue ? updatedJob.EndDate.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@Notes", (object)updatedJob.Notes ?? string.Empty);
                    command.Parameters.AddWithValue("@CategoryId", updatedJob.Category.Id);

                    Console.WriteLine("Affected rows:" + command.ExecuteNonQuery());
                }
            }
            foreach (Job job in _jobs)
            {
                if (job.Id == updatedJob.Id)
                {
                    job.ContactName = updatedJob.ContactName;
                    job.Address = updatedJob.Address;
                    job.PhoneNumber = updatedJob.PhoneNumber;
                    job.HasDate = updatedJob.HasDate;
                    job.StartDate = updatedJob.StartDate;
                    job.EndDate = updatedJob.EndDate;
                    job.Notes = updatedJob.Notes;
                    job.Category = updatedJob.Category;
                }
            }
        }

        public static void Delete(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("The parameter id cannot be less than 0");
            }

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_DeleteJob", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
            List<Job> copy = new List<Job>(_jobs);
            foreach(Job job in copy)
            {
                if (job.Id == id)
                {
                    _jobs.Remove(job);
                }
            }
        }

        public static List<Job> PerformSearch(string query)
        {
            query = query.ToLower();

            List<Job> filteredJobs = _jobs
                .Where(job => (job.ContactName?.ToLower().StartsWith(query) ?? false) ||
                              (job.Address?.ToLower().StartsWith(query) ?? false) ||
                              (job.PhoneNumber?.ToLower().StartsWith(query) ?? false) ||
                              (job.Category?.ToString().ToLower().StartsWith(query) ?? false )
                )
                .ToList();

            return filteredJobs;
        }
    }
}
