using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.Models.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;


namespace HavekrigerenApp.ViewModels
{
    public class ViewAllCategoriesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private CategoryRepository categoryRepo = new CategoryRepository();
        private JobRepository jobRepo = new JobRepository();
        private AlertService alertService = new AlertService();
        private NavigationService navigationService = new NavigationService();

        private ObservableCollection<CategoryViewModel>? _categoriesVM;
        public ObservableCollection<CategoryViewModel>? CategoriesVM 
        {
            get => _categoriesVM;
            set
            {
                _categoriesVM = value;
                OnPropertyChanged(nameof(CategoriesVM));
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set 
            { 
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        // Commands
        public ICommand CategoryClickedCmd { get; set; }
        public ICommand CreateCategoryCmd { get; set; }
        public ICommand RefreshCmd { get; set; }
        public ICommand DeleteCategoryCmd { get; set; }

        public ViewAllCategoriesViewModel()
        {
            _categoriesVM = new ObservableCollection<CategoryViewModel>();
            // Show the user that the page is loading
            RefreshPage(); // Could use LoadCategories() instead, but that doesnt show the reload animation

            // Command registration
            CategoryClickedCmd = new Command<Category>(CategoryClicked);
            CreateCategoryCmd = new Command(CreateCategory);
            RefreshCmd = new Command(async () => await RefreshPage());
            DeleteCategoryCmd = new Command<Category>(DeleteCategory);
        }

        private async Task LoadCategories()
        {
            await categoryRepo.LoadAllAsync();

            _categoriesVM.Clear();
            // Instatiate new CategoryViewModel for each category
            foreach (Category category in categoryRepo.GetAll())
            {
                CategoryViewModel categoryVM = new CategoryViewModel(category);
                _categoriesVM.Add(categoryVM);
            }
        }

        private async void CategoryClicked(Category category)
        {
            try
            {
                await navigationService.PushAsync(new ViewAllJobsPage(category.Name));
            }
            catch (InvalidOperationException ex)
            {
                await alertService.DisplayAlertAsync("Fejl!", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await alertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }

        private async void CreateCategory()
        {
            try
            {
                string result = await alertService.DisplayPromptAsync("Ny Kategori", "Indtast navn på ny kategori", "Opret", "Annuller");
                if (result == null)
                {
                    return; // Exit method
                }
                else if (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result))
                {
                    await alertService.DisplayAlertAsync("Opret Kategori", "Navnet på kategorien skal have indhold.");
                }
                else
                {
                    foreach (var categoryVM in CategoriesVM)
                    {
                        if (result.Contains(categoryVM.Name))
                        {
                            await alertService.DisplayAlertAsync("Opret Kategori", $"Kategorien \"{result}\" eksisterer allerede.");
                            return; // Exit method
                        }
                    }
                    // Successfully add category
                    await categoryRepo.AddAsync(result);
                    await RefreshPage();
                }
            }
            catch (InvalidOperationException ex)
            {
                await alertService.DisplayAlertAsync("Fejl!", ex.Message);
            }
            catch (Exception ex)
            {
                await alertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }

        private async Task RefreshPage()
        {
            try
            {
                IsRefreshing = true;
                await LoadCategories();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async void DeleteCategory(Category category)
        {
            try
            {
                bool answer = await alertService.DisplayAlertAsync("Slet Kategori", $"Er du sikker på, du vil slette kategorien \"{category.Name}\"?\nDette vil også slette alle opgaver i kategorien.\nDenne handling kan ikke fortrydes.", "Ja", "Nej");

                if (answer)
                {
                    // Delete every job in the category
                    List<Job> jobs = jobRepo.GetAll().ToList(); // Create a duplicate of jobs list
                    foreach (Job job in jobs)
                    {
                        if (category.Name == job.Category)
                        {
                            await jobRepo.DeleteAsync(job); // Delete job in the real jobs list
                        }
                    }
                    // Then delete the category
                    await categoryRepo.DeleteAsync(category);
                    await alertService.DisplayAlertAsync("Slet Kategori", $"Kategorien \"{category.Name}\" blev slettet.");
                    await RefreshPage();
                }
            }
            catch (ArgumentException ex)
            {
                await alertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }

        // Method for updating the UI on changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
