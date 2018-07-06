using Mammoth.Common;
using Mammoth.Common.ControllerApplication;
using Mammoth.Common.Email;
using Mammoth.Price.Controller.Common;
using Mammoth.Price.Controller.DataAccess.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Price.Controller.ApplicationModules
{
    public class ErrorAlerter : IErrorAlerter
    {
        private PriceControllerApplicationSettings settings;
        private IEmailClient emailClient;
        private IEmailBuilder emailBuilder;

        public ErrorAlerter(
            PriceControllerApplicationSettings settings,
            IEmailClient emailClient,
            IEmailBuilder emailBuilder)
        {
            this.settings = settings;
            this.emailClient = emailClient;
            this.emailBuilder = emailBuilder;
        }

        public void AlertErrors(List<PriceEventModel> priceEventModels)
        {
            var failedEvents = priceEventModels
               .Where(q => !String.IsNullOrEmpty(q.ErrorMessage) && !settings.NonAlertErrors.Contains(q.ErrorMessage))
               .Select(em => new
               {
                   Resolution = em.EventTypeId == Constants.EventTypes.Price ? "Perform Mammoth Price Refresh" : "Further Research Required",
                   Region = em.Region,
                   ScanCode = em.ScanCode,
                   BusinessUnitId = em.BusinessUnitId,
                   QueueId = em.QueueId,
                   EventType = em.EventTypeId == Constants.EventTypes.Price ? "Price" : "Price Rollback",
                   ExceptionSource = em.ErrorSource,
                   ExceptionType = em.ErrorDetails.IsJsonString() ? (string)JObject.Parse(em.ErrorDetails)[Constants.ExceptionProperties.ExceptionType] : em.ErrorDetails,
                   ExceptionMessage = em.ErrorDetails.IsJsonString() ? (string)JObject.Parse(em.ErrorDetails)[Constants.ExceptionProperties.ExceptionMessage] : em.ErrorDetails
               });

            if (failedEvents.Any())
            {
                emailClient.Send(emailBuilder
                    .BuildEmail(failedEvents.ToList(), "The following price changes had errors when being inserted/updated in Mammoth for SLAW. " +
                        "The resolution details are provided below. " + Environment.NewLine +
                        "The regional IRMA table 'mammoth.ChangeQueueHistory' will provide more details if necessary. " +
                        "Additionally the logs in the Mammoth database are available."),
                    "Mammoth Price Error - ACTION REQUIRED");
            }
        }

        public void AlertErrors(List<CancelAllSalesEventModel> cancelAllSalesEventModels)
        {
            var failedEvents = cancelAllSalesEventModels
               .Where(q => !String.IsNullOrEmpty(q.ErrorMessage) && !settings.NonAlertErrors.Contains(q.ErrorMessage))
               .Select(em => new
               {
                   Resolution = "Further Research Required",
                   Region = em.Region,
                   ScanCode = em.ScanCode,
                   BusinessUnitId = em.BusinessUnitId,
                   QueueId = em.QueueId,
                   EventType = "Cancel All Sales",
                   ExceptionSource = em.ErrorSource,
                   ExceptionType = em.ErrorDetails.IsJsonString() ? (string)JObject.Parse(em.ErrorDetails)[Constants.ExceptionProperties.ExceptionType] : em.ErrorDetails,
                   ExceptionMessage = em.ErrorDetails.IsJsonString() ? (string)JObject.Parse(em.ErrorDetails)[Constants.ExceptionProperties.ExceptionMessage] : em.ErrorDetails
               });

            if (failedEvents.Any())
            {
                emailClient.Send(emailBuilder
                    .BuildEmail(failedEvents.ToList(), "The following cancel all sales changes had errors when being updated in Mammoth for SLAW. " +
                        "The resolution details are provided below. " + Environment.NewLine +
                        "The regional IRMA table 'mammoth.ChangeQueueHistory' will provide more details if necessary. " +
                        "Additionally the logs in the Mammoth database are available."),
                    "Mammoth Price Error - ACTION REQUIRED");
            }
        }
    }
}
