using System;

namespace Icon.Common.CustomExceptions
{
    public class IconBaseException : Exception
    {
        /// <summary>
        /// The target, recipient, or destination of the action/task/request that was taking place when the exception occurred.
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// The action taking place when exception occurred.
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// Information about why the error occurred.
        /// </summary>
        public string Cause { get; set; }

        public IconBaseException()
        {
        }

        public IconBaseException(string message)
            : base(message)
        {
        }

        public IconBaseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public IconBaseException(string message, string target, string action, string cause, Exception inner)
            : base(message, inner)
        {
            this.Target = target;
            this.Action = action;
            this.Cause = cause;
        }

        public override string ToString()
        {
            return String.Format("Type=[{0}], Target=[{1}], Action=[{2}], Cause=[{3}], Exception=[{4}]",
                this.GetType(),
                this.Target,
                this.Action,
                this.Cause,
                this.Message);
        }
    }
}
