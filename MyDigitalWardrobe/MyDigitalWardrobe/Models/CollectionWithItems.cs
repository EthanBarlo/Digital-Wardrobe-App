using System;
using System.Collections.Generic;
using System.Text;

namespace MyDigitalWardrobe.Models
{
    public class CollectionWithItems
    {
        public string Name { get; set; }
        public List<Item> Items { get; set; }
    }
}
