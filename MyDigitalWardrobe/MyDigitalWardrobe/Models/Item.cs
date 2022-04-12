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
        public decimal PurchasedLongitude { get; set; }
        public decimal PurchasedLatitude { get; set; }
        public DateTime DatePurchased { get; set; }
        public DateTime WarrantyEnd { get; set; }
    }
}
