using HavekrigerenApp.Interfaces;

namespace HavekrigerenApp.Models
{
    public class CategoryRepository
    {
        private static List<Category> categories = new List<Category>();
        private DatabaseRepository databaseRepo;
        private string collectionName = "Categories";

        public CategoryRepository()
        {
            databaseRepo = new DatabaseRepository();
        }

        public async Task AddAsync(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                int id = 0;
                if (categories.Count != 0)
                {
                    id = GetHighestId() + 1;
                }
                Category newCategory = new Category(name, id);
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

        public Category Get(int id)
        {
            Category? result = null;

            foreach (Category category in categories)
            {
                if (category.Id == id)
                {
                    result = category;
                    break;
                }
            }
            return result;
        }

        public int GetHighestId()
        {
            int highestId = 0;

            foreach (Category category in categories)
            {
                if (category.Id > highestId)
                {
                    highestId = category.Id;
                }
            }
            return highestId;
        }

        public async Task DeleteAsync(Category category)
        {
            categories.Remove(category);
            await databaseRepo.DeleteAsync(collectionName, "Id", category.Id);
        }

    }
}
