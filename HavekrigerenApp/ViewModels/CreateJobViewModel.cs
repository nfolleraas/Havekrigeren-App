using HavekrigerenApp.Models;
using HavekrigerenApp.Persistance;
using HavekrigerenApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace HavekrigerenApp.ViewModels
{
    public class CreateJobViewModel : BaseViewModel
    {
        private DateTime dateCreated;
        public ObservableCollection<Category> Categories { get; set; }

        private string _contactName = string.Empty;
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

        private string _address = string.Empty;
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

        private string _phoneNumber = string.Empty;
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

        private Category? _category;
        public Category? Category
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

        private string? _notes;
        public string? Notes
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

        // Commands
        public ICommand CreateJobCommand { get; }
        public ICommand StartDateSelectedCommand { get; }
        public ICommand EndDateSelectedCommand { get; }

        public CreateJobViewModel()
        {
            Categories = new ObservableCollection<Category>();
            IsCreateButtonEnabled = false;

            // Command registration
            CreateJobCommand = new Command(CreateJob, () => IsCreateButtonEnabled);
            StartDateSelectedCommand = new Command<DateTime>(StartDateSelected);
            EndDateSelectedCommand = new Command<DateTime>(EndDateSelected);
        }

        public void LoadCategories()
        {
            Categories.Clear();

            foreach (Category category in CategoryRepository.GetAll())
            {
                Categories.Add(category);
            }
        }

        public void DatesToggled(bool isChecked)
        {
            if (isChecked)
            {
                StartDate = DateTime.Today;
                EndDate = DateTime.Today;
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
                && PhoneNumber?.Length == 8
                && Category != null;
        }

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
                await AlertService.DisplayAlertAsync("Opret Opgave", $"Oprettede opgaven \"{_contactName}, {_address}\"");

                Job newJob = new Job(ContactName, Address, PhoneNumber, Category, IsDateCheckBoxChecked, StartDate, EndDate, Notes, DateTime.Now);
                JobRepository.Add(newJob);
                ResetInputs();
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
