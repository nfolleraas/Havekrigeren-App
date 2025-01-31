using HavekrigerenApp.Models.Classes;

namespace HavekrigerenApp.ViewModels
{
    public class CategoryViewModel
    {
        // Properties to display
        private Category category;
        public string Name { get; set; }

        public CategoryViewModel(Category category)
        {
            this.category = category;
            Name = category.Name;
        }
    }
}
