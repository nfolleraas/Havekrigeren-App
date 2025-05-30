using HavekrigerenApp.Models;
using HavekrigerenApp.Persistance;
using HavekrigerenApp.Services;
using HavekrigerenApp.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using static Google.Cloud.Firestore.V1.StructuredAggregationQuery.Types.Aggregation.Types;


namespace HavekrigerenApp.ViewModels
{
    public class ViewAllCategoriesViewModel : BaseViewModel
    {
        public ObservableCollection<CategoryViewModel> CategoriesVM { get; set; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        private bool _showNoCategoriesMessage;
        public bool ShowNoCategoriesMessage
        {
            get { return _showNoCategoriesMessage; }
            set
            {
                _showNoCategoriesMessage = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand CategoryClickedCommand { get; }
        public ICommand CreateCategoryCommand { get; }
        public ICommand RefreshCommand { get; }

        public ViewAllCategoriesViewModel()
        {
            CategoriesVM = new ObservableCollection<CategoryViewModel>();

            // Command registration
            CategoryClickedCommand = new Command<Category>(CategoryClicked);
            CreateCategoryCommand = new Command(CreateCategory);
            RefreshCommand = new Command(RefreshPage);
        }

        public void LoadCategories()
        {
            CategoriesVM.Clear();
            // Instatiate new CategoryViewModel for each category
            foreach (Category category in CategoryRepository.GetAll())
            {
                CategoryViewModel categoryVM = new CategoryViewModel(category);
                CategoriesVM.Add(categoryVM);
            }
            ShowNoCategoriesMessage = CategoriesVM.Count <= 0 ? true : false;
        }

        public void RefreshPage()
        {
            try
            {
                IsRefreshing = true;
                LoadCategories();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async void CategoryClicked(Category category)
        {
            try
            {
                await NavigationService.PushAsync(new ViewAllJobsPage(category));
            }
            catch (InvalidOperationException ex)
            {
                await AlertService.DisplayAlertAsync("Fejl!", ex.Message);
            }
            catch (Exception ex)
            {
                await AlertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }

        private async void CreateCategory()
        {
            try
            {
                string result = await AlertService.DisplayPromptAsync("Ny Kategori", "Indtast navn på ny kategori", "Opret", "Annuller");
                if (result == null)
                {
                    return; // Exit method
                }
                else if (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result))
                {
                    await AlertService.DisplayAlertAsync("Opret Kategori", "Navnet på kategorien skal have indhold.");
                }
                else
                {
                    foreach (var categoryVM in CategoriesVM)
                    {
                        if (result == categoryVM.Name)
                        {
                            await AlertService.DisplayAlertAsync("Opret Kategori", $"Kategorien \"{result}\" eksisterer allerede.");
                            return; // Exit method
                        }
                    }
                    // Successfully add category
                    Category newCategory = new Category(result);
                    CategoryRepository.Add(newCategory);
                    RefreshPage();
                }
            }
            catch (InvalidOperationException ex)
            {
                await AlertService.DisplayAlertAsync("Fejl!", ex.Message);
            }
            catch (Exception ex)
            {
                await AlertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }
    }
}
