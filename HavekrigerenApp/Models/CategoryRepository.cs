namespace HavekrigerenApp.Models
{
    public class CategoryRepository
    {
        private List<Category> categories = new List<Category>();

        public CategoryRepository()
        {
            categories.Add(new Category("Kategori 1"));
            categories.Add(new Category("Kategori 2"));
            categories.Add(new Category("Kategori 3"));
            categories.Add(new Category("Kategori 4"));
        }

        public void Add(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Category newCategory = new Category(name);
                categories.Add(newCategory);
            }
            else
            {
                throw new ArgumentException("Not all arguments are valid!");
            }
        }

        public Category Get(int id)
        {
            Category result = null;

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

        public List<Category> GetAll()
        {
            return categories;
        }
    }
}
