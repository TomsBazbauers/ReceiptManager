using ReceiptManager.Core.Models;
using System.Collections.Generic;

namespace ReceiptManager.Core.Validations
{
    public class ItemCountValidator : IItemValidator
    {
        public bool IsValid(List<Item> items)
        {
            return items.Count > 0;
        }
    }
}
