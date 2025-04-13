using HavekrigerenApp.Models.Interfaces;

namespace HavekrigerenApp.Models.Classes
{
    public class CategoryRepository
    {
        private List<Category> _categories = new List<Category>();
        private DatabaseRepository _databaseRepo;
        private string _collectionName = "Categories";

        public CategoryRepository(DatabaseRepository databaseRepo)
        {
            _databaseRepo = databaseRepo;
        }

        public async Task AddAsync(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                int id = 0;
                await LoadAllAsync();
                if (_categories.Count != 0)
                {
                    id = GetHighestId() + 1;
                }
                Category newCategory = new Category(id, name);
                _categories.Add(newCategory);

                await _databaseRepo.AddAsync(_collectionName, newCategory);
            }
            else
            {
                throw new ArgumentException($"Category arguments cannot be null or empty!");
            }
        }

        public int GetHighestId()
        {
            int highestId = 0;

            foreach (Category category in _categories)
            {
                if (category.Id > highestId)
                {
                    highestId = category.Id;
                }
            }
            return highestId;
        }

        public async Task LoadAllAsync()
        {
            _categories = await _databaseRepo.GetAllAsync<Category>(_collectionName);
            _categories.Sort((x, y) => x.Name.CompareTo(y.Name));
        }

        public List<Category> GetAll()
        {
            return _categories;
        }

        public Category Get(int id)
        {
            Category? result = null;

            foreach (Category category in _categories)
            {
                if (category.Id == id)
                {
                    result = category;
                    break;
                }
            }
            return result;
        }

        public async Task DeleteAsync(Category category)
        {
            _categories.Remove(category);
            await _databaseRepo.DeleteAsync(_collectionName, "Id", category.Id);
        }

    }
}
