using System.Runtime;
using System.Text;
using HavekrigerenApp.Classes;

namespace HavekrigerenApp.Pages
{

    public partial class ViewJobPage : ContentPage
    {
        private Job jobInfo;
        public ViewJobPage(Job jobInfo)
        {
            InitializeComponent();

            this.jobInfo = jobInfo;

            DisplayJobInfo();
        }

        
        private void DisplayJobInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int i = 0;
            foreach (char c in jobInfo.PhoneNumber)
            {
                stringBuilder.AppendFormat("{0}{1}", c, (i++ & 1) == 0 ? "" : ' ');
            }
            jobInfo.PhoneNumber = stringBuilder.ToString().Trim();

            contactNameEditor.Text = jobInfo.ContactName;
            addressEditor.Text = jobInfo.Address;
            phoneNumberEditor.Text = $"(+45) {jobInfo.PhoneNumber}";
            categoryEditor.Text = jobInfo.Category;
            startDateEditor.Text = jobInfo.StartDate.ToString("dd/MM-yyyy");
            endDateEditor.Text = jobInfo.EndDate.ToString("dd/MM-yyyy");
            notesEditor.Text = jobInfo.Notes;
        }

        private void OnPhoneNumberClicked(object sender, EventArgs e)
        {
            if (PhoneDialer.Default.IsSupported)
            {
                PhoneDialer.Default.Open(jobInfo.PhoneNumber);
            }
            else
            {
                DisplayAlert("Fejl", "Din telefon understøtter ikke denne funktion.", "OK");
            }
        }
    }
}