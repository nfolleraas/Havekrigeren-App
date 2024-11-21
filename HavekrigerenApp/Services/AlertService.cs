using HavekrigerenApp.Interfaces;

namespace HavekrigerenApp.Services
{
    public class AlertService : IAlertService
    {
        public async Task ShowAlert(string title, string message, string cancel)
        {
            Page currentPage = Shell.Current.CurrentPage;

            if (currentPage != null)
            {
                await currentPage.DisplayAlert(title, message, cancel);
            }
            else
            {
                throw new InvalidOperationException("Current page is not accessible.");
            }
        }
    }
}