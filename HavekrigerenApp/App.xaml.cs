using HavekrigerenApp.Models.Classes;
using System.Globalization;

namespace HavekrigerenApp
{
    public partial class App : Application
    {
        public CultureInfo Culture { get; set; } = new CultureInfo("da-DK");

        public App()
        {
            InitializeComponent();

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

            //CategoryRepository categoryRepo = new CategoryRepository();
            //await categoryRepo.AddAsync("Kategori");
            //JobRepository jobRepo = new JobRepository();
            //await jobRepo.AddAsync("Jens Jensen", "12345678", "Vej 1", "Kategori", "12/3-1234", "14/3-1234");
            //await jobRepo.AddAsync("Jens Jensen", "12345678", "Vej 1", "Kategori", "12/3-1234", "14/3-1234");
            //await jobRepo.AddAsync("Jens Jensen", "12345678", "Vej 1", "Kategori", "12/3-1234", "14/3-1234");

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
