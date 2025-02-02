using HavekrigerenApp.Models.Classes;
using HavekrigerenApp.Models.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

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

        private DateTime dateCreated;

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

        private bool _isCheckBoxChecked;
        public bool IsCheckBoxChecked
        {
            get => _isCheckBoxChecked;
            set 
            {
                _isCheckBoxChecked = value; 
                OnPropertyChanged(nameof(IsCheckBoxChecked));
            }
        }

        // Commands
        public ICommand CreateJobCommand { get; set; }
        public ICommand StartDateSelectedCommand { get; set; }
        public ICommand EndDateSelectedCommand { get; set; }

        public CreateJobViewModel()
        {
            _categories = new ObservableCollection<Category>();
            IsButtonEnabled = false;

            // Command registration
            CreateJobCommand = new Command(OnCreateJob);
            StartDateSelectedCommand = new Command<DateTime>(OnStartDateSelected);
            EndDateSelectedCommand = new Command<DateTime>(OnEndDateSelected);
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
            IsButtonEnabled = !string.IsNullOrWhiteSpace(_contactName)
                && !string.IsNullOrWhiteSpace(_address)
                && !string.IsNullOrWhiteSpace(_phoneNumber)
                && _category != null;
        }

        // Resets the user inputs on the page
        private void ResetInputs()
        {
            ContactName = string.Empty;
            PhoneNumber = string.Empty;
            Address = string.Empty;
            Category = null;
            IsCheckBoxChecked = false;
            StartDate = null;
            EndDate = null;
            Notes = string.Empty;
        }

        public void OnToggledDates(bool isChecked)
        {
            IsCheckBoxChecked = isChecked;
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

        // Commands
        private async void OnCreateJob()
        {
            try
            {

                await alertService.DisplayAlertAsync("Opret Opgave", $"Oprettede opgaven \"{_contactName}, {_address}\"");
                dateCreated = DateTime.Now;
                await jobRepo.AddAsync(_contactName, _address, _phoneNumber, _category, _isCheckBoxChecked, _startDate, _endDate, _notes, dateCreated);
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
        private void OnStartDateSelected(DateTime selectedDate)
        {
            StartDate = selectedDate;
        }

        private void OnEndDateSelected(DateTime selectedDate)
        {
            EndDate = selectedDate;
        }

        // Method for updating the UI on changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
