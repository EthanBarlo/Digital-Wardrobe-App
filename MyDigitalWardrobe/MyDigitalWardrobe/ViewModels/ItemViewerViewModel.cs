using System;
using System.Collections.Generic;
using System.Text;
using MyDigitalWardrobe.Models;
using MyDigitalWardrobe.Services;

namespace MyDigitalWardrobe.ViewModels
{
    public class ItemViewerViewModel : ViewModelBase
    {
        private Item _item;
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
        }
        
        private async void GetCollectionName()
        {
            this.CollectionName = (await CollectionService.GetCollectionFromItemAsync(_item)).Name;            
        }
    }
}
