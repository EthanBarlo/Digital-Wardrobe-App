using System.Windows.Input;
using MvvmHelpers.Commands;
using MyDigitalWardrobe.Services;
using MyDigitalWardrobe.Views;

namespace MyDigitalWardrobe.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public ICommand AttemptRegister { get; private set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        private string errorMessage = "";
        public string ErrorMessage 
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

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
                await Xamarin.Forms.Shell.Current.GoToAsync($"//{nameof(ViewItems)}");
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }
    }
}
