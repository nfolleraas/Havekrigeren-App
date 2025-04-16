using HavekrigerenApp.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace HavekrigerenApp.Models.Classes
{
    public class CategoryRepository
    {
        private List<Category> _categories = new List<Category>();
        private string? _connectionString;

        public CategoryRepository()
        {
            _connectionString = App.ConnectionString;
        }

        public void Add(Category category)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_InsertCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Name", category.Name);
                    category.Id = (int)command.ExecuteScalar();
                }
            }
            _categories.Add(category);
        }

        public List<Category> GetAll()
        {
            _categories.Clear();

            List<Category> foundCategories = new List<Category>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_SelectAllCategories", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int categoryId = (int)reader["Id"];
                        string? categoryName = (string)reader["Name"];

                        Category category = new Category(categoryName) { Id = categoryId };

                        foundCategories.Add(category);
                        _categories.Add(category);
                    }
                }
            }
            return foundCategories;
        }

        public Category Get(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("The parameter id cannot be less than 0");
            }

            Category? foundCategory = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_SelectCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    using SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int categoryId = (int)reader["Id"];
                        string? categoryName = (string)reader["Name"];

                        foundCategory = new Category(categoryName) { Id = categoryId };
                    }
                }
            }

            if (foundCategory == null)
            {
                throw new NotFoundException($"Category with the id '{id}' was not found in the database.");
            }

            return foundCategory;
        }

        public void Update(Category category)
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
                using (SqlCommand command = new SqlCommand("sp_DeleteCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
            List<Category> copy = new List<Category>(_categories);
            foreach (Category category in copy)
            {
                if (category.Id == id)
                {
                    _categories.Remove(category);
                }
            }
        }
    }
}
