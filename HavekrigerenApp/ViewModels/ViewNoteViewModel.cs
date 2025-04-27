using HavekrigerenApp.Persistance;
using HavekrigerenApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HavekrigerenApp.ViewModels
{
    public class ViewNoteViewModel
    {
        public NoteViewModel SelectedNote { get; set; }

        public ICommand DeleteNoteCommand { get; }

        public ViewNoteViewModel(NoteViewModel selectedNote)
        {
            SelectedNote = selectedNote;

            DeleteNoteCommand = new Command<NoteViewModel>(DeleteNote);
        }

        private async void DeleteNote(NoteViewModel selectedNote)
        {
            try
            {
                bool answer = await AlertService.DisplayAlertAsync("Slet Note", $"Er du sikker på, du vil slette noten \"{selectedNote.Title}\"?\nDenne handling kan ikke fortrydes.", "Ja", "Nej");

                if (answer)
                {
                    // Delete the job
                    NoteRepository.Delete(selectedNote.Id);
                    await AlertService.DisplayAlertAsync("Slet Note", $"Noten \"{selectedNote.Title}\" blev slettet.");
                    await NavigationService.PopAsync();
                }
            }
            catch (ArgumentException ex)
            {
                await AlertService.DisplayAlertAsync("Fejl!", $"Fejlbesked:\n{ex.Message}");
            }
        }
    }
}
