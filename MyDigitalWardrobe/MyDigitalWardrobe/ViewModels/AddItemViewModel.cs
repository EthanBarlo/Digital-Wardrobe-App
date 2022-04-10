using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using MyDigitalWardrobe.Models;
using MyDigitalWardrobe.Interfaces;
using MyDigitalWardrobe.Services;
using System.IO;

namespace MyDigitalWardrobe.ViewModels
{
    public class AddItemViewModel : ViewModelBase
    {
        public ICommand SelectItemImage { get; set; }
        public ICommand TakeItemImage { get; set; }
        public ICommand SelectRecieptImage { get; set; }
        public ICommand TakeRecieptImage { get; set; }
        public ICommand AddItem { get; set; }
        
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PurchasedName { get; set; }
        public DateTime DatePurchased { get; set; }
        public DateTime WarrantyEnd { get; set; }
        public decimal PurchasedLongitude { get; set; }
        public decimal PurcahsedLatitude { get; set; }
        
        private string itemImage;
        public string ItemImage {
            get => itemImage;
            set{
                SetProperty(ref itemImage, value);
                OnPropertyChanged();
            }
        }
        private string recieptImage;
        public string RecieptImage { 
            get => recieptImage;
            set
            {
                SetProperty(ref recieptImage, value);
                OnPropertyChanged();
            } 
        }
        private Stream itemImageStream { get; set; }
        private Stream recieptImageStream { get; set; }


        public AddItemViewModel()
        {
            SelectItemImage = new Command(async () => { 
                itemImageStream = await SelectPhoto();
                ItemImage = SavePhoto("temp", "item", itemImageStream);
            });
            TakeItemImage = new Command(async () => { 
                itemImageStream = await TakePhoto();
                ItemImage = SavePhoto("temp", "item", itemImageStream);
            });
            SelectRecieptImage = new Command(async () => { 
                recieptImageStream = await SelectPhoto();
                RecieptImage = SavePhoto("temp", "reciept", recieptImageStream);
            });
            TakeRecieptImage = new Command(async () => { 
                recieptImageStream = await TakePhoto();
                RecieptImage = SavePhoto("temp", "reciept", recieptImageStream);
            });
            AddItem = new Command(AddItemCommand);
        }

        
        private async Task<Stream> SelectPhoto()
        {
            var selectedImage = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select a Photo"
            });
            return await selectedImage.OpenReadAsync();
        }
        
        private async Task<Stream> TakePhoto()
        {
            var selectedImage = await MediaPicker.CapturePhotoAsync();
            return await selectedImage.OpenReadAsync();
        }
        private string SavePhoto(string folder, string name, Stream imageStream)
        {
            if (imageStream == null)
                return null;
            DependencyService.Get<IFileService>().SavePicture($"{name}.jpg", imageStream, folder);
            return DependencyService.Get<IFileService>().GetImagePath(location:folder, name:name);
        }

        
        private async void AddItemCommand()
        {
            int NewIdent = await ItemService.GetNextItemID();

            var item = new Item
            {
                Name = this.Name,
                Price = this.Price,
                PurchasedName = this.PurchasedName,
                DatePurchased = this.DatePurchased,
                WarrantyEnd = this.WarrantyEnd,
                PurchasedLongitude = this.PurchasedLongitude,
                PurchasedLatitude = this.PurcahsedLatitude,
                ItemImage = HandleImage(ItemImage, "item"),
                RecieptImage = HandleImage(RecieptImage, "reciept"),
            };

            
            var rowsChanged = await ItemService.SaveItemAsync(item);
            if(rowsChanged == 0)
            {
                // TODO - Error for if it didnt upload
                return;
            }

            string HandleImage(string imagePath, string type)
            {
                var newImagePath = DependencyService.Get<IFileService>().GetImagePath(itemIdent:NewIdent, location:"Items");
                bool result = DependencyService.Get<IFileService>().MoveImageToStore(imagePath, newImagePath);
                newImagePath = Path.Combine(newImagePath, $"{type}.jpg");
                return newImagePath;
            }
        }
    }
}
