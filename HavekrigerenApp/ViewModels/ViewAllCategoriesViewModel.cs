using HavekrigerenApp.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace HavekrigerenApp.ViewModels
{
    public class ViewAllCategoriesViewModel
    {
        private CategoryRepository categoryRepo = new CategoryRepository();

        public ObservableCollection<CategoryViewModel> CategoriesVM { get; set; }

        // Commands
        public RelayCommand<string> CategoryClickedCmd { get; }

        public ViewAllCategoriesViewModel()
        {

            CategoriesVM = new ObservableCollection<CategoryViewModel>();

            foreach (Category category in categoryRepo.GetAll())
            {
                CategoryViewModel categoryVM = new CategoryViewModel(category);
                CategoriesVM.Add(categoryVM);
            }
        }
    }
}
