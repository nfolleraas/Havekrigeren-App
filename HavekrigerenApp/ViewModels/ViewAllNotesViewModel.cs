using HavekrigerenApp.Models;
using HavekrigerenApp.Persistance;
using HavekrigerenApp.Services;
using HavekrigerenApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HavekrigerenApp.ViewModels
{
    public class ViewAllNotesViewModel : BaseViewModel
    {
        public ObservableCollection<NoteViewModel> NotesVM { get; set; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public List<string> SortingChoices { get; set; } = new List<string>()
        {
            "Nyeste",
            "Ældste",
            "Alfabet (A-Å)",
            "Alfabet (Å-A)"
        };

        private string _selectedSortingChoice ;
        public string SelectedSortingChoice
        {
            get { return _selectedSortingChoice; }
            set
            {
                _selectedSortingChoice = value;
                switch (_selectedSortingChoice)
                {
                    case "Nyeste":
                        SortNotesBy(note => note.DateCreated);
                        break;
                    case "Ældste":
                        SortNotesBy(note => note.DateCreated, isDescending: true);
                        break;
                    case "Alfabet (A-Å)":
                        SortNotesBy(note => note.Title);
                        break;
                    case "Alfabet (Å-A)":
                        SortNotesBy(note => note.Title, isDescending: true);
                        break;
                }       
            }
        }


        public ICommand RefreshCommand { get; }
        public ICommand NoteClickedCommand { get; }

        public ViewAllNotesViewModel()
        {
            NotesVM = new ObservableCollection<NoteViewModel>();

            // Command registration
            RefreshCommand = new Command(RefreshPage);
            NoteClickedCommand = new Command<NoteViewModel>(NoteClicked);
        }

        public void LoadNotes()
        {
            NotesVM.Clear();

            foreach (Note note in NoteRepository.GetAll())
            {
                NoteViewModel noteVM = new NoteViewModel(note);
                NotesVM.Add(noteVM);
            }
        }

        private void SortNotesBy(Func<NoteViewModel, object> sortBy, bool isDescending = false)
        {
            List<NoteViewModel> sorted = (isDescending ? NotesVM.OrderByDescending(sortBy) : NotesVM.OrderBy(sortBy)).ToList();

            NotesVM = new ObservableCollection<NoteViewModel>(sorted);
        }

        // Commands
        private void RefreshPage()
        {
            try
            {
                IsRefreshing = true;
                LoadNotes();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async void NoteClicked(NoteViewModel selectedNote)
        {
            try
            {
                await NavigationService.PushAsync(new ViewNotePage(selectedNote));
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
    }
}
