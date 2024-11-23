using HavekrigerenApp.Models;
using System.Globalization;

namespace HavekrigerenApp
{
    public partial class App : Application
    {
        public static CultureInfo Culture { get; set; } = new CultureInfo("da-DK");
        private static CategoryRepository categoryRepo;

        public App()
        {
            InitializeComponent();

            categoryRepo = new CategoryRepository();

            MainPage = new AppShell();
        }


        public bool IsUserLoggedIn()
        {
            return Preferences.Default.Get("IsLoggedIn", false);
        }

        protected async override void OnStart()
        {
            base.OnStart();

            Console.WriteLine("App is starting up");

            if (IsUserLoggedIn())
            {
                await Shell.Current.GoToAsync("///HomePage");
            }
            else
            {
                await Shell.Current.GoToAsync("///LoginPage");
            }
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
