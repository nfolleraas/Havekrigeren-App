using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.Models.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;


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

        // Commands
        public ICommand CategoryClickedCommand { get; }
        public ICommand CreateCategoryCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteCategoryCommand { get; }

        public ViewAllCategoriesViewModel()
        {
            CategoriesVM = new ObservableCollection<CategoryViewModel>();

            // Command registration
            CategoryClickedCommand = new Command<Category>(CategoryClicked);
            CreateCategoryCommand = new Command(CreateCategory);
            RefreshCommand = new Command(RefreshPage);
            DeleteCategoryCommand = new Command<Category>(DeleteCategory);
        }

        public void LoadCategories()
        {
            CategoriesVM.Clear();
            // Instatiate new CategoryViewModel for each category
            foreach (Category category in _categoryRepo.GetAll())
            {
                CategoryViewModel categoryVM = new CategoryViewModel(category);
                CategoriesVM.Add(categoryVM);
            }
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
                await _navigationService.PushAsync(new ViewAllJobsPage(category.Name));
            }
            catch (InvalidOperationException ex)
            {
                await _alertService.DisplayAlertAsync("Fejl!", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _alertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }

        private async void CreateCategory()
        {
            try
            {
                string result = await _alertService.DisplayPromptAsync("Ny Kategori", "Indtast navn på ny kategori", "Opret", "Annuller");
                if (result == null)
                {
                    return; // Exit method
                }
                else if (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result))
                {
                    await _alertService.DisplayAlertAsync("Opret Kategori", "Navnet på kategorien skal have indhold.");
                }
                else
                {
                    foreach (var categoryVM in CategoriesVM)
                    {
                        if (result.Contains(categoryVM.Name))
                        {
                            await _alertService.DisplayAlertAsync("Opret Kategori", $"Kategorien \"{result}\" eksisterer allerede.");
                            return; // Exit method
                        }
                    }
                    // Successfully add category
                    Category newCategory = new Category(result);
                    _categoryRepo.Add(newCategory);
                    RefreshPage();
                }
            }
            catch (InvalidOperationException ex)
            {
                await _alertService.DisplayAlertAsync("Fejl!", ex.Message);
            }
            catch (Exception ex)
            {
                await _alertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }

        private async void DeleteCategory(Category category)
        {
            try
            {
                bool answer = await _alertService.DisplayAlertAsync("Slet Kategori", $"Er du sikker på, du vil slette kategorien \"{category.Name}\"?\nDette vil også slette alle opgaver i kategorien.\nDenne handling kan ikke fortrydes.", "Ja", "Nej");

                if (answer)
                {
                    // Delete every job in the category
                    List<Job> jobs = _jobRepo.GetAll().ToList(); // Create a duplicate of jobs list
                    foreach (Job job in jobs)
                    {
                        if (category.Name == job.Category.ToString())
                        {
                            _jobRepo.Delete(job.Id); // Delete job in the real jobs list
                        }
                    }
                    // Then delete the category
                    _categoryRepo.Delete(category.Id);
                    await _alertService.DisplayAlertAsync("Slet Kategori", $"Kategorien \"{category.Name}\" blev slettet.");
                    RefreshPage();
                }
            }
            catch (ArgumentException ex)
            {
                await _alertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }
    }
}
