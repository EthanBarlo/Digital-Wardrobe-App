using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyDigitalWardrobe.Services;

namespace MyDigitalWardrobe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfile : ContentPage
    {
        public UserProfile()
        {
            InitializeComponent();
            GetProfileUserInformationAndRefreshToken();
        }

        private async void GetProfileUserInformationAndRefreshToken()
        {
            var result = await FireBaseService.RefreshAuthTokenAsync();
            if(result.Status == FireBaseService.Status.Error)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Your login session has expired", "Ok");
                await Shell.Current.GoToAsync("//Login");
                return;
            }
            else
                username.Text = FireBaseService.CurrentUserInformation.User.Email;
        }
        
        private async void logout_Clicked(object sender, EventArgs e)
        {
            await FireBaseService.ClearAuth();
            await Shell.Current.GoToAsync("//Login");
        }
    }
}