using Microsoft.EntityFrameworkCore;
using ReceiptManager.Core.Models;
using ReceiptManager.Core.Services;
using ReceiptManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReceiptManager.Services
{
    public class ReceiptService : DbService, IReceiptService
    {
        public ReceiptService(IReceiptManagerDbContext context) : base(context)
        { }

        public ServiceResult CreateReceipt(List<Item> receiptItems)
        {
            return Create(new Receipt(receiptItems));
        }

        public ServiceResult DeleteReceipt(long id)
        {
            var receiptToDelete = GetReceiptById(id);

            return Delete(receiptToDelete);
        }

        public Receipt GetReceiptById(long id)
        {
            return Query<Receipt>().Include(receipt => receipt.Items)
                .FirstOrDefault(receipt => receipt.Id == id);
        }

        public List<Receipt> GetAllReceipts()
        {
            return Query<Receipt>().Include(receipt => receipt.Items).ToList();
        }

        public List<Receipt> FilterReceiptsByItem(string itemProductName)
        {
            return Query<Receipt>().Include(receipt => receipt.Items)
                .Where(receipt => receipt.Items.Any(item => item.ProductName == itemProductName)).ToList();
        }

        public List<Receipt> FilterReceiptsByPeriod(DateTime lowerRange, DateTime upperRange)
        {
            return Query<Receipt>().Include(receipt => receipt.Items)
                .Where(receipt => receipt.CreatedOn >= lowerRange && receipt.CreatedOn <= upperRange).ToList();
        }
    }
}