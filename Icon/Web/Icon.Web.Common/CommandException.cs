using System;
using System.Runtime.Serialization;

namespace Icon.Web.Common
{
    [Serializable]
    public class CommandException : Exception
    {
        public CommandException()
        {
        }

        public CommandException(string message)
            : base(message)
        {
        }

        public CommandException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected CommandException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
