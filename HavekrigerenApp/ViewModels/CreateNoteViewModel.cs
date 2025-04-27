using Google.Type;
using HavekrigerenApp.Models;
using HavekrigerenApp.Persistance;
using HavekrigerenApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace HavekrigerenApp.ViewModels
{
    public class CreateNoteViewModel : BaseViewModel
    {
        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                EnableCreateButton();
                OnPropertyChanged();
            }
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                EnableCreateButton();
                OnPropertyChanged();
            }
        }

        private bool _isCreateButtonEnabled;
        public bool IsCreateButtonEnabled
        {
            get { return _isCreateButtonEnabled; }
            set
            {
                _isCreateButtonEnabled = value;
                OnPropertyChanged();
            }
        }


        public ICommand CreateNoteCommand { get; }
        public CreateNoteViewModel()
        {
            CreateNoteCommand = new Command(CreateNote);
        }

        private void EnableCreateButton()
        {
            IsCreateButtonEnabled = !string.IsNullOrWhiteSpace(Title)
                && !string.IsNullOrWhiteSpace(Content);
        }

        private void ResetInputs()
        {
            Title = string.Empty;
            Content = string.Empty;
        }

        private async void CreateNote()
        {
            try
            {
                await AlertService.DisplayAlertAsync("Opret Note", $"Oprettede noten \"{Title}\"");

                Note newNote = new Note(Title, Content);
                NoteRepository.Add(newNote);

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
    }
}
