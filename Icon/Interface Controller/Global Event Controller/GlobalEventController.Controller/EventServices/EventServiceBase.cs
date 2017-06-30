using GlobalEventController.Common;
using Icon.Framework;
using System;
using System.Collections.Generic;

namespace GlobalEventController.Controller.EventServices
{
    public abstract class EventServiceBase : IEventService
    {
        public int? ReferenceId { get; set; }
        public string Message { get; set; }
        public string Region { get; set; }
        public int EventTypeId { get; set; }
        public List<ScanCode> ScanCodes { get; set; }
        public List<RegionalItemMessageModel> RegionalItemMessage { get; set; }

        public abstract void Run();

        /// <summary>
        /// Verifies the provided EventQueue parameters and throws an ArgumentException if 
        /// any is invalid. The excption will include a message describing the validation
        /// rules for the parameters and reflecting the provided values.
        /// </summary>
        /// <example>"MyEventSerivce was called with invalid arguments. ReferenceId must be 
        /// greater than 0. Region and Message must not be null or empty. ReferenceId = -1,
        /// Message = ' ', Region = 'FL'"</example>
        /// <param name="eventServiceName">The name of the event service calling the
        ///     validator, for identification in the error message</param>
        /// <param name="eventReferenceId">The id from the event being validated 
        ///     (must have a positive value)</param>
        /// <param name="eventMessage">The message from the event being validated
        ///     (cannot be null or empty)</param>
        /// <param name="region">The region from the event being validated
        ///     (cannot be null or empty)</param>
        protected void VerifyEventParameters(
            string eventServiceName,
            int? referenceId, 
            string eventMessage, 
            string region)
        {
            if (referenceId.GetValueOrDefault(0) < 1 || String.IsNullOrEmpty(eventMessage) || String.IsNullOrEmpty(region))
            {
                string errorMessage = String.Join(" ",
                    $"{eventServiceName} was called with invalid arguments.",
                    "ReferenceId must be greater than 0.",
                    "Region and Message must not be null or empty.",
                    $"ReferenceId = {referenceId}, Message = '{eventMessage}', Region = '{region}'");
                throw new ArgumentException(errorMessage);
            }
        }
    }
}
