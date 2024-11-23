using HavekrigerenApp.Models;

namespace HavekrigerenApp.ViewModels
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }
        public string Name { get; set; }

        public CategoryViewModel(Category category)
        {
            Category = category;
            Name = category.Name;
        }
    }
}
