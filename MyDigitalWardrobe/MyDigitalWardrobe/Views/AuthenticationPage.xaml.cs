using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyDigitalWardrobe.Services;

namespace MyDigitalWardrobe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthenticationPage : ContentPage
    {
        public AuthenticationPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")))
                return;
            await Shell.Current.GoToAsync($"//{nameof(ViewItems)}");
        }

        /// <summary>
        /// Registers a new user account using Firebase, will open an alert displaying the token.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Register_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(registerEmail.Text) || string.IsNullOrWhiteSpace(registerPassword.Text))
            {
                registerResponse.Text = "Please enter an email and Password";
                return;
            }

            var result = await FireBaseService.RegisterNewUserAsync(registerEmail.Text.Trim(), registerPassword.Text);

            if (result.Status == FireBaseService.Status.Success)
            {
                registerResponse.Text = "Registration Successful";
                Preferences.Set("MyFirebaseRefreshToken", result.AuthToken);
                await Shell.Current.GoToAsync($"//{nameof(ViewItems)}");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", result.ErrorMessage, "Ok");
            }
        }

        /// <summary>
        /// Will attempt to login with the users provided credentials
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Login_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(loginEmail.Text) || string.IsNullOrWhiteSpace(loginPassword.Text))
            {
                loginResponse.Text = "Please enter an email and Password";
                return;
            }

            var result = await FireBaseService.LoginWithCredentialsAsync(loginEmail.Text.Trim(), loginPassword.Text);

            if (result.Status == FireBaseService.Status.Success)
            {
                loginResponse.Text = "Login Successful";
                Preferences.Set("MyFirebaseRefreshToken", result.AuthToken);
                await Shell.Current.GoToAsync($"//{nameof(ViewItems)}");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", result.ErrorMessage, "Ok");
            }
        }
    }
}