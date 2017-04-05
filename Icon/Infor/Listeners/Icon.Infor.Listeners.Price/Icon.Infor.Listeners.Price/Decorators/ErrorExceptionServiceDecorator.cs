using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.Models;
using Icon.Infor.Listeners.Price.Services;
using Icon.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.Price.Decorators
{
    public class ErrorExceptionServiceDecorator<T> : IService<T> where T : PriceModel
    {
        private IService<T> service;
        private ILogger<T> logger;

        public ErrorExceptionServiceDecorator(IService<T> service, ILogger<T> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        public void Process(IEnumerable<T> data, IEsbMessage message)
        {
            try
            {
                this.service.Process(data, message);
            }
            catch (Exception exception)
            {
                foreach (var row in data)
                {
                    string errorCode = $"{row.Action}{service.GetType().Name}Error";
                    row.ErrorCode = errorCode;
                    row.ErrorDetails = $"{exception.Message}. InnerException: {exception.InnerException?.ToString()}";

                    logger.Error(JsonConvert.SerializeObject(new
                    {
                        ErrorCode = errorCode,
                        Error = exception.ToString(),
                        MessageId = message.GetProperty("MessageId"),
                        Prices = data,
                        Message = message.ToString()
                    }));
                }
            }
        }
    }
}
