using System;
using System.Collections.Generic;

namespace ReceiptManager.Core.Models
{
    public class Receipt : Entity
    {
        public Receipt()
        { }

        public Receipt(List<Item> items, DateTime? createdOn = null)
        {
            CreatedOn = createdOn ?? DateTime.Now;
            Items = items;
        }

        public DateTime CreatedOn { get; set; }

        public List<Item> Items { get; set; }
    }
}