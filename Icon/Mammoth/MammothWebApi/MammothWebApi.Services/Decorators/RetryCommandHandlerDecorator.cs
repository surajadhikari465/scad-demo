using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Settings;
using Newtonsoft.Json;
using System;

namespace MammothWebApi.Service.Decorators
{
    public class RetryCommandHandlerDecorator<T> : ICommandHandler<T> where T : class
    {
        private ICommandHandler<T> commandHandler;
        private DataAccessSettings settings;
        private ILogger logger;

        public RetryCommandHandlerDecorator(
            ICommandHandler<T> service,
            DataAccessSettings settings,
            ILogger logger)
        {
            this.commandHandler = service;
            this.settings = settings;
            this.logger = logger;
        }

        public void Execute(T command)
        {
            int retryCount = 0;
            ExecuteWithRetry(command, retryCount);
        }

        private void ExecuteWithRetry(T command, int retryCount)
        {
            try
            {
                this.commandHandler.Execute(command);
            }
            catch (Exception ex)
            {
                logger.Error(JsonConvert.SerializeObject(new
                {
                    Message = "Exception occurred while calling command. Will attempt a retry if CurrentRetryCount is less than MaxRetryCount.",
                    CurrentRetryCount = retryCount,
                    MaxRetryCount = settings.DatabaseRetryCount,
                    Command = command
                }), ex);
                if (retryCount < settings.DatabaseRetryCount)
                {
                    ExecuteWithRetry(command, retryCount + 1);
                }
                else
                {
                    logger.Error(JsonConvert.SerializeObject(new
                    {
                        Message = "Exception occurred while calling command and CurrentRetryCount has exceeded MaxRetryCount. Will rethrow exception.",
                        CurrentRetryCount = retryCount,
                        MaxRetryCount = settings.DatabaseRetryCount,
                        Command = command
                    }), ex);

                    throw;
                }

            }
        }
    }
}
