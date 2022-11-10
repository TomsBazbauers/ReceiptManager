using ReceiptManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptManager.Core.Services
{
    public interface IDbService
    {
        ServiceResult Create<T>(T entity) where T : Entity;

        ServiceResult Delete<T>(T entity) where T : Entity;

        IQueryable<T> Query<T>() where T : Entity;
    }
}
