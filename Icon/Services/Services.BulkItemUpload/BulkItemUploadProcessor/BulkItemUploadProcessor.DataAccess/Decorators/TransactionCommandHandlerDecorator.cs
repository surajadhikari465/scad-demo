using Icon.Common.DataAccess;
using System.Transactions;

namespace BulkItemUploadProcessor.DataAccess.Decorators
{
    public class TransactionCommandHandlerDecorator<TData> : ICommandHandler<TData>
    {
        private TransactionScope transactionScope;
        private ICommandHandler<TData> commandHandler;

        public TransactionCommandHandlerDecorator(
            ICommandHandler<TData> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        public void Execute(TData data)
        {
            try
            {
                transactionScope = new TransactionScope();
                commandHandler.Execute(data);
                transactionScope.Complete();
            }
            finally
            {
                transactionScope.Dispose();
            }
        }
    }
}