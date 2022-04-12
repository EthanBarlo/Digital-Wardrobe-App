using System;
using SQLite;

namespace MyDigitalWardrobe.Models
{
    public class Collection
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
