using Android.Webkit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HavekrigerenApp.Models.Classes
{
    public class JobRepository
    {
        private List<Job> _jobs = new List<Job>();
        private string? _connectionString;

        public JobRepository()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();

            _connectionString = config.GetConnectionString("DBConnection");
        }

        public void Add(Job job)
        {
            throw new NotImplementedException();
        }

        public List<Job> GetAll()
        {
            throw new NotImplementedException();
        }

        public Job Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Job job)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Job> PerformSearch(string input)
        {
            throw new NotImplementedException();
        }
    }
}
