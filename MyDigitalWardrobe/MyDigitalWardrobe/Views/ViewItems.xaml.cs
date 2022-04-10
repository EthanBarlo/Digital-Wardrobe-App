using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyDigitalWardrobe.Services;

namespace MyDigitalWardrobe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewItems : ContentPage
    {
        public ViewItems()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            itemsList.ItemsSource = await ItemService.GetItemsAsync();
        }
    }
}