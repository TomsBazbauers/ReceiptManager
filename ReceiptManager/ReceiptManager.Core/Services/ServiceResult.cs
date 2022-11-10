using ReceiptManager.Core.Models;
using System;
using System.Collections.Generic;

namespace ReceiptManager.Core.Services
{
    public class ServiceResult
    {
        public bool Success { get; private set; }

        public DateTime Created { get; private set; }

        public IEntity Entity { get; private set; }

        public IList<string> Errors { get; private set; }

        public string FormattedErrors => string.Join(", ", Errors);

        public ServiceResult(bool success)
        {
            Success = success;
            Created = DateTime.Now;
            Errors = new List<string>();
        }

        public ServiceResult SetEntity(IEntity entity)
        {
            Entity = entity;
            return this;
        }

        public ServiceResult AddError(string error)
        {
            Errors.Add(error);
            return this;
        }
    }
}
