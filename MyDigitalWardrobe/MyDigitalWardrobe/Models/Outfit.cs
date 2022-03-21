using System;
using System.Collections.Generic;
using SQLite;

namespace MyDigitalWardrobe.Models
{
    public class Outfit
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public Item[] Items { get; set; }
    }
}
