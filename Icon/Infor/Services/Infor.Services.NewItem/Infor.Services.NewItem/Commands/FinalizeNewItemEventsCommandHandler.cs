using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irma.Framework;
using Icon.Common;
using System.Data.SqlClient;
using System.Data;
using Icon.Common.Context;
using Infor.Services.NewItem.Infrastructure;
using Icon.Logging;
using Infor.Services.NewItem.Models;
using Infor.Services.NewItem.Constants;
using Newtonsoft.Json;

namespace Infor.Services.NewItem.Commands
{
    public class FinalizeNewItemEventsCommandHandler : ICommandHandler<FinalizeNewItemEventsCommand>
    {
        private IRenewableContext<IrmaContext> context;
        private ILogger<FinalizeNewItemEventsCommandHandler> logger;

        public FinalizeNewItemEventsCommandHandler(IRenewableContext<IrmaContext> context,
            ILogger<FinalizeNewItemEventsCommandHandler> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public IRenewableContext<IrmaContext> Context
        {
            get
            {
                return context;
            }

            set
            {
                context = value;
            }
        }

        public void Execute(FinalizeNewItemEventsCommand data)
        {
            try
            {
                if (data.NewItems.Any())
                {
                    var queueIds = data.NewItems
                        .Select(i => new { i.QueueId })
                        .ToTvp("queueIds", "infor.NewItemEventType");
                    var errorOccurred = new SqlParameter("errorOccurred", SqlDbType.Bit) { Value = data.ErrorOccurred };
                    var instanceId = new SqlParameter("instanceId", SqlDbType.Int) { Value = data.Instance };

                    context.Context.Database.ExecuteSqlCommand("EXEC infor.FinalizeNewItemEvents @queueIds, @instanceId, @errorOccurred", queueIds, instanceId, errorOccurred);
                }

                var instanceIdForFailUnprocessedEvents = new SqlParameter("instanceId", SqlDbType.Int) { Value = data.Instance };
                context.Context.Database.ExecuteSqlCommand("EXEC infor.FailUnprocessedEvents @instanceId", instanceIdForFailUnprocessedEvents);

                LogFinalizedItems(data);
            }
            catch (Exception ex)
            {
                LogException(ex, data);
            }
        }

        private void LogFinalizedItems(FinalizeNewItemEventsCommand data)
        {
            if (data.NewItems.Any())
            {
                logger.Info(string.Format(
                    "FinalizeNewItemEvents finalized {0} items. Region: {1}, ScanCodes: {2}",
                    data.NewItems.Count(),
                    data.Region,
                    string.Join(",", data.NewItems.Select(i => i.ScanCode))));
            }
            else
            {
                logger.Info(string.Format(
                    "FinalizeNewItemEvents finalize {0} items. Region {1}",
                    0,
                    data.Region));
            }
        }

        private void LogException(Exception ex, FinalizeNewItemEventsCommand data)
        {
            logger.Error(JsonConvert.SerializeObject(
                new
                {
                    Message = ApplicationErrors.Codes.FailedToFinalizeItemsError,
                    Region = data.Region,
                    Items = data.NewItems,
                    ExceptionMessage = ex.Message,
                    Exception = ex.ToString()
                }));
        }
    }
}
