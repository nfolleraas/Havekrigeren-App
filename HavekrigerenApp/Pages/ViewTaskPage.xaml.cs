namespace HavekrigerenApp.Pages {

    public partial class ViewTaskPage : ContentPage
    {
        private Task taskInfo;
        public ViewTaskPage(Task taskInfo)
        {
            InitializeComponent();

            this.taskInfo = taskInfo;

            DisplayTaskInfo();
        }

        
        private void DisplayTaskInfo()
        {
            contactNameEditor.Text = taskInfo.ContactName;
            addressEditor.Text = taskInfo.Address;
            phoneNumberEditor.Text = $"(+45) {taskInfo.PhoneNumber}";
            categoryEditor.Text = taskInfo.Category;
            dateEditor.Text = taskInfo.Date.ToString("dd/MM-yyyy");
            notesEditor.Text = taskInfo.Notes;
        } 
    }
}