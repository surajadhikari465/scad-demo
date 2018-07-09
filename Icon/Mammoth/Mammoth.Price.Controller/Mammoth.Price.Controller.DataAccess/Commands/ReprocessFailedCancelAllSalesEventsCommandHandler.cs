using Dapper;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Logging;
using Mammoth.Price.Controller.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Price.Controller.DataAccess.Commands
{
    public class ReprocessFailedCancelAllSalesEventsCommandHandler : ICommandHandler<ReprocessFailedCancelAllSalesEventsCommand>
    {
        private IDbProvider dbProvider;
        private PriceControllerApplicationSettings settings;
        private ILogger logger;

        public ReprocessFailedCancelAllSalesEventsCommandHandler(IDbProvider dbProvider, PriceControllerApplicationSettings settings, ILogger logger)
        {
            this.dbProvider = dbProvider;
            this.settings = settings;
            this.logger = logger;
        }

        public void Execute(ReprocessFailedCancelAllSalesEventsCommand data)
        {
            try
            {
                var eventsToReprocess = data.Events
                    .Join(
                        data.CancelAllSales.Where(c => c.ErrorMessage != null),
                        queuedEventModel => queuedEventModel.QueueId,
                        priceEventModel => priceEventModel.QueueId,
                        (queuedEventModel, priceEventModel) => new
                        {
                            QueueID = queuedEventModel.QueueId,
                            Item_Key = queuedEventModel.ItemKey,
                            Store_No = queuedEventModel.StoreNo,
                            Identifier = queuedEventModel.Identifier,
                            EventTypeID = queuedEventModel.EventTypeId,
                            EventReferenceID = queuedEventModel.EventReferenceId,
                            InsertDate = queuedEventModel.InsertDate,
                            ProcessFailedDate = (DateTime?)null,
                            InProcessBy = (int?)null,
                            ReprocessCount = queuedEventModel.ReprocessCount.GetValueOrDefault() + 1,
                        })
                    .Where(e => e.ReprocessCount <= settings.ReprocessCount)
                    .ToList();
                if (eventsToReprocess.Any())
                {
                    dbProvider.Connection.Execute(@"
                        INSERT INTO mammoth.PriceChangeQueue(Item_Key, Store_No, Identifier, EventTypeID, EventReferenceID, InsertDate, ProcessFailedDate, InProcessBy, ReprocessCount)
                        VALUES (@Item_Key, @Store_No, @Identifier, @EventTypeID, @EventReferenceID, @InsertDate, @ProcessFailedDate, @InProcessBy, @ReprocessCount)",
                        eventsToReprocess);
                }
            }
            catch(Exception ex)
            {
                logger.Error(JsonConvert.SerializeObject(new { data.Region, Message = "Error occurred when reprocessessing Cancel All Sales event." }), ex);
            }
        }
    }
}
