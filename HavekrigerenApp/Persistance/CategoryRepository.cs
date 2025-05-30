using HavekrigerenApp.Exceptions;
using HavekrigerenApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace HavekrigerenApp.Persistance
{
    public static class CategoryRepository
    {
        private static List<Category> _categories = new List<Category>();

        public static void Add(Category category)
        {
            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
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

        public static List<Category> GetAll()
        {
            _categories.Clear();

            List<Category> foundCategories = new List<Category>();
            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
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

        public static Category Get(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("The parameter id cannot be less than 0");
            }

            Category? foundCategory = null;

            foreach (Category category in _categories)
            {
                if (category.Id == id)
                {
                    foundCategory = category;
                }
            }

            if (foundCategory == null)
            {
                throw new NotFoundException($"Category with the id '{id}' was not found in the database.");
            }

            return foundCategory;
        }

        public static void Update(Category category)
        {
            throw new NotImplementedException();
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
