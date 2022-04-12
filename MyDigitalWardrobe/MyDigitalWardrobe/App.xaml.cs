using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using MyDigitalWardrobe.Views;
using Xamarin.Essentials;
using MyDigitalWardrobe.Services;

namespace MyDigitalWardrobe
{
    public partial class App : Application
    {
        private static SQLiteAsyncConnection databaseConnection;
        public static SQLiteAsyncConnection Database
        {
            get
            {
                if (databaseConnection == null && !string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")))
                {
                    databaseConnection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, $"{FireBaseService.CurrentUserInformation.User.Email}.db3"));
                }
                return databaseConnection;
            }
        }

        public App()
        {
            InitializeComponent();
            Plugin.Media.CrossMedia.Current.Initialize();
            MainPage = new AppShell();
        }
        
        protected override async void OnStart()
        {
            var result = await FireBaseService.RefreshAuthTokenAsync();
            if (result.Status == FireBaseService.Status.Success)
            {
                await Shell.Current.GoToAsync($"//{nameof(AddItem)}");
                return;
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
