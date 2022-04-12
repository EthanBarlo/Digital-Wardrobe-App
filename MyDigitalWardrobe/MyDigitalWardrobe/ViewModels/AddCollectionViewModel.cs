using MvvmHelpers.Commands;
using System.Windows.Input;
using MyDigitalWardrobe.Services;
using MyDigitalWardrobe.Models;

namespace MyDigitalWardrobe.ViewModels
{
    public class AddCollectionViewModel
    {
        public string Name { get; set; }
        public ICommand CreateCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public AddCollectionViewModel()
        {
            CreateCommand = new Command(CreateCollection);
            CloseCommand = new Command(Close);
        }

        private async void CreateCollection()
        {
            await CollectionService.SaveCollectionAsync(new Collection { Name = this.Name });
            Close();
        }
        private async void Close()
        {
            await Xamarin.Forms.Shell.Current.Navigation.PopModalAsync();
        }
    }
}
