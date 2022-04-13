using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers.Commands;
using MyDigitalWardrobe.Models;
using MyDigitalWardrobe.Services;
using System.Linq;
using MvvmHelpers;

namespace MyDigitalWardrobe.ViewModels
{
    public class ViewItemsViewModel : ViewModelBase
    {
        public ICommand RefreshCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand AddToOutfitCommand { get; set; }

        private Item lastSelectedItem;
        private Item selectedItem;
        public Item SelectedItem
        {
            get => selectedItem;
            set
            {
                if(value != null)
                {
                    lastSelectedItem = value;
                    value = null;
                }
                selectedItem = value;
                OnPropertyChanged();
            }
        }
        private string NameOfCollection { get; set; }
        private bool isBusy;
        public bool IsBusy {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        private List<Item> itemList;
        public List<Item> ItemList
        {
            get => itemList;
            set => SetProperty(ref itemList, value);
        }

        private List<Grouping<string, Item>> groupedItems;
        public List<Grouping<string, Item>> GroupedItems
        {
            get => groupedItems;
            set => SetProperty(ref groupedItems, value);
        }




        public ViewItemsViewModel(){
            NameOfCollection = "All";
            //PageAppearingCommand = new Command(PageAppearing);
            RefreshCommand = new Command(RefreshList);
            RemoveCommand = new AsyncCommand<Item>(RemoveItem);
            AddToOutfitCommand = new AsyncCommand<Item>(AddToOutfit);
            RefreshList();
        }

        private async void RefreshList()
        {
            if (IsBusy) return;
            IsBusy = true;
            //ItemList = await ItemService.GetItemsAsync();

            GroupedItems = await GetGroupedItems();
            IsBusy = false;
            return;
        }
        

        private async Task<List<Grouping<string, Item>>> GetGroupedItems()
        {
            var groupedItems = new List<Grouping<string, Item>>();

            var Collections = await CollectionService.GetCollectionsAsync();
            var Items = await ItemService.GetItemsAsync();

            foreach (Collection c in Collections)
            {
                List<Item> items = Items.Where(i => i.Collection == c.ID).ToList();
                groupedItems.Add(new Grouping<string, Item>(c.Name, items));
            }
            var itemsWithNoCollection = Items.Where(i => i.Collection == -1).ToList();
            if (itemsWithNoCollection.Count > 0)
            {
                groupedItems.Add(new Grouping<string, Item>("Un-Sorted", itemsWithNoCollection));
            }

            return groupedItems;
        }

        
        private async Task AddToOutfit(Item item) 
        {
            // TODO: Add to outfit
        }
        private async Task RemoveItem(Item item)
        {
            await ItemService.DeleteItemAsync(item.ID);
            RefreshList();
        }
    }
}
