using Mammoth.Common.Email;
using MammothWebApi.Service.Services;
using System;

namespace MammothWebApi.Service.Decorators
{
    public class EmailErrorServiceDecorator<T> : IService<T> where T: class
    {
        private IService<T> service;
        private IEmailClient emailClient;
        private IEmailMessageBuilder<T> emailBuilder;

        public EmailErrorServiceDecorator(IService<T> service,
            IEmailClient emailClient,
            IEmailMessageBuilder<T> emailBuilder)
        {
            this.service = service;
            this.emailClient = emailClient;
            this.emailBuilder = emailBuilder;
        }

        public void Handle(T data)
        {
            try
            {
                this.service.Handle(data);
            }
            catch (Exception e)
            {
                string messageBody = this.emailBuilder.BuildMessage(data, e);
                string messageSubject = "Mammoth Service Error - Unhandled Exception";
                this.emailClient.Send(messageBody, messageSubject);

                throw;
            }
        }
    }
}
