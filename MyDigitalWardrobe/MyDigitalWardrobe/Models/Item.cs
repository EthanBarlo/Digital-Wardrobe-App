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
        public string Image { get; set; }
        public int TimesWorn { get; set; }
        public string LocationName { get; set; }
        public decimal LocationLongitude { get; set; }
        public decimal LocationLatitude { get; set; }
        public DateTime DatePurchased { get; set; }
        public DateTime WarrantyEnd { get; set; }
    }
}
