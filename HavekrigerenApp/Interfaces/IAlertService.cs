namespace HavekrigerenApp.Interfaces
{
    public interface IAlertService
    {
        Task DisplayAlert(string title, string message, string cancel);

        Task<string> DisplayPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string? placeholder = default, int maxLength = -1, Keyboard? keyboard = default, string initialValue = "");

    }
}
