namespace HavekrigerenApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            base.OnStart();

            Console.WriteLine("App is starting up");
        }

        protected override void OnSleep()
        {
            base.OnSleep();

            Console.WriteLine("App is going to sleep");
        }

        protected override void OnResume()
        {
            base.OnResume();

            Console.WriteLine("App is resuming");
        }

    }
}
