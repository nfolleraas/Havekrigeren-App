using HavekrigerenApp.Models.Classes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace HavekrigerenApp.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private JobRepository jobRepo = new JobRepository();

        private ObservableCollection<JobViewModel> _jobsVM;
        public ObservableCollection<JobViewModel> JobsVM
        {
            get => _jobsVM;
            set 
            { 
                _jobsVM = value;
                OnPropertyChanged(nameof(JobsVM));
            }
        }


        public HomeViewModel()
        {
            _jobsVM = new ObservableCollection<JobViewModel>();

            LoadJobs();
        }

        private async void LoadJobs()
        {
            await jobRepo.LoadAllAsync();

            _jobsVM.Clear();
            // Instatiate new JobViewModel for each job
            foreach (Job job in jobRepo.GetAll())
            {
                JobViewModel jobVM = new JobViewModel(job);
                _jobsVM.Add(jobVM);
            }
        }

        // Method for updating the UI on changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
