using HavekrigerenApp.Models.Classes;

namespace HavekrigerenApp.ViewModels
{
    public class CategoryViewModel
    {
        // Properties to display
        public Category Category { get; set; }
        public string Name { get; set; }

        public CategoryViewModel(Category category)
        {
            Category = category;
            Name = category.Name;
        }
    }
}
