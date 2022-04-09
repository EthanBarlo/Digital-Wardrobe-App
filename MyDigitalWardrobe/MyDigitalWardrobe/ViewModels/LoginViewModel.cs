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
        public ICommand AttemptLogin { get; private set; }
        public ICommand GoToRegister { get; private set; }

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
            AttemptLogin = new Command(AttemptLoginCommand);
            GoToRegister = new AsyncCommand(GoToRegisterCommand);
        }
        
        private async Task GoToRegisterCommand()
        {
            await Xamarin.Forms.Shell.Current.Navigation.PushModalAsync(new Register());
        }

        private async void AttemptLoginCommand()
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
