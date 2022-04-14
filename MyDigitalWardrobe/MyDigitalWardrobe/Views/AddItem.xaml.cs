using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyDigitalWardrobe.Models;
using MyDigitalWardrobe.ViewModels;

namespace MyDigitalWardrobe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItem : ContentPage
    {
        public AddItem()
        {
            InitializeComponent();
            BindingContext = new AddItemViewModel();
        }
        public AddItem(Item item)
        {
            InitializeComponent();
            BindingContext = new AddItemViewModel(item);
        }
    }
}