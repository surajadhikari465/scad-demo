using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Icon.Web.DataAccess.Decorators
{
    public class RetryUniqueConstraintManagerHandlerDecorator<T> : IManagerHandler<T> where T : class
    {
        private IManagerHandler<T> managerHandler;

        private ILogger logger;
        private int databaseRetryCount;

        public RetryUniqueConstraintManagerHandlerDecorator(
            IManagerHandler<T> service,
            ILogger logger)
        {
            this.managerHandler = service;
            this.logger = logger;
        }

        public void Execute(T command)
        {
            int retryCount = 0;
            ExecuteWithRetry(command, retryCount);
        }

        private void ExecuteWithRetry(T command, int retryCount)
        {
            if (!int.TryParse(ConfigurationManager.AppSettings["databaseRetryCount"].ToString(), out databaseRetryCount))
            {
                databaseRetryCount = 3;
            }

            try
            {
                this.managerHandler.Execute(command);
            }

            catch (SqlException ex)
            {
                // if unique constraint error then only retry 
                if ((ex.Number == 2601 || ex.Number == 2627) && retryCount < databaseRetryCount)
                {
                    ExecuteWithRetry(command, retryCount + 1);
                }
                else
                {
                    logger.Error("Exception occurred while calling command and CurrentRetryCount has exceeded MaxRetryCount. Will rethrow exception.");
                    throw;
                }
            }

            catch (Exception)
            {
                    logger.Error("Exception occurred while calling command and CurrentRetryCount has exceeded MaxRetryCount. Will rethrow exception.");
                    throw;
            }

        }
    }
}