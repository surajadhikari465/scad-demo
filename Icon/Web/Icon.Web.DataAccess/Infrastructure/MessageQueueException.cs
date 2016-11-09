using Icon.Common.CustomExceptions;
using System;

namespace Icon.Web.DataAccess.Infrastructure
{
    public class MessageQueueException : IconBaseException
    {
        public MessageQueueException()
        {
        }

        public MessageQueueException(string message)
            : base(message)
        {
        }

        public MessageQueueException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public MessageQueueException(string message, string target, string action, string cause, Exception inner)
            : base(message, target, action, cause, inner)
        {
        }
    }
}
