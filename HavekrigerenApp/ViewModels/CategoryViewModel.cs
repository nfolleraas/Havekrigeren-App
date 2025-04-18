using HavekrigerenApp.Models;

namespace HavekrigerenApp.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        private Category _category;
        public Category Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }


        public CategoryViewModel(Category category)
        {
            Category = category;
            Name = category.Name;
        }
    }
}
