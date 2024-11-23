using HavekrigerenApp.Models;

namespace HavekrigerenApp.ViewModels
{
    public class CategoryViewModel
    {
        private Category category;

        public string Name { get; set; }

        public CategoryViewModel(Category category)
        {
            this.category = category;
            Name = category.Name;
        }

        
    }
}
