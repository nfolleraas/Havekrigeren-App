using HavekrigerenApp.Interfaces;

namespace HavekrigerenApp.Services
{
    public class AlertService : IAlertService
    {
        public async Task DisplayAlertAsync(string title, string message, string cancel = "OK")
        {
            Page currentPage = Shell.Current.CurrentPage;

            if (currentPage != null)
            {
                await currentPage.DisplayAlert(title, message, cancel);
            }
            else
            {
                throw new InvalidOperationException("Siden du prøver at tilgå er ikke tilgængelig.");
            }
        }
        public async Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
        {
            Page currentPage = Shell.Current.CurrentPage;

            if (currentPage != null)
            {
                return await currentPage.DisplayAlert(title, message, accept, cancel);
            }
            else
            {
                throw new InvalidOperationException("Siden du prøver at tilgå er ikke tilgængelig.");
            }
        }

        public async Task<string> DisplayPromptAsync(string title, string message, string accept = "OK", string cancel = "Annuller", string? placeholder = null, int maxLength = -1, Keyboard? keyboard = null, string initialValue = "")
        {
            Page currentPage = Shell.Current.CurrentPage;

            if (currentPage != null)
            {
                return await currentPage.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard, initialValue);
            }
            else
            {
                throw new InvalidOperationException("Siden du prøver at tilgå er ikke tilgængelig.");
            }
        }

        public async Task<string> DisplayActionSheetAsync(string title, string cancel, string destruction, string[] buttons)
        {
            Page currentPage = Shell.Current.CurrentPage;

            if (currentPage != null)
            {
                return await currentPage.DisplayActionSheet(title, cancel, destruction, buttons);
            }
            else
            {
                throw new InvalidOperationException("Siden du prøver at tilgå er ikke tilgængelig.");
            }
        }
    }
}