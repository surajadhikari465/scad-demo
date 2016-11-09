using Icon.Common.DataAccess;
using Icon.Esb.CchTax.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.CchTax.Decorator
{
    public class DataConnectionCommandHandlerDecorator<T> : ICommandHandler<T>
    {
        private string connectionString;
        private DataConnectionManager connectionManager;
        private ICommandHandler<T> inner;
        public DataConnectionCommandHandlerDecorator(DataConnectionManager connectionManager, string connectionString, ICommandHandler<T> inner)
        {
            this.connectionManager = connectionManager;
            this.connectionString = connectionString;
            this.inner = inner;
        }

        public void Execute(T data)
        {
            using (IDataConnection connection = connectionManager.InitializeConnection(connectionString))
            {
                inner.Execute(data);
            }
        }
    }
}
