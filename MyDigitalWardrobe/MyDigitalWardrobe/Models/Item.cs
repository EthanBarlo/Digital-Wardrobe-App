using System;
using SQLite;

namespace MyDigitalWardrobe.Models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Collection { get; set; }
        public string ItemImage { get; set; }
        public string RecieptImage { get; set; }
        public int TimesWorn { get; set; }
        public string PurchasedName { get; set; }
        public double PurchasedLongitude { get; set; }
        public double PurchasedLatitude { get; set; }
        public DateTime DatePurchased { get; set; }
        public DateTime WarrantyEnd { get; set; }

        public Item()
        {
            Name = "";
            Price = 0;
            Collection = 0;
            ItemImage = "";
            RecieptImage = "";
            TimesWorn = 0;
            PurchasedName = "";
            PurchasedLongitude = 0;
            PurchasedLatitude = 0;
            DatePurchased = DateTime.Now;
            WarrantyEnd = DateTime.Now;
        }
    }
}
