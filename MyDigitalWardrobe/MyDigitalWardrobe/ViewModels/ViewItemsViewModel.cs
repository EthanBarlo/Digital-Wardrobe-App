using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers.Commands;
using MyDigitalWardrobe.Models;
using MyDigitalWardrobe.Services;
using System.Linq;

namespace MyDigitalWardrobe.ViewModels
{
    public class ViewItemsViewModel : ViewModelBase
    {
        public ICommand PageAppearingCommand { get; set; }
        private string NameOfCollection { get; set; }
        
        private List<Item> itemList;
        public List<Item> ItemList
        {
            get => itemList;
            set => SetProperty(ref itemList, value);
        }
        public Item SelectedItem { get; set; }

        public ViewItemsViewModel(){
            NameOfCollection = "All";
            PageAppearingCommand = new Command(PageAppearing);
        }

        private async void PageAppearing()
        {
            ItemList = await ItemService.GetItemsAsync();
        }
    }
}
