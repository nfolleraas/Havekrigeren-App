namespace HavekrigerenApp.Interfaces
{
    public interface IAlertService
    {
        Task DisplayAlertAsync(string title, string message, string cancel);

        Task<string> DisplayPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string? placeholder = default, int maxLength = -1, Keyboard? keyboard = default, string initialValue = "");

        Task<string> DisplayActionSheetAsync(string title, string cancel, string destruction, string[] buttons);
    }
}
