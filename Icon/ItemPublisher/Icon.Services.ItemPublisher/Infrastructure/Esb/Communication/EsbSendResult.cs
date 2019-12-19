using System;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    public class EsbSendResult
    {
        private Exception exception;

        /// <summary>
        /// Whether the ESB call was successful
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// A message from our app to tell what happened with a failure
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// The request sent to the ESB
        /// </summary>
        public string Request { get; private set; }

        /// <summary>
        /// ESB message headers
        /// </summary>
        public Dictionary<string, string> Headers { get; private set; }

        /// <summary>
        /// Correlation id for the ESB
        /// </summary>
        public Guid MessageId { get; private set; }

        /// <summary>
        /// Warnings that did not prevent us from sending to the ESB but we want to log
        /// </summary>
        public List<string> Warnings { get; private set; }

        public EsbSendResult(bool success, string message, string request = "", Dictionary<string, string> headers = null, Guid messageId = default(Guid), List<string> warnings = null, Exception exception = null)
        {
            this.Message = message;
            this.Success = success;
            this.Request = request;
            this.exception = exception;
            this.MessageId = messageId;
            this.Headers = headers;
            this.Warnings = warnings ?? new List<string>();
        }

        public void SetWarnings(List<string> warnings)
        {
            this.Warnings = warnings;
        }

        public override string ToString()
        {
            if (this.Success)
            {
                return string.Empty;
            }
            else
            {
                return $@"Message:{this.Message}" + Environment.NewLine +
                    $"Warnings: {(this.Warnings != null ? String.Join(Environment.NewLine, this.Warnings.ToArray()) : string.Empty)}" +
                    $"Exception:{(this.exception == null ? string.Empty : this.exception.ToString())}";
            }
        }
    }
}