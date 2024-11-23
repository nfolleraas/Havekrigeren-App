using HavekrigerenApp.Models;
using HavekrigerenApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;


namespace HavekrigerenApp.ViewModels
{
    public class ViewAllCategoriesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private CategoryRepository categoryRepo = new CategoryRepository();
        private AlertService alertService = new AlertService();

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

        public ViewAllCategoriesViewModel()
        {
            CategoriesVM = new ObservableCollection<CategoryViewModel>();
            OnRefresh();

            // Command registration
            CategoryClickedCmd = new Command<string>(OnCategoryClicked);
            CreateCategoryCmd = new Command(OnCreateCategoryClicked);
            RefreshCmd = new Command(async () => await OnRefresh());
        }

        private async Task LoadCategories()
        {
            await categoryRepo.LoadAllAsync();

            CategoriesVM.Clear();
            foreach (Category category in categoryRepo.GetAll())
            {
                CategoryViewModel categoryVM = new CategoryViewModel(category);
                CategoriesVM.Add(categoryVM);
            }
        }

        private async void OnCategoryClicked(string name)
        {
            try
            {
                await alertService.DisplayAlert("Category Clicked", name, "OK");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something else went wrong: " + ex.Message);
            }
        }

        private async void OnCreateCategoryClicked()
        {
            try
            {
                string result = await alertService.DisplayPromptAsync("Ny Kategori", "Indtast navn på ny kategori", "Opret", "Annuller");
                await categoryRepo.AddAsync(result);

                await OnRefresh();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something else went wrong: " + ex.Message);
            }
        }

        private async Task OnRefresh()
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
