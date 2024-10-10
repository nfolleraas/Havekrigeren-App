using System.Runtime.CompilerServices;

namespace HavekrigerenApp.Pages
{

	public partial class CreateTaskPage : ContentPage
	{
		public CreateTaskPage()
		{
			InitializeComponent();

        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            DateTime selectedDate = e.NewDate;
        }

		private void OnTextChanged(object sender, EventArgs e)
		{
            if (!string.IsNullOrEmpty(contactNameEntry.Text) && 
                !string.IsNullOrEmpty(addressEntry.Text) &&
                !string.IsNullOrEmpty(phoneNumberEntry.Text) &&
                !string.IsNullOrEmpty(categoryEntry.Text) &&
                !string.IsNullOrEmpty(notesEditor.Text))
            {
                createTaskButton.IsEnabled = true;
            }
            else
            {
                createTaskButton.IsEnabled = false;
            }
        }

        private async void OnCreateTaskClicked(object sender, EventArgs e)
        {
            string contactName = contactNameEntry.Text;
            string address = addressEntry.Text;
            int phoneNumber = int.Parse(phoneNumberEntry.Text);
            string category = categoryEntry.Text;
            DateOnly date = DateOnly.FromDateTime(datePicker.Date);
            string notes = notesEditor.Text;

            Task.AddTask(contactName, address, phoneNumber, category, date, notes);

            contactNameEntry.Text = "";
            addressEntry.Text = "";
            phoneNumberEntry.Text = "";
            categoryEntry.Text = "";
            datePicker.Date = DateTime.Now;
            notesEditor.Text = "";

            await DisplayAlert("Opret Opgave", $"Oprettede opgave: \"{contactName} på {address}\". \nGenindlæs siden for at se ændringerne.", "OK");
        }
    }
}