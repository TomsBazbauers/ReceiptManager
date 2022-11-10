using Microsoft.EntityFrameworkCore;
using ReceiptManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptManager.Data
{
    public class ReceiptManagerDbContext : DbContext, IReceiptManagerDbContext
    {
        public ReceiptManagerDbContext(DbContextOptions options) : base(options)
        { }

        public ReceiptManagerDbContext() { }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<Item> Items { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
