using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyDigitalWardrobe.Views;
using Xamarin.Essentials;

namespace MyDigitalWardrobe
{
    public partial class App : Application
    {
        private static Database database;
        public static Database Database
        {
            get
            {
                if (database == null) 
                    database = new Database(Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData), "wardrobe.db3"));
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            if(string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")))
            {
                MainPage = new AuthenticationPage();
            }
            else
            {
                database = new Database(Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData), "wardrobe.db3"));
                MainPage = new AppShell();
            }

            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
