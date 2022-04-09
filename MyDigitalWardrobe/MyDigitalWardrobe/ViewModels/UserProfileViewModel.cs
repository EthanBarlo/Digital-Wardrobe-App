using MvvmHelpers.Commands;
using System.Windows.Input;
using MyDigitalWardrobe.Services;

namespace MyDigitalWardrobe.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
        public ICommand LogoutAction { get; private set; }

        public string UserName
        {
            get => FireBaseService.CurrentUserInformation.User.Email;
        }

        public UserProfileViewModel()
        {
            LogoutAction = new Command(LogoutCommand);
            Title = "Welcome to your Digital Wardrobe!";
        }

        private async void LogoutCommand()
        {
            FireBaseService.ClearAuth();
            await Xamarin.Forms.Shell.Current.GoToAsync("//Login");
        }
    }
}
