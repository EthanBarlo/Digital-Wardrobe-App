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
        }
        public AddItem(Item item = null)
        {
            InitializeComponent();
            BindingContext = item == null ? new AddItemViewModel() : new AddItemViewModel(item);
        }
    }
}