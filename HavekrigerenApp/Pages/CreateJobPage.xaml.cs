using System.Runtime.CompilerServices;

namespace HavekrigerenApp.Pages
{

	public partial class CreateJobPage : ContentPage
	{
		public CreateJobPage()
		{
			InitializeComponent();

            LoadCategories();

            categoryPicker.SelectedIndexChanged += DisplayCategoryPicker;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ClearInputs();
        }

        private async void LoadCategories()
        {
            var categories = await Database.GetDocuments<Category>("Categories");

            foreach (Category category in categories)
            {
                categoryPicker.Items.Add(category.CategoryName);
            }
        }
        private void DisplayCategoryPicker(object sender, EventArgs e)
        {
            if (categoryPicker.SelectedIndex != -1)
            {
                string selectedCategory = categoryPicker.Items[categoryPicker.SelectedIndex];
                Console.WriteLine($"Selected Category: {selectedCategory}");
            }
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
                categoryPicker.SelectedIndex != -1 )
            {
                createJobButton.IsEnabled = true;
            }
            else
            {
                createJobButton.IsEnabled = false;
            }
        }

        private void ClearInputs()
        {
            contactNameEntry.Text = "";
            addressEntry.Text = "";
            phoneNumberEntry.Text = "";
            categoryPicker.SelectedItem = "";
            datePicker.Date = DateTime.Now;
            notesEditor.Text = "";
        }

        private async void OnCreateJobClicked(object sender, EventArgs e)
        {
            string contactName = contactNameEntry.Text;
            string address = addressEntry.Text;
            string phoneNumber = phoneNumberEntry.Text;
            string category = categoryPicker.SelectedItem.ToString() ?? "";
            DateOnly date = DateOnly.FromDateTime(datePicker.Date);
            string notes = notesEditor.Text;

            if (phoneNumber.Length != 8)
            {
                ClearInputs();

                await DisplayAlert("Fejl", "Telefonnummer skal v�re 8 tal", "OK");
                return;
            }

            // Add inputs to dictionary
            Dictionary<string, object> jobData = new Dictionary<string, object>()
            {
                {"contactName", contactName},
                {"address", address},
                {"phoneNumber", phoneNumber},
                {"category", category},
                {"date", date.ToString("dd/MM-yyyy")},
                {"notes", notes}
            };

            // Add job to database
            Database.AddDocument("Jobs", jobData);

            ClearInputs();

            await DisplayAlert("Opret Opgave", $"Oprettede opgave: \"{contactName} p� {address}\".", "OK");
        }
    }
}