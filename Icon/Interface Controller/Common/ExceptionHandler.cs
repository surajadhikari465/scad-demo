using Icon.Logging;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;

namespace InterfaceController.Common
{
    public class ExceptionHandler<T> where T : class
    {
        private ILogger<T> logger;

        public ExceptionHandler(ILogger<T> logger)
        {
            this.logger = logger;
        }

        public void HandleException(string message, Exception ex, Type callingClass, MethodBase callingMethod)
        {
            logger.Error(String.Format("{0}  Exception details - Method: {1}  Class: {2}  Exception: {3}",
                message,
                callingMethod.Name,
                callingClass.Name,
                ex.ToString()
            ));
        }

        public void HandleException(Exception ex, Type callingClass, MethodBase callingMethod)
        {
            var dbEntityValidationException = ex as DbEntityValidationException;

            if (dbEntityValidationException == null)
            {
                logger.Error(String.Format("Exception in Method: {0}  Class: {1}.  Exception: {2}",
                    callingMethod.Name,
                    callingClass.Name,
                    ex.ToString()
                ));
            }
            else
            {
                HandleException(dbEntityValidationException, callingClass, callingMethod);
            }
        }

        public void HandleException(DbEntityValidationException ex, Type callingClass, MethodBase callingMethod)
        {
            var entityValidationErrors = String.Join(",", ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage));

            logger.Error(String.Format("Exception in Method: {0}  Class: {1}.  EntityValidationErrors: {3}  Exception: {2}",
                callingMethod.Name,
                callingClass.Name,
                entityValidationErrors,
                ex.ToString()
            ));
        }

        public void HandleException(Exception ex, Type callingClass, MethodBase callingMethod, Type eventHandlerClass, string region, int referenceId, string message)
        {
            var dbEntityValidationException = ex as DbEntityValidationException;

            if (dbEntityValidationException == null)
            {
                logger.Error(String.Format("Exception in Method: {0}  Class: {1}  Event Handler: {2}.  Parameters: Region = {3}, ReferenceId = {4}, Message = {5}  Exception: {6}.",
                    callingMethod.Name,
                    callingClass.Name,
                    eventHandlerClass.Name,
                    region,
                    referenceId,
                    message,
                    ex.ToString()
                ));
            }
            else
            {
                HandleException(dbEntityValidationException, callingClass, callingMethod, eventHandlerClass, region, referenceId, message);
            }

        }

        public void HandleException(DbEntityValidationException ex, Type callingClass, MethodBase callingMethod, Type eventHandlerClass, string region, int referenceId, string message)
        {
            var entityValidationErrors = String.Join(",", ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage));

            logger.Error(String.Format("Exception in Method: {0}  Class: {1}  Event Handler: {2}.  EntityValidationErrors: {3}  Parameters: Region = {4}, ReferenceId = {5}, Message = {6}  Exception: {7}.",
                callingMethod.Name,
                callingClass.Name,
                eventHandlerClass.Name,
                entityValidationErrors,
                region,
                referenceId,
                message,
                ex.ToString()
            ));
        }

        public void HandleException(Exception ex, Type callingClass, MethodBase callingMethod, Type eventHandlerClass)
        {
            var dbEntityValidationException = ex as DbEntityValidationException;

            if (dbEntityValidationException == null)
            {
                logger.Error(String.Format("Exception in Method: {0}  Class: {1}  Event Handler: {2}.  Exception: {3}",
                    callingMethod.Name,
                    callingClass.Name,
                    eventHandlerClass.Name,
                    ex.ToString()
                ));
            }
            else
            {
                HandleException(dbEntityValidationException, callingClass, callingMethod, eventHandlerClass);
            }
        }

        public void HandleException(DbEntityValidationException ex, Type callingClass, MethodBase callingMethod, Type eventHandlerClass)
        {
            var entityValidationErrors = String.Join(",", ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage));

            logger.Error(String.Format("Exception in Method: {0}  Class: {1}  Event Handler: {2}.  EntityValidationErrors: {4}  Exception: {3}",
                callingMethod.Name,
                callingClass.Name,
                eventHandlerClass.Name,
                entityValidationErrors,
                ex.ToString()
            ));
        }
    }
}
