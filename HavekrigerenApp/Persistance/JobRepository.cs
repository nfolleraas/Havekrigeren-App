using HavekrigerenApp.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;

namespace HavekrigerenApp.Models.Classes
{
    public class JobRepository
    {
        private List<Job> _jobs = new List<Job>();
        private string? _connectionString;

        public JobRepository()
        {
            _connectionString = App.ConnectionString;
        }

        public void Add(Job job)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
        }

        public List<Job> GetAll()
        {
            if (_jobs.Count > 0)
            {
                return _jobs;
            }
            _jobs.Clear();

            List<Job> foundJobs = new List<Job>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
            return foundJobs;
        }

        public Job Get(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("The parameter id cannot be less than 0");
            }

            Job? foundJob = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_SelectJob", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

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

                        Job job = new Job(contactName, address, phoneNumber, category, hasDate, startDate, endDate, notes, dateCreated);

                        foundJob = job;
                    }
                }
            }
            if (foundJob == null)
            {
                throw new NotFoundException($"Category with the id '{id}' was not found in the database.");
            }

            return foundJob;
        }

        public void Update(Job job)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("The parameter id cannot be less than 0");
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
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

        public List<Job> PerformSearch(string query)
        {
            query = query.ToLower();

            List<Job> filteredJobs = _jobs
                .Where(job => (job.ContactName?.ToLower().StartsWith(query) ?? false) ||
                              (job.Address?.ToLower().StartsWith(query) ?? false) ||
                              (job.PhoneNumber?.ToLower().StartsWith(query) ?? false))
                .ToList();

            return filteredJobs;
        }
    }
}
