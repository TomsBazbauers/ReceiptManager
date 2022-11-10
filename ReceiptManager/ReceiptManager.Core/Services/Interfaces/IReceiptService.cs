using ReceiptManager.Core.Models;
using System;
using System.Collections.Generic;

namespace ReceiptManager.Core.Services
{
    public interface IReceiptService
    {
        ServiceResult CreateReceipt(List<Item> receiptItems);

        ServiceResult DeleteReceipt(long id);

        Receipt GetReceiptById(long id);

        List<Receipt> GetAllReceipts();

        List<Receipt> FilterReceiptsByPeriod(DateTime lowerRange, DateTime upperRange);

        List<Receipt> FilterReceiptsByItem(string item);
    }
}
