using System.Windows.Input;
using Xamarin.Forms;
using MyDigitalWardrobe.Services;
using Xamarin.Essentials;
using MyDigitalWardrobe.Views;

namespace MyDigitalWardrobe.ViewModels
{
    public class RegisterViewModel
    {
        public ICommand AttemptRegister { get; private set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ErrorMessage { get; set; }

        public RegisterViewModel()
        {
            AttemptRegister = new Command(AttemptRegisterCommand);
        }

        private async void AttemptRegisterCommand()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorMessage = "Please enter all required fields.";
                return;
            }

            if (!Password.Equals(ConfirmPassword))
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            var result = await FireBaseService.RegisterNewUserAsync(Email.Trim(), Password);

            if (result.Status == FireBaseService.Status.Success)
            {
                ErrorMessage = "Registration Successful";
                await Shell.Current.GoToAsync($"//{nameof(ViewItems)}");
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
                //await App.Current.MainPage.DisplayAlert("Error", result.ErrorMessage, "Ok");
            }
        }
    }
}
