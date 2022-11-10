using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReceiptManager.Core.Models;
using System.Threading.Tasks;

namespace ReceiptManager.Data
{
    public interface IReceiptManagerDbContext
    {
        DbSet<T> Set<T>() where T : class;

        EntityEntry<T> Entry<T>(T entity) where T : class;

        DbSet<Receipt> Receipts { get; set; }

        DbSet<Item> Items { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
