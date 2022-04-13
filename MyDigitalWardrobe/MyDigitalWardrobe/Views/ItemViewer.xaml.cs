using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyDigitalWardrobe.Models;
using MyDigitalWardrobe.ViewModels;

namespace MyDigitalWardrobe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemViewer : ContentPage
    {
        public ItemViewer(Item item)
        {
            InitializeComponent();
            BindingContext = new ItemViewerViewModel(item);
        }
    }
}