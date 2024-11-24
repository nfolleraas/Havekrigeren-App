﻿using Android.Icu.Text;
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
        public ICommand DeleteCategoryCmd { get; set; }

        public ViewAllCategoriesViewModel()
        {
            CategoriesVM = new ObservableCollection<CategoryViewModel>();
            RefreshPage();

            // Command registration
            CategoryClickedCmd = new Command<string>(OnCategoryClicked);
            CreateCategoryCmd = new Command(OnCreateCategoryClicked);
            RefreshCmd = new Command(async () => await RefreshPage());
            DeleteCategoryCmd = new Command<Category>(OnDeleteCategoryClicked);
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
                await alertService.DisplayAlertAsync("Category Clicked", name, "OK");
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

        private async void OnCreateCategoryClicked()
        {
            try
            {
                string result = await alertService.DisplayPromptAsync("Ny Kategori", "Indtast navn på ny kategori", "Opret", "Annuller");
                if (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result))
                {
                    await alertService.DisplayAlertAsync("Opret Kategori", "Navnet på kategorien skal have indhold.");
                }
                else
                {
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

        private async void OnDeleteCategoryClicked(Category category)
        {
            try
            {
                bool answer = await alertService.DisplayAlertAsync("Slet Kategori", $"Er du sikker på, du vil slette kategorien \"{category.Name}\"?\nDenne handling kan ikke fortrydes.", "Ja", "Nej");

                if (answer)
                {
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
