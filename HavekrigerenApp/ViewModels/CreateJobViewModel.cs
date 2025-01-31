﻿using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.Models.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace HavekrigerenApp.ViewModels
{
    public class CreateJobViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private CategoryRepository categoryRepo = new CategoryRepository();
        private JobRepository jobRepo = new JobRepository();
        private AlertService alertService = new AlertService();

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set 
            { 
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        private string _contactName;
        public string ContactName
        {
            get => _contactName;
            set
            {
                _contactName = value;
                EnableCreateButton();
                OnPropertyChanged(nameof(ContactName));
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                EnableCreateButton();
                OnPropertyChanged(nameof(Address));
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                EnableCreateButton();
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        private Category? _category;
        public Category? Category
        {
            get => _category;
            set
            {
                _category = value;
                EnableCreateButton();
                OnPropertyChanged(nameof(Category));
            }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        
        private string _notes;
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        private DateTime _dateCreated;
        public DateTime DateCreated
        {
            get => _dateCreated;
            set
            {
                _dateCreated = value;
                OnPropertyChanged(nameof(DateCreated));
            }
        }

        private bool _isButtonEnabled;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set 
            { 
                _isButtonEnabled = value; 
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set 
            { 
                _isChecked = value; 
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        // Commands
        public ICommand CreateJobCommand { get; set; }
        public ICommand StartDateSelectedCommand { get; set; }
        public ICommand EndDateSelectedCommand { get; set; }
        public ICommand ToggleDatesCommand { get; set; }

        public CreateJobViewModel()
        {
            _categories = new ObservableCollection<Category>();
            _isButtonEnabled = false;
            //LoadCategories();

            // Command registration
            CreateJobCommand = new Command(CreateJob);
            StartDateSelectedCommand = new Command<DateTime>(StartDateSelected);
            EndDateSelectedCommand = new Command<DateTime>(EndDateSelected);
            ToggleDatesCommand = new Command<bool>(ToggleDates);
        }

        public async Task LoadCategories()
        {
            Categories.Clear();

            await categoryRepo.LoadAllAsync();

            foreach (Category category in categoryRepo.GetAll())
            {
                _categories.Add(category);
            }
        }

        private void EnableCreateButton()
        {
            _isButtonEnabled = !string.IsNullOrWhiteSpace(_contactName)
                && !string.IsNullOrWhiteSpace(_address)
                && !string.IsNullOrWhiteSpace(_phoneNumber)
                && _category != null;

            OnPropertyChanged(nameof(IsButtonEnabled));
        }

        // Resets the user inputs on the page
        private void ResetInputs()
        {
            ContactName = string.Empty;
            PhoneNumber = string.Empty;
            Address = string.Empty;
            Category = null;
            IsChecked = false;
            StartDate = null;
            EndDate = null;
            Notes = string.Empty;
        }

        // Commands

        private async void CreateJob()
        {
            try
            {

                await alertService.DisplayAlertAsync("Opret Opgave", $"Oprettede opgaven \"{_contactName}, {_address}\"");
                _dateCreated = DateTime.Now;
                await jobRepo.AddAsync(_contactName, _address, _phoneNumber, _category, _isChecked, _startDate, _endDate, _notes, _dateCreated);
                ResetInputs();
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
        private void StartDateSelected(DateTime selectedDate)
        {
            _startDate = selectedDate;
        }

        private void EndDateSelected(DateTime selectedDate)
        {
            _endDate = selectedDate;
        }
        private void ToggleDates(bool isChecked)
        {
            if (isChecked)
            {
                Console.WriteLine("yay");
                _startDate = DateTime.Now;
                _endDate = DateTime.Now;
            }
            else
            {
                Console.WriteLine("nay");
                _startDate = null;
                _endDate = null;
            }
        }

        // Method for updating the UI on changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
