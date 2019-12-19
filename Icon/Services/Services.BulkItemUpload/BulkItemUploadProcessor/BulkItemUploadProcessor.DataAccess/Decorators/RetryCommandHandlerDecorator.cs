using BulkItemUploadProcessor.Common;
using Icon.Common.DataAccess;
using System;
using System.Threading;

namespace BulkItemUploadProcessor.DataAccess.Decorators
{
    public class RetryCommandHandlerDecorator<TData> : ICommandHandler<TData>
    {
        private readonly BulkItemUploadProcessorSettings settings;
        private ICommandHandler<TData> commandHandler;

        public RetryCommandHandlerDecorator(
            BulkItemUploadProcessorSettings settings,
            ICommandHandler<TData> commandHandler)
        {
            this.settings = settings;
            this.commandHandler = commandHandler;
        }

        public void Execute(TData data)
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