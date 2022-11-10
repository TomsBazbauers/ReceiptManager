using ReceiptManager.Core.Models;
using ReceiptManager.Core.Services;
using ReceiptManager.Data;
using System;
using System.Linq;

namespace ReceiptManager.Services
{
    public class DbService : IDbService
    {
        protected IReceiptManagerDbContext _dbContext;

        public DbService(IReceiptManagerDbContext context)
        {
            _dbContext = context;
        }

        public ServiceResult Create<T>(T entity) where T : Entity
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return new ServiceResult(true).SetEntity(entity);
        }

        public ServiceResult Delete<T>(T entity) where T : Entity
        {
            try
            {
                _dbContext.Set<T>().Remove(entity);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return new ServiceResult(false).AddError(e.Message);
            }

            return new ServiceResult(true);
        }

        public IQueryable<T> Query<T>() where T : Entity
        {
            return _dbContext.Set<T>().AsQueryable();
        }
    }
}
