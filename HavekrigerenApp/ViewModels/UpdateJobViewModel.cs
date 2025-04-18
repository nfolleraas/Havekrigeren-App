using HavekrigerenApp.Models;
using HavekrigerenApp.Persistance;
using HavekrigerenApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HavekrigerenApp.ViewModels
{
    public class UpdateJobViewModel : BaseViewModel
    {
        public JobViewModel SelectedJobVM { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        private string _contactName;
        public string ContactName
        {
            get => _contactName;
            set
            {
                _contactName = value;
                EnableSaveButton();
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
                EnableSaveButton();
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
                EnableSaveButton();
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
                EnableSaveButton();
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

        private bool _isSaveButtonEnabled;
        public bool IsSaveButtonEnabled
        {
            get => _isSaveButtonEnabled;
            set
            {
                _isSaveButtonEnabled = value;
                OnPropertyChanged();

                if (UpdateJobCommand is Command command)
                {
                    command.ChangeCanExecute();
                }
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

        public ICommand UpdateJobCommand { get; }
        public ICommand StartDateSelectedCommand { get; }
        public ICommand EndDateSelectedCommand { get; }

        public UpdateJobViewModel(JobViewModel jobVM)
        {
            Categories = new ObservableCollection<Category>();

            SelectedJobVM = jobVM;
            ContactName = jobVM.ContactName;
            Address = jobVM.Address;
            PhoneNumber = jobVM.PhoneNumber.Replace(" ", "");
            Category = jobVM.Category;
            IsDateCheckBoxChecked = jobVM.HasDate;
            StartDate = jobVM.StartDate;
            EndDate = jobVM.EndDate;
            Notes = jobVM.Notes;

            UpdateJobCommand = new Command<JobViewModel>(UpdateJob, job => IsSaveButtonEnabled);
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

            if (SelectedJobVM?.Category is not null)
            {
                Category = Categories.FirstOrDefault(category => category.Id == SelectedJobVM.Category.Id);
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

        private void EnableSaveButton()
        {
            IsSaveButtonEnabled = !string.IsNullOrWhiteSpace(ContactName)
                && !string.IsNullOrWhiteSpace(Address)
                && PhoneNumber?.Length == 8
                && Category != null;
        }

        private async void UpdateJob(JobViewModel jobVM)
        {
            try
            {
                jobVM.Id = SelectedJobVM.Id;
                jobVM.ContactName = ContactName;
                jobVM.Address = Address;
                jobVM.PhoneNumber = PhoneNumber;
                jobVM.Category = Category;
                jobVM.HasDate = IsDateCheckBoxChecked;
                jobVM.Job.StartDate = StartDate;
                jobVM.Job.EndDate = EndDate;
                jobVM.Job.Notes = Notes;

                JobRepository.Update(jobVM.Job);

                await AlertService.DisplayAlertAsync("Opdater Opgave", $"Opdaterede opgaven \"{ContactName}, {Address}\"");
                await NavigationService.PopAsync();
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
