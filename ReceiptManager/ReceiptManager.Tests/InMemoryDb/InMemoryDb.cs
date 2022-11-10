using Microsoft.EntityFrameworkCore;
using ReceiptManager.Core.Models;
using ReceiptManager.Data;
using System;
using System.Collections.Generic;

namespace ReceiptManager.Tests
{
    public class InMemoryDb : IDisposable
    {
        public ReceiptManagerDbContext _dbContext;

        public InMemoryDb()
        {
            var inMemoryDb = new DbContextOptionsBuilder<ReceiptManagerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var testReceipts = new List<Receipt>()
            {
                new Receipt(new List<Item>(){ new Item("Lenovo laptop") }, new DateTime(2022, 10, 30)),
                new Receipt(new List<Item>(){ new Item("Dell laptop"), new Item("Logitech mouse") }),
                new Receipt(new List<Item>(){ new Item("Bose headphones"), new Item("Chewing gum") }, new DateTime(2022, 11, 01))
            };

            _dbContext = new ReceiptManagerDbContext(inMemoryDb);
            _dbContext.Receipts.AddRange(testReceipts);

            _dbContext.SaveChanges();
            _dbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}