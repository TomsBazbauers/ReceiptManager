using ReceiptManager.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReceiptManager.Core.Validations
{
    public class ItemNameValidator : IItemValidator
    {
        public bool IsValid(List<Item> items)
        {
            return !items.Any(item => string.IsNullOrEmpty(item.ProductName.Trim()));
        }
    }
}
