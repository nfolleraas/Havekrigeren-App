using HavekrigerenApp.Interfaces;

namespace HavekrigerenApp.Models
{
    public class CategoryRepository
    {
        private static List<Category> categories = new List<Category>();
        private DatabaseRepository databaseRepo;
        private string collectionName = "Categories";
        private string fieldName = "Name";

        public CategoryRepository()
        {
            databaseRepo = new DatabaseRepository();
        }

        public async Task AddAsync(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Category newCategory = new Category(name);
                categories.Add(newCategory);
                
                await databaseRepo.AddAsync(collectionName, newCategory);
            }
            else
            {
                throw new ArgumentException($"Name \"{name}\" cannot be null or empty!");
            }
        }

        public async Task LoadAllAsync()
        {
            categories = await databaseRepo.GetAllAsync<Category>(collectionName);
            categories.Sort((x, y) => x.Name.CompareTo(y.Name));
        }

        public List<Category> GetAll()
        {
            return categories;
        }

        public Category Get(string name)
        {
            Category result = null;

            foreach (Category category in categories)
            {
                if (category.Name == name)
                {
                    result = category;
                    break;
                }
            }
            return result;
        }

        public async Task DeleteAsync(Category category)
        {
            if (!string.IsNullOrEmpty(category.Name))
            {
                categories.Remove(category);
                await databaseRepo.DeleteAsync(collectionName, fieldName, category.Name);
            }
            else
            {
                throw new ArgumentException($"Name \"{category.Name}\" cannot be null or empty!");
            }
        }

    }
}
