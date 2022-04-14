using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MvvmHelpers.Commands;
using MyDigitalWardrobe.Models;
using MyDigitalWardrobe.Services;
using Xamarin.Essentials;

namespace MyDigitalWardrobe.ViewModels
{
    public class ItemViewerViewModel : ViewModelBase
    {
        private Item _item;
        public ICommand ShowPurchasedLocationCommand { get; set; }
        
        public string Name { get; set; }
        private string collectionName;
        public string CollectionName {
            get => collectionName;
            set => SetProperty(ref collectionName, value);
        }
        public decimal Price { get; set; }
        public string ItemImage { get; set; }
        public string PurchasedName { get; set; }
        public DateTime DatePurchased { get; set; }
        public DateTime WarrantyEnd { get; set; }
        public string RecieptImage { get; set; }


        private string showLocationError;
        public string ShowLocationError
        {
            get => showLocationError;
            set => SetProperty(ref showLocationError, value);
        }


        public ItemViewerViewModel(Item item)
        {
            _item = item;
            this.Name = item.Name;
            this.Price = item.Price;
            this.ItemImage = item.ItemImage;
            this.PurchasedName = item.PurchasedName;
            this.DatePurchased = item.DatePurchased;
            this.WarrantyEnd = item.WarrantyEnd;
            this.RecieptImage = item.RecieptImage;
            GetCollectionName();
            ShowPurchasedLocationCommand = new Command(ShowPurchasedLocation);
        }
        
        private async void GetCollectionName()
        {
            this.CollectionName = (await CollectionService.GetCollectionFromItemAsync(_item)).Name;            
        }

        private async void ShowPurchasedLocation()
        {
            try
            {
                var placemarks = await Geocoding.GetLocationsAsync(PurchasedName);
                var firstLocation = placemarks?.FirstOrDefault();
                if (firstLocation != null)
                {
                    await Map.OpenAsync(firstLocation.Latitude, firstLocation.Longitude, new MapLaunchOptions { Name = PurchasedName });
                }
                else
                    throw new Exception("Location not found");
            }
            catch(Exception e)
            {
                ShowLocationError = "The location was unable to be opened";
            }
        }
    }
}
