using System.Windows.Input;
using MvvmHelpers.Commands;
using MyDigitalWardrobe.Services;
using MyDigitalWardrobe.Views;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace MyDigitalWardrobe.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand AttemptLoginCommand { get; private set; }
        public ICommand GoToRegisterCommand { get; private set; }

        public string Email { get; set; }
        public string Password { get; set; }
        private string errorMessage = "";
        public string ErrorMessage 
        { 
            get => errorMessage; 
            set => SetProperty(ref errorMessage, value);
        }

        public LoginViewModel()
        {
            AttemptLoginCommand = new Command(AttemptLogin);
            GoToRegisterCommand = new Command(GoToRegister);
        }

        private async void GoToRegister()
        {
            await Xamarin.Forms.Shell.Current.Navigation.PushModalAsync(new Register());
        }

        private async void AttemptLogin()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter an email and Password";
                return;
            }

            var result = await FireBaseService.LoginWithCredentialsAsync(Email.Trim(), Password);

            if (result.Status == FireBaseService.Status.Success)
                await Xamarin.Forms.Shell.Current.GoToAsync($"//{nameof(ViewItems)}");
            else
                ErrorMessage = result.ErrorMessage;
        }
    }
}
