using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.DataAccess.Commands;
using Icon.Common.DataAccess;
using System;
using System.Threading;

namespace BulkItemUploadProcessor.DataAccess.Decorators
{
    public class ProcessErrorRecordsDecorator<TData> : ICommandHandler<AddItemsCommand>
    {
        private readonly BulkItemUploadProcessorSettings settings;
        private ICommandHandler<AddItemsCommand> commandHandler;

        public ProcessErrorRecordsDecorator(
            BulkItemUploadProcessorSettings settings,
            ICommandHandler<AddItemsCommand> commandHandler)
        {
            this.settings = settings;
            this.commandHandler = commandHandler;
        }

        public void Execute(AddItemsCommand data)
        {
            int count = 0;
            do
            {
                try
                {
                    commandHandler.Execute(data);
                    break;
                }
                catch (Exception)
                {
                    if(count >= settings.RetryAddUpdateCount - 1)
                    {
                        throw;
                    }
                    Thread.Sleep(settings.RetryAddUpdateMillisecondDelay);
                    count++;
                }
            } while (count < settings.RetryAddUpdateCount);
        }
    }
}