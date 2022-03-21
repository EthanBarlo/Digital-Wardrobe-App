using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyDigitalWardrobe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthenticationPage : ContentPage
    {
        private readonly string authKey = "AIzaSyB7wv7xyG148I_XUDv8OvNn-iQE1fjpbp4";
        public AuthenticationPage()
        {
            InitializeComponent();
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

            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(registerEmail.Text.Trim(), registerPassword.Text);
                string myToken = auth.FirebaseToken;
                await App.Current.MainPage.DisplayAlert("Alert", myToken, "Ok");
            }
            catch (FirebaseAuthException ex)
            {
                registerResponse.Text = ex.Reason.ToString();
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

            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(registerEmail.Text.Trim(), registerPassword.Text);
                var content = await auth.GetFreshAuthAsync();
                var serialContent = JsonConvert.SerializeObject(content);
                Preferences.Set("MyFirebaseRefreshToken", serialContent);

                // TODO - Move user into the applicaion page
                //Shell.Current.CurrentItem = 

                await Navigation.PushAsync(new UserProfile());
            }
            catch (FirebaseAuthException ex)
            {
                loginResponse.Text = ex.Reason.ToString();
            }
        }
    }
}