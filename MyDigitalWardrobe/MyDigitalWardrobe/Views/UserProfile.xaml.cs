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
            username.Text = FireBaseService.CurrentUserInformation.User.Email;
        }
        
        private async void logout_Clicked(object sender, EventArgs e)
        {
            FireBaseService.ClearAuth();
            await Shell.Current.GoToAsync("//Login");
        }
    }
}