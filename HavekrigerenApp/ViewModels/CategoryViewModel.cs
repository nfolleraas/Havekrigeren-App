using System.Windows.Input;
using HavekrigerenApp.Services;
using HavekrigerenApp.Models;

namespace HavekrigerenApp.ViewModels
{
    public class CategoryViewModel
    {
        private Category category;
        private AlertService alertService = new AlertService();

        public string Name { get; set; }

        public ICommand CategoryClickedCmd { get; set; }

        public CategoryViewModel(Category category)
        {
            this.category = category;
            Name = category.Name;

            // Command registration
            CategoryClickedCmd = new Command<string>(OnCategoryClicked);
        }

        private async void OnCategoryClicked(string name)
        {
            await alertService.ShowAlert("Category Clicked", name, "OK");
        }
    }
}
