using HavekrigerenApp.Models.Classes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace HavekrigerenApp.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private JobRepository jobRepo = new JobRepository();

        private ObservableCollection<string> _searchResults;
        public ObservableCollection<string> SearchResults
        {
            get => _searchResults;

            set 
            { 
                _searchResults = value;
                OnPropertyChanged(nameof(SearchResults));
            }
        }


        // Commands
        public ICommand PerformSearchCommand { get; set; }

        public HomeViewModel()
        {
            LoadJobs();

            // Command registration
            PerformSearchCommand = new Command<string>(PerformSearch);
        }

        private async void LoadJobs()
        {
            await jobRepo.LoadAllAsync();
        }

        // Commands
        private void PerformSearch(string contactName)
        {
            ObservableCollection<string> result = new ObservableCollection<string>(jobRepo.Get(contactName));
            if (result == null)
            {
                Console.WriteLine("No Result");
                return;
            }

            SearchResults = result;
        }

        // Method for updating the UI on changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
