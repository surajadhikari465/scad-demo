using Icon.Common.DataAccess;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.ErrorUtility;
using Icon.Logging;
using System;
using System.Threading;

namespace Icon.Infor.Listeners.HierarchyClass.Decorators
{
    public class RetryCommandHandlerDecorator<T> : ICommandHandler<T>
        where T : IHierarchyClassCommand
    {
        private ICommandHandler<T> commandHandler;
        private IHierarchyClassListenerSettings hierarchyClassListenerSettings;
        private IErrorMapper errorMapper;
        private ILogger<RetryCommandHandlerDecorator<T>> logger;

        public RetryCommandHandlerDecorator(
            ICommandHandler<T> commandHandler,
            IHierarchyClassListenerSettings hierarchyClassListenerSettings,
            IErrorMapper errorMapper,
            ILogger<RetryCommandHandlerDecorator<T>> logger)
        {
            this.commandHandler = commandHandler;
            this.hierarchyClassListenerSettings = hierarchyClassListenerSettings;
            this.errorMapper = errorMapper;
            this.logger = logger;
        }

        public void Execute(T data)
        {
            int executeCount = 1;

            do
            {
                try
                {
                    commandHandler.Execute(data);
                    return;
                }
                catch (Exception ex)
                {
                    logger.Warn($"Error occurred in {typeof(T)}. Executed {executeCount} time(s). Set to execute {hierarchyClassListenerSettings.MaxNumberOfRetries}. Error Details:{ex}");
                    if (executeCount >= hierarchyClassListenerSettings.MaxNumberOfRetries)
                    {
                        string errorCode = errorMapper.GetAssociatedErrorCode(typeof(T));
                        string errorDetails = errorMapper.GetFormattedErrorDetails(typeof(T), ex);
                        foreach (var item in data.HierarchyClasses)
                        {
                            item.ErrorCode = errorCode;
                            item.ErrorDetails = errorDetails;
                        }
                    }
                    Thread.Sleep(hierarchyClassListenerSettings.RetryDelayInMilliseconds);
                    executeCount++;
                }
            } while (executeCount <= hierarchyClassListenerSettings.MaxNumberOfRetries);
        }
    }
}
