namespace HavekrigerenApp.Pages {

    public partial class ViewTaskPage : ContentPage
    {
        private TaskManager taskInfo;
        public ViewTaskPage(TaskManager taskInfo)
        {
            InitializeComponent();

            this.taskInfo = taskInfo;

            DisplayTaskInfo();
        }

        
        private void DisplayTaskInfo()
        {
            contactPersonEditor.Text = taskInfo.ContactName;
            addressEditor.Text = taskInfo.Address;
            phoneNumberEditor.Text = $"(+45) {taskInfo.PhoneNumber}";
            categoryEditor.Text = taskInfo.Category;
            dateEditor.Text = taskInfo.Date;
            notesEditor.Text = taskInfo.Notes;

            /*
            Frame LabeledFrame(string labelText)
            {
                return new Frame
                {
                    CornerRadius = 10,
                    Padding = new Thickness(10),
                    HasShadow = false,
                    Margin = new Thickness(0, 5),
                    Content = new Label
                    {
                        Text = labelText,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Start,
                        LineBreakMode = LineBreakMode.CharacterWrap,
                        MaximumWidthRequest = 300
                    }
                };
            }

            taskLayout.Children.Add(LabeledFrame(taskInfo.ContactName));
            taskLayout.Children.Add(LabeledFrame(taskInfo.Address));
            taskLayout.Children.Add(LabeledFrame($"{taskInfo.PhoneNumber}"));
            taskLayout.Children.Add(LabeledFrame(taskInfo.Category));
            taskLayout.Children.Add(LabeledFrame(taskInfo.Date));
            taskLayout.Children.Add(LabeledFrame(taskInfo.Notes));
            */
        }
        
    }
}