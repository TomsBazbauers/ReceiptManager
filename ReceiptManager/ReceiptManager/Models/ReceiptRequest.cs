using ReceiptManager.Core.Models;
using System;
using System.Collections.Generic;

namespace ReceiptManager.Models
{
    public class ReceiptRequest
    {
        public ReceiptRequest(List<Item> items)
        {
            Items = items;
        }

        public List<Item> Items { get; set; }
    }
}