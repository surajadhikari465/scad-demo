using Mammoth.Logging;
using MammothWebApi.Service.Services;
using System;

namespace MammothWebApi.Service.Decorators
{
    public class LoggingServiceDecorator<T> : IUpdateService<T> where T: class
    {
        private IUpdateService<T> service;
        private ILogger logger;

        public LoggingServiceDecorator(IUpdateService<T> service, ILogger logger)
        {
            this.service = service;
            this.logger = logger;
        }

        public void Handle(T data)
        {
            try
            {
                logger.Debug(String.Format("Executing ICommandHandler<{0}>.", typeof(T)));
                this.service.Handle(data);
            }
            catch (Exception e)
            {
                logger.Error("An exception occurred.", e);
                throw;
            }
        }
    }
}
