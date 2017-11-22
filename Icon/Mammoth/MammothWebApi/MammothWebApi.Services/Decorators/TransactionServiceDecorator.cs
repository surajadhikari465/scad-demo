using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.Service.Services;
using System;

namespace MammothWebApi.Service.Decorators
{
    public class TransactionServiceDecorator<T> : IUpdateService<T> where T: class
    {
        private IUpdateService<T> service;
        private IDbProvider db;

        public TransactionServiceDecorator(IUpdateService<T> service, IDbProvider db)
        {
            this.service = service;
            this.db = db;
        }

        public void Handle(T data)
        {
            using (this.db.Transaction = this.db.Connection.BeginTransaction())
            {
                try
                {
                    this.service.Handle(data);
                    this.db.Transaction.Commit();
                }
                catch (Exception)
                {
                    this.db.Transaction.Rollback();
                    throw;
                }
            }
        }
    }
}