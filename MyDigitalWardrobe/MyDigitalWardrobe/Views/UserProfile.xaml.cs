using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace MyDigitalWardrobe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfile : ContentPage
    {
        private readonly string authKey = "AIzaSyB7wv7xyG148I_XUDv8OvNn-iQE1fjpbp4";
        public UserProfile()
        {
            InitializeComponent();
            GetProfileUserInformationAndRefreshToken();
        }

        private async void GetProfileUserInformationAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));
            try
            {
                var savedAuth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedAuth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                username.Text = savedAuth.User.Email;
            }
            catch (FirebaseAuthException ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Your login session has expired", "Ok");
            }
        }

        private async void logout_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            App.Current.MainPage = new AuthenticationPage();
        }
    }
}