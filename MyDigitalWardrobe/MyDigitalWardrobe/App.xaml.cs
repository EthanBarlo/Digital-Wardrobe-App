using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using MyDigitalWardrobe.Views;
using Xamarin.Essentials;

namespace MyDigitalWardrobe
{
    public partial class App : Application
    {
        private static SQLiteAsyncConnection databaseConnection;
        public static SQLiteAsyncConnection Database
        {
            get
            {
                if (databaseConnection == null)
                {
                    databaseConnection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "database.db"));
                }
                return databaseConnection;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
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
