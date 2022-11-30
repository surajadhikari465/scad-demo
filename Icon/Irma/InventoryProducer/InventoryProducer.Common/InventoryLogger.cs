using Icon.DbContextFactory;
using Icon.Logging;
using Irma.Framework;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;

namespace InventoryProducer.Common
{
    public class InventoryLogger<T>: IInventoryLogger<T> where T : class
    {
        private readonly ILogger<T> logger;
        private readonly IDbContextFactory<IrmaContext> irmaContextFactory;
        private readonly InventoryProducerSettings settings;

        public InventoryLogger(ILogger<T> logger, IDbContextFactory<IrmaContext> irmaContextFactory, InventoryProducerSettings settings)
        {
            this.logger = logger;
            this.irmaContextFactory = irmaContextFactory;
            this.settings = settings;
        }
        public InventoryLogger(ILogger<T> logger)
        {
            this.logger = logger;
        }

        public void LogException(Exception ex, Type callingClass, MethodBase callingMethod)
        {
            logger.Error(string.Format("Exception in Method: {0}  Class: {1}.  Exception: {2}",
                callingMethod.Name,
                callingClass.Name,
                ex.ToString()
            ));
        }

        public void LogException(string message, Exception ex, Type callingClass, MethodBase callingMethod)
        {
            logger.Error(string.Format("{0}  Method: {1}.  Class: {2}.  Exception: {3}.",
                message,
                callingMethod.Name,
                callingClass.Name,
                ex.ToString()
            ));
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
            if (irmaContextFactory != null && settings != null)
            {
                using (var irmaContext = irmaContextFactory.CreateContext($"Irma_{settings.RegionCode}"))
                {
                    irmaContext.Database.CommandTimeout = 10;
                    string archiveSqlStatement = @"INSERT INTO dbo.AppLog(LogDate, ApplicationID, HostName, UserName, Thread, Level, Logger, Message, InsertDate) VALUES (@LogDate, @ApplicationId, @HostName, @UserName, @Thread, @Level, @Logger, @Message, @InsertDate)";
                    irmaContext
                        .Database
                        .ExecuteSqlCommand(
                        archiveSqlStatement,
                        new SqlParameter("@LogDate", DateTime.Now),
                        new SqlParameter("@ApplicationID", settings.InstanceGUID),
                        new SqlParameter("@HostName", Environment.MachineName),
                        new SqlParameter("@UserName", Environment.UserName),
                        new SqlParameter("@Thread", Thread.CurrentThread.ManagedThreadId),
                        new SqlParameter("@Level", "Info"),
                        new SqlParameter("@Logger", typeof(T).FullName.Length <= 255 ? typeof(T).FullName : typeof(T).ToString()),
                        new SqlParameter("@Message", message.Substring(0, message.Length <= 4000 ? message.Length : 4000)),
                        new SqlParameter("@InsertDate", DateTime.Now)
                        );
                }
            }
        }

        public void LogError(string message, string exception)
        {
            logger.Error(message);
            if (irmaContextFactory != null && settings != null)
            {
                using (var irmaContext = irmaContextFactory.CreateContext($"Irma_{settings.RegionCode}"))
                {
                    irmaContext.Database.CommandTimeout = 10;
                    string archiveSqlStatement = @"INSERT INTO dbo.AppLog(LogDate, ApplicationID, HostName, UserName, Thread, Level, Logger, Message, Exception, InsertDate) VALUES (@LogDate, @ApplicationId, @HostName, @UserName, @Thread, @Level, @Logger, @Message, @Exception, @InsertDate)";
                    irmaContext
                        .Database
                        .ExecuteSqlCommand(
                        archiveSqlStatement,
                        new SqlParameter("@LogDate", DateTime.Now),
                        new SqlParameter("@ApplicationID", settings.InstanceGUID),
                        new SqlParameter("@HostName", Environment.MachineName),
                        new SqlParameter("@UserName", Environment.UserName),
                        new SqlParameter("@Thread", Thread.CurrentThread.ManagedThreadId),
                        new SqlParameter("@Level", "Error"),
                        new SqlParameter("@Logger", typeof(T).FullName.Length <= 255 ? typeof(T).FullName : typeof(T).ToString()),
                        new SqlParameter("@Message", message.Substring(0, message.Length <= 4000 ? message.Length : 4000)),
                        new SqlParameter("@Exception", exception.Substring(0, exception.Length <= 2000 ? exception.Length : 2000)),
                        new SqlParameter("@InsertDate", DateTime.Now)
                        );
                }
            }
        }
    }
}
