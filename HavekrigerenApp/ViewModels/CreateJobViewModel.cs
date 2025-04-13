using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.Models.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace HavekrigerenApp.ViewModels
{
    public class CreateJobViewModel : BaseViewModel
    {
        private DateTime dateCreated;

        private string _contactName;
        public string ContactName
        {
            get => _contactName;
            set
            {
                _contactName = value;
                EnableCreateButton();
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        private Category _category;
        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
                EnableCreateButton();
                OnPropertyChanged();
            }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }
        
        private string _notes;
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        private bool _isCreateButtonEnabled;
        public bool IsCreateButtonEnabled
        {
            get => _isCreateButtonEnabled;
            set 
            {
                _isCreateButtonEnabled = value; 
                OnPropertyChanged();
            }
        }

        private bool _isDateCheckBoxChecked;
        public bool IsDateCheckBoxChecked
        {
            get => _isDateCheckBoxChecked;
            set 
            {
                _isDateCheckBoxChecked = value;
                DatesToggled(_isDateCheckBoxChecked);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Category> Categories { get; set; }

        // Commands
        public ICommand CreateJobCommand { get; }
        public ICommand StartDateSelectedCommand { get; }
        public ICommand EndDateSelectedCommand { get; }

        public CreateJobViewModel()
        {
            Categories = new ObservableCollection<Category>();
            IsCreateButtonEnabled = false;

            // Command registration
            CreateJobCommand = new Command(CreateJob);
            StartDateSelectedCommand = new Command<DateTime>(StartDateSelected);
            EndDateSelectedCommand = new Command<DateTime>(EndDateSelected);
        }

        public async Task LoadCategories()
        {
            Categories.Clear();

            await _categoryRepo.LoadAllAsync();

            foreach (Category category in _categoryRepo.GetAll())
            {
                Categories.Add(category);
            }
        }

        public void DatesToggled(bool isChecked)
        {
            IsDateCheckBoxChecked = isChecked;
            if (isChecked)
            {
                StartDate = DateTime.Now;
                EndDate = DateTime.Now;
            }
            else
            { 
                StartDate = null;
                EndDate = null;
            }
        }

        private void EnableCreateButton()
        {
            IsCreateButtonEnabled = !string.IsNullOrWhiteSpace(ContactName)
                && !string.IsNullOrWhiteSpace(Address)
                && !string.IsNullOrWhiteSpace(PhoneNumber)
                && Category != null;
        }

        // Resets the user inputs on the page
        private void ResetInputs()
        {
            ContactName = string.Empty;
            PhoneNumber = string.Empty;
            Address = string.Empty;
            Category = null;
            IsDateCheckBoxChecked = false;
            StartDate = null;
            EndDate = null;
            Notes = string.Empty;
        }

        private async void CreateJob()
        {
            try
            {
                await _alertService.DisplayAlertAsync("Opret Opgave", $"Oprettede opgaven \"{_contactName}, {_address}\"");
                dateCreated = DateTime.Now;
                await _jobRepo.AddAsync(ContactName, Address, PhoneNumber, Category, IsDateCheckBoxChecked, StartDate, EndDate, Notes, dateCreated);
                ResetInputs();
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
        private void StartDateSelected(DateTime selectedDate)
        {
            StartDate = selectedDate;
        }

        private void EndDateSelected(DateTime selectedDate)
        {
            EndDate = selectedDate;
        }
    }
}
