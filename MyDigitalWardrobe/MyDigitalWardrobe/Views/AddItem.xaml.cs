using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDigitalWardrobe.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyDigitalWardrobe.Services;

namespace MyDigitalWardrobe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItem : ContentPage
    {
        public AddItem()
        {
            InitializeComponent();
        }

        private async void Btn_addItem(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name.Text)) return;
            if (string.IsNullOrWhiteSpace(price.Text)) return;
            if (string.IsNullOrWhiteSpace(locationPurchased.Text)) return;

            Item item = new Item()
            {
                Name = name.Text,
                Price = decimal.Parse(price.Text),
                LocationName = locationPurchased.Text,
                DatePurchased = datePurchased.Date,
                WarrantyEnd = warrantyEnd.Date,
            };

            await ItemService.SaveItemAsync(item);

            name.Text = "";
            price.Text = "";
            locationPurchased.Text = "";
            datePurchased.Date = DateTime.Now;
            warrantyEnd.Date = DateTime.Now;
        }
    }
}