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

        public async Task Add(string name)
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

        public async Task LoadAll()
        {
            categories = await databaseRepo.GetAllAsync<Category>(collectionName);
        }

        public List<Category> GetAll()
        {
            categories.Sort((x, y) => x.Name.CompareTo(y.Name));
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

    }
}
