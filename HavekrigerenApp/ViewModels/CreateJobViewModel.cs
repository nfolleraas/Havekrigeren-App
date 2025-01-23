using HavekrigerenApp.Models.Classes;
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
        private NavigationService navigationService = new NavigationService();
        private DatabaseRepository databaseRepo = new DatabaseRepository();

        private ObservableCollection<string> _categories;

        public ObservableCollection<string> Categories
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

        private Category _category;
        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
                //EnableCreateButton();
                OnPropertyChanged(nameof(Category));
                Console.WriteLine(Category);
            }
        }

        private DateTime _startDate = DateTime.Today;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate
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


        // Commands
        public ICommand CreateJobCmd { get; set; }
        public ICommand StartDateSelectedCmd { get; set; }
        public ICommand EndDateSelectedCmd { get; set; }

        public CreateJobViewModel()
        {
            _categories = new ObservableCollection<string>();
            _isButtonEnabled = false;
            LoadCategories();

            // Command registration
            CreateJobCmd = new Command(CreateJob);
            StartDateSelectedCmd = new Command<DateTime>(StartDateSelected);
            EndDateSelectedCmd = new Command<DateTime>(EndDateSelected);
        }

        private async Task LoadCategories()
        {
            await categoryRepo.LoadAllAsync();

            foreach (Category category in categoryRepo.GetAll())
            {
                _categories.Add(category.Name);
            }
        }

        private void EnableCreateButton()
        {
            _isButtonEnabled = !string.IsNullOrWhiteSpace(_contactName)
                && !string.IsNullOrWhiteSpace(_address)
                && !string.IsNullOrWhiteSpace(_phoneNumber);
                //&& !string.IsNullOrWhiteSpace(_category.ToString());

            OnPropertyChanged(nameof(IsButtonEnabled));
        }

        private void StartDateSelected(DateTime selectedDate)
        {
            _startDate = selectedDate;
        }

        private void EndDateSelected(DateTime selectedDate)
        {
            _endDate = selectedDate;
        }

        private async void CreateJob()
        {
            try
            {
                await alertService.DisplayAlertAsync("Opret Opgave", $"Oprettede opgaven \"{_contactName}, {_address}\"");
                Console.WriteLine(ContactName + " " + PhoneNumber + " " + Address + " " + Category + " " + StartDate + " " + EndDate + " " + Notes);
                jobRepo.AddAsync(ContactName, PhoneNumber, Address, Category.Name, StartDate.ToString(), EndDate.ToString(), Notes);
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

        // Method for updating the UI on changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
