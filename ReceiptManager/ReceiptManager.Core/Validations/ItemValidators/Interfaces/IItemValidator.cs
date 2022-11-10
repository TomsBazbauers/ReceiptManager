using ReceiptManager.Core.Models;
using System.Collections.Generic;

namespace ReceiptManager.Core.Validations
{
    public interface IItemValidator
    {
        bool IsValid(List<Item> item);
    }
}
