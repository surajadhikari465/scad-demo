using Icon.Logging;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;

namespace Icon.Web.Common
{
    public class ExceptionLogger
    {
        private ILogger logger;

        public ExceptionLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public void LogException(Exception ex, Type callingClass, MethodBase callingMethod)
        {
            logger.Error(String.Format("Exception in Method: {0}  Class: {1}.  Exception: {2}",
                callingMethod.Name,
                callingClass.Name,
                ex.ToString()
            ));
        }

        public void LogException(DbEntityValidationException ex, Type callingClass, MethodBase callingMethod)
        {
            string entityValidationErrors = String.Join(",", ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage));

            logger.Error(String.Format("Exception in Method: {0}  Class: {1}  EntityValidationErrors: {2}  Exception: {2}",
                callingMethod.Name,
                callingClass.Name,
                entityValidationErrors,
                ex.ToString()
            ));
        }

        public void LogException(CommandException ex, Type callingClass, MethodBase callingMethod)
        {
            DbEntityValidationException dbEntityValidationException = ex.InnerException as DbEntityValidationException;
            if (dbEntityValidationException != null)
            {
                logger.Error(ex.Message);
                LogException(dbEntityValidationException, callingClass, callingMethod);
            }
            else
            {
                LogException((Exception)ex, callingClass, callingMethod);
            }
        }
    }
}
