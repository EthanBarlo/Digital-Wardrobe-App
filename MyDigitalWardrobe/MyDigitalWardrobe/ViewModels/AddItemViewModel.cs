using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using MyDigitalWardrobe.Models;
using MyDigitalWardrobe.Interfaces;
using MyDigitalWardrobe.Services;
using System.IO;
using MyDigitalWardrobe.Views;
using MvvmHelpers.Commands;
using Command = MvvmHelpers.Commands.Command;
using System.Linq;

namespace MyDigitalWardrobe.ViewModels
{
    public class AddItemViewModel : ViewModelBase
    {
        private bool isEditingItem = false;
        public ICommand SelectItemImage { get; set; }
        public ICommand TakeItemImage { get; set; }
        public ICommand SelectRecieptImage { get; set; }
        public ICommand TakeRecieptImage { get; set; }
        public ICommand AddItem { get; set; }
        public ICommand CreateCollectionCommand { get; set; }
        public ICommand RefreshCollectionCommand { get; set; }
        public ICommand GetCurrentLocationCommand { get; set; }

        private Item _item;
        private string name;
        public string Name
        {
            get => name;
            set {
                name = value;
                OnPropertyChanged();
            }
        }
        private decimal price;
        public decimal Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }
        private Collection collection;
        public Collection Collection
        {
            get => collection;
            set
            {
                collection = value;
                OnPropertyChanged();
            }
        }
        private string purchasedName;
        public string PurchasedName
        {
            get => purchasedName;
            set
            {
                purchasedName = value;
                OnPropertyChanged();
            }
        }
        private DateTime datePurchased;
        public DateTime DatePurchased
        {
            get => datePurchased;
            set
            {
                datePurchased = value;
                OnPropertyChanged();
            }
        }
        private DateTime warrantyEnd;
        public DateTime WarrantyEnd
        {
            get => warrantyEnd;
            set
            {
                warrantyEnd = value;
                OnPropertyChanged();
            }
        }


        private string itemImage;
        public string ItemImage {
            get => itemImage;
            set{
                itemImage = value;
                OnPropertyChanged();
            }
        }
        private string recieptImage;
        public string RecieptImage { 
            get => recieptImage;
            set
            {
                recieptImage = value;
                OnPropertyChanged();
            }
        }
        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }
        private Stream itemImageStream { get; set; }
        private Stream recieptImageStream { get; set; }

        private List<Collection> collectionItems;
        public List<Collection> CollectionItems
        {
            get => collectionItems;
            set{
                collectionItems = value;
                OnPropertyChanged();
            }
        }
        private string locationErrorMessage;
        public string LocationErrorMessage
        {
            get => locationErrorMessage;
            set => SetProperty(ref locationErrorMessage, value);
        }

        public AddItemViewModel(Item item = null)
        {
            MapItemData(item == null ? new Item() : item);
            isEditingItem = item != null;
            Init();
        }

        private void Init()
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
            CreateCollectionCommand = new AsyncCommand(CreateCollection);
            RefreshCollectionCommand = new Command(RefreshCollections);
            GetCurrentLocationCommand = new Command(GetCurrentLocation);
            RefreshCollections();
        }
        
        private async void GetCurrentLocation()
        {
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest 
            { 
                DesiredAccuracy = GeolocationAccuracy.Medium, 
                Timeout = TimeSpan.FromSeconds(15) 
            });
            if (location == null)
            {
                LocationErrorMessage = "Unable to get your location.";
                return; 
            }

            var result = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
            if (result == null)
            {
                LocationErrorMessage = "Unable to get your location.";
                return;
            }
            var l = result.FirstOrDefault();
            PurchasedName = $"{l.FeatureName} {l.Thoroughfare} {l.Locality} {l.SubAdminArea} {l.PostalCode}";
        }
        private async void MapItemData(Item item)
        {
            _item = item;
            Name = item.Name;
            Price = item.Price;
            Collection = await CollectionService.GetCollectionFromItemAsync(item);
            PurchasedName = item.PurchasedName;
            DatePurchased = item.DatePurchased;
            WarrantyEnd = item.WarrantyEnd;
            ItemImage = item.ItemImage;
            RecieptImage = item.RecieptImage;
        }

        private async Task CreateCollection()
        {
            await Xamarin.Forms.Shell.Current.Navigation.PushModalAsync(new AddCollection());
        }
        private async void RefreshCollections()
        {
            CollectionItems = await CollectionService.GetCollectionsAsync();
        }


        private async Task<Stream> SelectPhoto()
        {
            var selectedImage = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select a Photo"
            });
            if (selectedImage == null)
                return null;
            return await selectedImage.OpenReadAsync();
        }
        
        private async Task<Stream> TakePhoto()
        {
            var selectedImage = await MediaPicker.CapturePhotoAsync();
            if (selectedImage == null)
                return null;
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
                Collection = this.Collection.ID,
                PurchasedName = this.PurchasedName,
                DatePurchased = this.DatePurchased,
                WarrantyEnd = this.WarrantyEnd,
                ItemImage = _item.ItemImage.Equals(ItemImage) ? ItemImage : HandleImage(ItemImage, "item"),
                RecieptImage = _item.RecieptImage.Equals(RecieptImage) ? RecieptImage : HandleImage(RecieptImage, "reciept"),
            };

            int rowsChanged;
            if(isEditingItem)
            {
                item.ID = _item.ID;
                rowsChanged = await ItemService.UpdateItemAsync(item);
                await Xamarin.Forms.Shell.Current.Navigation.PopModalAsync();
                return;
            }
            else
            {
                rowsChanged = await ItemService.SaveItemAsync(item);
            }
                
            if(rowsChanged == 0)
            {
                ErrorMessage = "Item could not be saved";
                return;
            }

            if(item.WarrantyEnd > DateTime.Now)
                NotificationService.ScheduleWarantyNotification(item.Name, item.WarrantyEnd, item.ID);


            MapItemData(new Item());

            

            string HandleImage(string imagePath, string type)
            {
                if (imagePath == null)
                    return null;
                var newImagePath = DependencyService.Get<IFileService>().GetImagePath(itemIdent:NewIdent, location:"Items");
                bool result = DependencyService.Get<IFileService>().MoveImageToStore(imagePath, newImagePath, $"{type}.jpg");
                newImagePath = Path.Combine(newImagePath, $"{type}.jpg");
                return newImagePath;
            }
        }
    }
}
