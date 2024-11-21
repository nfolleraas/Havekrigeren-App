namespace HavekrigerenApp.Interfaces
{
    public interface IAlertService
    {
        Task ShowAlert(string title, string message, string cancel);
    }
}
